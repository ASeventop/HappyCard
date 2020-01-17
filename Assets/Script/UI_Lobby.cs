using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class UI_Lobby : MonoBehaviour
{
    [SerializeField] Button b_quickStart;
    [SerializeField] Button b_reconnect;
    private void Start()
    {
        PhotonNetworkConsole.Instance.isConnected.AsObservable().Subscribe(connected =>
        {
            if(b_quickStart!=null) b_quickStart.interactable = connected;
            if(b_reconnect!=null) b_reconnect.gameObject.SetActive(!connected);
        });

        
    
    

    }
}
