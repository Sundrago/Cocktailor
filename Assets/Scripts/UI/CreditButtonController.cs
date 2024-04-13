using Cocktailor.Utility;
using UnityEngine;
using UnityEngine.Serialization;

public class CreditButtonController : MonoBehaviour
{
    [SerializeField] private GameObject creditButton;
    [SerializeField] private GameObject creditImage;
    
    private Animator creditAnimator;
    private bool isCreditsVisible = false;

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
        OpenURL(PrivateData.PlayStoreURL);
#elif UNITY_IOS
        UnityEngine.iOS.Device.RequestStoreReview();
        Application.OpenURL(PrivateData.AppStoreURL);
#endif
    }
    
    public void OpenInstagram()
    {
        Application.OpenURL(PrivateData.InstagramURL);
    }
}