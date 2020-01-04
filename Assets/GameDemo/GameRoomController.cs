using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using System;

public class GameRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Canvas gameCanvas;
    PhotonView photonView;

    private void Start()
    {
        photonView = PhotonView.Get(this);
        AssetManager.Instance.Init();
        MyPlayer.Instance.SetLocalPlayer(PhotonNetwork.LocalPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("player " + otherPlayer.NickName + "lefted ");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(string.Format("Player {0} has connect to the room {1}", newPlayer.NickName, newPlayer.UserId));
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public void CreatePlayerPrefab()
    {
        var go  = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        //go.transform.SetParent(gameCanvas.transform);
    }
    public void OnEvent(EventData photonEvent)
    {
       // EventCode code = (EventCode)photonEvent.Code;
        CheckEventCode((EventCode)photonEvent.Code,photonEvent);
        Debug.Log("onvent " + photonEvent.Code + "sender "+ photonEvent.Sender+" data "+photonEvent.CustomData);
      //  Debug.Log("Customdata type "+photonEvent.CustomData.GetType());
        // Hashtable hash = (Hashtable)photonEvent.CustomData;
        //Debug.Log("createplayerprefab");
        /*if (photonEvent.CustomData is ExitGames.Client.Photon.Hashtable)
        {
            ExitGames.Client.Photon.Hashtable hash = photonEvent.CustomData as ExitGames.Client.Photon.Hashtable;
            // var dic = hash.Cast<DictionaryEntry>().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var dic = hash.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Debug.Log(dic.ContainsKey(1));
            Debug.Log(dic[11]);
            Debug.Log(dic[1]);
        }*/
        //table.Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value);
        /* if (eventCode == MoveUnitsToTargetPositionEvent)
         {
             object[] data = (object[])photonEvent.CustomData;

             Vector3 targetPosition = (Vector3)data[0];

             for (int index = 1; index < data.Length; ++index)
             {
                 int unitId = (int)data[index];

                 UnitList[unitId].TargetPosition = targetPosition;
             }
         }*/

    }
    public void OnEnable()
    {
        
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        RequestSeatData();
       /* byte evCode = (byte)EventCode.PlayerReady; // Custom Event 1: Used as "MoveUnitsToTargetPosition" event
        object[] content = new object[] { new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10 }; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
        Debug.Log("send event " + evCode);*/

    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    
    
    void CheckEventCode(EventCode @event, EventData photonEvent)
    {
        switch (@event)
        {
            case EventCode.ReceiveSeatData:
                ReceiveSeatData(photonEvent);
                break;
            case EventCode.PlayerReady:
                Debug.Log("PlayerReady !!");
                break;
            case EventCode.SendCard:
                Debug.Log("SendCard !!");
                break;
            case EventCode.SitAccept:
                AcceptSit(photonEvent);
                break;
            case EventCode.ReadyAccept:
                ReadyAccept(photonEvent);
                break;
            case EventCode.GameReady:
                GameReady();
                break;
            case EventCode.DistributeCard:
                DistributeCard(photonEvent);
                break;
            case EventCode.UpdateTimer:
                UpdateTimer(photonEvent);
                break;
            case EventCode.PlayerUpdateDeckEnd://confirm playe end
                PlayerUpdateDeckEnd(photonEvent);
                break;
            case EventCode.UpdateDeckEnd://end to all update
                UpdateDeckEnd(photonEvent);
                break;
            case EventCode.RestartGameTimer:
                UpdateTimeRestartGame(photonEvent);
                break;
            case EventCode.GameResult:
                SetGameResult(photonEvent);
                break;
        }
    }
    void SetGameResult(EventData photonEvent)
    {
        Debug.Log("SetGameResult ---------------------------------------");
        var data = photonEvent.CustomData as Dictionary<object, object>;
        foreach (var player in data)
        {
            foreach (var playerData in player.Value as Dictionary<object,object>)
            {
                Debug.Log("playerData " + playerData.Key + "value "+playerData.Value);
            }
        }
        UIManager.Instance.OpenGameResult(data);
    }
    void PlayerUpdateDeckEnd(EventData photonEvent)
    {
        Debug.Log("sender " + photonEvent.Sender);
        Debug.Log("ActorNumber " + PhotonNetwork.LocalPlayer.ActorNumber);
        if(PhotonNetwork.LocalPlayer.ActorNumber == photonEvent.Sender)
            Game.Instance.PlayerDeckConfirm();
    }
    void UpdateTimeRestartGame(EventData photonEvent)
    {
        var data = photonEvent.CustomData as object[];
        UIManager.Instance.RestartGame((float)data[0]);
    }
    void UpdateDeckEnd(EventData photonEvent)
    {
        Debug.Log("UpdateDeckEnd ");
        Game.Instance.OpenPlayerDeck(false);
    }
    void UpdateTimer(EventData photonEvent)
    {
        var data = photonEvent.CustomData as object[];
        var time = (float)data[0];
        UIManager.Instance.SetTimer(time);
    }
    void DistributeCard(EventData photonEvent) {
       if (photonEvent.Sender != PhotonNetwork.LocalPlayer.ActorNumber) return;
        UIManager.Instance.CloseGameResult();
       var data = photonEvent.CustomData as Dictionary<string, object>;
        byte[] deckIDs = data["deck"] as byte[];
        Game.Instance.OpenPlayerDeck(true);
        Game.Instance.playerDeck.ShowCardFormDeck(deckIDs);
    }
    void RequestSeatData()
    {
        PhotonMessage.RequestSeatData();
    }
    
    void ReadyAccept(EventData photonEvent)
    {
        object[] aaa = photonEvent.CustomData as object[];
        var readyAccept = new ReadyAccept { actorNumber = (int)aaa[0], viewId = (byte)aaa[1]};
        if (readyAccept.actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            MyPlayer.Instance.Ready(true);
        }
        SeatManager.Instance.ReadyAccept(readyAccept);
    }
    void AcceptSit(EventData photonEvent)
    {
        object[] aaa = photonEvent.CustomData as object[];
        var acceptSit = new AcceptSit { actorNumber = (int)aaa[0], id = (byte)aaa[1] };
        var playerSit = PhotonNetwork.PlayerList.FirstOrDefault(player => player.ActorNumber == acceptSit.actorNumber);
       if(playerSit.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            MyPlayer.Instance.SitSeat(acceptSit.id);
        }
        foreach (var item in PhotonNetwork.PlayerList)
        {
            Debug.Log("name " + item.NickName);
            Debug.Log("userid " + item.UserId);
            Debug.Log("actornumber  " + item.ActorNumber);
        }
        acceptSit.player = playerSit;
        SeatManager.Instance.AcceptSit(acceptSit);

    }
    void GameReady()
    {
        Debug.Log("GAME READY !!!!!!!!!!!!!!");
    }
    void ReceiveSeatData(EventData photonEvent)
    {
        Debug.Log("getseatata " + photonEvent.CustomData);
        ExitGames.Client.Photon.Hashtable hash = photonEvent.CustomData as ExitGames.Client.Photon.Hashtable;
        var dic = hash.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var seats = dic["seats"] as Dictionary<object, object>;
        SeatManager.Instance.SetSeatData(seats);
        foreach (var item in seats)
        {
            var acceptSit = new AcceptSit { actorNumber = (int)item.Key, id = (byte)item.Value };
            var playerSit = PhotonNetwork.PlayerList.FirstOrDefault(player => player.ActorNumber == acceptSit.actorNumber);
            acceptSit.player = playerSit;
            SeatManager.Instance.AcceptSit(acceptSit);
        }
    }
    private IEnumerable<DictionaryEntry> CastDict(IDictionary dictionary)
    {
        foreach (DictionaryEntry entry in dictionary)
        {
            yield return entry;
        }
    }
}


class SeatData
{
    public Dictionary<int, byte> seats;
    public Dictionary<int, bool> playerReady;
}
