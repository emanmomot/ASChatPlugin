using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

	public enum PollState {
		setup,
		polling,
		results
	}

	public static UIManager singleton;

	public Text m_connectionStatus;

	public GameObject m_instructions;

	public GameObject m_lockButton;


	void Awake() {
		singleton = this;
	}

	void Start () {
		EnterSetupState ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateConnectionStatus (WSServer.singleton.IsConnected ());

		if (Input.GetKeyUp (KeyCode.R) && !m_lockButton.activeSelf) {
			EnterSetupState ();
		} else if (Input.GetKeyDown (KeyCode.Return) && m_lockButton.activeSelf) {
			LockOptions ();
		} else if (Input.GetKeyDown (KeyCode.Return) && !m_lockButton.activeSelf) {
			Timer.singleton.StartTimer ();
		}
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
		m_lockButton.SetActive(true);

		VoteBannerController.singleton.m_hideBannersButton.SetActive (true);

		Timer.singleton.ResetTimer ();

		while (OptionScript.optionList.Count > 0) {
			AddRemoveOption.singleton.RemoveOption ();
		}

		VoteBannerController.singleton.ClearBanners ();

		AddRemoveOption.singleton.AddNewOption ();

		UnlockOptions ();

		PollHeader.singleton.inputField.text = "";

		EventSystem.current.SetSelectedGameObject (Timer.singleton.inputField.gameObject);
		Timer.singleton.inputField.OnPointerClick (new PointerEventData (EventSystem.current));
	}

	public void EnterPollingState() {
		LockOptions ();
		m_lockButton.SetActive (false);
		((PollMaster)MessageReciever.singleton).StartVote ();
	}

	public void EnterResultsState() {
		((PollMaster)MessageReciever.singleton).StopVote ();
		foreach (OptionScript option in OptionScript.optionList) {
			option.ActivateRevealPlayerButton ();
		}
	}

	public void LockOptions() {
		m_lockButton.SetActive (false);

		Timer.singleton.inputField.enabled = false;

		foreach (OptionScript option in OptionScript.optionList) {
			option.LockInOption ();
		}

		AddRemoveOption.singleton.gameObject.SetActive (false);
		PollHeader.singleton.inputField.enabled = false;

		VoteBannerController.singleton.m_showBannersButton.SetActive (false);
		VoteBannerController.singleton.m_hideBannersButton.SetActive (false);

		m_instructions.SetActive (false);
	}

	public void UnlockOptions() {
		AddRemoveOption.singleton.gameObject.SetActive (true);
		Timer.singleton.inputField.enabled = true;
		PollHeader.singleton.inputField.enabled = true;

		foreach (OptionScript option in OptionScript.optionList) {
			option.UnlockOption ();
		}
	}
}
