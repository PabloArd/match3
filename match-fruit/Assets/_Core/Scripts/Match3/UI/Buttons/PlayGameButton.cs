using Match3.Services.SceneLoader;
using UnityEngine.SceneManagement;
using Zenject;

namespace Match3.UI.Buttons
{
    public class PlayGameButton : ButtonBase
    {
        private ISceneLoaderService _sceneLoaderService;

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        protected override void OnButtonClick()
        {
            _sceneLoaderService.LoadScene(Constants.Scenes.GAMEPLAY, LoadSceneMode.Single);
        }
    }
}

