using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

	GameplayManager gm;
	SoundManager sm;

	int levelCount;
	GameObject loadLVLButton;
	Transform contents;
	int contentsLength;

	List<Button> lvlLoad = new List<Button>();

	GameObject showButton, hideButton;
	[HideInInspector] public bool showing = true;
	Vector3 hiddenPos, showingPos, toogleButtShowingPos, hideButtShowingPos;

	public Color completedColor;

	void Start () {
		gm = GameObject.FindObjectOfType<GameplayManager> ();
		sm = GameObject.FindObjectOfType<SoundManager> ();
		loadLVLButton = this.transform.Find ("Viewport/Content/Load").gameObject;
		levelCount = GameObject.FindObjectOfType<LevelLoader> ().LevelCount ();
		contents = this.transform.Find ("Viewport/Content");
		contentsLength = 40 * (Mathf.CeilToInt((levelCount) / 5f)) + 8;
		contents.GetComponent<RectTransform> ().sizeDelta = new Vector2(contentsLength, 214);

		SpawnSelections ();

		Destroy(loadLVLButton.gameObject);
		Destroy(contents.Find ("Load (1)").gameObject);
		UpdateLVLSelection ();

		showingPos = this.transform.position;
		hiddenPos = showingPos;
		hiddenPos.x = 50000;

		showButton = GameObject.Find ("ShowMenu");
		hideButton = GameObject.Find ("HideMenu");
		toogleButtShowingPos = showButton.transform.position;
		hideButtShowingPos = hideButton.transform.position;
		ToogleLevelSelect ();
	}

	public void UpdateLVLSelection(){
		for (int i = 2; i < lvlLoad.Count; i++) {
			if(PlayerPrefs.GetInt ((i).ToString (), 0) == 1 || PlayerPrefs.GetInt ((i - 1).ToString (), 0) == 1)
				lvlLoad [i].interactable = true;
			else
				lvlLoad [i].interactable = false;			
		}
		for (int i = 0; i < lvlLoad.Count; i++)
			if (PlayerPrefs.GetInt ((i + 1).ToString (), 0) == 1)
				lvlLoad [i].image.color = completedColor;
	}

	public void ToogleLevelSelect(){
		if (showing) {
			this.transform.position = hiddenPos;
			hideButton.transform.position = hiddenPos;
			showButton.transform.position = toogleButtShowingPos;
		} else {
			this.transform.position = showingPos;
			hideButton.transform.position = hideButtShowingPos;
			showButton.transform.position = hiddenPos;
		}
		showing = !showing;
	}

	void SpawnSelections(){
		Vector3 startPos = loadLVLButton.transform.position;
		float offset = contents.Find ("Load (1)").position.x - startPos.x;
		for (int i = 0; i <= levelCount / 5; i++) {
			Vector3 newPos = startPos;
			newPos.x += offset * i;
			for (int j = 0; j < 5; j++) {
				GameObject newButt = Instantiate (loadLVLButton, newPos, Quaternion.identity, contents) as GameObject;
				newButt.name = loadLVLButton.name + (i * 5 + j + 1).ToString ();
				lvlLoad.Add (newButt.GetComponent<Button> ());
				int t = i * 5 + j + 1;
				lvlLoad [lvlLoad.Count - 1].onClick.AddListener (() => gm.LoadLevel (t));
				lvlLoad [lvlLoad.Count - 1].onClick.AddListener (() => sm.Play (SoundManager.Type.shortbeep));
				lvlLoad [lvlLoad.Count - 1].onClick.AddListener (() => ToogleLevelSelect());
				lvlLoad [lvlLoad.Count - 1].transform.Find ("Text").GetComponent<Text> ().text = t.ToString ();
				newPos.y -= offset;
				if (i * 5 + j + 1 >= levelCount)
					return;
			}
		}
	}
}
