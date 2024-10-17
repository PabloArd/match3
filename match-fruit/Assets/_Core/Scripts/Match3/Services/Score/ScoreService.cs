using System;

namespace Match3.Services.Score
{
    public class ScoreService : IScoreService
    {
        public int Score => _score;
        public event Action<int> Changed;
        private int _score;

        public void Set(int score)
        {
            _score = score;
            Changed?.Invoke(_score);
        }

        public void Add(int score)
        {
            _score += score;
            Changed?.Invoke(_score);
        }
    }
}