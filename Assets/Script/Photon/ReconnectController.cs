using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReconnectController : MonoBehaviourPunCallbacks
{
   // [SerializeField] GameObject obj_disconnect;
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            PhotonNetwork.Disconnect();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
    }
    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("OnConnected ...");
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster ... ");
    }
    private void OnPlayerDisconnected()
    {
        Debug.Log("OnDiscon OnPlayerDisconnectednected");
      //  PhotonNetwork.LoadLevel((int)SceneIndex.Lobby);
    }
    private void Update()
    {
        if (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState == PeerStateValue.Disconnected)
        {
            if (!PhotonNetwork.ReconnectAndRejoin())
            {
                Debug.Log("Failed reconnecting and joining!!", this);
            }
            else
            {
                Debug.Log("Successful reconnected and joined!", this);
                Debug.Log("nickname "+PhotonNetwork.LocalPlayer.NickName);
            }
        }
    }
}