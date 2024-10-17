using Match3.Game.Grid;
using Match3.Game.Pieces;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Services.Match
{
    public class MatchService : IMatchService
    {
        private GridManager _gridManager;
        private PieceManager _pieceManager;

        public MatchService(GridManager gridManager, PieceManager gridPieceManager)
        {
            _gridManager = gridManager;
            _pieceManager = gridPieceManager;
        }

        public List<Vector2Int> CheckMatches()
        {
            var matches = new List<Vector2Int>();
            int rows = _gridManager.Grid.GetLength(0);
            int columns = _gridManager.Grid.GetLength(1);
            const int targetMatchCount = 3;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    var currentPiece = _pieceManager.GetPiece(x, y);
                    if (currentPiece == null)
                        continue;

                    if (HasMatch(x, y, targetMatchCount, currentPiece.Type, Vector2Int.right))
                    {
                        for (int i = 0; i < targetMatchCount; i++)
                            matches.Add(new Vector2Int(x + i, y));
                    }

                    if (HasMatch(x, y, targetMatchCount, currentPiece.Type, Vector2Int.up))
                    {
                        for (int i = 0; i < targetMatchCount; i++)
                            matches.Add(new Vector2Int(x, y + i));
                    }
                }
            }
            return matches;
        }

        private bool HasMatch(int startX, int startY, int targetMatchCount, PieceType initialType, Vector2Int direction)
        {
            for (int i = 1; i < targetMatchCount; i++)
            {
                var nextPiece = _pieceManager.GetPiece(startX + i * direction.x, startY + i * direction.y);
                if (nextPiece == null || nextPiece.Type != initialType)
                    return false;
            }
            return true;
        }
    }
}
