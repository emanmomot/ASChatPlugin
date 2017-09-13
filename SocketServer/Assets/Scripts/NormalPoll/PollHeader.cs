using System;
using UnityEngine;
using UnityEngine.UI;

public class PollHeader : MonoBehaviour {

	public static PollHeader singleton;

	public InputField inputField;

	void Awake() {
		singleton = this;
	}

	void Start() {

	}
}

