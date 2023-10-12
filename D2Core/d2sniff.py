import sys
import pyshark
import socket
from scapy.all import *
from struct import unpack


def readPacket(packet):
    header, = unpack('>'+'H',packet[:2])
    print("Packet ID : ", getId(header),"\n")
    print("LenType : ",getLenType(header),"\n")
    getData(packet)
    return

def getId(header):
    print("Header : ")
    print(bin(header));
    id = header >> 2
    print("shifted (2) : ",bin(id))
    return id

def getLenType(header): 
    return header & 3

def getData(packet):
    return ""

capture = pyshark.LiveCapture(interface='en1',bpf_filter='tcp port 5555 and len > 66',use_json=True,include_raw=True) # On bind le module WireShark sur l'interface WiFi (pour moi)
capture.sniff(timeout=5) # On sniffe l'interface pendant 20 secondes
print("[DEBUG] {} paquets reçus".format( len(capture) ) )   
for packet in capture.sniff_continuously(packet_count =10):
            if packet.ip.src == socket.gethostbyname(socket.gethostname()):
                        print("[DEBUG] Paquet Dofus Envoyé")
            else:
                        print("[DEBUG] Paquet Dofus Reçu")
            hex_packet = packet.frame_raw.value
            binary_data = bytearray.fromhex(hex_packet)
            #print("Packet ID : ",int.from_bytes(binary_data,'big'))
            readPacket(binary_data)
            #data, = unpack('>'+'H'*(len(binary_data[:4])//2),binary_data[:4])
            #print(bin(data))

        
