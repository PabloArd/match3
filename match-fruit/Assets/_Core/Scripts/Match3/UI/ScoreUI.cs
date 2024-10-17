using Match3.Services.Score;
using TMPro;
using UnityEngine;
using Zenject;

namespace Match3.UI.Score
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;

        private IScoreService _scoreService;

        [Inject]
        private void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void OnEnable()
        {
            _scoreService.Changed += OnScoreAdded;
        }

        private void OnDisable()
        {
            _scoreService.Changed -= OnScoreAdded;
        }

        private void OnScoreAdded(int score)
        {
            m_text.text = score.ToString();
        }
    }
}