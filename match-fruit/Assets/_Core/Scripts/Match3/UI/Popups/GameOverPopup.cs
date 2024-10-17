using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.UI.Popups
{
    public class GameOverPopup : MonoBehaviour
    {
        [SerializeField] private Transform _panel;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _menuButton;

        public event Action MenuButtonClicked;

        private void OnEnable()
        {
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnDisable()
        {
            _menuButton.onClick.RemoveAllListeners();
        }

        public void Initialize(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);

            if (show)
                _panel.DOScale(_panel.localScale, 1).From(Vector3.zero).SetEase(Ease.OutBack);
        }

        private void OnMenuButtonClicked()
        {
            MenuButtonClicked?.Invoke();
        }

    }
}