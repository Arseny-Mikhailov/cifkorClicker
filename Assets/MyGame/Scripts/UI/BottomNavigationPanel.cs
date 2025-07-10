using MyGame.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyGame.Scripts.UI
{
    public class BottomNavigationPanel : MonoBehaviour
    {
        public Button clickerButton;
        public Button weatherButton;
        public Button breedsButton;

        [Inject] private TabController _tabController;

        private void Start()
        {
            clickerButton.onClick.AddListener(() => _tabController.SwitchTo(TabType.Clicker));
            weatherButton.onClick.AddListener(() => _tabController.SwitchTo(TabType.Weather));
            breedsButton.onClick.AddListener(() => _tabController.SwitchTo(TabType.Breeds));
        }
    }
}