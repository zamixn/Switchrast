using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : ISaving
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get 
        {
            if (_instance == null)
                _instance = new SaveManager();
            return _instance;
        }
    }

    private ISaving SavingProxy;
    private SaveManager()
    {
        SavingProxy = new PlayerPrefsSaving();
    }

    public string GetString(string key, string defaultValue = "")
    {
        return SavingProxy.GetString(key, defaultValue);
    }

    public void SetString(string key, string value)
    {
        SavingProxy.SetString(key, value);
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        return SavingProxy.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        SavingProxy.SetInt(key, value);
    }

    public float GetFloat(string key, float defaultValue = 0)
    {
        return SavingProxy.GetFloat(key, defaultValue);
    }

    public void SetFloat(string key, float value)
    {
        SavingProxy.SetFloat(key, value);
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        return SavingProxy.GetBool(key, defaultValue);
    }

    public void SetBool(string key, bool value)
    {
        SavingProxy.SetBool(key, value);
    }

    public void Save()
    {
        SavingProxy.Save();
    }
}
