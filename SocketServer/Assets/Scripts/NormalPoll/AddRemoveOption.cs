﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddRemoveOption : MonoBehaviour {

	public static AddRemoveOption singleton;
	public Color[] col;

	public Transform optionsTransform;
	public Transform addButtonTransform;
	public GameObject optionPrefab;
	public GameObject removeButton;
	public RectTransform panelBorder, panelMain;

	private int baseColorInd;
	private int colorInd;

	void Awake() {
		singleton = this;
	}

	// Use this for initialization
	void Start () {
		removeButton.SetActive (false);
		baseColorInd = Random.Range (0, col.Length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddNewOption() {
		GameObject newOption = GameObject.Instantiate (optionPrefab, optionsTransform, false) as GameObject;
		OptionScript opScript = newOption.GetComponent<OptionScript> ();

		// assign random base color for new poll
		if (OptionScript.optionList.Count == 0) {
			baseColorInd = Random.Range (0, col.Length);
			colorInd = 0;
		}

		panelBorder.sizeDelta = new Vector2 (panelBorder.sizeDelta.x, panelBorder.sizeDelta.y + 30);
		panelMain.sizeDelta = new Vector2 (panelMain.sizeDelta.x, panelMain.sizeDelta.y + 30);

		opScript.SetColor (col[(baseColorInd + colorInd) % col.Length]);
		colorInd++;

		addButtonTransform.SetAsLastSibling ();
		if (OptionScript.optionList.Count > 0) {
			removeButton.gameObject.SetActive (true);
		}

	}

	public void RemoveOption(){
		if (OptionScript.optionList.Count > 0) {
			GameObject.Destroy (OptionScript.optionList [OptionScript.optionList.Count - 1].gameObject);
			OptionScript.optionList.RemoveAt (OptionScript.optionList.Count - 1);
			panelBorder.sizeDelta = new Vector2 (panelBorder.sizeDelta.x, panelBorder.sizeDelta.y - 30);
			panelMain.sizeDelta = new Vector2 (panelMain.sizeDelta.x, panelMain.sizeDelta.y - 30);
		}
		if (OptionScript.optionList.Count <= 1) {
			removeButton.gameObject.SetActive (false);
		}
	}
}
