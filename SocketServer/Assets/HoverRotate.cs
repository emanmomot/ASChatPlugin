using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverRotate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool pointerIn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (pointerIn) {
			transform.RotateAround (Vector3.forward, -.03f);
			//iTween.RotateBy (gameObject, iTween.Hash ("z", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		pointerIn = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		pointerIn = false;
	}
}
