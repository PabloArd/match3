using System;
using UnityEngine;

namespace Match3.Game.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Transform _tilesContent;
        [SerializeField] private SpriteMask _mask;
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private float _tileSize = 1.0f;

        public event Action GridCreated;
        public Vector2[] WorldPosition;
        public int[,] Grid => _grid;

        private int[,] _grid;
        private Vector2 _origin;
        private Vector2 _center;

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _mask.transform.localScale = new Vector3(_columns * _tileSize, _rows * _tileSize, 1);

            _grid = new int[_columns, _rows];
            WorldPosition = new Vector2[_columns * _rows];
            _center = Vector2.zero;
            _origin = _center - new Vector2((_columns - 1) * _tileSize / 2, (_rows - 1) * _tileSize / 2);

            int index = 0;
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    Vector2 pos = _origin + new Vector2(col * _tileSize, row * _tileSize);

                    GameObject tile = Instantiate(_tilePrefab, pos, Quaternion.identity, _tilesContent);
                    tile.name = $"Tile ({col}, {row})";
                    WorldPosition[index] = pos;
                    index++;
                }
            }
            GridCreated?.Invoke();
        }

        public Vector2Int WorldToGrid(Vector2 worldPosition)
        {
            Vector3 offset = worldPosition - _origin;
            int gridX = Mathf.RoundToInt(offset.x / _tileSize);
            int gridY = Mathf.RoundToInt(offset.y / _tileSize);
            return new Vector2Int(gridX, gridY);
        }

        public Vector2 GridToWorld(Vector2Int gridPosition)
        {
            Vector2 worldPosition = _origin + new Vector2(gridPosition.x * _tileSize, gridPosition.y * _tileSize);
            return worldPosition;
        }

        public bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < _columns && y >= 0 && y < _rows;
        }
    }
}