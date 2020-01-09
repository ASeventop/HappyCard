using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Ready_UI ui_ready;
    public UI_PlayerDeck playerDeck;
    public UI_GameResult ui_gameResult;
    public TextMeshProUGUI restartTime_txt;
    public Button b_exit;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        b_exit.OnClickAsObservable().Subscribe(_ =>
        {
            RoomManager.Instance.ExitRoom();
           
        });
    }
    public void SetLocalPlayerSit(bool isSit)
    {
        ui_ready.b_ready.gameObject.SetActive(isSit);
    }
    public void SetTimer(float time)
    {
        playerDeck.SetTimer(time);
    }
    //opengame result when game is end 
    public void OpenGameResult(Dictionary<object, object> data)
    {
        ui_gameResult.OpenGameResult(data);
    }
    public void CloseGameResult()
    {
        ui_gameResult.CloseGameResult();
    }
    public void RestartGame(float time)
    {
        restartTime_txt.DOFade(1, 0).SetAutoKill();
        restartTime_txt.text = string.Format("เกมจะเริ่มในอีก... {0}", time);
        restartTime_txt.DOFade(0, 1).SetAutoKill();
    }

}
