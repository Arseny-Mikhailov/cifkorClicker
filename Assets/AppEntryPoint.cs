using MyGame.Scripts;
using UnityEngine;
using Zenject;

public class AppEntryPoint : MonoBehaviour
{
    [Inject] private TabController _tabController;

    private void Start()
    {
        _tabController.SwitchTo(TabType.Clicker); // показываем кликер при запуске
    }
}