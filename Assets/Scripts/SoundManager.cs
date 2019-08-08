using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

	public AudioSource bgm, shortbeep, doublebeep, doublebeep2, invalidinput;
	public enum Type{
		shortbeep, doublebeep, doublebeep2, invalidinput
	}

	void Start(){
		volumeSlider.value = PlayerPrefs.GetFloat ("Volume", 0);
		ChangeVolume ();
	}

	public void Play(Type type){
		if (type == Type.shortbeep)
			shortbeep.Play ();
		else if(type == Type.doublebeep)
			doublebeep.Play ();
		else if(type == Type.doublebeep2)
			doublebeep2.Play ();
		else if(type == Type.invalidinput)
			invalidinput.Play ();
	}

	public AudioMixer mainMix;
	public Slider volumeSlider;
	public void ChangeVolume(){
		if (volumeSlider.value == volumeSlider.minValue) {
			if (bgm.isPlaying)
				bgm.Pause ();
		} else {
			if (!bgm.isPlaying)
				bgm.Play ();
		}
		mainMix.SetFloat ("MasterVolume", volumeSlider.value);
		PlayerPrefs.SetFloat ("Volume", volumeSlider.value);
		PlayerPrefs.Save ();
	}
}
