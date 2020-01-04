using System.Collections;
using System.Collections.Generic;
using Seventop.Scenes;
using UnityEngine;
using Seventop.Page;
using UnityEngine.SceneManagement;
namespace Seventop.Page {
    public abstract class Page : MonoBehaviour,IPage
    {
        protected FlowController flowController;
        public Subpage mainSubpage;
        Subpage currentSubpage;
        public Dictionary<string, object> parameter;
        public virtual void SetParameter(Dictionary<string,object> _param)
        {
            parameter = _param;
        }
        public abstract void OnCreate();
        public abstract void OnShow();
        public abstract void OnDestroy();
        //must declare public Subpage in this class - -"
        public virtual void OpenSubPage(Subpage subPage, Dictionary<string, object> _param = null)
        {
            if (subPage == null) return;
            if (currentSubpage == subPage) return;
            if (currentSubpage != null) currentSubpage.gameObject.SetActive(false);
            currentSubpage = subPage;
            subPage.SetParameter(_param);
            subPage.gameObject.SetActive(true);
        }
        private void OnEnable()
        {
            if (!PageStack.Instance.isInit)
            {
                SceneManager.LoadScene("PageFlow");
                return;
            }
            OnCreate();
        }
        private void OnDisable()
        {
            OnDestroy();
        }
        private void Start()
        {
            OnShow();
        }
        private void OnApplicationQuit()
        {
            OnDestroy();
        }

    }
    public interface IPage
    {
        void OnCreate();
        void OnDestroy();
    }
}