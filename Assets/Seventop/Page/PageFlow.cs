using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seventop.Scenes;
using Seventop.Page;
public class PageFlow : MonoBehaviour
{
   // List<FlowState> flowStates = new List<FlowState>();
    FlowChart flowChart;
    private void Awake()
    {
        PageStack.Instance.Init();
        flowChart = PageStack.Instance.flowChart;
        var scene_a = new FlowState(SceneName.A, SceneName.A);
        var scene_b = new FlowState(SceneName.B, SceneName.B);
        var intro = new FlowState(SceneName.intro, SceneName.intro);
        var lobby = new FlowState(SceneName.Lobby, SceneName.Lobby);
        var loading = new FlowState(SceneName.Loading, SceneName.Loading);
        var gameplay = new FlowState(SceneName.GamePlay, SceneName.GamePlay);
        var endgame = new FlowState(SceneName.endgame, SceneName.endgame);
        flowChart.SetRootState(lobby);
        flowChart.AddNavigation(scene_a, scene_b, FlowNavigationType.Switch);
        flowChart.AddNavigation(scene_b, scene_a, FlowNavigationType.Switch);
        flowChart.AddNavigation(intro, lobby, FlowNavigationType.Switch);
        flowChart.AddNavigation(lobby, gameplay, FlowNavigationType.Switch);
        flowChart.AddNavigation(loading, gameplay, FlowNavigationType.Switch);
        flowChart.AddNavigation(gameplay, endgame, FlowNavigationType.Switch);
        flowChart.AddNavigation(gameplay, lobby, FlowNavigationType.Switch);
        flowChart.AddNavigation(endgame, lobby, FlowNavigationType.Switch);
        flowChart.AddNavigation(endgame, loading, FlowNavigationType.Switch);

        StartCoroutine(GoExecute());
    }
    IEnumerator GoExecute()
    {
        yield return new WaitForSeconds(1f);
        Execute();
    }
    public void Execute()
    {
        var param = new Dictionary<string, object>();
        flowChart.Execute(param);
    }
}
public static class SceneName
{
    public const string A = "SceneA";
    public const string B = "SceneB";
    public const string intro = "intro";
    public const string Lobby = "Lobby";
    public const string Loading = "Loading";
    public const string GamePlay = "GamePlay";
    public const string endgame = "endgame";
}
