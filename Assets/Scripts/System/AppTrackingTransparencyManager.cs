using System;
using UnityEngine;
using UnityEditor;
// using UnityEditor.Callbacks;
using System.IO;

#if UNITY_IPHONE
using Unity.Advertisement.IosSupport;
// using UnityEditor.iOS.Xcode;
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
    
    // public class PostBuildStep {
    //     // Set the IDFA request description:
    //     const string k_TrackingDescription = "개인에게 최적화된 광고를 제공하기 위해 사용자의 광고 활동 정보를 수집합니다.";
    //     [PostProcessBuild(0)]
    //     public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
    //         if (buildTarget == BuildTarget.iOS) {
    //             AddPListValues(pathToXcode);
    //         }
    //     }
    //
    //     // Implement a function to read and write values to the plist file:
    //     static void AddPListValues(string pathToXcode) {
    //         // Retrieve the plist file from the Xcode project directory:
    //         string plistPath = pathToXcode + "/Info.plist";
    //         PlistDocument plistObj = new PlistDocument();
    //
    //
    //         // Read the values from the plist file:
    //         plistObj.ReadFromString(File.ReadAllText(plistPath));
    //
    //         // Set values from the root object:
    //         PlistElementDict plistRoot = plistObj.root;
    //
    //         // Set the description key-value in the plist:
    //         plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);
    //
    //         // Save changes to the plist:
    //         File.WriteAllText(plistPath, plistObj.WriteToString());
    //     }
    // }
}