using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnectController : MonoBehaviourPunCallbacks
{
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            PhotonNetwork.Disconnect();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected "+cause);
        PhotonNetwork.LoadLevel((int)SceneIndex.Lobby);
    }
    private void OnPlayerDisconnected()
    {
        Debug.Log("OnDisconOnPlayerDisconnectednected");
        PhotonNetwork.LoadLevel((int)SceneIndex.Lobby);
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