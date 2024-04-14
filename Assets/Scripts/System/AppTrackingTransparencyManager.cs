using System;
using UnityEngine;
#if UNITY_IPHONE
using Unity.Advertisement.IosSupport;
#endif

namespace Cocktailor
{
    public class AppTrackingTransparencyManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
#if UNITY_IPHONE
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                sentTrackingAuthorizationRequest?.Invoke();
            }
#endif
        }
#if UNITY_IPHONE
        public event Action sentTrackingAuthorizationRequest;
#endif
    }
}