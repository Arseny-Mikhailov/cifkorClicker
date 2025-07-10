using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.UI
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private Image weatherIcon;
        [SerializeField] private TextMeshProUGUI weatherText;
        public event Action Showed;
        public event Action Hidden;

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
            weatherText.text = label;
            weatherIcon.sprite = icon;
        }
    }
}