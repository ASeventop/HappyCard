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
        MyPlayer.Instance.isSit.AsObservable().Subscribe(sit =>
        {
            //Debug.Log(" MyPlayer.Instance.isSit.AsObservable() " + sit);
            if(sit)
                b_ready.gameObject.SetActive(true);
            else
                b_ready.gameObject.SetActive(false);
        });
        MyPlayer.Instance.isReady.AsObservable().Subscribe(ready =>
        {
            if (ready)
                b_ready.gameObject.SetActive(false);
        });
    }
}
