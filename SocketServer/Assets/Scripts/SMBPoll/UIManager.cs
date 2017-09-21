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

		public Text m_voteCount;

		private float m_scoreLineLength;
		public Transform m_avgVoteTransform;

		void Awake() {
			singleton = this;
		}

		//float avg;
		// Use this for initialization
		void Start () {
			m_scoreLineLength = m_scoreLineRight.anchoredPosition.x - m_scoreLineLeft.anchoredPosition.x;
		}


		// Update is called once per frame
		void Update () {
		}

		public void DisplayNewAvg(float avg) {
			//Vector2 offset = new Vector2 ((avg / (m_emojis.Length-2)) * m_scoreLineLength, 0);
			//m_avgVoteTransform.localScale.x =
			iTween.ScaleTo(m_avgVoteTransform.gameObject,iTween.Hash("x", avg/9.0f, "easeType", "easeOutCirc","time",1.5f));
			m_avgVote.sprite = m_emojis [Mathf.FloorToInt (avg)].m_tex;
		}

		public void DisplayNewVoteCount(int count) {
			m_voteCount.text = count.ToString();
		}

		public void StartVote() {
			((SMBMaster)MessageReciever.singleton).StartVote ();
			DisplayNewVoteCount (0);
			DisplayNewAvg (0);
		}

		public void EndVote() {
			((SMBMaster)MessageReciever.singleton).StopVote ();
		}
	}
}
