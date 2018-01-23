# UnityNetworkingExamples
Examples of various networking tech within unity3d to and from the outside.  These scripts were created to make a networking lauchpad for other projects... simple sketches containing easily transferable code snippets into larger, more complex, and complicated projects.  As these sketches evolve they will eventually come to inhabit an architecture, but as I am learning these technologies as I use them they won't have great shape at the start.

## Disclaimer
Opening ports of your application or network to the outside world (ie, the internet; untrusted actors) is an inherently dangerous thing to do.  Security is neither easy nor trivial.  These examples do not include any security measures and are thereby not secure.  If you use these examples please acknowledge the risk of leaving unprotected ports open and use appropriate measures of confidentiality, integrity, and authenticity to prevent the loss, theft and damage that could be incurred otherwise.

## User Datagram Protocol (UDP)
https://en.wikipedia.org/wiki/User_Datagram_Protocol

To explore UDP with Unity and Python there is a unity scene and two script components:
 - Scenes/UDPExampleScene.unity
 - Scripts/UDP/UDPReceiver.cs
 - Scripts/UDP/UDPSender.cs

Along with Two Python Scripts:
 - RaspberryPi/Python/UDPScripts/UDPListener.py
 - RaspberryPi/Python/UDPScripts/UDPSender.py
 
When running the UDPExampleScene in the Unity project the UDPListener & UDPSender python scripts can be used as corrolary (and even on remote devices) to send and receive messages from an external source(s).
These scripts send only strings back and forth.

## Transmission Control Protocol (TCP)
https://en.wikipedia.org/wiki/Transmission_Control_Protocol

There are several components to explore TCP connectivity with in Unity, Processing, and Python.
 - UnityAssets/Scripts/MyTCPListener.cs
 - UnityAssets/Scripts/TCPClient.cs
 
 Python Scripts:
 - /RaspberryPi/Python/TCPScripts/TCPClient.py 
 - /RaspberryPi/Python/TCPScripts/TCPServer.py 

 Processing Code:
 - /Processing/SharedCanvasClient/SharedCanvasClient.pde

## WebSockets
https://en.wikipedia.org/wiki/WebSocket

To explore WebSockets via Unity there are some scripts, a library, and a scene
- Plugins/websocket-sharp/websocket-sharp.dll
- Scenes/WebSocketExampleScene.unity
- Scripts/WebSockets/WebSocketClient.cs
- Scripts/WebSockets/WebSocketServerComponent.cs

Along with Two Python Scripts:
- RaspberryPi/Python/WebSocketClient.py
- RaspberryPi/Python/WebSocketServer.py

When using the WebSocketExample Scene in the Unity project use the cooresponding WebSocketClient and WebSocketServer python scripts located in RaspberryPi/Python/WebSocketScripts to send and receive messages from another source.

## Credits
Uses https://github.com/sta/websocket-sharp for c# WebSocket connectivity in Unity - pulled in Dec 2017

Python WebSocket client code uses code derived from https://github.com/ilkerkesen/tornado-websocket-client-example
