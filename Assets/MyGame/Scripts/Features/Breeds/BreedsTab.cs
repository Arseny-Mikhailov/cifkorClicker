using UnityEngine;
using Zenject;

namespace MyGame.Scripts
{
    public class BreedsTab : MonoBehaviour, ITab
    {
        public TabType TabType => TabType.Breeds;

        [Inject] private BreedsPresenter _presenter;

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