using Match3.Game.Pieces;
using Match3.Services.Config;
using Match3.Utils.MonoAsync;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Match3.Factories
{
    public class MatchVfxFactory : PlaceholderFactory<PieceType, Vector2, Transform, ParticleSystem>
    {
        private readonly DiContainer _container;
        private readonly IMonoAsyncProcessor _asyncProcessor;
        private readonly IConfigService _configService;

        private Dictionary<PieceType, ObjectPool<ParticleSystem>> _vfxPools;
        private readonly Dictionary<ParticleSystem, PieceType> _particleToTypeMap;

        public MatchVfxFactory(DiContainer container, IMonoAsyncProcessor asyncProcessor, IConfigService configService)
        {
            _container = container;
            _asyncProcessor = asyncProcessor;
            _configService = configService;
            _particleToTypeMap = new Dictionary<ParticleSystem, PieceType>();
            InitPool();
        }

        public override ParticleSystem Create(PieceType type, Vector2 position, Transform parent)
        {
            if (_vfxPools.TryGetValue(type, out ObjectPool<ParticleSystem> pool))
            {
                ParticleSystem vfx = pool.Get();
                vfx.transform.position = position;
                vfx.transform.SetParent(parent);
                return vfx;
            }
            else
            {
                Debug.LogError($"No pool found for VFXType: {type}");
                return null;
            }
        }

        private void InitPool()
        {
            _vfxPools = new Dictionary<PieceType, ObjectPool<ParticleSystem>>();
            
            foreach (var pieceData in _configService.MatchInfo.Pieces)
            {
                PieceType pieceType = pieceData.Key;
                _vfxPools[pieceType] = new ObjectPool<ParticleSystem>(
                    createFunc: () => CreateVfx(pieceType),
                    OnSpawned,
                    OnRelease,         
                    OnDestroyed,
                    defaultCapacity: 5
                );
            }
        }

        private ParticleSystem CreateVfx(PieceType type)
        {
            GameObject matchVfxPrefab = _configService.MatchInfo.Pieces[type].MatchVfx;
            ParticleSystem particle = _container.InstantiatePrefabForComponent<ParticleSystem>(matchVfxPrefab);
            _particleToTypeMap[particle] = type;
            return particle;
        }

        private void OnSpawned(ParticleSystem particle)
        {
            particle.gameObject.SetActive(true);
            _asyncProcessor.StartCoroutine(PlayAndRelease(particle));
        }

        private void OnRelease(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }

        private void OnDestroyed(ParticleSystem particle)
        {
            GameObject.Destroy(particle.gameObject);
        }

        private IEnumerator PlayAndRelease(ParticleSystem particle)
        {
            yield return new WaitForSeconds(particle.main.duration);

            if (_particleToTypeMap.TryGetValue(particle, out PieceType pieceType))
            {
                Release(pieceType, particle);
            }
        }

        private void Release(PieceType type, ParticleSystem vfxInstance)
        {
            if (_vfxPools.TryGetValue(type, out ObjectPool<ParticleSystem> pool))
            {
                pool.Release(vfxInstance);
            }
            else
            {
                Debug.LogError($"No pool exists for VFXType: {type}");
            }
        }
    }
}
