using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UniRx;
public class RoomInfo : MonoBehaviourPun
{
    [SerializeField]TextMeshProUGUI roomName_txt,players_txt;
    private void Start()
    {
        PhotonNetwork.CurrentRoom.Name.ObserveEveryValueChanged(room => room).Subscribe(roomname =>
        {
            roomName_txt.text = roomname;
        });
        PhotonNetwork.CurrentRoom.Players.ObserveEveryValueChanged(players => players.Count).Subscribe(playerCount =>
        {
            players_txt.text = playerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        });
    }
}
