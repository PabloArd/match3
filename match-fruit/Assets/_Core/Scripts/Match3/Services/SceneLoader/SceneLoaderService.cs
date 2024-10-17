using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace Match3.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService, IInitializable, IDisposable
    {
        private string _lastSceneNameRequest;
        private Action _loadSceneCallback;
        private Action _unloadSceneCallback;

        public void Initialize()
        {
            ManageListeners(true);
        }

        public void Dispose()
        {
            ManageListeners(false);
        }

        private void ManageListeners(bool add)
        {
            if (add)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
            }
        }

        public void LoadScene(string gameScene, LoadSceneMode loadSceneMode, Action callback = null)
        {
            Scene globeLevelScene = SceneManager.GetSceneByName(gameScene);
            _lastSceneNameRequest = gameScene;
            if (callback != null)
                _loadSceneCallback = callback;
            SceneManager.LoadSceneAsync(gameScene, loadSceneMode);
        }

        public void UnloadScene(string gameScene, Action callback = null)
        {
            if (callback != null)
                _unloadSceneCallback = callback;
            SceneManager.UnloadSceneAsync(gameScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == _lastSceneNameRequest)
                SceneManager.SetActiveScene(scene);

            _loadSceneCallback?.Invoke();
            _loadSceneCallback = null;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            _unloadSceneCallback?.Invoke();
            _unloadSceneCallback = null;
        }
    }
}
