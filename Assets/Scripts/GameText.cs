using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameText : MonoBehaviour {

	Text gameText;
	Vector3 oriScale;
	void Awake () {
		gameText = GameObject.Find ("GameText").GetComponent<Text> ();

		oriScale = gameText.transform.localScale;
		ClearText ();
	}

	public void SetTextNoAnim(string text){
		gameText.text = text;
	}

	public void SetText(string text){
		float textZoom = 1.05f, textTime = 1.5f;
		StartCoroutine (SetText (textZoom, textTime, text));
	}

	IEnumerator SetText(float textZoom, float textTime, string text){
		gameText.text = text;
		gameText.transform.localScale = textZoom * oriScale;
		while (gameText.transform.localScale.x > oriScale.x) {
			gameText.transform.localScale = Vector3.Lerp (gameText.transform.localScale, oriScale, Time.deltaTime / textTime);
			if (gameText.transform.localScale.x - .0025f < oriScale.x)
				gameText.transform.localScale = oriScale;
			yield return null;
		}
	}

	void ClearText(){
		StopAllCoroutines ();
		Color c = gameText.color;
		c.a = 1;
		gameText.color = c;
		gameText.text = "";
		gameText.transform.localScale = oriScale;
	}
}
