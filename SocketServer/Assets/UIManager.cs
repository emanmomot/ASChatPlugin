using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	public static UIManager singleton;
	public Text m_pollStartText;
	public RectTransform pollTimerUI;

	void Awake(){
		singleton = this;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PollMaster.singleton.m_isPolling) {
			pollTimerUI.localScale = new Vector3 (1 - PollMaster.singleton.m_pollTimer / PollMaster.singleton.m_pollLength, 1, 1);
		} 
		
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

	public void StopVote(){
		pollTimerUI.parent.gameObject.SetActive(false);
		m_pollStartText.text = "Start Vote";
	}

	public void StartVote(){
		pollTimerUI.parent.gameObject.SetActive(true);
		m_pollStartText.text = "Stop Vote";
	}
}
