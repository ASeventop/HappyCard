using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Seat : MonoBehaviourPun,IPunObservable
{
    [SerializeField] GameObject obj_sit, obj_player;
    [Header("Sit when player not sit")]
    [SerializeField] Button b_sit;
    [Header("Player")]
    [SerializeField] Button b_ready;
    [SerializeField] Image img_ready;
    [SerializeField] TextMeshProUGUI name_txt;
    Player player;
    public PhotonView photonView;
    public int viewID;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        viewID = photonView.ViewID;
        Debug.Log("photon view " + photonView.ViewID);
        SeatManager.Instance.RegisterSeat(this);
        b_sit.OnClickAsObservable().Subscribe(_ =>
        {
            SitRequest();
        });
        b_ready.OnClickAsObservable().Subscribe(_ =>
        {
            ReadyRequest();
        });
    }
    void SitRequest()
    {
        byte[] content = new byte[] {(byte)photonView.ViewID};
        PhotonMessage.RaiseEvent(EventCode.SitRequest, content, ReceiverGroup.MasterClient);
    }
    public void Occupied(AcceptSit accept)
    {
        obj_sit.SetActive(false);
        obj_player.SetActive(true);
        player = accept.player;
        name_txt.text = player.NickName;
      
    }
    void ReadyRequest()
    {
        PhotonMessage.PlayerReadyRequest((byte)player.ActorNumber, (byte)viewID);
    }
    public void Ready()
    {
        img_ready.color = Color.green;
        b_ready.gameObject.SetActive(false);
    }


    //ipunobserv
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }
}
