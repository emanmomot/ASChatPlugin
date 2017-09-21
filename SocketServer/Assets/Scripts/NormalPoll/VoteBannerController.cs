using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteBannerController : MonoBehaviour {

	public static VoteBannerController singleton;

	public Color[] m_voteColors;
	public GameObject m_bannerPrefab;
	public Transform m_bannerParent;
	public GameObject m_showBannersButton;
	public GameObject m_hideBannersButton;


	private List<RectTransform> m_banners;
	private int m_colorInd;


	void Awake() {
		singleton = this;
		m_banners = new List<RectTransform> ();
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_bannerParent.childCount; i++) {
			RectTransform t = m_bannerParent.GetChild (i) as RectTransform;
			m_banners.Add (t);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HideBanners() {
		m_bannerParent.gameObject.SetActive (false);
		m_hideBannersButton.SetActive (false);
		m_showBannersButton.SetActive (true);
	}

	public void ShowBanners() {
		m_hideBannersButton.SetActive (true);
		m_showBannersButton.SetActive (false);
		m_bannerParent.gameObject.SetActive (true);
	}

	public void ShowNewBanner(string username, string vote) {
		foreach (RectTransform banner in m_banners) {
			banner.anchoredPosition3D += new Vector3 (0, 43, 0);
		}

		GameObject newBanner = GameObject.Instantiate (m_bannerPrefab, m_bannerParent, false) as GameObject;
		newBanner.GetComponent<VoteBanner> ().SetText (username, vote, m_voteColors [m_colorInd]);

		m_colorInd = (m_colorInd + 1) % m_voteColors.Length;
		m_banners.Add (newBanner.transform as RectTransform);
	}

	public void ClearBanners() {
		while (m_banners.Count > 0) {
			GameObject.Destroy (m_banners [0].gameObject);
			m_banners.RemoveAt (0);
		}
	}
}
