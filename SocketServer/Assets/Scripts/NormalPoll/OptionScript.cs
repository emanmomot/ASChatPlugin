using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionScript : MonoBehaviour {

	public static List<OptionScript> optionList = new List<OptionScript> ();

	public Text secondaryTextField;
	public Text percentageText;
	public RectTransform bar;
	public Image barImage;
	public InputField mainInputField;

	public string player;

	public Button revealPlayerButton;

	private float maxWidth;

	void Awake() {
		maxWidth = bar.rect.width;
	}

	// Use this for initialization
	void Start () {
		optionList.Add (this);
		if (optionList.Count > 1) {
			EventSystem.current.SetSelectedGameObject (mainInputField.gameObject);
			mainInputField.OnPointerClick (new PointerEventData (EventSystem.current));
		} else {
			Navigation customNav = new Navigation();
			customNav.mode = Navigation.Mode.Explicit;
			customNav.selectOnDown = mainInputField;
			PollHeader.singleton.inputField.navigation = customNav;
		}
	}
	
	// Update is called once per frame
	void Update () {
		secondaryTextField.text = mainInputField.text;

	}

	public void SetBarPercentage(float percentage) {
		bar.sizeDelta = new Vector2 (percentage * maxWidth, bar.sizeDelta.y);
		percentageText.text = ((int)(percentage * 100)).ToString()+ "%";
	}

	public void LockInOption() {
		mainInputField.text = mainInputField.text.Trim ();
		mainInputField.enabled = false;
	}

	public void UnlockOption() {
		mainInputField.enabled = true;
	}

	public void SetColor(Color color) {
		barImage.color = color;
		percentageText.color = color;
	}

	public string GetKey() {
		return mainInputField.text;
	}

	public void ActivateRevealPlayerButton () {
		revealPlayerButton.gameObject.SetActive (true);
	}

	public void RevealPlayer() {
		string voter = ((PollMaster)MessageReciever.singleton).m_firstVoter [optionList.IndexOf (this)];
		if (string.IsNullOrEmpty (voter)) {
			voter = "No votes :(";
		} else {
			voter = "First vote by " + voter;
		}

		mainInputField.text = voter;
		revealPlayerButton.gameObject.SetActive (false);
	}
}
