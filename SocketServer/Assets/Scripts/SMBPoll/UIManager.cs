using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SMBPoll {
	public class UIManager : MonoBehaviour {

		public static UIManager singleton;

		public Emoji[] m_emojis;
		public Image m_avgVote;

		public RectTransform m_scoreLineLeft;
		public RectTransform m_scoreLineRight;

		private float m_scoreLineLength;
		private RectTransform m_avgVoteTransform;

		void Awake() {
			singleton = this;
		}

		float avg;
		// Use this for initialization
		void Start () {
			m_avgVoteTransform = m_avgVote.transform as RectTransform;
			m_scoreLineLength = m_scoreLineRight.anchoredPosition.x - m_scoreLineLeft.anchoredPosition.x;
		}


		// Update is called once per frame
		void Update () {
			avg += .01f;
			if (avg > 8) {
				avg = 0;
			}
			DisplayNewAvg (avg);
		}

		public void DisplayNewAvg(float avg) {
			Vector2 offset = new Vector2 ((avg / (m_emojis.Length-1)) * m_scoreLineLength, 0);
			m_avgVoteTransform.anchoredPosition = m_scoreLineLeft.anchoredPosition + offset;

			m_avgVote.sprite = m_emojis [Mathf.FloorToInt (avg)].m_tex;
		}

		public void StartVote() {
			((SMBMaster)MessageReciever.singleton).StartVote ();
			DisplayNewAvg (0);
		}

		public void EndVote() {
			((SMBMaster)MessageReciever.singleton).StopVote ();
		}
	}
}
