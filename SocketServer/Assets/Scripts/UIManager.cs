using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager singleton;

	public Text m_connectionStatus;

	void Awake() {
		singleton = this;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		UpdateConnectionStatus (WSServer.singleton.IsConnected ());
	}

	public void PollButtonClicked() {
	}


	public void UpdateConnectionStatus(bool connected) {
		if (connected) {
			m_connectionStatus.text = "Connected to AS Chat";
			m_connectionStatus.color = Color.yellow;
		} else {
			m_connectionStatus.text = "Waiting for connection...";
			m_connectionStatus.color = Color.red;
		}
	}
}
