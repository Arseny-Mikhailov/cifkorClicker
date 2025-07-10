using MyGame.Scripts.Core;
using MyGame.Scripts.UI;
using UnityEngine;

namespace MyGame.Scripts.Features.Weather
{
    public class WeatherTab : MonoBehaviour, ITab
    {
        [SerializeField] private WeatherView view;
        public TabType TabType => TabType.Weather;

        public void OnShow()
        {
            view.Show();
        }

        public void OnHide()
        {
            view.Hide();
        }
    }
}