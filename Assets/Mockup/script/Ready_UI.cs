using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Ready_UI : MonoBehaviourPunCallbacks
{
    public Button b_ready;
    private void Start()
    {
        b_ready.onClick.AsObservable().Subscribe(_ =>
        {
            PhotonMessage.PlayerReadyRequest((byte)MyPlayer.Instance.localPlayer.ActorNumber, (byte)MyPlayer.Instance.viewID.Value);
        });

        MyPlayer.Instance.viewID.AsObservable().Subscribe(_ =>
        {

        });
        MyPlayer.Instance.OnSit.Subscribe(isSit =>
        {
            b_ready.gameObject.SetActive(isSit);
        });
        MyPlayer.Instance.OnReady.Subscribe(isReady => {
            b_ready.gameObject.SetActive(!isReady);
        });
    }
}
