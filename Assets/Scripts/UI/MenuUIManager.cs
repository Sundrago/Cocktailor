using System;
using UnityEngine;

namespace UI
{
    public class MenuUIManager : MonoBehaviour
    {
        public static MenuUIManager Instance { get; private set; }

        [SerializeField] private Animator menuPanelAnimator;
        [SerializeField] private Animator menuButtonAnimator;
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private AdManager adManager;

        private bool isMenuVisible;

        private static readonly int Menu = Animator.StringToHash("menu");
        private static readonly int Hide = Animator.StringToHash("hide");
        private static readonly int Back = Animator.StringToHash("back");
        private static readonly int Show = Animator.StringToHash("show");

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            HideMenuInterface();
        }

        public void HandleMenuButtonClick()
        {
            sfxManager.PlaySfx(0);
            adManager.ShowAds(AdManager.AdType.InterstitialAd, OnAdFinished);
        }

        private void OnAdFinished()
        {
            if (!isMenuVisible)
                ShowMenuInterface();
            else
                HideMenuInterface();
        }

        public void HideMenuInterface()
        {
            if (isMenuVisible)
            {
                sfxManager.PlaySfx(0);
                menuButtonAnimator.SetTrigger(Menu);
                menuPanelAnimator.SetTrigger(Hide);
                isMenuVisible = false;
            }
        }

        public void ShowMenuInterface()
        {
            if (!isMenuVisible)
            {
                sfxManager.PlaySfx(0);
                menuPanelAnimator.gameObject.SetActive(true);
                menuButtonAnimator.SetTrigger(Back);
                menuPanelAnimator.SetTrigger(Show);
                isMenuVisible = true;
            }
        }
    }
}