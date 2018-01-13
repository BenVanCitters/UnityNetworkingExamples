using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;


//transcribed from https://msdn.microsoft.com/en-us/library/system.net.sockets.tcpclient(v=vs.110).aspx
// Create a TcpClient.
// Note, for this client to work you need to have a TcpServer 
// connected to the same address as specified by the server, port
// combination.
public class TCPClient : MonoBehaviour 
{
    public Int32 port = 13000;
    public string server = "";
    public string MessageToSend = "a string";
    TcpClient client;
    NetworkStream stream;
    int count = 0;

	// Use this for initialization
	void Start () 
    {
        
        Connect(MessageToSend);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Connect(MessageToSend + " " + count++);
        }
 	}


    void Connect(string message)
    {
        try
        {
            client = new TcpClient(server, port);
            // Get a client stream for reading and writing.
            stream = client.GetStream();

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Send the message to the connected TcpServer. 
            //stream.w
            stream.Write(data, 0, data.Length);

            Debug.Log(string.Format("Sent: {0}", message));

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            string responseData = string.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log(string.Format("Received: {0}", responseData));

            // Close everything.
            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log(string.Format("ArgumentNullException: {0}", e));
        }
        catch (SocketException e)
        {
            Debug.Log(string.Format("SocketException: {0}", e));
        }

        Debug.Log("\n Press Enter to continue...");
    }

    private void OnDestroy()
    {

    }
}
