using UnityEngine;
using Zenject;
using TestWork.UI;
using TestWork.Units;

namespace TestWork.Infrastructure
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private UIService _uIService;
        [SerializeField] private Config _config;
        [SerializeField] private Player _player;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            Container.Bind<Factory>().AsSingle().NonLazy();
            Container.Bind<Spawner>().AsSingle().NonLazy();
            Container.Bind<InputController>().FromInstance(_inputController).AsSingle().NonLazy();
            Container.Bind<UIService>().FromInstance(_uIService).AsSingle().NonLazy();
            Container.Bind<Config>().FromInstance(_config).AsSingle().NonLazy();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
        }
    }
}
