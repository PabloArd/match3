using Match3.Services.SceneLoader;
using Zenject;

namespace Match3.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SceneLoaderService>().AsSingle().NonLazy();
        }
    }
}
