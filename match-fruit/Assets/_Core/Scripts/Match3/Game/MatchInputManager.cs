using Match3.Game.Grid;
using Match3.Game.Pieces;
using Match3.Services.Inputs;
using System;
using UnityEngine;
using Zenject;

namespace Match3.Game
{
    public class MatchInputManager : IMatchInputManager, IInputReceiver, IInitializable, IDisposable
    {
        public event Action<PieceView, PieceView> PiecesSelected;

        private readonly IInputService _inputService;
        private readonly GridManager _gridManager;
        private readonly PieceManager _pieceManager;

        private PieceView _selectedPiece;
        private bool _enableInput = true;

        public MatchInputManager(IInputService inputService, GridManager gridManager, PieceManager pieceManager)
        {
            _inputService = inputService;
            _gridManager = gridManager;
            _pieceManager = pieceManager;
        }

        public void Initialize()
        {
            _inputService.RegisterReceiver(this);
        }

        public void Dispose()
        {
            _inputService.UnregisterReceiver(this);
        }

        public void EnableInput(bool enable)
        {
            _enableInput = enable;
        }

        public void Tap(Vector2 worldPosition)
        {
            if (!HandlePieceSelection(worldPosition))
                DeselectPiece();
        }

        public void Drag(Vector2 worldPosition)
        {
            if (_selectedPiece != null)
                HandlePieceSelection(worldPosition);
        }

        private bool HandlePieceSelection(Vector2 worldPosition)
        {
            if (!_enableInput)
                return false;

            bool selected = TrySelectTile(worldPosition, out PieceView piece);
            if (selected)
                SelectPiece(piece);
            return selected;
        }

        private bool TrySelectTile(Vector2 worldPosition, out PieceView piece)
        {
            piece = null;
            Vector2Int tilePos = _gridManager.WorldToGrid(worldPosition);
            piece = _pieceManager.GetPiece(tilePos.x, tilePos.y);
            return piece != null && piece != _selectedPiece;
        }

        public void SelectPiece(PieceView piece)
        {
            if (_selectedPiece != null)
            {
                PiecesSelected?.Invoke(_selectedPiece, piece);
                DeselectPiece();
            }
            else
            {
                _selectedPiece = piece;
                _selectedPiece.Select(true);
            }
        }

        public void DeselectPiece()
        {
            if (_selectedPiece != null)
            {
                _selectedPiece.Select(false);
                _selectedPiece = null;
            }
        }
    }
}
