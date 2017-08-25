using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollMaster : MonoBehaviour {

	public static PollMaster singleton;

	public float m_pollLength;
	public List<string> m_options;

	private int[] m_voteCounts;
	private List<string> m_voters;

	private bool m_isPolling;
	private float m_pollTimer;

	void Awake () {
		singleton = this;
	}

	// Use this for initialization
	void Start () {
		m_voters = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isPolling) {
			m_pollTimer -= Time.deltaTime;
			if (m_pollTimer < 0) {
				m_isPolling = false;

				Debug.Log ("Poll has ended");
				for (int i = 0; i < m_options.Count; i++) {
					Debug.Log (m_options [i] + ": " + m_voteCounts [i]);
				}
			}
		}
	}

	public void RecieveMessage (Message message) {
		
		if (m_isPolling) {
			// dont vote twice
			//if (!m_voters.Contains (message.username)) {
				// trim text
				string voteText = message.text.Trim ();
				for (int i = 0; i < m_options.Count; i++) {
					if (voteText.Contains (m_options[i])) {
						m_voters.Add (message.username);
						m_voteCounts [i]++;
						Debug.Log ("Vote for " + m_options [i] + ", current total: " + m_voteCounts[i]);
					}
				}
			//}
		} else {
			Debug.Log (message.username + ": " + message.text);
			//Debug.Log ("Not Accepting Messages Yet.. Start Poll");
		}
	}

	public void StartVote () {
		m_voters.Clear ();
		m_voteCounts = new int[m_options.Count];

		m_isPolling = true;
		m_pollTimer = m_pollLength;

		Debug.Log ("Vote started");
	}
}
