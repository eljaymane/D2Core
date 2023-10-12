using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using D2Core.core;
using D2Core.core.events.abstractions;
using D2Core.core.events.eventbus;
using D2Core.core.network.events;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;

namespace D2Core.infrastructure.networking
{
	public class DofusNetworkSniffer
	{
		private Thread _captureThread;
		private Thread _processThread;
		private ILogger<DofusNetworkSniffer> logger;
		private ILiveDevice captureDevice;
		private IPAddress localAddress;
		private ConcurrentQueue<Packet> _packets;
		private MessageFactory messageFactory;
		private ProtocolEventBus eventBus;
		private bool isCancellationRequested = false;

		public IList<ILiveDevice> deviceList { get; private set; }

		public DofusNetworkSniffer(ILogger<DofusNetworkSniffer> logger, MessageFactory messageFactory, ProtocolEventBus eventBus)
		{
			this.logger = logger;
			this.messageFactory = messageFactory;
			this.eventBus = eventBus;
			_packets = new ConcurrentQueue<Packet>();
			this.deviceList = CaptureDeviceList.Instance;
			_captureThread = new Thread(Capture);
			_processThread = new Thread(StartProcessing);
		}

		public List<string> GetAvailableDevices()
		{
			var devices = new List<string>();
			foreach(var device in CaptureDeviceList.Instance)
			{
				devices.Add(device.Name);
			}
			return devices;
		}

		public void SetCaptureDevice(string name)
		{
			if (this.captureDevice != null) this.captureDevice.Close();
			this.captureDevice = CaptureDeviceList.Instance.Where(d => d.Name == name).FirstOrDefault();
			this.captureDevice.Open();
			this.captureDevice.Filter = "tcp port 5555";
			captureDevice.OnPacketArrival += Device_OnPacketArrival;
		}

		public void SetCaptureDevice(ILiveDevice device)
		{
            if (this.captureDevice != null) this.captureDevice.Close();
            this.captureDevice = device;
            this.captureDevice.Open();
            this.captureDevice.Filter = "tcp port 5555";
            captureDevice.OnPacketArrival += Device_OnPacketArrival;
        }

        public void Device_OnPacketArrival(object s, PacketCapture e)
        {
            var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
            if (e.GetPacket().GetPacket().PayloadPacket.Bytes != null) _packets.Enqueue(packet);
        }

		public void Send(byte[] packet)
		{
			captureDevice.SendPacket(packet);
		}

		public void StartCapture()
		{
			isCancellationRequested = false;
			logger.LogInformation("Starting capture...");
			_captureThread.Start();
			_processThread.Start();
			//StartProcessing();
		}

        private void Capture()
        {
			captureDevice.StartCapture();
        }

        private void StartProcessing()
		{
			var processingThread = new Thread(new ThreadStart(Process));
			processingThread.Start();
		}

		public void StopCapture()
		{
			isCancellationRequested = true;
			_captureThread.Interrupt();
			_processThread.Interrupt();
		}

		private void Process()
		{
            while (!isCancellationRequested)
            {
                if (_packets.Count > 0)
                {
                    _packets.TryDequeue(out var packet);
					ParsePacket(packet).ConfigureAwait(true);
                }
            }

		
        }

		private string getDestination(Packet packet)
		{
			string direction = "";
            if (packet is EthernetPacket eth)
            {
                var ip = packet.Extract<IPPacket>();
                direction = ip.SourceAddress.Address.Equals(IPAddress.Parse("192.168.1.23").Address) ? "[SENT]" : "[RECEIVED]";
            }
			return direction;
        }

        private Task ParsePacket(Packet packet)
        {

            var payload = packet.PayloadPacket.PayloadPacket;
			if (payload == null || payload.PayloadData == null || payload.PayloadData.Length < 10) return Task.CompletedTask;
            var ms = new MemoryStream(payload.PayloadData);
			ushort header = ASBinaryOps.readShort(ref ms);
			var protocolId = D2PacketParseUtil.getProtocolId(header);
			var lenType = D2PacketParseUtil.getLenType(header);
			var dataLength = lenType > 0 ? D2PacketParseUtil.getLength(lenType, payload.PayloadData) : 0;
			var data = lenType > 0 ? D2PacketParseUtil.getData(payload.PayloadData, lenType, dataLength) : null;
			logger.LogInformation($"{getDestination(packet)} Message with ID : {protocolId} lenType : {lenType}  Length : {dataLength}");
			if(data!=null)messageFactory.BuildMessage(protocolId, payload.PayloadData.Skip(2).Skip((int)lenType).Skip(1).ToArray()).Wait();
			return Task.CompletedTask;
        }

		

		



		


    }
}

