using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class Server : MonoBehaviour {

	public int port = 6548;

	private List<ServerClient> Clients;
	private List<ServerClient> DisconnectList;

	private TcpListener server;
	private bool ServerStartted;

	public void init(){
		DontDestroyOnLoad (gameObject);
		Clients = new List<ServerClient> ();
		DisconnectList = new List<ServerClient> ();

		try{
			server = new TcpListener(IPAddress.Any,port);
			server.Start();

			StartListening();
			ServerStartted = true;
		}
		catch(Exception e){
			Debug.Log ("Socket Error: " + e.Message);
		}
	}

	private void Update()
	{
		if (!ServerStartted)
			return;

		foreach (ServerClient c in Clients) {
			// Is client still connected?

			if (!IsConnected (c.tcp)) {
				c.tcp.Close ();
				DisconnectList.Add (c);
				continue;
			} else {
				NetworkStream s = c.tcp.GetStream ();
				if (s.DataAvailable) {
					StreamReader reader = new StreamReader (s, true);
					String data = reader.ReadLine ();

					if (data != null)
						OnIncomingData (c, data);
				}

			}
		}

		for (int i = 0; i < DisconnectList.Count - 1; i++) {
			Clients.Remove (DisconnectList [i]);
			DisconnectList.RemoveAt (i);
		}
	}

	private void StartListening()
	{
		server.BeginAcceptTcpClient (AcceptTcpClient, server);
	}

	private void AcceptTcpClient(IAsyncResult ar)
	{
		TcpListener listener = (TcpListener)ar.AsyncState;

		String allUsers = "";
		foreach(ServerClient i in Clients)
		{
			allUsers += i.ClientName + '|';
		}

		ServerClient sc = new ServerClient (listener.EndAcceptTcpClient (ar));
		Clients.Add (sc);

		StartListening ();


		Broadcast ("SWHO|" + allUsers, Clients [Clients.Count - 1]);

		Debug.Log ("Someone Has Connected!!");
	}

	private bool IsConnected(TcpClient c)
	{
		try{
			if(c != null && c.Client != null && c.Client.Connected)
			{
				if(c.Client.Poll(0,SelectMode.SelectRead))
					return !(c.Client.Receive(new byte[1],SocketFlags.Peek) == 0);

				return true;
			}
			else{
				return false;
			}
		}
		catch{
			return false;
		}
	}

	// Server read
	private void OnIncomingData(ServerClient c,String data)
	{
		Debug.Log("Server:" + data);
		Debug.Log(data);
		string[] aData = data.Split ('|');
		switch (aData [0]) {
		case "CWHO":
			c.ClientName = aData [1];
			c.isHost = (aData [2] == "0") ? false : true;
			Broadcast ("SCNN|" + c.ClientName, Clients);
			break;

		case"CMDT":
			GameManager.Control.CurrentCard [0] = aData [1];
			GameManager.Control.CurrentCard [1] = aData [2];
			TokenChecker (Convert.ToInt32(aData [4]));
			Broadcast ("SMDT|" + aData [1] + "|" + aData [2], Clients);
			Debug.Log ("CurrentCard Updated from the client on server.");
			break;
		}
	}

	#region clientVideo
	//Server Send
	private void Broadcast(String data, List<ServerClient> cl)
	{
		foreach (ServerClient sc in cl) {
			try{
				StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
				writer.WriteLine(data);
				writer.Flush();
			}
			catch(Exception e) {
				Debug.Log ("Write Error : " + e.Message);
			}
		}
	}
	#endregion

	private void Broadcast(String data, ServerClient c)
	{
		List<ServerClient> sc = new List<ServerClient> { c };
		Broadcast (data, sc);
	}

	public void TokenChecker(int ClientId){
		int a;
		Clients [ClientId].TokenOn = false;
		ClientId = ClientId + 1;
		if (ClientId < Clients.Count) {
			Clients [ClientId].TokenOn = true;
			a = ClientId + 1;
		} else {
			Clients [0].TokenOn = true;
			a = 0;
		}

		String TokenMsg = "STOK|";
		TokenMsg += a.ToString();

		Broadcast (TokenMsg, Clients [a]);
		
	}
}

public class ServerClient
{
	public string ClientName;
	public TcpClient tcp;
	public bool isHost;

	public bool TokenOn = false;
	public ServerClient(TcpClient tcp)
	{
		this.tcp = tcp;
	}

}