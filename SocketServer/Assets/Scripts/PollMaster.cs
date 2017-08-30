using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollMaster : MonoBehaviour {

	public static PollMaster singleton;

	public float m_pollLength;
	public List<string> m_options;

	public int[] m_voteCounts { get; private set; }
	private List<string> m_voters;

	public bool m_isPolling { get; private set; }
	public float m_pollTimer { get; private set; }

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
			if (m_pollTimer <= 0) {
				UIManager.singleton.StopVote ();
				StopVote ();
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
					break;
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
	public void StopVote(){
		m_pollTimer = 0;
		m_isPolling = false;
		LogResults ();

	}
	public void LogResults(){
		for (int i = 0; i < m_options.Count; i++) {
			Debug.Log (m_options [i] + ": " + m_voteCounts [i]);
		}
	}
}
