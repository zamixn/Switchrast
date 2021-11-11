using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCompleteText : MonoBehaviour
{
    private const float TextZoom = 1.05f;
    private const float TextTime = 1.5f;

    [SerializeField] private Text Text;

    private Vector3 oriScale;

    void Awake()
    {

        oriScale = Text.transform.localScale;
        ClearText();
    }

    public void SetTextNoAnim(string text)
    {
        Text.text = text;
    }

    public void SetText(string text)
    {
        StartCoroutine(SetText(TextZoom, TextTime, text));
    }

    IEnumerator SetText(float textZoom, float textTime, string text)
    {
        Text.text = text;
        Text.transform.localScale = textZoom * oriScale;
        while (Text.transform.localScale.x > oriScale.x)
        {
            Text.transform.localScale = Vector3.Lerp(Text.transform.localScale, oriScale, Time.deltaTime / textTime);
            if (Text.transform.localScale.x - .0025f < oriScale.x)
                Text.transform.localScale = oriScale;
            yield return null;
        }
    }

    void ClearText()
    {
        StopAllCoroutines();
        Color c = Text.color;
        c.a = 1;
        Text.color = c;
        Text.text = "";
        Text.transform.localScale = oriScale;
    }
}
