using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollMaster : MessageReciever{


	public int[] m_voteCounts { get; private set; }
	private List<string> m_voters;
	public string[] m_firstVoter;

	public bool m_isPolling { get; private set; }

	private int m_totalVotes;

	void Awake () {
		MessageReciever.singleton = this;
	}

	// Use this for initialization
	void Start () {
		m_voters = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public override void RecieveMessage (Message message) {
		
		if (m_isPolling) {
			// dont vote twice
			if (!m_voters.Contains (message.username)) {
				// trim text
				string voteText = message.text.Trim ();
				for (int i = 0; i < OptionScript.optionList.Count; i++) {
					if (voteText.Contains (OptionScript.optionList [i].GetKey ())) {
						m_voters.Add (message.username);
						m_voteCounts [i]++;
						m_totalVotes++;

						if (string.IsNullOrEmpty (m_firstVoter [i])) {
							m_firstVoter[i] = message.username;
						}
						VoteBannerController.singleton.ShowNewBanner (message.username, OptionScript.optionList [i].GetKey ());

						for (int j = 0; j < OptionScript.optionList.Count; j++) {
							OptionScript.optionList [j].SetBarPercentage (((float)m_voteCounts [j]) / m_totalVotes);
						}

						UIManager.singleton.m_voteCount.text = "votes: " + m_totalVotes;

						Debug.Log ("Vote for " + OptionScript.optionList [i].GetKey () + ", current total: " + m_voteCounts [i]);
						break;
					}
				}
			}
		} else {
			Debug.Log (message.username + ": " + message.text);
			//Debug.Log ("Not Accepting Messages Yet.. Start Poll");
		}
	}

	public void StartVote () {
		m_voters.Clear ();
		m_voteCounts = new int[OptionScript.optionList.Count];
		m_firstVoter = new string[OptionScript.optionList.Count];
		m_totalVotes = 0;

		m_isPolling = true;

		foreach (OptionScript option in OptionScript.optionList) {
			option.SetBarPercentage (0);
		}

		Debug.Log ("Vote started");
	}
	public void StopVote () {
		m_isPolling = false;
		LogResults ();

	}

	public void LogResults () {
		for (int i = 0; i < OptionScript.optionList.Count; i++) {
			Debug.Log (OptionScript.optionList [i].GetKey() + ": " + m_voteCounts [i]);
		}
	}
}
