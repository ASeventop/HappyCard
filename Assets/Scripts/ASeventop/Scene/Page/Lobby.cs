using Seventop.Page;
using Seventop.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : Page
{
    public override void OnCreate()
    {
        flowController = FlowController.Get(SceneName.Lobby);
        PageStack.Instance.SetCurrentFlowController(flowController, false);
    }
    public override void OnShow()
    {

    }
    public override void OnDestroy()
    {
      
    }

   
}
