using Match3.Game.Pieces;
using Match3.Services.Config;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Match3.Factories
{
    public class PieceFactory : PlaceholderFactory<PieceType, Vector2, Transform, PieceView>
    {
        private readonly DiContainer _container;
        private readonly MatchInfo _matchInfo;
        private readonly PieceView _piecePrefab;

        private IObjectPool<PieceView> _pool;

        public PieceFactory(DiContainer container, IConfigService configService)
        {
            _container = container;
            _matchInfo = configService.MatchInfo;
            _piecePrefab = configService.MatchGameSettings.PiecePrefab;
            InitPool();
        }

        public override PieceView Create(PieceType type, Vector2 position, Transform parent)
        {
            PieceView piece = _pool.Get();
            piece.Initialize(type, _matchInfo.Pieces[type].Sprite);
            piece.transform.position = position;
            piece.transform.SetParent(parent);
            return piece;
        }

        private void InitPool()
        {
            _pool = new ObjectPool<PieceView>(
                CreatePiece,
                OnSpawned,
                OnRelease,
                OnDestroyed
            );
        }

        private PieceView CreatePiece()
        {
            PieceView piece = _container.InstantiatePrefabForComponent<PieceView>(_piecePrefab);
            return piece;
        }

        private void OnSpawned(PieceView visual)
        {
            visual.gameObject.SetActive(true);
        }

        private void OnRelease(PieceView visual)
        {
            visual.gameObject.SetActive(false);
        }

        private void OnDestroyed(PieceView visual)
        {
            GameObject.Destroy(visual.gameObject);
        }

        public void Release(PieceView piece)
        {
            _pool.Release(piece);
        }
    }
}
