using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum PopupType
    {
        SingleButton,
        TwoButtons
    }

    public class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance { get; private set; }

        [SerializeField] private GameObject confirmButton;
        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private Text popupTextContent;
        [SerializeField] private Transform popupContainer;
        [SerializeField] private Image backGroundImage;

        private Action confirmAction, cancelAction;

        private void Awake()
        {
            Instance = this;
        }

        public void OpenPopup(PopupType popupType, string message, Action yesAction = null,
            Action noAction = null)
        {
            ConfigurePopup(popupType, message);
            confirmAction = yesAction;
            cancelAction = noAction;
            sfxManager.PlaySfx(7);
            BeginPopupAnimation();
            popupContainer.gameObject.SetActive(true);
            backGroundImage.gameObject.SetActive(true);
        }

        private void ConfigurePopup(PopupType popupType, string message)
        {
            confirmButton.SetActive(popupType == PopupType.SingleButton);
            yesButton.SetActive(popupType == PopupType.TwoButtons);
            noButton.SetActive(popupType == PopupType.TwoButtons);

            popupTextContent.GetComponent<Text>().text = message;
        }

        private void BeginPopupAnimation()
        {
            if (!DOTween.IsTweening(popupContainer.transform))
                popupContainer.transform.DOScale(0.85f, 0.25f)
                    .From()
                    .SetEase(Ease.OutElastic)
                    .OnComplete(PauseGame);
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ClosePopup()
        {
            Time.timeScale = 1;
            popupContainer.gameObject.SetActive(false);
            backGroundImage.gameObject.SetActive(false);
        }

        public void OnYesButtonClick()
        {
            confirmAction?.Invoke();
            ClosePopup();
        }

        public void OnNoButtonClick()
        {
            cancelAction?.Invoke();
            ClosePopup();
        }
    }
}