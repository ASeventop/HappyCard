using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Seventop.Scenes
{
    // flow runtime
    public class Flow
    {
        private bool _isStarted;
        private FlowChart _flowChart;
        private FlowState _currentState;
        private Stack<RuntimeState> _currentStatesStack = new Stack<RuntimeState>();

        private class RuntimeState
        {
            public FlowState state;
            public FlowController controller;
            public RuntimeState parent;
        }

        public Flow(FlowChart flowChart)
        {
            this._flowChart = flowChart;
        }

        public bool IsReadyToNavigate
        {
            get { return FlowEngine.IsReady; }
        }

        public void Execute(IDictionary<string, object> parameters = null)
        {
            Debug.Assert(_currentStatesStack.Count == 0);
            Debug.Assert(!_isStarted);
            Debug.Assert(_flowChart.Root != null);

            if (_isStarted || _flowChart.Root == null)
                return;
            _isStarted = true;

            _PushActiveState(_flowChart.Root, parameters);

            var _tmp = _currentState;
            FlowEngine.Replace(_currentState.SceneName, (scene) => 
            {
                Debug.Assert(scene.name == _tmp.SceneName);
            }, true);
        }

        public void Navigate(string flowStateName, IDictionary<string, object> parameters = null, System.Action<IDictionary<string, object>> onPopped = null)
        {
            if (!_ValidateNavigation())
                return;
            
            FlowChart.FlowNavigation navigation;
            if(!this._flowChart.TryGetNavigation(this._currentState.Name, flowStateName, out navigation))
            {
                Debug.LogError("can't find flow navigation from " + this._currentState.Name + " to " + flowStateName);
                return;
            }
            Debug.Log("flow navigation from " + this._currentState.Name + " to " + flowStateName);
            Debug.Assert(navigation.from == this._currentState);
            Debug.Assert(navigation.to.Name == flowStateName);
            string errorMessage;
            if(!navigation.to.ValidateParameters(parameters, out errorMessage))
            {
                Debug.LogError("invalid navigation parameters : " + errorMessage);
                return;
            }

            // navigate
            if (navigation.navigationType == FlowNavigationType.Switch)
            {
                _PopActiveState(null);
                _PushActiveState(navigation.to, parameters, onPopped);
               
                var _tmp = _currentState;
                FlowEngine.Switch(navigation.to.SceneName, (scene) => 
                {
                    Debug.Assert(scene.name == _tmp.SceneName);
                });
            }
            else if (navigation.navigationType == FlowNavigationType.Stack)
            {
                _PushActiveState(navigation.to, parameters, onPopped);

                var _tmp = _currentState;
                FlowEngine.Push(navigation.to.SceneName, (scene) => 
                {
                    Debug.Assert(scene.name == _tmp.SceneName);
                });
            }
            else
            {
                Debug.LogError("navigation type " + navigation.navigationType + " is not supported");
                return;
            }
        }

        public void Pop(IDictionary<string, object> parameters = null)
        {
            if (!_ValidateNavigation())
                return;

            Debug.Log("FlowEngine.CurrentScene " + FlowEngine.CurrentScene);
            Debug.Log("_currentState.SceneName " + _currentState.SceneName);
            if (FlowEngine.CurrentScene != _currentState.SceneName)
            {
                Debug.LogError("invalid pop, active scene and current flow state mismatched");
                return;
            }

            var sceneToPop = _currentState.SceneName;
            _PopActiveState(parameters);
            FlowEngine.Pop(sceneToPop);
        }

        private bool _ValidateNavigation()
        {
            if (!IsReadyToNavigate)
            {
                Debug.LogError("flow is not ready to navigate, check with IsReadyToNavigate first ");
                return false;
            }
            if (!_isStarted)
            {
                Debug.LogError("can't navigate because flow has not yet started");
                return false;
            }
            if (_currentState == null)
            {
                Debug.LogError("can't navigate because current state does not exist");
                return false;
            }
            return true;
        }

        private void _PopActiveState(IDictionary<string, object> parameters = null)
        {
            var state = _currentStatesStack.Pop();
            var top = _currentStatesStack.Count > 0 ? _currentStatesStack.Peek() : null;
            _currentState = top != null ? top.state : null;
            GameObject.Destroy(state.controller.gameObject);

            if(state.controller.OnPopped != null) 
            {
                state.controller.OnPopped(parameters);
            }
            // fix when poped. set active scene is previous form index
            if(_currentState != null)
            {
                Scene scene = SceneManager.GetSceneByName(_currentStatesStack.Peek().state.SceneName);
                if (!scene.IsValid()) return;
                SceneManager.SetActiveScene(scene);
            }
        }

        private void _PushActiveState(FlowState flowState, IDictionary<string, object> parameters, System.Action<IDictionary<string, object>> onPopped = null)
        {
            var parent = _currentStatesStack.Count > 0 ? _currentStatesStack.Peek() : null;

            _currentState = flowState;
            var flowController = FlowController.Create(this, _currentState);
            flowController._SetParameters(parameters, onPopped);
           
            var state = new RuntimeState()
            {
                state = flowState,
                controller = flowController,
                parent = parent
            };
            _currentStatesStack.Push(state);
        }

        public void SetCurrentState(FlowState flowState)
        {
            _currentState = flowState;
        }
    }
}