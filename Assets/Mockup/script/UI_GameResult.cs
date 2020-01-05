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
    public GameObject summary_gameObject;
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
    /// <summary>
    /// summary result
    /// </summary>
    Dictionary<int, int> playerSummary = new Dictionary<int, int>();
    [SerializeField] TextMeshProUGUI playerName_txt;
    [SerializeField] TextMeshProUGUI playerSummaryPoint_txt;
    [SerializeField] TextMeshProUGUI[] duelName_txt;
    [SerializeField] TextMeshProUGUI[] duelSummaryPoint_txt;
    [SerializeField] Image[] duelBGSummary;

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
        summary_gameObject.SetActive(false);
        objResult.SetActive(false);
    }
    public void OpenGameResult(Dictionary<object, object> data) {
        
       
        Reset();
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
            Debug.Log("summary point " + playerData.ContainsKey("summary"));
            playerSummary.Add((int)player.Key, (int)playerData["summary"]);
        }
        NextDuel();
    }

    void NextDuel()
    {
        if (listIndex >= duelList.Count)
        {
            OpenSummary();
            return;
        }
      
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
           /* if(decktotal == 0)
            {
                message = string.Format("เสมอ {0} Point", decktotal);
            }else if(decktotal > 0)
            {
                message = string.Format("ชนะ {0} Point", decktotal);
            }
            else
            {
                message = string.Format("แพ้ {0} Point", decktotal);
            }*/
            bgPointSprite[i].sprite = decktotal > 0 ? winSprite : LoseSprite;
            playerPoint_txt[i].text = string.Format("{0} {1} Point ", ValueString(decktotal, "ชนะ", "แพ้", "เสมอ"), decktotal); ; ;
            playerPoint_txt[i].color = decktotal > 0 ? winColor : loseColor;
        }
        var playerMessage = "";
        var duelMessage = "";
        int pointResult = (int)deckTotals[listIndex]["pointResult"]; 
        float currenyAmount = (float)deckTotals[listIndex]["currencyAmount"]; 
        float duelAmount = (float)deckTotals[listIndex]["duelCurrencyAmount"];
        /*if (pointResult > 0)
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
        }*/
        playerBGTotalPoint.sprite = pointResult > 0 ? winSprite : LoseSprite;
        playerResult_txt.text = string.Format("{0} {1} Point ({2} {3})",ValueString(pointResult,"ชนะ","แพ้","เสมอ"),pointResult, duelAmount, "Tik");
        playerResult_txt.color = pointResult > 0 ? winColor : loseColor;

        duelBGTotalPoint.sprite = pointResult > 0 ? LoseSprite : winSprite;
        duelResult_txt.color = pointResult > 0 ? loseColor : winColor;
        duelResult_txt.text = duelMessage;
    }
    void OpenSummary()
    {
        objResult.SetActive(false);
        summary_gameObject.SetActive(true);
        var ind = 0;
        foreach (var summary in playerSummary)
        {
            if ((int)summary.Key == PhotonNetwork.LocalPlayer.ActorNumber) {

                playerName_txt.text = PhotonNetwork.CurrentRoom.GetPlayer(summary.Key).NickName;
                playerSummaryPoint_txt.text = summary.Value.ToString();
                playerSummaryPoint_txt.color = summary.Value > 0 ? winColor : loseColor;
                playerSummaryPoint_txt.text = string.Format("รวมแล้ว{0} {1} Point ({2} {3}))", ValueString(summary.Value, "ได้", "เสีย", "เสมอ"), summary.Value, 999, "Tik");
            }
            else
            {
                duelName_txt[ind].gameObject.SetActive(true);
                duelSummaryPoint_txt[ind].gameObject.SetActive(true);
                duelBGSummary[ind].gameObject.SetActive(true);

                duelName_txt[ind].text = PhotonNetwork.CurrentRoom.GetPlayer(summary.Key).NickName;
                //duelSummaryPoint_txt[ind].text = summary.Value.ToString();
                duelBGSummary[ind].sprite = summary.Value > 0 ? winSprite : LoseSprite;
                duelSummaryPoint_txt[ind].color = summary.Value > 0 ? winColor : loseColor;
                duelSummaryPoint_txt[ind].text = string.Format("รวมแล้ว{0} {1} Point ({2} {3}))", ValueString(summary.Value, "ได้", "เสีย", "เสมอ"), summary.Value, 999, "Tik");
                ind++;
            }
        }
    }
    private void Reset()
    {
        listIndex = 0;
        duelList.Clear();
        playerSummary.Clear();
        objResult.SetActive(true);
        for (int i = 0; i<duelName_txt.Length; i++)
			{
                duelName_txt[i].gameObject.SetActive(false);
                duelSummaryPoint_txt[i].gameObject.SetActive(false);
                duelBGSummary[i].gameObject.SetActive(false);
            }
    }
    string ValueString(int value , string winString,string loseString ,string drawString)
    {
        if (value > 0)
            return winString;
        else if (value < 0)
            return loseString;
        else
            return drawString;
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