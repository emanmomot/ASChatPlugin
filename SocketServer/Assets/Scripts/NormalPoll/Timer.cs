﻿using System.Collections;
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
	public void StartTimer () {
		if (!int.TryParse (inputField.text, out initTime)) {
			inputField.text = "15";
			initTime = 15;
		}

		time = initTime;

		UIManager.singleton.EnterPollingState ();
	}

	void StopTimer() {
		UIManager.singleton.EnterResultsState ();
	}

	void Update () {
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
		time = 0;
	}

	private void OnTimerValueChanged () {
		
		if (time <= 0) {
			if (!int.TryParse (inputField.text, out initTime)) {
				initTime = 0;
			}
		}
	}

	public bool IsRunning() {
		return time > 0;
	}
}
