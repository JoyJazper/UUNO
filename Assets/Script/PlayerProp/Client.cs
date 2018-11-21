using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;


public class Client : MonoBehaviour {

	public static Client Instance { set; get;}

	public bool socketReady;
	public TcpClient socket;
	public NetworkStream stream;
	public StreamWriter Writer;
	public StreamReader Reader;

	public bool Token;

	public bool IsHost = false;

	private GameObject ForegroundPanel1;
	private GameObject BackgroundPanel1;

	private List<GameClient> players = new List<GameClient>();

	public string ClientName;
	public int ClientID;

	void Start()
	{
		Instance = this;
		DontDestroyOnLoad (this);
	}
	//Checks for the data every frame.
	private void Update()
	{
		if (socketReady) {
			if (stream.DataAvailable) {
				string data = Reader.ReadLine ();
				if (data != null)
					OnIncomingData (data);
			}
		}
	}

	//checks the connection
	public bool ConnectToServer(string Host, int port)
	{
		if (socketReady) {
			return false;
		}
		try{
			socket = new TcpClient(Host, port);
			stream = socket.GetStream();
			Writer = new StreamWriter(stream);
			Reader = new StreamReader(stream);

			socketReady = true;
		}
		catch (Exception e) {
			Debug.Log ("Socket Error : " + e.Message);
		}

		return socketReady;
	}
	//Sending messages to the server.
	public void Send(string data)
	{
		if (!socketReady)
			return;

		Writer.WriteLine (data);
		Writer.Flush ();
		Debug.Log (data + "(2)");
	}

	//Read Messages from the server.
	private void OnIncomingData(String data)
	{
		Debug.Log("Client:" + data);
		string[] aData = data.Split ('|');
		switch (aData [0]) {
			case "SWHO":
				for (int i = 1; i < aData.Length - 1; i++) {
					UserConnected (aData [i], false);
				}
				Send ("CWHO|" + ClientName + "|" + ((IsHost)?1:0).ToString());
				break;

			case "SCNN":
				UserConnected (aData [1], false);
			if (aData [1] == ClientName && IsHost == false) {
					GameManager.Control.ForegroundPanel.SetActive (false);
					GameManager.Control.BackgroundPanel.SetActive (true);
				}
				break;

			case "SMDT|":
				GameManager.Control.CurrentCard[0] = aData[1];
				GameManager.Control.CurrentCard[1] = aData[2];
				Debug.Log ("CurrentCard Updated from the Server on Clients.");
				break;

		case "STOK":
			if (aData [1] == ClientID.ToString ()) {
				GameManager.Control.token = true;
				Debug.Log ("Token Updated from the Server on Clients.");
			} else {
				Debug.Log("Token Updated But Not Changed!");
			}
				break;
		}
	}

	private void UserConnected(string name,bool host)
	{
		GameClient c = new GameClient ();
		c.name = name;

		players.Add (c);
	}

	//Closing the Socket when not required
	private void CloseSocket()
	{
		if (!socketReady)
			return;

		Writer.Close ();
		Reader.Close ();
		socket.Close ();
		socketReady = false;
	}

	//Other ending checks
	private void OnApplicationQuit()
	{
		CloseSocket();
	}

	private void OnDisable()
	{
		CloseSocket();
	}
	//end ending check
}

public class GameClient
{
	public string name;
	public string SNo;
	public bool isHost;
}