using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class UI_DisConnect : MonoBehaviour
{
    [SerializeField] GameObject rootObj;


    private void Start()
    {
        PhotonNetworkConsole.Instance.isConnected.AsObservable().Subscribe(connected =>
        {
            Debug.Log("isConnected Subscribe " + connected);
            OpenDisconnected(!connected);
        });
    }
    public void OpenDisconnected(bool isConnect)
    {
        if(rootObj != null) rootObj.SetActive(isConnect);
    }
}
