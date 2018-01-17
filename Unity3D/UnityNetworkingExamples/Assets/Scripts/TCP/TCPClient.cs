using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;


// transcribed from https://msdn.microsoft.com/en-us/library/system.net.sockets.tcpclient(v=vs.110).aspx
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

    /// <summary>
    /// Class needed to pass 'data' to the 'ReadCompletedCallback' along with
    /// the stream, oddly c# doesn't seem to anticipate this
    /// </summary>
    class StreamStruct{
        public NetworkStream stream;
        public Byte[] data;
        public StreamStruct(Byte[] d, NetworkStream s) { stream = s; data = d; }
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
            stream.Write(data, 0, data.Length);
            Debug.Log(string.Format("Sent: {0}", message));

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            string responseData = string.Empty;
            Debug.Log("Beginning read");
            // Read the first batch of the TcpServer response bytes.
            stream.BeginRead(data, 0, data.Length, 
                             ReadCompletedCallback, 
                             new StreamStruct(data,stream));
        }
        //exceptions
        catch (ArgumentNullException e)
        {  Debug.Log(string.Format("ArgumentNullException: {0}", e)); }
        catch (SocketException e)
        { Debug.Log(string.Format("SocketException: {0}", e));  }
    }

    private void ReadCompletedCallback(IAsyncResult ar)
    {
        StreamStruct streamStruct = (StreamStruct)ar.AsyncState;
        int mycount = streamStruct.stream.EndRead(ar);
        Debug.Log("Completing read " + mycount + ": " + 
                  System.Text.Encoding.ASCII.GetString(streamStruct.data,0,mycount));
        // Close everything.

        stream.Close();
        client.Close();
    }

    private void OnDestroy()
    {
        // Close everything.
        stream.Close();
        client.Close();
    }
}
