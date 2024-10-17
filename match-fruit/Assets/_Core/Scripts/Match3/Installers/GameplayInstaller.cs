using Match3.Factories;
using Match3.Game;
using Match3.Game.Grid;
using Match3.Game.Pieces;
using Match3.ScriptableObjects;
using Match3.Services.Config;
using Match3.Services.Inputs;
using Match3.Services.Match;
using Match3.Services.Score;
using Match3.Services.Timer;
using Match3.Utils.MonoAsync;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private MatchGameSettings _matchGameSettings;
        [SerializeField] private MonoAsyncProcessor m_monoAsyncProcessor;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ConfigService>().AsSingle().WithArguments(_matchGameSettings).NonLazy();
            Container.Bind<IMonoAsyncProcessor>().FromInstance(m_monoAsyncProcessor).AsSingle();
            Container.BindFactory<PieceType, Vector2, Transform, PieceView, PieceFactory>();
            Container.BindFactory<PieceType, Vector2, Transform, ParticleSystem, MatchVfxFactory>();
            Container.BindInterfacesTo<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<MatchInputManager>().AsSingle().NonLazy();
            Container.Bind<GridManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PieceManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesTo<MatchService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ScoreService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<TimerService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameManager>().AsSingle().NonLazy();
        }
    }
}
