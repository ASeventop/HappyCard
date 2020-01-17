using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FriendList : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject root;
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject friend_prefab;//Friend prefab with class FriendsData
    // Start is called before the first frame update

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("UI_FriendList OnFriendListUpdate");
        base.OnFriendListUpdate(friendList);
        ClearFriendList();
        friendList.ForEach(friendInfo =>
        {
            Instantiate(friend_prefab).GetComponent<FriendsData>().SetData(friendInfo);
        });
    }
    void ClearFriendList()
    {
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            if(contentTransform.GetChild(0) != null)
                Destroy(contentTransform.GetChild(0));
        }
    }
    public void Open(bool isOpen)
    {
        root.gameObject.SetActive(isOpen);
    }
}
