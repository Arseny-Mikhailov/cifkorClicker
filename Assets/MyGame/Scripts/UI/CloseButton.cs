using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts.UI
{
    public class CloseButton : MonoBehaviour
    {
        public Button Btn;
        [SerializeField] private GameObject panel;

        public void Start()
        {
            Btn.onClick.AddListener(BtnOnclicked);
        }

        private void BtnOnclicked()
        {
            panel.SetActive(false);
        }
    }
}
