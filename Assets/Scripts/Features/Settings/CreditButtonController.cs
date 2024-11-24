using UnityEngine;
#if UNITY_IPHONE
using UnityEngine.iOS;
#endif

namespace Cocktailor
{
    public class CreditButtonController : MonoBehaviour
    {
        [SerializeField] private GameObject creditButton;
        [SerializeField] private GameObject creditImage;

        private Animator creditAnimator;
        private bool isCreditsVisible;

        private void Awake()
        {
            creditAnimator = creditImage?.GetComponent<Animator>();
        }

        public void ToggleCreditsVisibility()
        {
            isCreditsVisible = !isCreditsVisible;
            UpdateCreditVisibility(isCreditsVisible);
        }

        private void UpdateCreditVisibility(bool show)
        {
            if (creditAnimator == null) return;

            creditImage.gameObject.SetActive(show);
            creditAnimator.SetTrigger(show ? "Show" : "Hide");
        }

        public void RequestStoreReview()
        {
#if UNITY_ANDROID
            Application.OpenURL(PrivateData.PlayStoreURL);
#elif UNITY_IOS
            Device.RequestStoreReview();
            Application.OpenURL(PrivateData.AppStoreURL);
#endif
        }

        public void OpenInstagram()
        {
            Application.OpenURL(PrivateData.InstagramURL);
        }
    }
}