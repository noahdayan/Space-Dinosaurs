using UnityEngine;
using System.Collections;

public class Connect : MonoBehaviour {

/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop me a line if you made something exciting! 
*/

	string connectToIP = "127.0.0.1";
	int connectPort = 25001;
	
	public Rect networkArea;
	Rect networkAreaNormalized;
	public Rect status;
	public Rect client;
	public Rect server;
	public Rect ip;
	public Rect port;
	
	// Use this for initialization
	void Start () {
		networkAreaNormalized = new Rect(networkArea.x * Screen.width - (networkArea.width * 0.5f), networkArea.y * Screen.height - (networkArea.height * 0.5f), networkArea.width, networkArea.height);
	}
	
	//Obviously the GUI is for both client&servers (mixed!)
	void OnGUI ()
	{
		GUI.BeginGroup(networkAreaNormalized);
		if(MainMenuGUI.network)
		{
			if (Network.peerType == NetworkPeerType.Disconnected){
			//We are currently disconnected: Not a client or host
				GUI.Label(new Rect(status), "Connection status: Disconnected");
				
				connectToIP = GUI.TextField(new Rect(ip), connectToIP);
				connectPort = int.Parse(GUI.TextField(new Rect(port), connectPort.ToString()));

				if (GUI.Button (new Rect(client), "Connect as client"))
				{
					//Connect to the "connectToIP" and "connectPort" as entered via the GUI
					Network.Connect(connectToIP, connectPort);
				}
				
				if (GUI.Button (new Rect(server), "Start Server"))
				{
					//Start a server for 32 clients using the "connectPort" given via the GUI
					Network.InitializeServer(32, connectPort, false);
				}			
				
			}else{
				//We've got a connection(s)!
				
		
				if (Network.peerType == NetworkPeerType.Connecting){
				
					GUI.Label(new Rect(status), "Connection status: Connecting");
					
				} else if (Network.peerType == NetworkPeerType.Client){
					
					GUI.Label(new Rect(status), "Connection status: Client!");
					GUI.Label(new Rect(ip), "Ping to server: "+Network.GetAveragePing(  Network.connections[0] ) );		
					
				} else if (Network.peerType == NetworkPeerType.Server){
					
					GUI.Label(new Rect(status), "Connection status: Server!");
					GUI.Label(new Rect(ip), "Connections: "+Network.connections.Length);
					if(Network.connections.Length>=1){
						GUI.Label(new Rect(port), "Ping to first player: "+Network.GetAveragePing(  Network.connections[0] ) );
					}			
				}
		
				if (GUI.Button (new Rect(server), "Disconnect"))
				{
					Network.Disconnect(200);
				}
			}
		}
		GUI.EndGroup();
	}
	
	// NONE of the functions below is of any use in this demo, the code below is only used for demonstration.
	// First ensure you understand the code in the OnGUI() function above.
	
	//Client functions called by Unity
	void OnConnectedToServer() {
		Debug.Log("This CLIENT has connected to a server");	
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("This SERVER OR CLIENT has disconnected from a server");
	}
	
	void OnFailedToConnect(NetworkConnectionError error){
		Debug.Log("Could not connect to server: "+ error);
	}
	
	
	//Server functions called by Unity
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from: " + player.ipAddress +":" + player.port);
	}
	
	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player disconnected from: " + player.ipAddress+":" + player.port);
	}
	
	
	// OTHERS:
	// To have a full overview of all network functions called by unity
	// the next four have been added here too, but they can be ignored for now
	
	void OnFailedToConnectToMasterServer(NetworkConnectionError info){
		Debug.Log("Could not connect to master server: "+ info);
	}
	
	void OnNetworkInstantiate (NetworkMessageInfo info) {
		Debug.Log("New object instantiated by " + info.sender);
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		//Custom code here (your code!)
	}

/* 
 The last networking functions that unity calls are the RPC functions.
 As we've added "OnSerializeNetworkView", you can't forget the RPC functions 
 that unity calls..however; those are up to you to implement.
 
 @RPC
 function MyRPCKillMessage(){
	//Looks like I have been killed!
	//Someone send an RPC resulting in this function call
 }
*/

}