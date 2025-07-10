using MyGame.Scripts.Core;
using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class ClickerTab : MonoBehaviour, ITab
    {
        [Inject] private ClickerPresenter _presenter;
        public TabType TabType => TabType.Clicker;

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