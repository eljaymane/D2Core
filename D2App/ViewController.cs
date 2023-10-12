using System.Threading;
using CoreFoundation;
using D2Core.core.network.eventHandlers;
using D2Core.core.network.messages;
using D2Core.Game.network.eventHandlers;
using D2Core.infrastructure.networking;
using Microsoft.Extensions.DependencyInjection;
using ObjCRuntime;
namespace D2App;

public partial class ViewController : NSViewController {

	private DofusNetworkSniffer networkSniffer;
	protected ViewController (NativeHandle handle) : base (handle)
	{
		// This constructor is required if the view controller is loaded from a xib or a storyboard.
		// Do not put any initialization here, use ViewDidLoad instead.
	}

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
		Services.ConfigureServices();
		networkSniffer = Services.Resolve<DofusNetworkSniffer>();
		this.Initialize();
        // Do any additional setup after loading the view.
    }

	private void Write(object sender,ProtocolEventArgs eventArgs)
	{
		var msg = (ChatServerMessage)eventArgs.message;
		var text = $" [{msg.senderName}] : {msg.content}";
		WriteLine(text);
	}

	private void WriteLine(string line)
	{
        InvokeOnMainThread(() => {
            // manipulate UI controls
            ((NSTextView)ChatBox.DocumentView).Value += line + "\n";
        });
        
	}

	private void Initialize()
	{
        ChatMessageServerEventHandler.ChatMessageEvent += Write;
        InvokeOnMainThread(() => {
            // manipulate UI controls
            

            DeviceCombo.UsesDataSource = true;
            DeviceCombo.DataSource = new DeviceComboDataSource(networkSniffer.GetAvailableDevices());
            DeviceCombo.SelectionChanged += DeviceCombo_SelectionChanged;

            //ChatBox.DocumentView = ChatText;
            WriteLine("Initialization finisehd !");
        });
        
    }


    public override NSObject RepresentedObject {
		get => base.RepresentedObject;
		set {
			base.RepresentedObject = value;

			// Update the view, if already loaded.
		}
	}

    partial void SendChat(NSButton sender)
    {
		var content = MsgText.StringValue;
		MsgText.StringValue = "";

    }

	partial void StartCapture(NSButton sender)
	{
        InvokeOnMainThread(() => {
            if (StartBtn.Title == "Stop")
            {
                this.networkSniffer.StopCapture();
                StartBtn.Title = "Start";
                DeviceCombo.Editable = true;
                DeviceCombo.Selectable = true;
                return;
            }
            else
            {
                this.networkSniffer.SetCaptureDevice(DeviceCombo.StringValue);
                StartBtn.Title = StartBtn.Title == "Stop" ? "Start" : "Stop";
                DeviceCombo.Editable = false;
                DeviceCombo.Selectable = false;
                start();
            }
        });
        
        
		
	}

    private void DeviceCombo_SelectionChanged(object? sender, EventArgs e)
    {
		
    }

    private void start()
    {
        networkSniffer.StartCapture();
    }



}

