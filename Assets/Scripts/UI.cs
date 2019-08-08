using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	
	Text levelNumText;

	Button retryB;

	public Color activeCol, disabledCol;

	void Awake () {
		levelNumText = GameObject.Find ("LevelNum").GetComponent<Text> ();
		retryB = GameObject.Find ("Retry").GetComponent<Button> ();
	}

	void Start(){
		levelNumText.color = disabledCol;
		levelNumText.text = "#";
		retryB.interactable = false;
	}

	public void SetLevelNum(int num){
		levelNumText.color = activeCol;
		levelNumText.text = "#" + num.ToString ();
		retryB.interactable = true;
	}

	public void ExitApp(){
		Application.Quit ();
	}
}
