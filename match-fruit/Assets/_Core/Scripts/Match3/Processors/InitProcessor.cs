using Match3.Services.SceneLoader;
using UnityEngine;
using Zenject;

namespace Match3.Processors
{
    public class InitProcessor : MonoBehaviour
    {
        private ISceneLoaderService _sceneLoaderService;

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        private void Start()
        {
            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            _sceneLoaderService.LoadScene(Constants.Scenes.MAIN_MENU, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}