using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MyGame.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ClickerConfig config;
        [SerializeField] private ClickerView view;
    
        public override void InstallBindings()
        {
            Container.Bind<ClickerTab>().FromComponentInHierarchy().AsSingle();
            Container.Bind<WeatherTab>().FromComponentInHierarchy().AsSingle();
        
            Container.BindInstance(config);
            Container.Bind<ClickerModel>().AsSingle();
            Container.BindInstance(view);
            Container.Bind<ClickerPresenter>().AsSingle();
        }
    }
}