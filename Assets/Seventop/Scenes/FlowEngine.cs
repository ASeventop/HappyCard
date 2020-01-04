using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Seventop.Scenes
{
    public class FlowEngine : MonoBehaviour
    {
        #region API
        public static bool IsReady
        {
            get { return Instance.isReady; }
        }
        public static string CurrentScene
        {
            get { return SceneManager.GetActiveScene().name; }
        }
        public static void Push(string sceneName, Action<Scene> sceneLoaded = null)
        {
            Instance._Push(sceneName, sceneLoaded);
        }
        public static void Pop(string sceneName)
        {
            Instance._Pop(sceneName);
        }
        public static void Switch(string sceneName, Action<Scene> sceneLoaded = null)
        {
            Instance._Switch(sceneName, sceneLoaded);
        }
        public static void Replace(string sceneName, Action<Scene> sceneLoaded = null, bool nextFrame = false)
        {
            Instance._Replace(sceneName, sceneLoaded, nextFrame);
        }
        #endregion

        bool isReady;
        Action<Scene> sceneLoadedCallback;
        string previousSceneToUnload;
        string nextFrameReplace;

        static FlowEngine _instance;
        static FlowEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("FlowEngine");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<FlowEngine>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            isReady = true;
        }
        void Update()
        {
            if (!string.IsNullOrEmpty(nextFrameReplace))
            {
                SceneManager.LoadScene(nextFrameReplace, LoadSceneMode.Single);
                nextFrameReplace = null;
            }
        }
        void OnDestroy()
        {
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        void _Switch(string sceneName, Action<Scene> sceneLoaded)
        {
            if (!isReady)
                throw new System.InvalidOperationException("check IsReady first before Switch");
            isReady = false;
            if (previousSceneToUnload != null)  //TODO: if this happens, implement queueing
                throw new System.InvalidOperationException();
            this.sceneLoadedCallback = sceneLoaded;
            previousSceneToUnload = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        void _Pop(string sceneName)
        {
            if (!isReady)
                throw new System.InvalidOperationException("check IsReady first before Pop");
            Debug.Assert(previousSceneToUnload == null);
            previousSceneToUnload = sceneName;
            SceneManager.UnloadSceneAsync(previousSceneToUnload);
        }
        void _Push(string sceneName, Action<Scene> sceneLoaded)
        {
            this.sceneLoadedCallback = sceneLoaded;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        void _Replace(string sceneName, Action<Scene> sceneLoaded, bool nextFrame)
        {
            this.sceneLoadedCallback = sceneLoaded;
            if (nextFrame)
                nextFrameReplace = sceneName;
            else
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.SetActiveScene(arg0);
            this.sceneLoadedCallback(arg0);
            this.sceneLoadedCallback = null;

            if (previousSceneToUnload != null)
            {
                SceneManager.UnloadSceneAsync(previousSceneToUnload);
            }
        }

        void SceneManager_sceneUnloaded(Scene arg0)
        {

            if (arg0.name == previousSceneToUnload)
            {
                previousSceneToUnload = null;
                isReady = true;
            }
        }
    }
}