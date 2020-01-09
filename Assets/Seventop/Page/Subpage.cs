using System.Collections;
using System.Collections.Generic;
using Seventop.Scenes;
using UnityEngine;
using Seventop.Page;
namespace Seventop.Page{
    public abstract class Subpage : MonoBehaviour
    {
        public FlowController flowController;
        Dictionary<string, object> param;
        public abstract void OnCreated();
        public abstract void SetParameter(Dictionary<string, object> _param = null);
        public abstract void OnShow();
        public abstract void OnDestroy();
        private void OnEnable()
        {
            OnCreated();
            OnShow();
        }
        private void OnDisable()
        {
            OnDestroy();
        }
    }
}
