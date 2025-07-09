using System.Collections.Generic;
using MyGame.Scripts.UI;
using UnityEngine;
using Zenject;

namespace MyGame.Scripts.Installers
{
    public class AllInOneInstaller : MonoInstaller
    {
        [SerializeField] private ClickerTab clickerTab;
        [SerializeField] private WeatherTab weatherTab;
        [SerializeField] private BreedsTab breedsTab;
        
        [SerializeField] private ClickerConfig config;
        [SerializeField] private ClickerView clickerView;
        
        [SerializeField] private BreedsView breedsView;

        public override void InstallBindings()
        {
            Container.Bind<AppEntryPoint>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<BottomNavigationPanel>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesTo<ClickerTab>().FromInstance(clickerTab);
            Container.BindInterfacesTo<WeatherTab>().FromInstance(weatherTab);
            Container.BindInterfacesTo<BreedsTab>().FromInstance(breedsTab);
            
            Container.Bind<TabController>().AsSingle()
                .WithArguments(new List<ITab> { clickerTab, weatherTab, breedsTab });
            
            Container.BindInstance(config);
            Container.Bind<ClickerModel>().AsSingle();
            Container.BindInstance(clickerView);
            Container.Bind<ClickerPresenter>().AsSingle();
            
            Container.BindInstance(breedsView);
            Container.Bind<BreedsPresenter>().AsSingle();
            Container.Bind<RequestQueue>().FromInstance(RequestQueue.Instance).AsSingle();
        }
    }
}