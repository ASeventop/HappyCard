using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seventop.Page;
using Seventop.Scenes;
using System;
using UniRx;
using UnityEngine.UI;
public class GamePlay : Page
{
    public static event Action Created;
    public static event Action Destroyed;
    public Button b_tutorial_play;
    public GameObject Object_tutorial;
    public override void OnCreate()
    {
        flowController = FlowController.Get(SceneName.GamePlay);
        PageStack.Instance.SetCurrentFlowController(flowController, false);
    }

    public override void OnDestroy()
    {

    }

    public override void OnShow()
    {

    }
}
