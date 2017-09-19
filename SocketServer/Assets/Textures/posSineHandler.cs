using UnityEngine;
using System.Collections;

public class posSineHandler : MonoBehaviour {

		Vector3 startPos;
		
	public bool up, down, left, right, forward, back = false;
		public float amplitude = 10f;
		public float period = 5f;

		protected void Start() {
			startPos = transform.localPosition;
		}

		protected void Update() {
			float theta = Time.timeSinceLevelLoad / period;
			float distance = amplitude * Mathf.Sin(theta);
		if (up == true) {
			transform.localPosition = startPos + Vector3.up * distance;
		}
		if (down == true) {
			transform.localPosition = startPos + Vector3.down * distance;
		}
		if (left == true) {
			transform.localPosition = startPos + Vector3.left * distance;
		}
		if (right == true) {
			transform.localPosition = startPos + Vector3.right * distance;
		}
		if (forward == true) {
			transform.localPosition = startPos + Vector3.forward * distance;
		}
		if (back == true) {
			transform.localPosition = startPos + Vector3.back * distance;
		}
		}
	}