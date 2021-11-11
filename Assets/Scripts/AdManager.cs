using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : Singleton<AdManager> {

	[SerializeField] private int showAdEveryComp = 7;
	int adCount;

	void Start () {
		adCount = SaveManager.Instance.GetInt ("LeftToAd", showAdEveryComp + 1);
	}

	public void CheckAdShow(){
		adCount--;
		if (adCount <= 0)
			ShowAd ();
	}

	void ShowAd(){
		//Advertisement.Show ();
		adCount = showAdEveryComp + Random.Range (-1, 1);
		SaveManager.Instance.SetInt ("LeftToAd", adCount);
	}
}
