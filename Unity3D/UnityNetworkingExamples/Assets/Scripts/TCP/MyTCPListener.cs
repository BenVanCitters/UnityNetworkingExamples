using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// transcribed into unity from example code at:
// https://msdn.microsoft.com/en-us/library/system.net.ipendpoint(v=vs.110).aspx

/// <summary>
/// My TCP listener/ TCP server.
/// This class works by creating a tcp listener and acting on tcp connections received 
/// thereby.
/// 
/// This class cooperates with unity by using an asynchronous model
/// 
/// When a tcp message is received by this server - the same message is sent 
/// back with all characters capitalized
/// </summary>
public class MyTCPListener : MonoBehaviour 
{
    public Int32 PortNumber = 9999;
    public string IPAddressString = "10.102.52.122";
    TcpListener tcpListener = null;  

    // Use this for initialization
    void Start()
    { 
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse(IPAddressString), PortNumber);

            // Start listening for client requests.
            tcpListener.Start();
            Debug.Log("Server started!");
        }
        catch(Exception e)
        {
            Debug.Log(string.Format("Exception: {0}", e));
        }
    } 

    /// <summary>
    /// Callback for when a TCP client connection is accepted
    /// </summary>
    /// <param name="ar">Ar.</param>
    public void AcceptTCPClientCallback(IAsyncResult ar) 
    {
        // Get the listener that handles the client request.
        TcpListener listener = (TcpListener)ar.AsyncState;

        // End the operation and display the received data on the console.
        TcpClient client = listener.EndAcceptTcpClient(ar);

        Debug.Log("Client connected completed");
        SendMSGBack(client.GetStream());
    }

    /// <summary>
    /// prepare an asycn read from a given stream
    /// </summary>
    /// <param name="stream">Stream.</param>
    void SendMSGBack(NetworkStream stream)
    {
        Byte[] bytes = new Byte[256]; //256 size buffer

        //inits a async stream read
        stream.BeginRead(bytes, 0, bytes.Length, 
                         OnReadCompletedCallback, new StreamStruct(bytes, stream));
    }

    /// <summary>
    /// Class needed to pass 'data' to the 'ReadCompletedCallback' along with
    /// the stream, oddly c# doesn't seem to anticipate this
    /// </summary>
    class StreamStruct
    {
        public NetworkStream stream;
        public Byte[] data;
        public StreamStruct(Byte[] d, NetworkStream s) {stream = s; data = d;}
    }

    /// <summary>
    /// Asynchronously read from network stream
    /// </summary>
    /// <param name="ar">Ar.</param>
    private void OnReadCompletedCallback(IAsyncResult ar)
    {
        StreamStruct streamStruct = (StreamStruct)ar.AsyncState;
        int mycount = streamStruct.stream.EndRead(ar);

        // Translate data bytes to a ASCII string.
        String data = System.Text.Encoding.ASCII.GetString(streamStruct.data, 
                                                           0, mycount);
        Debug.Log(string.Format("Received: {0}", data));

        // Process the data sent by the client.
        data = data.ToUpper();

        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

        // Send back a response. use begin write to avoid blocking 
        // the main unity thread
        Debug.Log(string.Format("Sending: {0} ...", data));
        streamStruct.stream.BeginWrite(msg, 
                                       0, 
                                       msg.Length, 
                                       myWriteCallBack, 
                                       streamStruct.stream);

        //queue up another read
        SendMSGBack(streamStruct.stream);
    }

    /// <summary>
    /// Establishes a non-blocking, async writeback to incoming tcp connections
    /// </summary>
    /// <param name="ar">Ar.</param>
    public static void myWriteCallBack(IAsyncResult ar)
    {
        NetworkStream myNetworkStream = (NetworkStream)ar.AsyncState;
        myNetworkStream.EndWrite(ar);
    }

    /// <summary>
    /// Unity component call - Update is called once per frame
    /// if the tcp listener has pending connections then they one an async 
    /// connection is attempt is queued.
    /// </summary>
	void Update () 
    {
        // if we successfully created a tcp listener and if there is at least
        // one pending 'client' connection 
        if(tcpListener != null && tcpListener.Pending())
        {
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptTCPClientCallback),
                                       tcpListener);
        }
	}

    /// <summary>
    /// Gets the helper sting method
    /// </summary>
    /// <returns>The TCPS tring.</returns>
    string GetTCPString()
    {
        return "ip" + IPAddressString + ":" + PortNumber;
    }

    //make sure to stop the tcp listener
    private void OnDestroy()
    {
        tcpListener.Stop();
        Debug.Log("Stopped server!");
    }
}
