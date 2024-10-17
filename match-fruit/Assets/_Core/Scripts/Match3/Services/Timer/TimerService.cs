using System;
using UnityEngine;
using Zenject;

namespace Match3.Services.Timer
{
    public class TimerService : ITimerService, ITickable
    {
        private float _timeLeft;
        private bool _isRunning;

        public event Action<float> TimeStarted;
        public event Action<float> TimeLeftChanged;
        public event Action TimeEnded;

        public void StartTimer(float time)
        {
            _timeLeft = time;
            _isRunning = true;
            TimeStarted?.Invoke(_timeLeft);
        }

        public void Tick()
        {
            if (!_isRunning) return;

            int lastTime = (int)_timeLeft;
            float newTime = _timeLeft - Time.deltaTime;

            if (lastTime != (int)newTime)
                TimeLeftChanged?.Invoke(_timeLeft);

            _timeLeft = newTime;


            if (_timeLeft <= 0)
            {
                _isRunning = false;
                TimeLeftChanged?.Invoke(0);
                TimeEnded?.Invoke();
            }
        }
    }
}