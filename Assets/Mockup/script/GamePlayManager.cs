using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    static GamePlayManager instance;
    int viewID = -1;
    bool notReady = false;
    public static GamePlayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GamePlaymanager", typeof(GamePlayManager)).GetComponent<GamePlayManager>();
            }
            return instance;
        }
    }
    public void PlayerRegisterSeat(int _viewID)
    {
        viewID = _viewID;
    }
    public void PlayerSittingSeat()
    {

    }
}
