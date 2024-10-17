using Match3.Game.Pieces;
using Match3.ScriptableObjects;
using System.Collections.Generic;

namespace Match3.Services.Config
{
    public class MatchInfo
    {
        public Dictionary<PieceType, PieceData> Pieces => _piecesData;
        private Dictionary<PieceType, PieceData> _piecesData;

        public void Initialize(PieceData[] pieces)
        {
            FillPiecesData(pieces);
        }

        public void Dispose()
        {
            if (_piecesData != null)
            {
                _piecesData.Clear();
                _piecesData = null;
            }
        }

        private void FillPiecesData(PieceData[] pieces)
        {
            _piecesData = new();

            for (int i = 0; i < pieces.Length; i++)
            {
                if (!_piecesData.TryGetValue(pieces[i].Type, out PieceData data))
                    _piecesData.Add(pieces[i].Type, pieces[i]);
            }
        }
    }

}

