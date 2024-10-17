using System;
using UnityEngine.SceneManagement;

namespace Match3.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        void LoadScene(string gameScene, LoadSceneMode loadSceneMode, Action callback = null);
    }
}
