using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
public class RoomManager : MonoBehaviourPun
{
    static RoomManager instance;
    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("RoomManager", typeof(RoomManager)).GetComponent<RoomManager>();
            }
            return instance;
        }
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        AssetManager.Instance.Dispose();
        PageStack.Instance.CurrentSceneSwitch(SceneName.Lobby);
    }
}
