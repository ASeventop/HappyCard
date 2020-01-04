using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seventop.Page;
using UniRx;
using Seventop.Scenes;
using System;

public class Loading : Page
{

    public override void OnCreate()
    {
        flowController = FlowController.Get(SceneName.Loading);
        PageStack.Instance.SetCurrentFlowController(flowController, false);

        
    }
    

    public override void OnDestroy()
    {
       // throw new System.NotImplementedException();
    }

    public override void OnShow()
    {
        /*DataManager.Instance.Init(() =>
        {
            AssetManager.Instance.Init();
        });
        AssetManager.Instance.LoadComplete.Subscribe(loaded =>
        {
            Observable.IntervalFrame(2).Subscribe(_ =>
            {
                PageStack.Instance.CurrentSceneSwitch(SceneName.gameplay);
            }).AddTo(this);
        }).AddTo(this);*/
    }

}
