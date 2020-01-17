using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Photon.Pun;

public class UI_Login : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject object_login;
    [SerializeField] TMP_InputField input_name;
    [SerializeField] TMP_InputField input_userID;
    [SerializeField] Button b_login;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            object_login.SetActive(false);
        }
        UserManager.Instance.SetData();
        input_name.text = "RNG_" + Random.Range(0, 999);
        b_login.OnClickAsObservable().Subscribe(_ =>
        {
            if (!string.IsNullOrEmpty(input_name.text)&& !string.IsNullOrEmpty(input_userID.text))
                PhotonNetworkConsole.Instance.Connect(input_name.text, input_userID.text);
        });
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        object_login.SetActive(false);
    }
    public override void OnConnected()
    {
        base.OnConnected();
    }

}
