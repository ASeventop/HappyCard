using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seventop.Scenes;
using UnityEngine.SceneManagement;
public class PageStack : MonoBehaviour
{
    public static event System.Action<FlowController> OnPoped;
    static PageStack _instance;
    public static PageStack Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("FlowStack");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<PageStack>();
            }
            return _instance;
        }
    }
    public Stack<string> previousScene = new Stack<string>();
    public List<FlowController> previousPop = new List<FlowController>();
    public FlowController CurrentFlowController;
    public FlowController mainFlowController;
    public bool isInit = false;
    public FlowChart flowChart;
    private FlowState popupState;
    public void Init()
    {
        if (!isInit) isInit = true;
        if (flowChart == null) flowChart = new FlowChart();
    }
    public static void MoveScene(string sceneName, Dictionary<string, object> parameter = null)
    {
        _instance.CurrentFlowController.Navigate(sceneName, parameter, null);
    }
    public static void MoveScene(string sceneName, bool setPreviousFlowController, Dictionary<string, object> parameter = null, System.Action<IDictionary<string, object>> onPopped = null)
    {

        if (setPreviousFlowController)
        {
            _instance.SetPreviousFlowController(_instance.CurrentFlowController);
        }
        _instance.CurrentFlowController.Navigate(sceneName, parameter, onPopped);
    }
    public void SetCurrentFlowController(FlowController _controller, bool isPop = false)
    {
        if (isPop)
        {
            previousPop.Add(_controller);
            return;
        }
        else
        {
            mainFlowController = _controller;
        }
        CurrentFlowController = _controller;
    }
    public void RemoveFlowController(FlowController flowController)
    {
        if (previousPop.Contains(flowController))
        {
            previousPop.Remove(flowController);
        }

    }
    #region SceneManage
    public void CloseScene(string sceneName,string fromScene = "")
    {
        FlowController tempFlowController = null;
        FlowController targetPop = null;
        if (!string.IsNullOrEmpty(fromScene))
        {
            tempFlowController = previousPop.Find((FlowController target) => target.name == fromScene);
        }
        tempFlowController = (tempFlowController == null) ? CurrentFlowController : tempFlowController;
        targetPop = (targetPop == null) ? previousPop.Find(target => target.FlowState.SceneName == sceneName) : targetPop;
        if (targetPop != null)
        {
            previousPop.Remove(targetPop);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetPop.FlowState.SceneName));
            targetPop.Flow.SetCurrentState(targetPop.FlowState);
            targetPop.Pop();
            targetPop = null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(tempFlowController.FlowState.SceneName));
            tempFlowController.Flow.SetCurrentState(tempFlowController.FlowState);
        }
        targetPop = null;
    }
    //
    public void ClearPopup()
    {
        StartCoroutine(DelayClearPopup());
    }
    //
    public void CloseAllSceneAndMoveScene(string toScene, IDictionary<string, object> parameters = null)
    {
        StartCoroutine(DelayClearPopup(toScene, parameters));
    }
    IEnumerator DelayClearPopup(string forceMoveScene = "", IDictionary<string, object> parameters = null)
    {

        for (var i = previousPop.Count - 1; i >= 0; i--)
        {
            previousPop[i].Pop();
            previousPop.RemoveAt(i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.01f);
        if (!string.IsNullOrEmpty(forceMoveScene))
        {
            CurrentSceneSwitch(forceMoveScene, parameters);
        }
    }
    public void CurrentSceneSwitch(string toScene, IDictionary<string, object> parameters = null)
    {
        Debug.Log("mainflow " + mainFlowController);
        if (mainFlowController != null)
            mainFlowController.Navigate(toScene, parameters);
    }
    public void SetPreviousFlowController(FlowController _previousFlow)
    {
        if (_previousFlow == null) return;
        previousScene.Push(_previousFlow.FlowState.Name);
    }
    #endregion
}
