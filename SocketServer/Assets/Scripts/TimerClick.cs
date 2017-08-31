using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TimerClick : MonoBehaviour, IPointerClickHandler {
	public GameObject timer;
	public Timer timerScript;
	// Use this for initialization
	void Start () {
		timerScript = timer.GetComponent<Timer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerClick(PointerEventData eventData)
	{

		if (eventData.button == PointerEventData.InputButton.Right) {
			if (timerScript.GetTimerLength() < 60) {
				timerScript.SetTimerLength (timerScript.GetTimerLength () + 15);
				
			} else {
				timerScript.SetTimerLength (15);
			}
		}

	}
}
