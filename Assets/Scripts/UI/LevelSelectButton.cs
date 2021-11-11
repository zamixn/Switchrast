using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Text Text;
    [SerializeField] private Image Image;

    public bool Interactable
    {
        get { return Button.interactable; }
        set { Button.interactable = value; }
    }

    public Color Color
    {
        get { return Image.color; }
        set { Image.color = value; }
    }

    public void Initialize(string name, Action onClick, string text)
    {
        gameObject.name = name;
        Button.onClick.AddListener(() => { onClick?.Invoke(); });
        Text.text = text;
    }
}
