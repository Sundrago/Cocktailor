using System;
using UnityEngine;
#if UNITY_IPHONE
using Unity.Advertisement.IosSupport;
#endif

namespace Cocktailor
{
    /// <summary>
    /// Handles the management of tracking authorization requests for iOS devices.
    /// </summary>
    public class AppTrackingTransparencyManager : MonoBehaviour
    {
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