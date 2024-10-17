using System;

namespace Match3.Services.Score
{
    public interface IScoreService
    {
        int Score { get; }
        event Action<int> Changed;
        void Set(int score);
        void Add(int score);
    }
}