using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private ClickerTab clickerTab;
        [SerializeField] private WeatherTab weatherTab;
 

        public override void InstallBindings()
        {
            Container.Bind<AppEntryPoint>().AsSingle();
            
            Container.BindInterfacesTo<ClickerTab>().FromInstance(clickerTab);
            Container.BindInterfacesTo<WeatherTab>().FromInstance(weatherTab);
            
            Container.Bind<TabController>().AsSingle()
                .WithArguments(new List<ITab> { clickerTab, weatherTab});
        }
    }
}