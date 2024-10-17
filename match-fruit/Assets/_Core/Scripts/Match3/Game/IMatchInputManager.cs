using Match3.Game.Pieces;
using System;

namespace Match3.Game
{
    public interface IMatchInputManager
    {
        event Action<PieceView, PieceView> PiecesSelected;
        void EnableInput(bool enable);
    }
}
