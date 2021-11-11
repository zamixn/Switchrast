using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Requirements : MonoBehaviour
{
    [System.Serializable]
    public class Requirement
    {
        public Transform x;
        public Text text, cross;
        public List<Image> images;
        [HideInInspector] public int amount, left;
    }

    [SerializeField] private Color activeCol, disabledCol, usedCol, overUsedCol;
    [SerializeField] private List<Requirement> Reqs = new List<Requirement>();
    [SerializeField] private int ReqOffsetIndex = 3;
    [SerializeField] private int GridSize = 7;

    private string currentReqs;

    void Start()
    {
        for (int i = 0; i < Reqs.Count; i++)
            DisableRequirement(i);
    }

    public void LoadRequirements(string requirements)
    {
        currentReqs = requirements;
        string[] reqArr = requirements.Split(' ');
        for (int i = 0; i < Reqs.Count; i++)
        {
            if (reqArr[i] == "0")
                DisableRequirement(i);
            else
                EnableRequirement(i, reqArr[i]);
        }
    }

    public void LoadCurrentRequirements()
    {
        LoadRequirements(currentReqs);
    }

    public void SubtractRequirement(int reqLen)
    {
        var req = Reqs[reqLen - ReqOffsetIndex];
        req.left--;
        req.text.text = req.left.ToString() + " / " + req.amount.ToString();
        if (req.left == 0)
            req.text.color = usedCol;
        else if (Reqs[reqLen].left < 0)
            req.text.color = overUsedCol;
    }

    void EnableRequirement(int reqIndex, string amountUsable)
    {
        var req = Reqs[GridSize - (reqIndex + ReqOffsetIndex)];
        req.amount = int.Parse(amountUsable);
        req.left = int.Parse(amountUsable);
        req.text.text = amountUsable + " / " + amountUsable;
        req.text.color = activeCol;
        req.cross.color = activeCol;
        for (int j = 0; j < req.images.Count; j++)
            req.images[j].color = activeCol;
    }

    void DisableRequirement(int reqIndex)
    {
        var req = Reqs[GridSize - (reqIndex + ReqOffsetIndex)];
        req.text.text = "0 / 0";
        req.text.color = disabledCol;
        req.left = 0;
        req.amount = 0;
        req.cross.color = disabledCol;
        for (int j = 0; j < req.images.Count; j++)
            req.images[j].color = disabledCol;
    }

    public bool AreRequirementsMet()
    {
        for (int i = 0; i < Reqs.Count; i++)
            if (Reqs[i].left < 0)
                return false;
        return true;
    }
}
