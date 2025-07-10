using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts
{
    public class ClickerView : MonoBehaviour
    {
        public Button clickButton;
        public TextMeshProUGUI currencyText;
        public TextMeshProUGUI energyText;

        public ParticleSystem clickParticles;
        public Transform currencyFlyTarget;
        public GameObject currencyPrefab;

        public AudioSource clickSound;

        private void Start()
        {
            clickButton.onClick.AddListener(() => OnClick?.Invoke());
        }

        public event Action OnClick;

        public void SetCurrency(int value)
        {
            currencyText.text = $"Заработано: {value}";
        }

        public void SetEnergy(int value)
        {
            energyText.text = $"Энергии: {value}";
        }

        public void PlayClickVFX()
        {
            clickParticles.Play();

            var go = Instantiate(currencyPrefab, clickButton.transform.position, Quaternion.identity, transform);
            go.transform.DOMove(currencyFlyTarget.position, 1f).OnComplete(() => Destroy(go));

            clickButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1);
            clickSound.Play();
        }
    }
}