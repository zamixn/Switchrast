using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Object
{
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            if (_instance == null)
                throw new System.Exception($"Instance of {nameof(T)} not found");
            return _instance;
        }
    }
}
