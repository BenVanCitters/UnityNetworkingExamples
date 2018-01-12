using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Net;
using System.Text;

/// <summary>
/// Web socket server component.
/// Starts the websocket server on 'start' being called and stops on 'ondestroy'
/// being called.
/// </summary>
public class WebSocketServerComponent : MonoBehaviour 
{
    //[SerializeField] string IPAddressString = "192.168.0.1"; //probably not needed
    [SerializeField] int Port = 9999;
    [SerializeField] string ServiceName = "service";
    WebSocketServer webSocketServer;


	// Use this for initialization
	void Start () 
    {
        InitAndStartServer();
	}

    /// <summary>
    /// Inits the and start server with one 'service' endpoint, namely, "service"
    /// </summary>
    void InitAndStartServer()
    {
        webSocketServer = new WebSocketServer(Port);

        //this constructor doesn't take an ip address
        webSocketServer.AddWebSocketService<MyWebSocketBehavior>("/" + ServiceName);

        Debug.Log("Starting websocket server at " + webSocketServer.Address.ToString() + " : " + webSocketServer.Port );
        webSocketServer.Start();
    }

    /// <summary>
    /// stops the server
    /// </summary>
    private void OnDestroy()
    {
        webSocketServer.Stop();
    }
}

/// <summary>
/// Encapsulation for a specific type of 'service' endpoint
/// </summary>
public class MyWebSocketBehavior : WebSocketBehavior
{
    /// <summary>
    /// Callback for when a message comes down
    /// </summary>
    /// <param name="e">E.</param>
    protected override void OnMessage(MessageEventArgs e)
    {
        //Send(msg);
        //Sessions.Broadcast(Encoding.ASCII.GetBytes(msg));
        string receivedString = Encoding.UTF8.GetString(e.RawData);
        Debug.Log("Received message! - " + receivedString);
    }

    /// <summary>
    /// Callback when a connection is closed
    /// </summary>
    /// <param name="e">E.</param>
    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("MyWebSocketBehavior OnClose - " + e.Reason);
    }

    /// <summary>
    /// Ons the error.
    /// </summary>
    /// <param name="e">E.</param>
    protected override void OnError(ErrorEventArgs e)
    {
        Debug.LogError("MyWebSocketBehavior OnError - " + e.Message);
    }

    /// <summary>
    /// Callback for when a connection is opened
    /// </summary>
    protected override void OnOpen()
    {
        Debug.Log("MyWebSocketBehavior OnOpen");
    }
}