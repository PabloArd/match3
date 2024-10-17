using Match3.Services.Timer;
using Match3.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace Match3.UI.Timer
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;

        private ITimerService _timerService;

        private float _timer;

        [Inject]
        private void Construct(ITimerService timerService)
        {
            _timerService = timerService;
        }

        private void OnEnable()
        {
            _timerService.TimeStarted += SetTime;
            _timerService.TimeLeftChanged += SetTime;
        }

        private void OnDisable()
        {
            _timerService.TimeStarted -= SetTime;
            _timerService.TimeLeftChanged -= SetTime;
        }

        public void SetTime(float time)
        {
            _timer = time;
            UpdateText();
        }

        private void UpdateText()
        {
            m_text.text = ((long)_timer).ToTimeFormat();
        }
    }
}