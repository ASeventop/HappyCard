using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seventop.Page;
using Seventop.Scenes;
using UnityEngine.UI;
using UnityEngine.Video;
using UniRx;
public class Intro : Page
{
    [Header("Button")]
    [SerializeField] Button b_skip;
    public VideoPlayer player;
    public override void OnCreate()
    {
        
        flowController = FlowController.Get(SceneName.intro);
        PageStack.Instance.SetCurrentFlowController(flowController, false);
        
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
        ToMainmenu();
    }
    void ToMainmenu()
    {
        player.Pause();
        
        //Destroy(player.gameObject);
        //player.Pause();
        //player.clip.
        PageStack.Instance.CurrentSceneSwitch(SceneName.Lobby);
    }

    public override void OnDestroy()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnShow()
    {
        //throw new System.NotImplementedException();
        player.loopPointReached += Player_loopPointReached;
        b_skip.onClick.AsObservable().Subscribe(_ =>
        {
            ToMainmenu();
        });
    }

   
}
