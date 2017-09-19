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

	public GameObject m_lockButton;
	public GameObject m_unlockButton;
	public GameObject m_instructions;
	public GameObject m_instructionsButton;
	public GameObject m_bannerButton;
	public GameObject m_bannerButtonOff;
	public GameObject m_connectionText;
	public GameObject[] m_pollInstructions;

	private PollState m_pollState;

	private bool m_isLocked;
	bool instructionsOn = false;

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
		m_unlockButton.SetActive (true);

		Timer.singleton.ResetTimer ();

		while (OptionScript.optionList.Count > 0) {
			AddRemoveOption.singleton.RemoveOption ();
		}

		VoteBannerController.singleton.ClearBanners ();

		AddRemoveOption.singleton.AddNewOption ();

		UnlockOptions ();

		PollHeader.singleton.inputField.text = "";

	}

	public void EnterPollingState() {
		LockOptions ();
		m_lockButton.SetActive (false);
		m_unlockButton.SetActive (false);
		((PollMaster)MessageReciever.singleton).StartVote ();
	}

	public void EnterResultsState() {
		((PollMaster)MessageReciever.singleton).StopVote ();
		m_resetButton.gameObject.SetActive (true);
	}

	public void LockButtonClick() {
		if (m_isLocked) {
			UnlockOptions ();
			m_lockButton.SetActive (false);
			m_unlockButton.SetActive (true);
			m_instructionsButton.SetActive (true);
			m_connectionText.SetActive (true);
			m_instructions.SetActive (true);

		} else {
			LockOptions ();
			m_lockButton.SetActive (true);
			m_unlockButton.SetActive (false);
			m_instructions.SetActive (false);
			m_bannerButton.SetActive (false);
			m_bannerButtonOff.SetActive (false);
			m_instructionsButton.SetActive (false);
			m_connectionText.SetActive (false);
			for(int i = 0; i<m_pollInstructions.Length; i++)
			{
				m_pollInstructions [i].SetActive (false);
			}
			instructionsOn = false;
		}
	}

	public void LockOptions() {
		m_isLocked = true;

		Timer.singleton.inputField.enabled = false;

		foreach (OptionScript option in OptionScript.optionList) {
			option.LockInOption ();
		}

		AddRemoveOption.singleton.gameObject.SetActive (false);
		PollHeader.singleton.inputField.enabled = false;


	}

	public void UnlockOptions() {
		m_isLocked = false;

		AddRemoveOption.singleton.gameObject.SetActive (true);
		Timer.singleton.inputField.enabled = true;
		PollHeader.singleton.inputField.enabled = true;

		foreach (OptionScript option in OptionScript.optionList) {
			option.UnlockOption ();
		}
	}
	public void ShowInstructions(){
		if (instructionsOn == false) {
			iTween.ScaleTo (m_instructions, iTween.Hash (
				"scale", new Vector3 (0.58f, 0.5f, 0.5f),
				"time", 0.2f,
				"easeType", iTween.EaseType.easeOutBack
			));
			for(int i = 0; i<m_pollInstructions.Length; i++)
			{
				m_pollInstructions [i].SetActive (true);
			}
			instructionsOn = true;
		} else if (instructionsOn == true) {
			iTween.ScaleTo (m_instructions, iTween.Hash (
				"scale", new Vector3 (0.0f, 0.5f, 0.5f),
				"time", 0.2f,
				"easeType", iTween.EaseType.easeInBack
			));
			for(int i = 0; i<m_pollInstructions.Length; i++)
			{
				m_pollInstructions [i].SetActive (false);
			}
			instructionsOn = false;
		}
	}
}
