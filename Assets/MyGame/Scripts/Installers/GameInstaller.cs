using System.Collections.Generic;
using MyGame.Scripts.Core;
using MyGame.Scripts.Features;
using MyGame.Scripts.Features.Breeds;
using MyGame.Scripts.Features.Weather;
using MyGame.Scripts.UI;
using UnityEngine;
using Zenject;

namespace MyGame.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ClickerTab clickerTab;
        [SerializeField] private WeatherTab weatherTab;
        [SerializeField] private BreedsTab breedsTab;

        [SerializeField] private ClickerConfig config;
        [SerializeField] private ClickerView clickerView;
        [SerializeField] private BreedsView breedsView;

        public override void Start()
        {
            var tabController = Container.Resolve<TabController>();
            tabController.SwitchTo(TabType.Clicker);
        }

        public override void InstallBindings()
        {
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

            Container.Bind<WeatherView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle();

            Container.BindInstance(breedsView);
            Container.Bind<BreedsPresenter>().AsSingle();

            Container.Bind<RequestQueue>().FromInstance(RequestQueue.Instance).AsSingle();
        }
    }
}