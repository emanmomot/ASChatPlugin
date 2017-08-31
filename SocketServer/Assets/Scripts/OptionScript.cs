using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour {

	public static List<OptionScript> optionList = new List<OptionScript> ();

	public Text secondaryTextField;
	public Text percentageText;
	public RectTransform bar;
	public Image barImage;
	public InputField mainInputField;

	private float maxWidth;

	void Awake() {
		maxWidth = bar.rect.width;
	}

	// Use this for initialization
	void Start () {
		optionList.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
		secondaryTextField.text = mainInputField.text;

	}

	public void SetBarPercentage(float percentage) {
		bar.sizeDelta = new Vector2 (percentage * maxWidth, bar.sizeDelta.y);
		percentageText.text = ((int)(percentage * 100)).ToString()+ "%";
	}

	public void SetColor(Color color) {
		barImage.color = color;
		percentageText.color = color;
	}

	public string GetKey() {
		return mainInputField.text;
	}
}
