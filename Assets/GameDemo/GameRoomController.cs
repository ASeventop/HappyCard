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
    public Subject<bool> isConnected;
    private void Start()
    {
        photonView = PhotonView.Get(this);
        AssetManager.Instance.Init();
        Debug.Log(string.Format("Actor ID {0}",PhotonNetwork.LocalPlayer.ActorNumber));
        MyPlayer.Instance.SetLocalPlayer(PhotonNetwork.LocalPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
       // Debug.Log("player " + otherPlayer.NickName + "lefted ");
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
       // Debug.Log(string.Format("OnEvent {0} : ({1})   , Sender = {2}", (EventCode)photonEvent.Code, photonEvent.Code, photonEvent.Sender));
        CheckEventCode((EventCode)photonEvent.Code,photonEvent);
        // Debug.Log("----------onvent " + (EventCode)photonEvent.Code+",,"+photonEvent.Code+ "sender "+ photonEvent.Sender+" data "+photonEvent.CustomData);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnected = new Subject<bool>();
        isConnected.OnNext(false);
    }
    public void OnEnable()
    {
        
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        RequestSeatData();


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
            case EventCode.PlayerLeave:
                OnPlayerLeave(photonEvent);
                break;
            case EventCode.Rejoin:
                OnplayerRejoin(photonEvent);
                break;
            case EventCode.TestData:
                OnTestSerailizeData(photonEvent);
                break;
            case EventCode.ConfirmPlayerDeck:
                OnDeackUpdateConfirm(photonEvent);
                break;

        }
    }
    void OnDeackUpdateConfirm(EventData photonEvent)
    {
        var data = photonEvent.CustomData as byte[];
        var sender = photonEvent.Sender;
        Debug.Log("sender " + sender);
        CT_PlayerDeckUpdate playerDeckUpdate = CT_PlayerDeckUpdate.Deserialize(data) as CT_PlayerDeckUpdate;
        Debug.Log("Deserializecompleted--------------------- ");
       
        var allCard = "";
        for (int i = 0; i < playerDeckUpdate.cards_length; i++)
        {
            allCard += playerDeckUpdate.cards[i] + ",";
        }
        var allRank = "";
        for (int i = 0; i < playerDeckUpdate.ranks_length; i++)
        {
            allRank += playerDeckUpdate.ranks[i] + ",";
        }
        var allhigherCards = "";
        for (int i = 0; i < playerDeckUpdate.higherCards_length; i++)
        {
            allhigherCards += playerDeckUpdate.higherCards[i] + ",";
        }
        Debug.Log("allcard = " + allCard);
        Debug.Log("allRank = " + allRank);
        Debug.Log("allHigherCard = " + allhigherCards);
        Debug.Log("card isrule " + playerDeckUpdate.isRule);
        UIManager.Instance.UpdateCardDeck(playerDeckUpdate);
    }
    void OnTestSerailizeData(EventData photonEvent)
    {
        var data = photonEvent.CustomData as byte[];
        var type = CustomPluginType.Deserialize(data) as CustomPluginType;
        Debug.Log("get custompluginType " + type.byteField + " int " + type.intField + " string " + type.stringField);
    }
    void OnplayerRejoin(EventData photonEvent)
    {
        PhotonMessage.RequestFullState();
        PhotonNetworkConsole.Instance.OnConnected();
    }
    void OnPlayerLeave(EventData photonEvent)
    {
        Debug.Log("OnplayerLeave");
        var data = photonEvent.CustomData as int[];
        for (int i = 0; i < data.Length; i++)
        {
            SeatManager.Instance.RemoveSit(data[i]);
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
        acceptSit.player = playerSit;
        SeatManager.Instance.AcceptSit(acceptSit);

    }
    void GameReady()
    {
        Debug.Log("GAME READY !!!!!!!!!!!!!!");
    }
    void ReceiveSeatData(EventData photonEvent)
    {
        ExitGames.Client.Photon.Hashtable hash = photonEvent.CustomData as ExitGames.Client.Photon.Hashtable;
        Debug.Log("On Receive seat data");
        Debug.Log("hash  :  " + hash);

        Debug.Log("------Key name-----");
        foreach (var item in hash)
        {
            Debug.Log(string.Format("Key {0} Value {1} ", item.Key, item.Value));
        }

        var dic = hash.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var seats = dic["seats"] as Dictionary<object, object>;
        SeatManager.Instance.SetSeatData(seats);
        Debug.Log("------Seat data-----");
        foreach (var seat in seats)
        {
            Debug.Log(string.Format("seat Key : {0} , seat Value : {1}  ", seat.Key, seat.Value));

            var acceptSit = new AcceptSit { actorNumber = (int)seat.Key, id = (byte)seat.Value };
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
    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}


class SeatData
{
    public Dictionary<int, byte> seats;
    public Dictionary<int, bool> playerReady;
}
