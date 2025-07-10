using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Scripts
{
    public class BreedsView : MonoBehaviour
    {
        public Transform listContainer;
        public GameObject breedItemPrefab;
        public GameObject loadingIndicator;

        public GameObject popup;
        public TMP_Text popupName;
        public TMP_Text popupDescription;
        public Button popupButton;

        private void Awake()
        {
            popupButton.onClick.AddListener(HidePopup);
        }

        public void ClearList()
        {
            foreach (Transform child in listContainer)
                Destroy(child.gameObject);
        }

        public void ShowLoading(bool show)
        {
            loadingIndicator.SetActive(show);
        }

        public void ShowPopup(string textName, string description)
        {
            popup.SetActive(true);
            popupName.text = textName;
            popupDescription.text = description;
        }

        public void HidePopup()
        {
            popup.SetActive(false);
        }

        public void AddBreedItem(int index, string textName, Action onClick)
        {
            var go = Instantiate(breedItemPrefab, listContainer);
            go.GetComponentInChildren<TMP_Text>().text = $"{index + 1} - {textName}";
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
        }
    }
}