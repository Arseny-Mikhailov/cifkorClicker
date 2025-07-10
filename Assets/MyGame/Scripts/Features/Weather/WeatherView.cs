using System;
using MyGame.Scripts.Features.Weather;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.UI
{
    public class WeatherView : MonoBehaviour
    {
        public event Action Showed;
        public event Action Hidden;

        [SerializeField] private Image _weatherIcon;
        [SerializeField] private TextMeshProUGUI _weatherText;

        public void Show()
        {
            gameObject.SetActive(true);
            Showed?.Invoke();
        }

        public void Hide()
        {
            Hidden?.Invoke();
            gameObject.SetActive(false);
        }

        public void SetWeather(string label, Sprite icon)
        {
            _weatherText.text = label;
            _weatherIcon.sprite = icon;
        }
    }
}