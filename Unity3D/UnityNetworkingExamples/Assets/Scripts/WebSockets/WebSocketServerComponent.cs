using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Net;
using System.Text;

public class WebSocketServerComponent : MonoBehaviour 
{
    [SerializeField] string IPAddressString = "192.168.0.1";
    [SerializeField] int Port = 9999;
    WebSocketServer webSocketServer;

	// Use this for initialization
	void Start () 
    {
        IPAddress IPAddress = IPAddress.Parse(IPAddressString);
        //WebSocketServer webSocketServer = new WebSocketServer(IPAddress, Port);
        webSocketServer = new WebSocketServer(Port);
        webSocketServer.AddWebSocketService<MyWebSocketBehavior>("/service");
        Debug.Log("Starting websocket server");
        webSocketServer.Start();
        //Console.ReadKey(true);
        //webSocketServer.Stop();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    private void OnDestroy()
    {
        webSocketServer.Stop();
    }
}

public class MyWebSocketBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        //Send(msg);
        //Sessions.Broadcast(Encoding.ASCII.GetBytes(msg));
        string receivedString = Encoding.UTF8.GetString(e.RawData);
        Debug.Log("Recieved message! - " +receivedString);
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("MyWebSocketBehavior OnClose - " + e.Reason);
    }

    protected override void OnError(ErrorEventArgs e)
    {
        Debug.LogError("MyWebSocketBehavior OnError - " + e.Message);
    }

    protected override void OnOpen()
    {
        Debug.Log("MyWebSocketBehavior OnOpen");
    }
}