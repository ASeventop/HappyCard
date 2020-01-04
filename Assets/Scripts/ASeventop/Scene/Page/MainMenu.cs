using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Seventop.Page;
using Seventop.Scenes;
using UnityEngine.Video;
public class MainMenu : Page
{
    /*[Header("Button")]
    [SerializeField] Button b_play;
    [SerializeField] Button b_learning;
    [SerializeField] Button b_skip;
    [SerializeField] GameObject learningObject;
    [SerializeField] GameObject objectIntro;
    [SerializeField] GameObject objectMainmenu;
    [SerializeField] VideoPlayer videoPlayer;
    public override void OnCreate()
    {
        
        flowController = FlowController.Get(SceneName.mainmenu);
        PageStack.Instance.SetCurrentFlowController(flowController, false);
        b_play.onClick.AsObservable().Subscribe(_ =>{
            PageStack.Instance.CurrentSceneSwitch(SceneName.loading);
        });
        b_learning.onClick.AsObservable().Subscribe(_ =>
        {
            learningObject.gameObject.SetActive(true);
        });
        
        b_skip.onClick.AsObservable().Subscribe(_ =>
        {
            CloseIntro();
            OpenMainMenu();
        });
        if (PlayerPrefs.HasKey("intro"))
        {
            CloseIntro();
            OpenMainMenu();
        }
        else
        {
            PlayerPrefs.SetInt("intro", 1);
        }
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        CloseIntro();
        OpenMainMenu();
    }

    public void CloseIntro()
    {
        Debug.Log("closeintro");
        videoPlayer.Stop();
        objectIntro.SetActive(false);
    }
    public void OpenMainMenu()
    {
        objectMainmenu.SetActive(true);
        SoundManager.Instance.playBGM(AudioName.BGM_1);
    }
    public override void OnDestroy()
    {
        videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
    }

    public override void OnShow()
    {
       
    }*/
    public override void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
        throw new System.NotImplementedException();
    }

    public override void OnShow()
    {
        throw new System.NotImplementedException();
    }
}
