using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Requirements : MonoBehaviour {

	class Requirement{
		public Transform x;
		public int amount, left;
		public Text text, cross;
		public List<Image> images;
	}

	List<Requirement> reqs = new List<Requirement>();
	public Color activeCol, disabledCol, usedCol, overUsedCol;

	string currentReqs;

	void Start () {
		for (int i = 0; i < this.transform.childCount; i++) {
			Requirement r = new Requirement();
			r.x = this.transform.GetChild (i);
			r.text = r.x.Find ("text").GetComponent<Text> ();
			r.cross = r.x.Find ("x").GetComponent<Text> ();
			r.images = new List<Image> ();
			for (int j = 0; j < 7 - i; j++)
				r.images.Add (r.x.GetChild (2 + j).GetComponent<Image> ());
			r.amount = 0;
			r.left = 0;
			reqs.Add (r);
		}
		for (int i = 0; i < reqs.Count; i++)
			DisableRequirement (i);
	}

	public void LoadRequirements(string requirements){
		currentReqs = requirements;
		string[] reqArr = requirements.Split (' ');
		for (int i = 0; i < reqs.Count; i++) {
			if (reqArr [i] == "0")
				DisableRequirement (i);
			else
				EnableRequirement (i, reqArr [i]);
		}
	}

	public void LoadCurrentRequirements(){
		LoadRequirements (currentReqs);
	}

	public void SubtractRequirement(int reqLen){
		reqs [7 - reqLen].left--;
		reqs [7 - reqLen].text.text = reqs [7 - reqLen].left.ToString () + " / " + reqs [7 - reqLen].amount.ToString ();
		if (reqs [7 - reqLen].left == 0)
			reqs [7 - reqLen].text.color = usedCol;
		else if (reqs [7 - reqLen].left < 0)
			reqs [7 - reqLen].text.color = overUsedCol;
	}

	void EnableRequirement(int reqIndex, string amountUsable){
		reqs [reqIndex].amount = int.Parse (amountUsable);
		reqs [reqIndex].left = int.Parse (amountUsable);
		reqs [reqIndex].text.text = amountUsable + " / " + amountUsable;
		reqs [reqIndex].text.color = activeCol;
		reqs [reqIndex].cross.color = activeCol;
		for (int j = 0; j < reqs [reqIndex].images.Count; j++)
			reqs [reqIndex].images [j].color = activeCol;
	}

	void DisableRequirement(int reqIndex){
		reqs [reqIndex].text.text = "0 / 0";
		reqs [reqIndex].text.color = disabledCol;
		reqs [reqIndex].left = 0;
		reqs [reqIndex].amount = 0;
		reqs [reqIndex].cross.color = disabledCol;
		for (int j = 0; j < reqs [reqIndex].images.Count; j++)
			reqs [reqIndex].images [j].color = disabledCol;
	}

	public bool AreRequirementsMet(){
		for (int i = 0; i < reqs.Count; i++)
			if (reqs [i].left < 0)
				return false;
		return true;
	}
}
