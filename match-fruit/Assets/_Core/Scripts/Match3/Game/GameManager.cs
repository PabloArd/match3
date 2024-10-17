using Match3.Game.Grid;
using Match3.Game.Pieces;
using Match3.Services.Config;
using Match3.Services.Match;
using Match3.Services.Score;
using Match3.Services.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Match3.Game
{
    public class GameManager : IGameManager, IInitializable, IDisposable
    {
        private readonly IMatchInputManager _matchInputManager;
        private readonly IMatchService _matchService;
        private readonly IConfigService _configService;
        private readonly IScoreService _scoreService;
        private readonly ITimerService _timerService;
        private readonly PieceManager _pieceManager;

        public event Action GameOver;

        private bool _isGameActive;

        public GameManager(IMatchInputManager matchInputManager, IMatchService matchService,
                IConfigService configService, IScoreService scoreService, ITimerService timerService,
                PieceManager pieceManager)
        {
            _matchInputManager = matchInputManager;
            _matchService = matchService;
            _configService = configService;
            _scoreService = scoreService;
            _timerService = timerService;
            _pieceManager = pieceManager;
        }

        public void Initialize()
        {
            ManageListeners(true);
            _scoreService.Set(0);
            _timerService.StartTimer(_configService.MatchGameSettings.GameTime);
            _isGameActive = true;
        }

        public void Dispose()
        {
            ManageListeners(false);
        }

        private void ManageListeners(bool add)
        {
            if (add)
            {
                _pieceManager.PiecesCreated += OnPiecesCreated;
                _matchInputManager.PiecesSelected += OnPiecesSelected;
                _pieceManager.AllPiecesStopped += OnAllPiecesStopped;
                _timerService.TimeEnded += OnTimeEnded;
            }
            else
            {
                _pieceManager.PiecesCreated -= OnPiecesCreated;
                _matchInputManager.PiecesSelected -= OnPiecesSelected;
                _pieceManager.AllPiecesStopped -= OnAllPiecesStopped;
                _timerService.TimeEnded -= OnTimeEnded;
            }
        }

        private void OnPiecesCreated()
        {
            CheckMatches();
        }

        private void OnPiecesSelected(PieceView previousPiece, PieceView piece)
        {
            if (_pieceManager.ArePiecesAdjacent(previousPiece, piece))
            {
                EnableInput(false);
                _pieceManager.SwapPieces(previousPiece, piece);
            }
        }

        private void OnAllPiecesStopped()
        {
            CheckMatches();
        }

        private void OnTimeEnded()
        {
            EnableInput(false);
            _isGameActive = false;
            GameOver?.Invoke();
        }

        private void CheckMatches()
        {
            List<Vector2Int> matches = _matchService.CheckMatches();

            for (int i = 0; i < matches.Count; i++)
            {
                PieceView piece = _pieceManager.GetPiece(matches[i].x, matches[i].y);
                if (piece != null)
                {
                    _pieceManager.RemovePiece(matches[i].x, matches[i].y);
                    int score = _configService.MatchInfo.Pieces[piece.Type].ScoreBase;
                    _scoreService.Add(score);
                }
            }

            if (matches.Count > 0)
            {
                EnableInput(false);
                _pieceManager.DropPiecesAfterMatch();
            }
            else
            {
                EnableInput(true);
            }
        }

        private void EnableInput(bool enable)
        {
            if (!_isGameActive)
                return;

            _matchInputManager.EnableInput(enable);
        }
    }
}
