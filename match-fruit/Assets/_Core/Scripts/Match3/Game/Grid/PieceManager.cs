using Match3.Factories;
using Match3.Game.Pieces;
using Match3.ScriptableObjects;
using Match3.Services.Config;
using Match3.Services.Match;
using System;
using UnityEngine;
using Zenject;

namespace Match3.Game.Grid
{
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private Transform _piecesContent;
        [SerializeField] private Transform _vfxContent;

        public event Action AllPiecesStopped;
        public event Action PiecesCreated;

        private IConfigService _configService;
        private PieceFactory _pieceFactory;
        private MatchVfxFactory _matchVfxFactory;
        private GridManager _gridManager;
        private MatchGameSettings MatchSettings => _configService.MatchGameSettings;

        private int[,] Grid => _gridManager.Grid;
        private int Rows => Grid.GetLength(0);
        private int Columns => Grid.GetLength(1);

        private IPieceView[,] _piecesGrid;
        private int _movingPiecesCount = 0;


        [Inject]
        private void Construct(IConfigService configService, IMatchService matchService, PieceFactory pieceFactory,
            MatchVfxFactory matchVfxFactory, GridManager gridManager)
        {
            _configService = configService;
            _pieceFactory = pieceFactory;
            _matchVfxFactory = matchVfxFactory;
            _gridManager = gridManager;
        }

        private void OnEnable()
        {
            _gridManager.GridCreated += CreatePieces;
        }

        private void OnDisable()
        {
            _gridManager.GridCreated -= CreatePieces;
        }

        private void CreatePieces()
        {
            _piecesGrid = new IPieceView[Rows, Columns];

            int gridLength = _gridManager.WorldPosition.Length;
            for (int i = 0; i < gridLength; i++)
            {
                var pieceType = MatchSettings.GetRandomPiece().Type;
                Vector2Int gridPos = _gridManager.WorldToGrid(_gridManager.WorldPosition[i]);
                PieceView piece = CreatePiece(pieceType, gridPos);
                SetPiece(gridPos.x, gridPos.y, piece);
            }
            PiecesCreated?.Invoke();
        }

        public PieceView CreatePiece(PieceType type, Vector2Int gridPos)
        {
            Vector2 worldPos = _gridManager.GridToWorld(gridPos);
            PieceView piece = _pieceFactory.Create(type, worldPos, _piecesContent);
            return piece;
        }

        public void RemovePiece(int x, int y)
        {
            PieceView piece = GetPiece(x, y);
            if (piece != null)
            {
                _piecesGrid[x, y] = null;
                _pieceFactory.Release(piece);
                _matchVfxFactory.Create(piece.Type, piece.transform.position, _vfxContent);
            }
        }

        public PieceView GetPiece(int x, int y)
        {
            if (_gridManager.IsInsideGrid(x,y))
            {
                return (PieceView)_piecesGrid[x, y];
            }
            return null;
        }

        public void SetPiece(int x, int y, IPieceView piece)
        {
            if (_gridManager.IsInsideGrid(x,y))
                _piecesGrid[x, y] = piece;
        }

        public bool ArePiecesAdjacent(PieceView piece1, PieceView piece2)
        {
            if (piece1 == null || piece2 == null)
                return false;

            Vector2Int pos1 = _gridManager.WorldToGrid(piece1.transform.position);
            Vector2Int pos2 = _gridManager.WorldToGrid(piece2.transform.position);

            bool isXadjacent = Mathf.Abs(pos1.x - pos2.x) == 1 && pos1.y == pos2.y;
            bool isYadjacent = Mathf.Abs(pos1.y - pos2.y) == 1 && pos1.x == pos2.x;

            return isXadjacent || isYadjacent;
        }

        public void SwapPieces(PieceView piece1, PieceView piece2)
        {
            if (piece1 == null || piece2 == null)
                return;

            Vector2Int fromTilePos = _gridManager.WorldToGrid(piece1.transform.position);
            Vector2Int toTilePos = _gridManager.WorldToGrid(piece2.transform.position);
            SetPiece(fromTilePos.x, fromTilePos.y, piece2);
            SetPiece(toTilePos.x, toTilePos.y, piece1);

            MovePiece(piece1, piece2.transform.position);
            MovePiece(piece2, piece1.transform.position);
        }

        public void DropPiecesAfterMatch()
        {
            for (var x = 0; x < Columns; x++)
            {
                int emptyRow = -1;
                int emptyAmount = 0;
                for (var y = 0; y < Rows; y++)
                {
                    var piece = GetPiece(x, y);
                    if (piece == null)
                    {
                        emptyAmount++;

                        if (emptyRow == -1)
                            emptyRow = y;
                    }
                    else if (emptyRow != -1)
                    {
                        SetPiece(x, emptyRow, piece);
                        SetPiece(x, y, null);
                        MovePiece(piece, _gridManager.GridToWorld(new Vector2Int(x, emptyRow)));
                        emptyRow++;
                    }
                }

                FillGridWithNewPieces(x, emptyAmount);
            }
        }

        public void FillGridWithNewPieces(int x, int emptyAmount)
        {
            for (int i = 0; i < emptyAmount; i++)
            {
                int spawnRow = Rows + i;
                int targetRow = Rows - emptyAmount + i;

                Vector2Int gridPosition = new Vector2Int(x, targetRow);
                Vector2 spawnPosition = _gridManager.GridToWorld(new Vector2Int(x, spawnRow));

                var newPiece = CreatePiece(GetRandomPiece(), gridPosition);
                SetPiece(x, targetRow, newPiece);

                newPiece.transform.position = spawnPosition;

                var targetPosition = _gridManager.GridToWorld(gridPosition);
                MovePiece(newPiece, targetPosition);
            }
        }

        private void MovePiece(PieceView piece, Vector2 position)
        {
            float distance = Vector2.Distance(piece.transform.position, position);
            bool fall = distance > 1;
            float timeToMove = fall ? MatchSettings.TimeToFallPieceUnit : MatchSettings.TimeToMovePieceUnit;
            float time = distance * timeToMove;

            _movingPiecesCount++;

            piece.MoveTo(position, time, fall, () =>
            {
                _movingPiecesCount--;
                CheckIfAllPiecesStopped();
            });
        }

        private void CheckIfAllPiecesStopped()
        {
            if (_movingPiecesCount == 0)
                AllPiecesStopped?.Invoke();
        }

        private PieceType GetRandomPiece()
        {
            return _configService.MatchGameSettings.GetRandomPiece().Type;
        }
    }
}