using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

	LevelLoader lvlLoader;
	[HideInInspector]public GameText gText;
	Grid grid;
	Requirements reqs;
	UI ui;
	LevelSelect lvlSelect;
	AdManager admanager;
	SoundManager sm;

	public bool isCurrComplete = false;

	public int currLevel;
	int levelCount;

	void Awake () {
		lvlLoader = GameObject.FindObjectOfType<LevelLoader> ();
		gText = GameObject.FindObjectOfType<GameText> ();
		grid = GameObject.FindObjectOfType<Grid> ();
		reqs = GameObject.FindObjectOfType<Requirements> ();
		ui = GameObject.FindObjectOfType<UI>();
		lvlSelect = GameObject.FindObjectOfType<LevelSelect> ();
		levelCount = lvlLoader.LevelCount ();
		admanager = GameObject.FindObjectOfType<AdManager> ();
		sm = GameObject.FindObjectOfType<SoundManager> ();
	}

	void Start(){
		int lastLoadedLVL = PlayerPrefs.GetInt ("LastLoadedLVL", 0);
		if (lastLoadedLVL != 0)
			LoadLevel (lastLoadedLVL);
	}

	public void CompleteLevel(){
		if (!isCurrComplete) {
			gText.SetText ("level complete");
			PlayerPrefs.SetInt (currLevel.ToString (), 1);
			PlayerPrefs.Save ();
			isCurrComplete = true;
			lvlSelect.UpdateLVLSelection ();
			admanager.CheckAdShow ();
			sm.Play (SoundManager.Type.doublebeep2);
		}
	}

	public void LoadLevel(int levelNum){
		currLevel = levelNum;
		string[] levelArr = lvlLoader.LoadLevel (levelNum);
		grid.LoadGrid (levelArr [0], true);
		reqs.LoadRequirements (levelArr [1]);
		ui.SetLevelNum (levelNum);

		if (PlayerPrefs.GetInt (levelNum.ToString (), 0) == 1) {
			gText.SetTextNoAnim ("level complete");
			isCurrComplete = true;
		} else {
			gText.SetTextNoAnim ("");
			isCurrComplete = false;
		}
		PlayerPrefs.SetInt ("LastLoadedLVL", currLevel);
		PlayerPrefs.Save ();
	}

	public void UseRequirement(int length){
		reqs.SubtractRequirement (length);
	}

	public void Retry(){
		grid.LoadCurrentGrid ();
		reqs.LoadCurrentRequirements ();
		sm.Play (SoundManager.Type.shortbeep);
	}

	public void LoadNextLevel(){
		if(PlayerPrefs.GetInt(currLevel.ToString(), 0) == 1 && currLevel < levelCount)
			LoadLevel (currLevel + 1);
		sm.Play (SoundManager.Type.shortbeep);
	}

	public bool AreRequirementsMet(){
		return reqs.AreRequirementsMet ();
	}
}
