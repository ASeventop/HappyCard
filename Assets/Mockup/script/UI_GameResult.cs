using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UniRx;
public class UI_GameResult : MonoBehaviourPun
{
    public GameObject objResult;
    [SerializeField] Image[] playerCards;
    [SerializeField] Image[] duelCards;
    [SerializeField] Image[] bgPointSprite;
    [SerializeField] Image playerBGTotalPoint,duelBGTotalPoint;
    [SerializeField] TextMeshProUGUI[] playerPoint_txt;
    [SerializeField] TextMeshProUGUI playerResult_txt;
    [SerializeField] TextMeshProUGUI duelResult_txt;
    [SerializeField] TextMeshProUGUI player_name_txt;
    [SerializeField] TextMeshProUGUI duel_name_txt;
    [SerializeField] Button b_next;
    [SerializeField] Color winColor, loseColor;
    public Sprite winSprite, LoseSprite;
    List<Dictionary<object, object>> duelList = new List<Dictionary<object, object>>();
    ExitGames.Client.Photon.Hashtable[] deckTotals;
    int listIndex = 0;
    private void Start()
    {
        b_next.onClick.AsObservable().Subscribe(_ =>
        {
            listIndex++;
            NextDuel();
        });
        
    }
    public void CloseGameResult()
    {
        objResult.SetActive(false);
    }
    public void OpenGameResult(Dictionary<object, object> data) {
        listIndex = 0;
        duelList.Clear();
        objResult.SetActive(true);
       
        foreach (var player in data)
        {
            var playerData = player.Value as Dictionary<object, object>;
            if ((int)player.Key == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                byte[] cards = playerData["cards"] as byte[];
                player_name_txt.text = PhotonNetwork.CurrentRoom.GetPlayer((int)player.Key).NickName;
                for (int i = 0; i < playerCards.Length; i++)
                {
                    playerCards[i].sprite = AssetManager.GetSprite(""+cards[i]);
                }
                deckTotals = playerData["totals"] as ExitGames.Client.Photon.Hashtable[];
            }
            else
            {
                duelList.Add(player.Value as Dictionary<object, object>);
            }
           
        }
        NextDuel();
    }

    void NextDuel()
    {
        if (listIndex >= duelList.Count) return;
      
        var other = duelList[listIndex];
        duel_name_txt.text = PhotonNetwork.CurrentRoom.GetPlayer((int)deckTotals[0]["duelActor"]).NickName;
        byte[] cards = other["cards"] as byte[];
        for (int i = 0; i < duelCards.Length; i++)
        {
            duelCards[i].sprite = AssetManager.GetSprite("" + cards[i]);
        }
        sbyte[] points = deckTotals[listIndex]["results"] as sbyte[];
        for (int i = 0; i < points.Length; i++)
        {
            var decktotal = points[i];
            string message = "";
            if(decktotal == 0)
            {
                message = string.Format("เสมอ {0} Point", decktotal);
            }else if(decktotal > 0)
            {
                message = string.Format("ชนะ {0} Point", decktotal);
            }
            else
            {
                message = string.Format("แพ้ {0} Point", decktotal);
            }
            bgPointSprite[i].sprite = decktotal > 0 ? winSprite : LoseSprite;
            playerPoint_txt[i].text = message;
            playerPoint_txt[i].color = decktotal > 0 ? winColor : loseColor;
        }
        var playerMessage = "";
        var duelMessage = "";
        int pointResult = (int)deckTotals[listIndex]["pointResult"]; 
        float currenyAmount = (float)deckTotals[listIndex]["currencyAmount"]; 
        float duelAmount = (float)deckTotals[listIndex]["duelCurrencyAmount"];
        if (pointResult > 0)
        {
            playerMessage = string.Format("ชนะ {0} Point (+{1} {2})", pointResult, currenyAmount,"Tik");
            duelMessage = string.Format("แพ้ {0} Point ({1} {2})", pointResult, duelAmount, "Tik");
        }
        else if (pointResult < 0)
        {
            playerMessage = string.Format("แพ้ {0} Point (-{1} {2})", pointResult, currenyAmount, "Tik");
            duelMessage = string.Format("ชนะ {0} Point ({1} {2})", pointResult, duelAmount, "Tik");
        }
        else
        {
            playerMessage = string.Format("เสมอ {0} Point ({1} {2})", pointResult, currenyAmount, "Tik");
            duelMessage = string.Format("เสมอ {0} Point ({1} {2})", pointResult, duelAmount, "Tik");
        }
        playerBGTotalPoint.sprite = pointResult > 0 ? winSprite : LoseSprite;
        playerResult_txt.text = playerMessage;
        playerResult_txt.color = pointResult > 0 ? winColor : loseColor;

        duelBGTotalPoint.sprite = pointResult > 0 ? LoseSprite : winSprite;
        duelResult_txt.color = pointResult > 0 ? loseColor : winColor;
        duelResult_txt.text = duelMessage;
    }
    
}
public class PlayerDuelTotal
{
    public object actor;
    public object duelActor;
    public sbyte[] results;
    public int pointResult;
    public float amount;
}