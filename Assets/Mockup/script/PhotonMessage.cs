using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonMessage : MonoBehaviourPunCallbacks
{
    static PhotonMessage instance;
    public static PhotonMessage Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("PhotonMessage", typeof(PhotonMessage)).GetComponent<PhotonMessage>();
            }
            return instance;
        }
    }
    public static void RaiseEvent(EventCode code,object obj,ReceiverGroup group)
    {
       byte evCode = (byte)code; // Custom Event 1: Used as "MoveUnitsToTargetPosition" event
       RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = group }; // You would have to set the Receivers to All in order to receive this event on the local client as well
       SendOptions sendOptions = new SendOptions { Reliability = true };
       PhotonNetwork.RaiseEvent(evCode, obj, raiseEventOptions, sendOptions);
     
       Debug.Log("send event aaaa " + evCode);


        /*byte evCode = (byte)EventCode.PlayerReady; // Custom Event 1: Used as "MoveUnitsToTargetPosition" event
        object[] content = new object[] { new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10 }; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
        Debug.Log("send event " + evCode);*/
    }
    public override void OnEnable()
    {
        base.OnEnable();

    }

    public override void OnDisable()
    {
        base.OnDisable();

    }

    ///message event
    ///
    public static void UpdatePlayerDeck(byte[] cards)
    {
        RaiseEvent(EventCode.UpdatePlayerDeck, cards, ReceiverGroup.All);
    }
    public static void PlayerReadyRequest(byte actorNumber,byte photonviewID)
    {
        byte[] sendParam = new byte[] { photonviewID,1};
        RaiseEvent(EventCode.ReadyRequest, sendParam, ReceiverGroup.All);
    }
    public static void RequestSeatData(){
        Debug.Log("requestseatdata");
        RaiseEvent(EventCode.RequestSeatData, null, ReceiverGroup.All);
    }
    public static void DeckUpdateEnd()
    {
        RaiseEvent(EventCode.PlayerUpdateDeckEnd, null, ReceiverGroup.Others);
    }

}
