using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class AppEntryPoint : MonoBehaviour
    {
        [Inject] private TabController _tabController;

        private void Start()
        {
            _tabController.SwitchTo(TabType.Clicker);
        }
    }
}