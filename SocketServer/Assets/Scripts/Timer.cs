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

		UIManager.singleton.EnterPollingState ();
	}

	void StopTimer() {
		UIManager.singleton.EnterResultsState ();
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

	public void ResetTimer() {
		SetTimerLength (15);
		fillImg.fillAmount = 1;
	}

	private void OnTimerValueChanged () {
		if (time <= 0) {
			initTime = int.Parse (inputField.text);
		}
	}

	public bool IsRunning() {
		return time > 0;
	}
}
