using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	public int showAdEveryComp = 7;
	int adCount;

	void Start () {
		adCount = PlayerPrefs.GetInt ("LeftToAd", showAdEveryComp + 1);
	}

	public void CheckAdShow(){
		adCount--;
		if (adCount <= 0)
			ShowAd ();
	}

	void ShowAd(){
		Advertisement.Show ();
		adCount = showAdEveryComp + Random.Range (-1, 1);
		PlayerPrefs.SetInt ("LeftToAd", adCount);
	}
}
