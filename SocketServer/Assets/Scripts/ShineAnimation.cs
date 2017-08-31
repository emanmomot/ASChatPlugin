using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ShineAnimation : MonoBehaviour {

	public float someValue = 0.0f;
	public float speed;
	public float frequency;
	public float start;
	public float end;

	public void Start () {
		TweenShine ();
	}

	public void TweenedSomeValue(float val){
		gameObject.GetComponent<ShineEffector> ().YOffset = val;
		someValue = val;
	}

	public void TweenShine()
	{
		Hashtable param = new Hashtable();
		param.Add ("from", start); //-0.4f);
		param.Add("to", end); //0.38f);
		param.Add("time", speed);
		param.Add("easeType", iTween.EaseType.easeInOutCirc);
		param.Add("onupdate", "TweenedSomeValue");
		iTween.ValueTo(gameObject, param);
		StartCoroutine(WaitAndPrint(frequency));
	}
	IEnumerator WaitAndPrint(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		TweenShine ();
	}
}

