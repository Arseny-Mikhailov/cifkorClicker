using MyGame.Scripts.UI;
using UnityEngine;

namespace MyGame.Scripts.Features.Weather
{
    public class WeatherTab : MonoBehaviour, ITab
    {
        public TabType TabType => TabType.Weather;

        [SerializeField] private WeatherView view;

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
