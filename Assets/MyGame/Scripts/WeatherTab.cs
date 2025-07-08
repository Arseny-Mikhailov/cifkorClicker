using UnityEngine;

namespace MyGame.Scripts
{
    public class WeatherTab : MonoBehaviour, ITab
    {
        public TabType TabType => TabType.Weather;

        public void OnShow()
        {
            gameObject.SetActive(true);
        }

        public void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}