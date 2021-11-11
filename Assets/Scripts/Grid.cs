using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [SerializeField] private LevelSelect LevelSelect;
    [SerializeField] private GameObject GridObject;

    [SerializeField] private Color activatedCol, disabledCol;

    private List<Image> Blocks = new List<Image>();

    private const int GridSize = 7;
    private bool[] GridFlags = new bool[GridSize * GridSize];

    private string CurrentGrid;

    void Start()
    {
        for (int i = 0; i < GridObject.transform.childCount; i++)
        {
            Blocks.Add(GridObject.transform.GetChild(i).GetComponent<Image>());
        }
        ClearGrid(true);
    }

    void Update()
    {
        CheckInput();
    }

    //----------------------------------------------------------------------------------------
    int currDragLen = 0;
    int[] currDrag = new int[GridSize];
    string tempGrid;
    void CheckInput()
    {
        if (LevelSelect.showing)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            currDragLen = 0;
            for (int i = 0; i < 7; i++)
                currDrag[i] = -1;
            tempGrid = getGrid();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ClearGrid(false);
            if (ValidateInput())
            {
                GameplayManager.Instance.UseRequirement(currDragLen);
                if (CheckGridLit())
                    GameplayManager.Instance.CompleteLevel();
                SoundManager.Instance.Play(SoundManager.SoundType.DoubleBeep);
            }
            else if (currDragLen > 0)
            {
                LoadGrid(tempGrid, false);
                SoundManager.Instance.Play(SoundManager.SoundType.Invalidinput);
            }
        }
        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (!GridFlags[i] && RectTransformUtility.RectangleContainsScreenPoint(Blocks[i].rectTransform, new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
                {
                    if (Blocks[i].color == disabledCol)
                        Blocks[i].color = activatedCol;
                    else
                        Blocks[i].color = disabledCol;
                    GridFlags[i] = true;
                    currDragLen++;
                    if (currDragLen <= 7)
                        currDrag[currDragLen - 1] = i;
                    SoundManager.Instance.Play(SoundManager.SoundType.ShortBeep);
                    break;
                }
            }
        }
    }

    bool ValidateInput()
    {
        if (currDragLen < 3 || currDragLen > 7)
            return false;
        for (int i = 1; i < currDragLen; i++)
        {
            if (currDrag[1] == currDrag[0] + 1 || currDrag[1] == currDrag[0] - 1)
            {
                if (currDrag[i] != currDrag[i - 1] + 1 && currDrag[i] != currDrag[i - 1] - 1)
                    return false;
            }
            else if (currDrag[1] == currDrag[0] + 7 || currDrag[1] == currDrag[0] - 7)
            {
                if (currDrag[i] != currDrag[i - 1] + 7 && currDrag[i] != currDrag[i - 1] - 7)
                    return false;
            }
            else
                return false;
        }
        return true;
    }
    //----------------------------------------------------------------------------------------

    public void LoadGrid(string blockStates, bool assignCurr)
    {
        if (assignCurr)
            CurrentGrid = blockStates;
        string[] bStates = blockStates.Split(' ');
        for (int i = 0; i < bStates.Length; i++)
        {
            if (bStates[i] == "0")
                Blocks[i].color = disabledCol;
            else
                Blocks[i].color = activatedCol;
        }
        ClearGrid(false);
    }

    public void LoadCurrentGrid()
    {
        LoadGrid(CurrentGrid, false);
    }

    string getGrid()
    {
        string rs = "";
        for (int i = 0; i < Blocks.Count - 1; i++)
        {
            if (Blocks[i].color == activatedCol)
                rs += "1 ";
            else
                rs += "0 ";
        }
        if (Blocks[Blocks.Count - 1].color == activatedCol)
            rs += "1";
        else
            rs += "0";
        return rs;
    }

    void ClearGrid(bool turnOff)
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            if (turnOff)
                Blocks[i].color = disabledCol;
            GridFlags[i] = false;
        }
    }

    public bool CheckGridLit()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            if (Blocks[i].color == disabledCol)
                return false;
        }
        return GameplayManager.Instance.AreRequirementsMet();
    }
}
