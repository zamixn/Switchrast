using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private LevelSelect LevelSelect;
    [SerializeField] private GameCompleteText GameText;
    [SerializeField] private Text LevelNumText;
    [SerializeField] private Button RetryButton;
    [SerializeField] private Slider VolumeSlider;

    [SerializeField] private Color ActiveColor;
    [SerializeField] private Color DisabledColor;

    [Header("Labels")]
    [SerializeField] private string LevelCompleteText = "level complete";

    void Start()
    {
        LevelNumText.color = DisabledColor;
        LevelNumText.text = "#";
        RetryButton.interactable = false;
    }

    public void CompleteLevel()
    {
        GameText.SetText(LevelCompleteText);
        LevelSelect.UpdateLVLSelection();
    }

    public void SetLevelText(int num, bool isLevelComplete)
    {
        LevelNumText.color = ActiveColor;
        LevelNumText.text = "#" + num.ToString();
        RetryButton.interactable = true;

        GameText.SetTextNoAnim(isLevelComplete ? LevelCompleteText : "");
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void SetVolumeSlider(float value)
    {
        VolumeSlider.value = value;
    }
    public float GetVolumeSliderValue()
    {
        return VolumeSlider.value;
    }
    public float GetVolumeSliderMin()
    {
        return VolumeSlider.minValue;
    }
}
