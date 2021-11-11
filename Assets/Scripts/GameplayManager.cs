using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Grid Grid;
    [SerializeField] private Requirements Requirements;

    public bool isCurrComplete { get; private set; }
    public int CurrentLevel { get; private set; }
    private int LevelCount;

    void Awake()
    {
        LevelCount = LevelLoader.Instance.LevelCount();
    }

    void Start()
    {
        int lastLoadedLVL = SaveManager.Instance.GetInt("LastLoadedLVL", 0);
        if (lastLoadedLVL != 0)
            LoadLevel(lastLoadedLVL);
    }

    public void CompleteLevel()
    {
        if (!isCurrComplete)
        {
            UIManager.Instance.CompleteLevel();
            SaveManager.Instance.SetInt(CurrentLevel.ToString(), 1);
            SaveManager.Instance.Save();
            isCurrComplete = true;
            AdManager.Instance.CheckAdShow();
            SoundManager.Instance.Play(SoundManager.SoundType.DoubleBeep2);
        }
    }

    public void LoadLevel(int levelNum)
    {
        CurrentLevel = levelNum;
        string[] levelArr = LevelLoader.Instance.LoadLevel(levelNum);
        Grid.LoadGrid(levelArr[0], true);
        Requirements.LoadRequirements(levelArr[1]);

        isCurrComplete = SaveManager.Instance.GetInt(levelNum.ToString(), 0) == 1;
        UIManager.Instance.SetLevelText(levelNum, isCurrComplete);
        SaveManager.Instance.SetInt("LastLoadedLVL", CurrentLevel);
        SaveManager.Instance.Save();
    }

    public void UseRequirement(int length)
    {
        Requirements.SubtractRequirement(length);
    }

    public void Retry()
    {
        Grid.LoadCurrentGrid();
        Requirements.LoadCurrentRequirements();
        SoundManager.Instance.Play(SoundManager.SoundType.ShortBeep);
    }

    public void LoadNextLevel()
    {
        if (SaveManager.Instance.GetInt(CurrentLevel.ToString(), 0) == 1 && CurrentLevel < LevelCount)
            LoadLevel(CurrentLevel + 1);
        SoundManager.Instance.Play(SoundManager.SoundType.ShortBeep);
    }

    public bool AreRequirementsMet()
    {
        return Requirements.AreRequirementsMet();
    }
}
