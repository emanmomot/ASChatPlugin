using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOption : MonoBehaviour {

	public static AddOption singleton;

	public Transform optionsTransform;
	public Transform addButtonTransform;
	public GameObject optionPrefab;

	void Awake() {
		singleton = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddNewOption() {
		GameObject newOption = GameObject.Instantiate (optionPrefab, optionsTransform, false) as GameObject;
		OptionScript opScript = newOption.GetComponent<OptionScript> ();
		addButtonTransform.SetAsLastSibling ();
	}
}
