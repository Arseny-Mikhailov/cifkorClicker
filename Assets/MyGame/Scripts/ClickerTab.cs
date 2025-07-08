using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class ClickerTab : MonoBehaviour, ITab
    {
        public TabType TabType => TabType.Clicker;

        [Inject] private ClickerPresenter _presenter;

        public void OnShow()
        {
            gameObject.SetActive(true);
            _presenter.Start();
        }

        public void OnHide()
        {
            _presenter.Stop();
            gameObject.SetActive(false);
        }
    }
}