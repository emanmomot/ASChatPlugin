using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteBanner : MonoBehaviour {

	public Text m_usernameField;
	public Text m_voteField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetText(string username, string vote, Color color) {
		m_usernameField.text = username;
		m_usernameField.color = color;
		m_voteField.text = "voted " + vote;
	}
}
