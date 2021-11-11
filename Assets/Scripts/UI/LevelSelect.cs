using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private LevelSelectButton LoadLVLButtonPrefab;
    [SerializeField] private RectTransform Contents;

    [HideInInspector] public bool Showing = true;

    [SerializeField] private Color completedColor;

    [Header("Layout")]
    [SerializeField] private int RowCount = 5;
    [SerializeField] private int HorizontalPadding = 8;
    [SerializeField] private int SizeDelta = 40;
    [SerializeField] private int ContentHeight = 214;

    private int levelCount;

    private List<LevelSelectButton> lvlLoad = new List<LevelSelectButton>();
    private Vector3 hiddenPos, showingPos;

    void Start()
    {
        levelCount = LevelLoader.Instance.LevelCount();
        var contentsLength = SizeDelta * (Mathf.CeilToInt((levelCount) / (float)RowCount)) + HorizontalPadding;
        Contents.sizeDelta = new Vector2(contentsLength, ContentHeight);

        SpawnSelections();

        UpdateLVLSelection();

        showingPos = this.transform.position;
        hiddenPos = showingPos;
        hiddenPos.x = 50000;

        ToogleLevelSelect();
    }

    public void UpdateLVLSelection()
    {
        for (int i = 0; i < lvlLoad.Count; i++)
        {
            bool completed = SaveManager.Instance.GetBool((i).ToString(), false);
            bool unlocked = SaveManager.Instance.GetBool((i - 1).ToString(), false);
            bool interactable = i < 2 || completed || unlocked;
            lvlLoad[i].Interactable =  interactable;
            if(completed)
                lvlLoad[i].Color = completedColor;
        }
    }

    public void ToogleLevelSelect()
    {
        this.transform.position = Showing ? hiddenPos : showingPos;
        HUDManager.Instance.SetLevelSelectButtons(Showing);
        Showing = !Showing;
    }

    void SpawnSelections()
    {
        for (int i = 0; i <= levelCount / RowCount; i++)
        {
            for (int j = 0; j < RowCount; j++)
            {
                LevelSelectButton newButt = Instantiate(LoadLVLButtonPrefab, Contents);
                int t = i * RowCount + j + 1;
                newButt.Initialize(
                    LoadLVLButtonPrefab.name + t.ToString(),
                    () =>
                    {
                        GameplayManager.Instance.LoadLevel(t);
                        SoundManager.Instance.Play(SoundManager.SoundType.ShortBeep);
                        ToogleLevelSelect();
                    },
                    t.ToString());
                lvlLoad.Add(newButt);
                if (t >= levelCount)
                    return;
            }
        }
    }
}
