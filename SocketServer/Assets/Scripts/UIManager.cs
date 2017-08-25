using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager singleton;

	public Text m_pollStartText;
	public RectTransform pollTimerUI;
	public RectTransform m_voteParent;

	public Text m_connectionStatus;

	public GameObject m_votePrefab;

	void Awake() {
		singleton = this;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PollMaster.singleton.m_isPolling) {
			pollTimerUI.localScale = new Vector3 (1 - PollMaster.singleton.m_pollTimer / PollMaster.singleton.m_pollLength, 1, 1);
		} 

		UpdateConnectionStatus (WSServer.singleton.IsConnected ());
	}

	public void PollButtonClicked(){
		if (PollMaster.singleton.m_isPolling) {
			PollMaster.singleton.StopVote ();
			StopVote ();
		} else {
			PollMaster.singleton.StartVote ();
			StartVote ();
		}
	}

	public void StopVote() {
		pollTimerUI.parent.gameObject.SetActive(false);
		m_pollStartText.text = "Start Vote";

		// create vote labels
		for (int i = 0; i < PollMaster.singleton.m_options.Count; i++) {
			GameObject newVoteLabel = GameObject.Instantiate (m_votePrefab, m_voteParent, false) as GameObject;
			newVoteLabel.GetComponent<Text> ().text = PollMaster.singleton.m_options [i] + 
				": " + PollMaster.singleton.m_voteCounts [i];
			newVoteLabel.GetComponent<RectTransform> ().position += i * new Vector3 (0, 50, 0);
		}
	}

	public void StartVote() {
		pollTimerUI.parent.gameObject.SetActive(true);
		m_pollStartText.text = "Stop Vote";

		// destroy old vote labels
		foreach (Transform child in m_voteParent) {
			GameObject.Destroy(child.gameObject);
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
}
