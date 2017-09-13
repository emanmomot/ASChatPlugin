using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.RotateBy( gameObject, iTween.Hash(
			"amount", new Vector3(0f,0f,0.05f),
			"time"    , 2,
			"looptype", "pingPong",
			"easetype", iTween.EaseType.easeInOutSine
		));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
