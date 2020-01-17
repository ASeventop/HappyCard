using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
public class FriendsData : MonoBehaviour
{
    [SerializeField] Image player_img;
    [SerializeField] TextMeshProUGUI player_name_txt,online_txt;
    [SerializeField] Button b_join,b_invite;
    FriendInfo friendInfo;
    private void Start()
    {
        b_join.OnClickAsObservable().Subscribe(_ =>
        {

        });
        b_invite.OnClickAsObservable().Subscribe(_ =>
        {
            
        });
    }
    public void SetData(FriendInfo _info)
    {
        friendInfo = _info;
        player_name_txt.text = friendInfo.Name;
        online_txt.color = friendInfo.IsOnline ? Color.green : Color.red;
        online_txt.text = friendInfo.IsOnline ? "Online" : "Offline";
        b_invite.gameObject.SetActive(!friendInfo.IsInRoom);
    }
}
