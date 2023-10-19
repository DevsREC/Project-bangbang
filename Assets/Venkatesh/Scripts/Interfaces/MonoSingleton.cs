using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if(_instance == null)
                Debug.Log(typeof(T).ToString() + "is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
    }
}
