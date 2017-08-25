using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

public class WSServer : MonoBehaviour {

	public static WSServer singleton;

	const string ip = "127.0.0.1";
	const short port = 7777;

	TcpListener server;
	NetworkStream stream;
	TcpClient client;

	bool waitingOnConnection;

	const float recMsgTimeout = 1.0f;
	float recMsgTimer;

	void Awake() {
		singleton = this;
	}

	// Use this for initialization
	void Start () {
		server = new TcpListener(IPAddress.Parse(ip), port);

		server.Start();
		Debug.Log("Server has started on " + ip + ":" + port + " Waiting for a connection...");

		waitingOnConnection = true;
		server.BeginAcceptTcpClient (new AsyncCallback (DoAcceptTcpClientCallback), server);
	}
	
	// Update is called once per frame
	void Update () {
		if (waitingOnConnection) {
			return;
		}

		if (!client.Connected) {
			ResetConnection ();
			return;
		}

		// poll to see if still connected
		if (recMsgTimer > recMsgTimeout) {
			recMsgTimer = 0;

			// Detect if client disconnected
			if( client.Client.Poll( 0, SelectMode.SelectRead ) )
			{
				byte[] buff = new byte[1];
				if (client.Client.Receive (buff, SocketFlags.Peek) == 0) {
					// Client disconnected
					ResetConnection ();
					return;
				}
			}
		}

		Byte[] bytes = new Byte[client.Available];
		if (bytes.Length == 0) {
			// count length of time where we haven't recieved a message
			recMsgTimer += Time.deltaTime;

			return;
		} else {
			recMsgTimer = 0;
		}

		stream.Read(bytes, 0, bytes.Length);

		//translate bytes of request to string
		String data = Encoding.UTF8.GetString(bytes);

		if (new Regex ("^GET").IsMatch (data)) {
			// upgrade connection to websocket
			Byte[] response = Encoding.UTF8.GetBytes ("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
			                  + "Connection: Upgrade" + Environment.NewLine
			                  + "Upgrade: websocket" + Environment.NewLine
			                  + "Sec-WebSocket-Accept: " + Convert.ToBase64String (
				                  SHA1.Create ().ComputeHash (
					                  Encoding.UTF8.GetBytes (
						                  new Regex ("Sec-WebSocket-Key: (.*)").Match (data).Groups [1].Value.Trim () + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
					                  )
				                  )
			                  ) + Environment.NewLine
			                  + Environment.NewLine);

			stream.Write (response, 0, response.Length);
		} else if (bytes.Length == 6 && bytes [0] == 0x88 && bytes [1] == 0x80) {
			// close message
			ResetConnection();
		} else {
			// normal message
			String newMessage = GetDecodedData (bytes, bytes.Length);
			try {
				Message message = JsonToMessage (newMessage);
				PollMaster.singleton.RecieveMessage (message);
			} catch(ArgumentException){
				Debug.Log ("Something Not JSON Sent");
			}

		}
	}

	private void ResetConnection() {
		client = null;

		waitingOnConnection = true;
		server.BeginAcceptTcpClient (new AsyncCallback (DoAcceptTcpClientCallback), server);

		Debug.Log ("Client disconnected. Waiting for new connection");
	}

	public static string GetDecodedData(byte[] buffer, int length)
	{
		byte b = buffer[1];
		int dataLength = 0;
		int totalLength = 0;
		int keyIndex = 0;

		if (b - 128 <= 125)
		{
			dataLength = b - 128;
			keyIndex = 2;
			totalLength = dataLength + 6;
		}

		if (b - 128 == 126)
		{
			dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
			keyIndex = 4;
			totalLength = dataLength + 8;
		}

		if (b - 128 == 127)
		{
			dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
			keyIndex = 10;
			totalLength = dataLength + 14;
		}

		if (totalLength > length)
			throw new Exception("The buffer length is small than the data length");

		byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

		int dataIndex = keyIndex + 4;
		int count = 0;
		for (int i = dataIndex; i < totalLength; i++)
		{
			buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
			count++;
		}

		return Encoding.ASCII.GetString(buffer, dataIndex, dataLength);
	}

	public void DoAcceptTcpClientCallback(IAsyncResult ar) 
	{
		// Get the listener that handles the client request.
		TcpListener listener = (TcpListener) ar.AsyncState;

		// End the operation and display the received data on 
		// the console.
		client = listener.EndAcceptTcpClient(ar);

		stream = client.GetStream();

		// Process the connection here. (Add the client to a
		// server table, read data, etc.)
		Debug.Log("Client connected completed");
		waitingOnConnection = false;

	}

	public Message JsonToMessage(String message){
		return JsonUtility.FromJson<Message> (message);
	}

	public bool IsConnected() {
		return !waitingOnConnection;
	}
}
