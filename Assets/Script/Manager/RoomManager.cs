using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
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
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("OnDisconnected");
        PhotonNetwork.ReconnectAndRejoin();
    }
    private void OnFailedToConnect()
    {
        Debug.Log("OnFailedToConnect");
    }
    private void OnFailedToConnectToMasterServer()
    {
        Debug.Log("OnFailedToConnectToMasterServer");
    }
    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        AssetManager.Instance.Dispose();
        PhotonNetwork.LoadLevel((int)SceneIndex.Lobby);
    }
}
