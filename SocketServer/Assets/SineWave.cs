using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour {
	public int numberOfDots = 28;
	public GameObject[] waveDots;
	public float factor;
	public float amplitude;

	void Update() {
		for (int i = 0; i < numberOfDots; i++) {
			Vector3 position = waveDots [i].transform.localPosition;
			position.y = Mathf.Sin (Time.time + i * factor) * amplitude;
			waveDots [i].transform.localPosition = position;
		}
	}
}
