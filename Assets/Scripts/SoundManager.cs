using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public enum SoundType
    {
        ShortBeep, DoubleBeep, DoubleBeep2, Invalidinput
    }

    [System.Serializable]
    public class SoundData
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
    }

    [SerializeField] private AudioMixer MainAudioMixer;
    [SerializeField] private AudioSource BgmSource, SfxSource;
    [SerializeField] private List<SoundData> GameSoundData;

    private Dictionary<SoundType, AudioClip> SoundDictionary;

    private void Awake()
    {
        InitializeSoundDictionary();
    }

    private void InitializeSoundDictionary()
    {
        SoundDictionary = new Dictionary<SoundType, AudioClip>();
        foreach (var data in GameSoundData)
        {
            SoundDictionary.Add(data.SoundType, data.AudioClip);
        }
    }

    private void Start()
    {
        UIManager.Instance.SetVolumeSlider(SaveManager.Instance.GetFloat("Volume", 0));
        ChangeVolume();
    }

    public void Play(SoundType type)
    {
        var audioClip = SoundDictionary[type];
        if (audioClip != null)
        {
            SfxSource.clip = audioClip;
            SfxSource.Play();
        }
    }

    public void ChangeVolume()
    {
        var value = UIManager.Instance.GetVolumeSliderValue();
        var minValue = UIManager.Instance.GetVolumeSliderMin();
        if (value == minValue)
        {
            if (BgmSource.isPlaying)
                BgmSource.Pause();
        }
        else
        {
            if (!BgmSource.isPlaying)
                BgmSource.Play();
        }
        MainAudioMixer.SetFloat("MasterVolume", value);
        SaveManager.Instance.SetFloat("Volume", value);
        SaveManager.Instance.Save();
    }
}
