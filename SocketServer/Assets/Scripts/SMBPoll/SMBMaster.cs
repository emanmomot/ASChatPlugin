using UnityEngine;
using System.Collections.Generic;

namespace SMBPoll {
	public class SMBMaster : MessageReciever { 
		
		public float m_averageVote;

		private List<string> m_voters;
		private bool m_isPolling;

		void Awake () {
			MessageReciever.singleton = this;
		}

		void Start() {
			m_voters = new List<string> ();
			StartVote ();
		}

		public override void RecieveMessage (Message message) {
			if (m_isPolling) {
				if (!m_voters.Contains (message.username)) {
					for (int i = 0; i < UIManager.singleton.m_emojis.Length; i++) {
						if(message.text.Contains(UIManager.singleton.m_emojis[i].m_key)){
							m_voters.Add (message.username);
							m_averageVote = (m_averageVote * (m_voters.Count - 1) + i) / m_voters.Count;
							UIManager.singleton.DisplayNewAvg (m_averageVote);
							UIManager.singleton.DisplayNewVoteCount (m_voters.Count);
							return;
						}
					}
				}
			}
		}

		public void StartVote () {
			m_isPolling = true;
			m_voters.Clear ();
			m_averageVote = 0;
		}

		public void StopVote () {
			m_isPolling = false;
		}
	}
}

