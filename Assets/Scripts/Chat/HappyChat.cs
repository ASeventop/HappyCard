using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HappyChat : MonoBehaviour, IChatClientListener
{
    [SerializeField] TextMeshProUGUI chat_txt;
    [SerializeField] TMP_InputField InputFieldChat;
    [SerializeField] GameObject chatObject;
    string UserName;
    protected internal AppSettings chatAppSettings;
    public ChatClient chatClient;
    void Start()
    {
        #if PHOTON_UNITY_NETWORKING
                this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        #endif
        Connect();
    }
    public void DebugReturn(DebugLevel level, string message)
    {
       
    }

    public void OnChatStateChange(ChatState state)
    {
       
    }
    public void Connect()
    {
        this.chatClient = new ChatClient(this);
        #if !UNITY_WEBGL
                this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        Debug.Log("this.chatAppSettings.AppIdChat "+ this.chatAppSettings.AppIdChat);
        UserName = PhotonNetwork.NickName;
        this.chatClient.Connect(this.chatAppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(UserName));

    }
    public void OnConnected()
    {
        Debug.Log("Onconnected");
        chatClient.Subscribe(new string[] { "channelA", "channelB" });
        //this.SendChatMessage("HI");
    }

    public void SendMessageFromInput(){
        SendChatMessage(InputFieldChat.text);
        InputFieldChat.text = "";
    }
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
           SendMessageFromInput();
        }
    }
    private void SendChatMessage(string inputMessage)
    {
        if(string.IsNullOrEmpty(inputMessage))return;
        chatClient.PublishMessage("channelA", inputMessage);
    }
        public void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }

        // check if we are missing context, which means we got kicked out to get back to the Photon Demo hub.
       /* if (this.StateText == null)
        {
            Destroy(this.gameObject);
            return;
        }

        this.StateText.gameObject.SetActive(this.ShowState); // this could be handled more elegantly, but for the demo it's ok.*/
    }

    public void OnDisconnected()
    {
       
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        string username ="";
        for (int i = 0; i < senders.Length; i++)
        {
            username = string.Format("<color=#35ADF8>{0}</color> : ", senders[i]);
            msgs = string.Format("{0}{1}\n", username, messages[i]);
        }
        chat_txt.text += msgs;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
       
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
      
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
       
    }

    public void OnUnsubscribed(string[] channels)
    {
      
    }

    public void OnUserSubscribed(string channel, string user)
    {
       
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
      
    }
    public void OpenChat()
    {
        chatObject.SetActive(!chatObject.activeSelf);
    }
    //Chat input
  
    void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }
}
