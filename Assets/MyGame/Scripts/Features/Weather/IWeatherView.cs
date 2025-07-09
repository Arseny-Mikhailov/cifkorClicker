using System;
using UnityEngine;

namespace MyGame.Scripts.Features.Weather
{
    public interface IWeatherView
    {
        event Action Showed;
        event Action Hidden;
        void Show();
        void Hide();
        void SetWeather(string label, Sprite icon);
    }
}