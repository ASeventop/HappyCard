using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex; //Number for the build index to the multiplay scene.

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this); 
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnJoinedRoom() //Callback function for when we successfully create or join a room.
    {
        Debug.Log("Joined Room");
        StartGame();
    }
    private void StartGame() //Function for loading into the multiplayer scene.
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            PhotonNetwork.LoadLevel(multiplayerSceneIndex); //because of AutoSyncScene all players who join the room will also be loaded into the multiplayer scene.
           // PageStack.Instance.CurrentSceneSwitch(SceneName.GamePlay);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("player " + otherPlayer.NickName + "lefted");
    }
}
