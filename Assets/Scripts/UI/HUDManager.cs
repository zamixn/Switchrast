using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField] private GameObject ShowButton;
    [SerializeField] private GameObject HideButton;

    private Vector3 hiddenPos, toogleButtShowingPos, hideButtShowingPos;

    private void Start()
    {
        toogleButtShowingPos = ShowButton.transform.position;
        hideButtShowingPos = HideButton.transform.position;

        hiddenPos = toogleButtShowingPos;
        hiddenPos.x = 50000;
    }

    public void SetLevelSelectButtons(bool showing)
    {
        if (showing)
        {
            HideButton.transform.position = hiddenPos;
            ShowButton.transform.position = toogleButtShowingPos;
        }
        else
        {
            HideButton.transform.position = hideButtShowingPos;
            ShowButton.transform.position = hiddenPos;
        }
    }

    public void OnClickNext()
    {
        GameplayManager.Instance.LoadNextLevel();
    }

    public void OnClickRetry()
    {
        GameplayManager.Instance.Retry();
    }
}
