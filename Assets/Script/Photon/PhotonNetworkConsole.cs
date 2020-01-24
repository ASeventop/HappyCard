using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ExitGames.Client.Photon;
using System.IO;

public class PhotonNetworkConsole : MonoBehaviourPunCallbacks
{
    static PhotonNetworkConsole instance;
    public string gameVersion;
    string nickName;
    public Subject<bool> isConnected;
    public static PhotonNetworkConsole Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("PhotonNetworkConsole", typeof(PhotonNetworkConsole)).GetComponent<PhotonNetworkConsole>();
                DontDestroyOnLoad(instance.gameObject);
                instance.Init();
            }
            return instance;
        }
    }
    private void Start()
    {
        ACustom.RegisterTypes();
    }
    private void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        isConnected = new Subject<bool>();
       
    }
    public void Connect(string _nickName,string userid)
    {
        nickName = _nickName;
        AuthenticationValues authValue = new AuthenticationValues(userid);
        PhotonNetwork.AuthValues = authValue;
        PhotonNetwork.NickName = _nickName;
        PhotonNetwork.SendRate = 10;
        PhotonNetwork.SerializationRate = 5;
        PhotonNetwork.AutomaticallySyncScene = true;
        
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("onjoined lobby ");
        UserManager.Instance.Friends = new string[] { "0", "1", "2" };
        PhotonNetwork.FindFriends(UserManager.Instance.Friends);
        //test custom type
     

    }
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
        Debug.Log("OnFriendListUpdate ");
        friendList.ForEach(friend =>
        {
            Debug.Log(string.Format("id {0} , name {1} ", friend.UserId, friend.IsOnline));
        });
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.LogWarning("on OnConnectedToMaster");
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.JoinLobby();
        isConnected.OnNext(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Disconnected " + cause);
        isConnected.OnNext(false);
    }
    public override void OnConnected()
    {
        base.OnConnected();
        Debug.LogWarning("on connected");
        isConnected.OnNext(true);
    }

    
}

static internal class ACustom
{
    internal static void RegisterTypes()
    {
        //  PhotonPeer.RegisterType(Type customType, byte code, SerializeMethod serializeMethod, DeserializeMethod deserializeMethod)
        //PhotonPeer.RegisterType(typeof(Vector2), (byte)'W', SerializeVector2, DeserializeVector2);
        PhotonPeer.RegisterType(typeof(CustomSerialization), (byte)'A', CustomSerialization.Serialize, CustomSerialization.Deserialize);
        PhotonPeer.RegisterType(typeof(CT_PlayerDeckUpdate), (byte)'C',CT_PlayerDeckUpdate.Serialize, CT_PlayerDeckUpdate.Deserialize);
        PhotonPeer.RegisterType(typeof(CT_RequestDeckUpdate),(byte)'D',CT_RequestDeckUpdate.Serialize,CT_RequestDeckUpdate.Deserialize);
        PhotonPeer.RegisterType(typeof(CT_PlayerSummary),(byte)'E',CT_PlayerSummary.Serialize,CT_PlayerSummary.Deserialize);
    }
}
[System.Serializable]
public class CustomSerialization
{
    public byte Id { get; set; }
    public static byte[] Serialize(object customType)
    {
        var c = (CustomSerialization)customType;
        return new byte[] { c.Id};
    }
    public static object Deserialize(byte[] bytes)
    {
        var result = new CustomSerialization { Id = bytes[0] };
        return result;
    }
}
[System.Serializable]
public class CT_PlayerDeckUpdate

{
    public int actorNr;
    public int cards_length;
    public int ranks_length;
    public int higherCards_length;
    public byte[] cards;
    public byte[] ranks;
    public byte[] higherCards;
    public byte[] swapCard;
    public bool isRule;
    public static byte[] Serialize(object o)
    {
        CT_PlayerDeckUpdate customType = o as CT_PlayerDeckUpdate;
        if (customType == null) { return null; }
        using (var s = new MemoryStream())
        {
            using (var bw = new BinaryWriter(s))
            {
                bw.Write(customType.actorNr);
                bw.Write(customType.cards_length);
                bw.Write(customType.ranks_length);
                bw.Write(customType.higherCards_length);
                bw.Write(customType.cards);
                bw.Write(customType.ranks);
                bw.Write(customType.higherCards);
                bw.Write(customType.swapCard);
                bw.Write(customType.isRule);
                return s.ToArray();
            }
        }
    }
    public static object Deserialize(byte[] bytes)
    {
        CT_PlayerDeckUpdate customObject = new CT_PlayerDeckUpdate();
        using (var s = new MemoryStream(bytes))
        {
            using (var br = new BinaryReader(s))
            {
                customObject.actorNr = br.ReadInt32();
                customObject.cards_length = br.ReadInt32();
                customObject.ranks_length = br.ReadInt32();
                customObject.higherCards_length = br.ReadInt32();
                customObject.cards = br.ReadBytes(customObject.cards_length);
                customObject.ranks = br.ReadBytes(customObject.ranks_length);
                customObject.higherCards = br.ReadBytes(customObject.higherCards_length);
                customObject.swapCard = br.ReadBytes(2);
                customObject.isRule = br.ReadBoolean();
            }
        }
        return customObject;
    }
}

[System.Serializable]
public class CT_RequestDeckUpdate{
    public byte[] cards;
    public byte[] swapCard; //0 from 1 to
    public static byte[] Serialize(object o){
        CT_RequestDeckUpdate customType = o as CT_RequestDeckUpdate;
        using (var s = new MemoryStream())
        {
            using (var bw = new BinaryWriter(s))
            {
                bw.Write(customType.cards);
                bw.Write(customType.swapCard);
                return s.ToArray();
            }
        }
    }
    public static object Deserialize(byte[] bytes){
CT_RequestDeckUpdate customObject = new CT_RequestDeckUpdate();
        using (var s = new MemoryStream(bytes))
        {
            using (var br = new BinaryReader(s))
            {
                customObject.cards = br.ReadBytes(8);
                customObject.swapCard = br.ReadBytes(2);
            }
        }
        return customObject;
    }
}
 [System.Serializable]
    public class CT_PlayerSummary
    {
        public bool isRule;
        public bool winAllPlayer;
        public bool loseAllPlayer;
        public int pointTotal;
        public float currencyAmount;
        public byte gameCurrency;
        public static byte[] Serialize(object o)
        {
            CT_PlayerSummary customType = o as CT_PlayerSummary;
            if (customType == null) { return null; }
            using (var s = new MemoryStream())
            {
                using (var bw = new BinaryWriter(s))
                {
                    bw.Write(customType.isRule);
                    bw.Write(customType.winAllPlayer);
                    bw.Write(customType.loseAllPlayer);
                    bw.Write(customType.pointTotal);
                    bw.Write(customType.currencyAmount);
                    bw.Write(customType.gameCurrency);
                    return s.ToArray();
                }
            }
        }
        public static object Deserialize(byte[] bytes)
        {
            CT_PlayerSummary customObject = new CT_PlayerSummary();
            using (var s = new MemoryStream(bytes))
            {
                using (var br = new BinaryReader(s))
                {
                    customObject.isRule = br.ReadBoolean();
                    customObject.winAllPlayer = br.ReadBoolean();
                    customObject.loseAllPlayer = br.ReadBoolean();
                    customObject.pointTotal = br.ReadInt32();
                    customObject.currencyAmount = br.ReadSingle();
                    customObject.gameCurrency = br.ReadByte();
                }
            }
            return customObject;
        }
    } 