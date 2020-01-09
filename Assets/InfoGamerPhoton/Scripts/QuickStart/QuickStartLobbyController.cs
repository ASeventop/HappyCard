using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button quickStartButton; //button used for creating and joining a game.
    [SerializeField]
    private GameObject quickCancelButton; //button used to stop searing for a game to join.
    [SerializeField]
    private int roomSize; //Manual set the number of player in the room at one time.

    public override void OnConnectedToMaster() //Callback function for when the first connection is established successfully.
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the master client has loaded is the scene all other clients will load
        quickStartButton.interactable = true;
    }

    public void QuickStart() //Paired to the Quick Start button
    {
        quickStartButton.interactable = false;
        quickCancelButton.SetActive(true);
        PhotonNetwork.NickName = "name" + Random.Range(0, 1000);
        PhotonNetwork.JoinRandomRoom(); //First tries to join an existing room
        Debug.Log("Quick start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Callback function for if we fail to join a rooom
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom() //trying to create our own room
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000); //creating a random name for the room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PlayerTtl = 100;
        roomOps.EmptyRoomTtl = 60000;
        roomOps.Plugins = new string[] {"MyFirstPlugin"}; // call plugin on create game
        roomOps.MaxPlayers = 6;
        roomOps.IsVisible = true;
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps); //attempting to create a new room
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //callback function for if we fail to create a room. Most likely fail because room name was taken.
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom(); //Retrying to create a new room with a different name.
    }

    public void QuickCancel() //Paired to the cancel button. Used to stop looking for a room to join.
    {
       // quickCancelButton.SetActive(false);
       // quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
