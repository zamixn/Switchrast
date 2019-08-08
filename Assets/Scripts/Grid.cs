using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	GameplayManager gm;
	SoundManager sm;
	LevelSelect lvlSelect;
	GameObject grid;

	List<Image> blocks = new List<Image>();
	bool[] b = new bool[49];

	public Color activatedCol, disabledCol;

	string currentGrid;

	void Awake () {
		gm = GameObject.FindObjectOfType<GameplayManager> ();
		sm = GameObject.FindObjectOfType<SoundManager> ();
		grid = this.transform.Find("Grid").gameObject;
		lvlSelect = GameObject.FindObjectOfType<LevelSelect> ();
	}

	void Start(){
		for (int i = 0; i < grid.transform.childCount; i++) {
			blocks.Add (grid.transform.GetChild (i).GetComponent<Image> ());
		}
		ClearGrid (true);
	}

	void Update () {
		CheckInput ();
	}

	//----------------------------------------------------------------------------------------
	int currDragLen = 0;
	int[] currDrag = new int[7];
	string tempGrid;
	void CheckInput(){
		if (lvlSelect.showing)
			return;
		if (Input.GetMouseButtonDown (0)) {
			currDragLen = 0;
			for (int i = 0; i < 7; i++)
				currDrag [i] = -1;
			tempGrid = getGrid ();
		}
		if (Input.GetMouseButtonUp (0)) {
			ClearGrid (false);
			if (ValidInput ()) {
				gm.UseRequirement (currDragLen);
				if (CheckGridLit ())
					gm.CompleteLevel ();
				sm.Play (SoundManager.Type.doublebeep);
			} else if (currDragLen > 0) {
				LoadGrid (tempGrid, false);
				sm.Play (SoundManager.Type.invalidinput);
			}
		}
		if (Input.GetMouseButton (0)) {
			for (int i = 0; i < blocks.Count; i++) {
				if (!b[i] && RectTransformUtility.RectangleContainsScreenPoint (blocks [i].rectTransform, new Vector2 (Input.mousePosition.x, Input.mousePosition.y))) {
					if (blocks [i].color == disabledCol)
						blocks [i].color = activatedCol;
					else
						blocks [i].color = disabledCol;
					b [i] = true;
					currDragLen++;
					if(currDragLen <= 7)
						currDrag [currDragLen - 1] = i;
					sm.Play (SoundManager.Type.shortbeep);
					break;
				}
			}
		}
	}

	bool ValidInput(){
		if(currDragLen < 3 || currDragLen > 7)
			return false;
		for (int i = 1; i < currDragLen; i++) {
			if (currDrag [1] == currDrag [0] + 1 || currDrag [1] == currDrag [0] - 1) {
				if (currDrag [i] != currDrag [i - 1] + 1 && currDrag [i] != currDrag [i - 1] - 1)
					return false;
			} else if (currDrag [1] == currDrag [0] + 7 || currDrag [1] == currDrag [0] - 7) {
				if (currDrag [i] != currDrag [i - 1] + 7 && currDrag [i] != currDrag [i - 1] - 7)
					return false;
			} else
				return false;
		}
		return true;
	}
	//----------------------------------------------------------------------------------------

	public void LoadGrid(string blockStates, bool assignCurr){
		if(assignCurr)
			currentGrid = blockStates;
		string[] bStates = blockStates.Split (' ');
		for (int i = 0; i < bStates.Length; i++) {
			if (bStates [i] == "0")
				blocks [i].color = disabledCol;
			else
				blocks [i].color = activatedCol;
		}
		ClearGrid (false);
	}

	public void LoadCurrentGrid(){
		LoadGrid (currentGrid, false);
	}

	string getGrid(){
		string rs = "";
		for (int i = 0; i < blocks.Count - 1; i++) {
			if (blocks [i].color == activatedCol)
				rs += "1 ";
			else
				rs += "0 ";
		}
		if (blocks [blocks.Count - 1].color == activatedCol)
			rs += "1";
		else
			rs += "0";
		return rs;
	}

	void ClearGrid(bool turnOff){
		for (int i = 0; i < blocks.Count; i++) {
			if(turnOff)
				blocks [i].color = disabledCol;
			b [i] = false;
		}
	}

	public bool CheckGridLit(){
		for (int i = 0; i < blocks.Count; i++) {
			if (blocks [i].color == disabledCol)
				return false;
		}
		return gm.AreRequirementsMet();
	}
}
