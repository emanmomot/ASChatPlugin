using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {

	public static Timer singleton;

	public InputField inputField;
	public GameObject timerCircle;

	private int initTime;
	private Image fillImg;
	private float time;

	void Awake() {
		singleton = this;
	}

	// Use this for initialization
	void Start () {
		fillImg = timerCircle.GetComponent<Image> ();
		initTime = int.Parse (inputField.text);

		inputField.onValueChanged.AddListener (delegate {
			OnTimerValueChanged ();
		});
	}
	
	// Update is called once per frame
	void StartTimer () {
		initTime = int.Parse (inputField.text);
		time = initTime;
		inputField.enabled = false;

		foreach (OptionScript option in OptionScript.optionList) {
			option.mainInputField.enabled = false;
		}

		AddOption.singleton.gameObject.SetActive (false);
		PollHeader.singleton.inputField.enabled = false;
		PollMaster.singleton.StartVote ();
	}

	void StopTimer() {
		inputField.enabled = true;

		foreach (OptionScript option in OptionScript.optionList) {
			option.mainInputField.enabled = true;
		}

		AddOption.singleton.gameObject.SetActive (true);
		PollHeader.singleton.inputField.enabled = true;
		PollMaster.singleton.StopVote ();
	}

	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Return) && time <= 0) {
			StartTimer ();
		}

		if (time > 0) {
			time -= Time.deltaTime;
			if (time <= 0) {
				StopTimer ();
				time = 0;
			}
			fillImg.fillAmount = time / initTime;
			inputField.text = Mathf.Round (time).ToString (); 

		}

		//timerCircle.GetComponent<Image> ().fillAmount = Mathf.Lerp (1, 0, Time.time / initTime);
	}

	public void SetTimerLength(int time) {
		initTime = time;
		inputField.text = time.ToString ();
	}

	public int GetTimerLength() {
		return initTime;
	}

	private void OnTimerValueChanged () {
		if (time <= 0) {
			initTime = int.Parse (inputField.text);
		}
	}
}
