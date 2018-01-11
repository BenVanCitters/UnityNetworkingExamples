using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WebSocketSharp;

//client
public class WebSocketClient : MonoBehaviour 
{
    WebSocket MyWebSocket;
    [SerializeField] int PortNumber = 9999;
    [SerializeField] string IPAddressString = "10.102.52.201";
    [SerializeField] string HandleString = "pollingservice";

	// Use this for initialization
	void Start () 
    {
        InitAndConnectWebSocket();
        SendMessageToSocket("SENDERER");
	}

    /// <summary>
    /// Getter for the combined websocket creation string
    /// </summary>
    /// <returns>The web socket string.</returns>
    string GetWebSocketString()
    {
        return "ws://" + IPAddressString + ":" + PortNumber + "/" + HandleString;
    }

    /// <summary>
    /// Inits the Websocket, initiates a connection to it, and adds callback events
    /// Presumeably the callbacks happen on the main thread?
    /// </summary>
    void InitAndConnectWebSocket()
    {
        Debug.Log("Starting websocket client...");
        MyWebSocket = new WebSocket(GetWebSocketString());
        MyWebSocket.EmitOnPing = true;

        MyWebSocket.OnError += WebSocketError;
        MyWebSocket.OnMessage += WebSocketMessageRecieved;
        MyWebSocket.OnOpen += WebSocketOpened;
        MyWebSocket.OnClose += WebSocketOnClose;
        MyWebSocket.Connect();
        Debug.Log("...Done starting websocket client");
    }


    /// <summary>
    /// Callback when socket is closed
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    void WebSocketOnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket OnClose: " + e.Reason);
    }


    /// <summary>
    /// Callback when errors are received
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    void WebSocketError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket OnError: " + e.Message);
    }


    /// <summary>
    /// Callback that is made when a message is received from the websocket
    /// Merely prints out the 'Data' field from the messageeventargs
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    void WebSocketMessageRecieved(object sender, MessageEventArgs e)
    {
        if (e.IsPing)
        {
            Debug.Log("Ping!");
        }
        Debug.Log("WebSocket OnMessage: " + e.Data);
    }


    /// <summary>
    /// Callback that is run when the socket is opened
    /// merely prints out the recieved params
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    void WebSocketOpened(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket OnOpen: " + e.ToString());
    }


    /// <summary>
    /// Sends a string message to socket.
    /// </summary>
    /// <param name="message">Message.</param>
    void SendMessageToSocket(string message)
    {
        if (MyWebSocket != null &&  MyWebSocket.IsAlive)
        {
            Debug.Log("WebSocket Sending msg:" +  message);
            MyWebSocket.Send(Encoding.ASCII.GetBytes(message));
        }
    }


    /// <summary>
    /// Method called from unity when the component is 'destroyed'
    /// Here we ought to close the socket like a responsible user.
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log("Closing WebSocket: " + GetWebSocketString());
        MyWebSocket.Close();
    }
}
