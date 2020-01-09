using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    static GameUIManager instance;
    public static GameUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameUIManager", typeof(GameUIManager)).GetComponent<GameUIManager>();
            }
            return instance;
        }
    }
    

}
