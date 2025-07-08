using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyGame.Scripts
{
    public class BottomNavigationPanel : MonoBehaviour
    {
        public Button clickerButton;
        public Button weatherButton;
   

        [Inject] private TabController _tabController;

        private void Start()
        {
            clickerButton.onClick.AddListener(() => _tabController.SwitchTo(TabType.Clicker));
            weatherButton.onClick.AddListener(() => _tabController.SwitchTo(TabType.Weather));
        }
    }
}