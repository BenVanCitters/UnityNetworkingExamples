using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPReceiver : MonoBehaviour 
{
    UdpClient udpClient;
    IPEndPoint RemoteIpEndPoint;

    //port exposed for unity
    [SerializeField] int EndPointPort = 9999;

	// Use this for initialization
	void Start () 
    {
        RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, EndPointPort);

        udpClient = new UdpClient();
        udpClient.Client.Bind(RemoteIpEndPoint);

        PrepareForUDP();
	}


    //init an asynchronous callback to be called on reciept of a udp packet
    void PrepareForUDP()
    {
        AsyncCallback callback = new AsyncCallback(UDPMsgRecievedCallback);
        udpClient.BeginReceive(callback, null);
    }

    /// <summary>
    /// Function designed to be called when a UDP message is recieved
    /// if we wanted to listen for the next we might put another 
    /// 'BeginReceive()' method at the end of this method.
    /// </summary>
    /// <param name="result">Result.</param>
    private void UDPMsgRecievedCallback(IAsyncResult result)
    {
        byte[] received = udpClient.EndReceive(result, ref RemoteIpEndPoint);

        string receiveString = Encoding.ASCII.GetString(received);

        Debug.Log("Received an encoded string: "+ receiveString);
    }


    private void OnDestroy()
    {
        // make sure to clean up sockets on exit
        udpClient.Client.Close();
        udpClient.Close();
    }
}
