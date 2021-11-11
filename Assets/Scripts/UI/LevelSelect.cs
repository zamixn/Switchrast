using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private LevelSelectButton loadLVLButton;
    [SerializeField] private LevelSelectButton loadLVLButton2;
    [SerializeField] private RectTransform contents;

    [HideInInspector] public bool showing = true;

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
        contents.sizeDelta = new Vector2(contentsLength, ContentHeight);

        SpawnSelections();

        Destroy(loadLVLButton);
        Destroy(loadLVLButton2);
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
        this.transform.position = showing ? hiddenPos : showingPos;
        HUDManager.Instance.SetLevelSelectButtons(showing);
        showing = !showing;
    }

    void SpawnSelections()
    {
        Vector3 startPos = loadLVLButton.transform.position;
        float offset = loadLVLButton2.transform.position.x - startPos.x;
        for (int i = 0; i <= levelCount / RowCount; i++)
        {
            Vector3 newPos = startPos;
            newPos.x += offset * i;
            for (int j = 0; j < RowCount; j++)
            {
                LevelSelectButton newButt = Instantiate(loadLVLButton, newPos, Quaternion.identity, contents);
                int t = i * RowCount + j + 1;
                newButt.Initialize(
                    loadLVLButton.name + t.ToString(),
                    () =>
                    {
                        GameplayManager.Instance.LoadLevel(t);
                        SoundManager.Instance.Play(SoundManager.SoundType.ShortBeep);
                        ToogleLevelSelect();
                    },
                    t.ToString());
                lvlLoad.Add(newButt);
                newPos.y -= offset;
                if (t >= levelCount)
                    return;
            }
        }
    }
}
