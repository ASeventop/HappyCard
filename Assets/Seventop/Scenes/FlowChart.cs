using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Seventop.Scenes
{
    // flow setup
    public class FlowChart
    {
        List<FlowState> _states = new List<FlowState>();
        Dictionary<string, FlowState> _statesLookup = new Dictionary<string, FlowState>();
        List<FlowNavigation> _navigations = new List<FlowNavigation>();
        Dictionary<string, FlowNavigation> _navigationsLookup = new Dictionary<string, FlowNavigation>();

        public Flow Execute(IDictionary<string, object> parameters = null)
        {
            var flow = new Flow(this);
            flow.Execute(parameters);
            return flow;
        }

        public FlowState Root
        {
            get; private set;
        }

        public void SetRootState(FlowState rootState)
        {
            Debug.Assert(Root == null);
            if (Root != null)
                return;
            
            this.Root = rootState;
            _Register(rootState);
        }

        public void AddNavigation(FlowState from, FlowState to, FlowNavigationType navigationType)
        {
            if(from == null || to == null || from == to)
            {
                Debug.LogError("invalid navigation");
                return;
            }

            var key = from.Name + "_" + to.Name;
            if(_navigationsLookup.ContainsKey(key))
            {
                //Debug.LogError("Redundant Navigation found : from " + from.Name + " to " + to.Name);
                return;
            }

            _Register(from);
            _Register(to);

            var navigation = new FlowNavigation() { from = from, to = to, navigationType = navigationType };
            _navigations.Add(navigation);
            _navigationsLookup.Add(key, navigation);
        }

        public bool TryGetNavigation(string fromStateName, string toStateName, out FlowNavigation navigation)
        {
            var key = fromStateName + "_" + toStateName;
            return _navigationsLookup.TryGetValue(key, out navigation);
        }

        private void _Register(FlowState state)
        {
            if (!this._states.Contains(state))
            {
                this._states.Add(state);
                if (this._statesLookup.ContainsKey(state.Name)) { }
                // Debug.LogError("Redundant FlowState name found : " + state.Name);
                else
                {
                    this._statesLookup.Add(state.Name, state);
                }
            }
        }

        public class FlowNavigation
        {
            public FlowState from;
            public FlowState to;
            public FlowNavigationType navigationType;
        }
    }

    public enum FlowNavigationType
    {
        Switch, Stack
    }

    public class FlowState
    {
        public string Name { get; private set; }
        public string SceneName { get; private set; }
        public FlowParameter[] RequiredParameters { get; private set; }

        public FlowState(string name, string sceneName, params FlowParameter[] requiredParameters)
        {
            this.Name = name;
            this.SceneName = sceneName;
            this.RequiredParameters = requiredParameters;
        }

        public bool ValidateParameters(IDictionary<string, object> parameters, out string errorMessage)
        {
            for (var i = 0; i < RequiredParameters.Length; i++)
            {
                var require = RequiredParameters[i];
                if(parameters == null || !parameters.ContainsKey(require.Name))
                {
                    errorMessage = "flow state \"" + Name + "\" requires \"" + require.Name + "\" (" + require.DataType + ") parameter";
                    return false;
                }
                else if(parameters[require.Name].GetType() != require.DataType)
                {
                    errorMessage = "flow state \"" + Name + "\" parameter \"" + require.Name + "\" type mismatched, expected \"" + require.DataType + "\" but got \"" + parameters[require.Name].GetType() + "\"";
                    return false;
                }
            }

            errorMessage = null;
            return true;
        }
    }

    public class FlowParameter
    {
        public string Name { get; private set; }
        public System.Type DataType { get; private set; }

        public FlowParameter(string name, System.Type dataType)
        {
            this.Name = name;
            this.DataType = dataType;
        }
    }
}