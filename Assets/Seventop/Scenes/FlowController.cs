using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Seventop.Scenes
{
    public class FlowController : MonoBehaviour
    {
        internal static FlowController Create(Flow flow, FlowState flowState)
        {
            var test = GameObject.Find("FlowController_" + flowState.Name);
            if (test != null)
            {
                Debug.LogError("redundant flow controller creation : " + flowState.Name);
                return null;
            }
            var gobj = new GameObject("FlowController_" + flowState.Name);
            DontDestroyOnLoad(gobj);
            var flowController = gobj.AddComponent<FlowController>();
            flowController._Init(flow, flowState);
            return flowController;
        }

        public static FlowController Get(string flowStateName)
        {
            var gobj = GameObject.Find("FlowController_" + flowStateName); 
            if(gobj == null)
            {
                Debug.LogError("Unable to find FlowController for state : " + flowStateName);
                return null;
            }

            var flowController = gobj.GetComponent<FlowController>();
            if(flowController == null)
            {
                Debug.LogError("Unable to find FlowController Component");
                return null;
            }

            return flowController;
        }

        public Flow Flow { get; private set; }
        public FlowState FlowState { get; private set; }
        public ReadOnlyDictionary<string, object> Parameters { get; private set; }
        public System.Action<IDictionary<string, object>> OnPopped;

        internal void _Init(Flow flow, FlowState flowState)
        {
            this.Flow = flow;
            this.FlowState = flowState;
        }

        internal void _SetParameters(IDictionary<string, object> parameters = null, System.Action<IDictionary<string, object>> onPopped = null)
        {
            string errorMessage;
            if(!this.FlowState.ValidateParameters(parameters, out errorMessage))
            {
               Debug.LogError("invalid parameters : " + errorMessage);
                return;
            }

            this.Parameters = new ReadOnlyDictionary<string, object>(parameters ?? new Dictionary<string, object>());
            this.OnPopped = onPopped;
        }

        public bool IsReadyToNavigate
        {
            get { return this.Flow != null || this.Flow.IsReadyToNavigate; }
        }

        public void Navigate(string flowStateName, IDictionary<string, object> parameters = null, System.Action<IDictionary<string, object>> onPopped = null)
        {
            if(!this.IsReadyToNavigate)
            {
                Debug.LogError("flow controller is not ready to navigate yet, check IsReadyToNavigate first");
                return;
            }

            this.Flow.Navigate(flowStateName, parameters, onPopped);
        }

        public void Pop(IDictionary<string, object> parameters = null)
        {
            if (!this.IsReadyToNavigate)
            {
                Debug.LogError("flow controller is not ready to navigate yet, check IsReadyToNavigate first");
                return;
            }

            this.Flow.Pop(parameters);
        }

        public static implicit operator FlowController(Flow v)
        {
            throw new NotImplementedException();
        }
    }
}