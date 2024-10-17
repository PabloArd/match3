using Match3.Game;
using Match3.Services.SceneLoader;
using Match3.Services.Score;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Match3.UI.Popups
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameOverPopup _gameOverPopupPrefab;
        [SerializeField] private Transform _popupsContent;

        private IGameManager _gameManager;
        private IScoreService _scoreService;
        private ISceneLoaderService _sceneLoaderService;

        private bool _gameOverMenuListenersAdded;

        [Inject]
        private void Construct(IGameManager gameManager, IScoreService scoreService,
            ISceneLoaderService sceneLoaderService)
        {
            _gameManager = gameManager;
            _scoreService = scoreService;
            _sceneLoaderService = sceneLoaderService;
        }

        private void OnEnable()
        {
            _gameManager.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            _gameManager.GameOver -= OnGameOver;

            if (_gameOverMenuListenersAdded)
                _gameOverPopupPrefab.MenuButtonClicked -= OnMenuButtonClicked;
        }

        private void OnGameOver()
        {
            GameOverPopup gameOverPopup = Instantiate(_gameOverPopupPrefab, _popupsContent);
            gameOverPopup.Initialize(_scoreService.Score);
            gameOverPopup.Show(true);
            _gameOverMenuListenersAdded = true;
            gameOverPopup.MenuButtonClicked += OnMenuButtonClicked;
        }

        private void OnMenuButtonClicked()
        {
            _gameOverPopupPrefab.MenuButtonClicked -= OnMenuButtonClicked;
            _gameOverMenuListenersAdded = false;

            _sceneLoaderService.LoadScene(Constants.Scenes.MAIN_MENU, LoadSceneMode.Single);
        }
    }
}