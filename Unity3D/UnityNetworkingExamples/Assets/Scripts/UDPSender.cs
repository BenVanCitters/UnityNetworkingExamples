using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSender : MonoBehaviour 
{
    public string IPAddressString = "10.102.52.201";
    public int EndpointPort = 5006; 
    public string MessageString = "Hello from Unity3D!";
	

	void Start () 
    {
        UDPSend();
	}

    void UDPSend()
    {
        //init socket object
        Socket sock = new Socket(AddressFamily.InterNetwork, 
                                 SocketType.Dgram,
                                 ProtocolType.Udp);

        //parse the IP address
        IPAddress serverAddr = IPAddress.Parse(IPAddressString);

        IPEndPoint endPoint = new IPEndPoint(serverAddr, EndpointPort);

        //convert to bytes
        byte[] send_buffer = Encoding.ASCII.GetBytes(MessageString);

        sock.SendTo(send_buffer, endPoint);

        //close up socket object
        sock.Close();
    }
}
