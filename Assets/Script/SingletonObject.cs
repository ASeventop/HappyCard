using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {/*
                T[] result = Resources.FindObjectsOfTypeAll<T>();
                Debug.Log("new singleton class " + typeof(T).ToString());

                if (result.Length == 0)
                {
                    Debug.LogError("result length is 0 for type " + typeof(T).ToString());
                }
                if (result.Length > 1)
                {
                    Debug.LogError("result greather than 1 for type " + typeof(T).ToString());
                }
                instance = result[0];*/
                instance = new GameObject("SingleTon_"+typeof(T).Name, typeof(T)).GetComponent<T>();
            }
            return instance;
        }
    }
}
