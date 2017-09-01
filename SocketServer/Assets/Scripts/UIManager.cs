using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public enum PollState {
		setup,
		polling,
		results
	}

	public static UIManager singleton;

	public Text m_connectionStatus;
	public Button m_resetButton;

	private PollState m_pollState;

	void Awake() {
		singleton = this;
	}

	void Start () {
		m_pollState = PollState.setup;
		m_resetButton.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		UpdateConnectionStatus (WSServer.singleton.IsConnected ());
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

	public void EnterSetupState() {
		m_resetButton.gameObject.SetActive (false);
		Timer.singleton.inputField.enabled = true;
		Timer.singleton.ResetTimer ();

		while (OptionScript.optionList.Count > 0) {
			AddRemoveOption.singleton.RemoveOption ();
		}

		AddRemoveOption.singleton.AddNewOption ();

		AddRemoveOption.singleton.gameObject.SetActive (true);

		PollHeader.singleton.inputField.text = "";
		PollHeader.singleton.inputField.enabled = true;
	}

	public void EnterPollingState() {
		Timer.singleton.inputField.enabled = false;

		foreach (OptionScript option in OptionScript.optionList) {
			option.mainInputField.enabled = false;
		}

		AddRemoveOption.singleton.gameObject.SetActive (false);
		PollHeader.singleton.inputField.enabled = false;
		PollMaster.singleton.StartVote ();
	}

	public void EnterResultsState() {
		PollMaster.singleton.StopVote ();
		m_resetButton.gameObject.SetActive (true);
	}
}
