using System;

namespace Match3.Services.Timer
{
    public interface ITimerService
    {
        event Action<float> TimeStarted;
        event Action<float> TimeLeftChanged;
        event Action TimeEnded;
        void StartTimer(float time);
    }
}