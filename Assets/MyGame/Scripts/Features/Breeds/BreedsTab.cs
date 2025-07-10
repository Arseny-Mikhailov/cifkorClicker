using MyGame.Scripts.Core;
using MyGame.Scripts.Features;
using MyGame.Scripts.Features.Breeds;
using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class BreedsTab : MonoBehaviour, ITab
    {
        [Inject] private BreedsPresenter _presenter;
        public TabType TabType => TabType.Breeds;

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