using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOption : MonoBehaviour {

	public static AddOption singleton;
	public Color[] col;

	public Transform optionsTransform;
	public Transform addButtonTransform;
	public GameObject optionPrefab;
	public RectTransform panelBorder, panelMain;

	int randomColor;
	int lastRandom = 0;

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

		randomColor = Random.Range (0, col.Length);

		if (lastRandom == randomColor) {
			while (lastRandom == randomColor) {
				randomColor = Random.Range (0, col.Length);
			}
			lastRandom = randomColor;
		} 
		lastRandom = randomColor;

		panelBorder.sizeDelta = new Vector2 (panelBorder.sizeDelta.x, panelBorder.sizeDelta.y + 30);
		panelMain.sizeDelta = new Vector2 (panelMain.sizeDelta.x, panelMain.sizeDelta.y + 30);

		opScript.SetColor (col[randomColor]);

		addButtonTransform.SetAsLastSibling ();
	}
}
