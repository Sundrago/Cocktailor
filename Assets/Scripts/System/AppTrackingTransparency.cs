using System;
#if UNITY_IPHONE
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;

public class AppTrackingTransparency : MonoBehaviour
{
#if UNITY_IPHONE
    public event Action sentTrackingAuthorizationRequest;
#endif
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IPHONE
        if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            sentTrackingAuthorizationRequest?.Invoke();
        }
#endif
    }
}
