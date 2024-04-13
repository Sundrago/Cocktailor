using System;
using System.Collections;
using System.Collections.Generic;
using Cocktailor.Utility;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Features.RecipeViewer;
using UnityEngine.Serialization;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SfxManager sfxManager;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown sfxDropdown;
    [SerializeField] private Dropdown fpsDropdown;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private GameObject subscriptionPanel;
    [SerializeField] private GameObject[] proPreviewImages = new GameObject[5];
    [SerializeField] private GameObject loadingIcon;
    [FormerlySerializedAs("mainControl")] [SerializeField] private RecipeViewerManager recipeViewerManager;
    [SerializeField] private SubscriptionOverviewCarousel subscriptionOverviewCarousel;

    Vector3 originalPos;

    public static SettingsManager Instance { get; private set;  }

    private void Awake()
    {
        Instance = this;
    }
    
    public void Start()
    {
        if(!PlayerPrefs.HasKey("resolutionDeviceType"))
        {
            var identifier = SystemInfo.deviceModel;
            if (identifier.StartsWith("iPhone"))
            {
                PlayerPrefs.SetInt("resolutionDeviceType", 0);
            }
            else if (identifier.StartsWith("iPad"))
            {
                PlayerPrefs.SetInt("resolutionDeviceType", 1);
            }
        }
        
        resolutionDropdown.value = PlayerPrefs.GetInt("resolutionDeviceType");
        sfxDropdown.value = PlayerPrefs.GetInt("sfxSetting");
        fpsDropdown.value = PlayerPrefs.GetInt("fpsSetting");
        
        ChangeResolution();
        ChangeFPS();
    }

    public void ChangeResolution() 
    {
        if(PlayerPrefs.GetInt("resolutionDeviceType") != resolutionDropdown.value)
        {
            AlertDialog dialog = AlertDialog.CreateInstance();
            dialog.Title = "경고";
            dialog.Message = "바뀐 해상도 설정을 유지할까요?";
          
            dialog.AddButton("네", () => {
                PlayerPrefs.SetInt("resolutionDeviceType", resolutionDropdown.value);
            });

            dialog.AddCancelButton("아니요", () => {
                resolutionDropdown.value = PlayerPrefs.GetInt("resolutionDeviceType");
                ChangeResolution();
            });

            dialog.Show(); 
            sfxManager.PlaySfx(7);
        }


        if (resolutionDropdown.value == 0)
        {
            canvasScaler.referenceResolution = new Vector2(800, 600);
            foreach(GameObject obj in proPreviewImages)
            {
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(800, obj.GetComponent<RectTransform>().sizeDelta.y);
            }
        } else
        {
            canvasScaler.referenceResolution = new Vector2(1200, 600);
            foreach (GameObject obj in proPreviewImages)
            {
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, obj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        sfxManager.PlaySfx(3);
    }

    public void ChangeSFXSetting()
    {
        PlayerPrefs.SetInt("sfxSetting", sfxDropdown.value);
        sfxManager.PlaySfx(3);
    }

    public void ChangeFPS()
    {
        PlayerPrefs.SetInt("fpsSetting", fpsDropdown.value);

        if(fpsDropdown.value == 0) Application.targetFrameRate = 60;
        else Application.targetFrameRate = 30;
        sfxManager.PlaySfx(3);
    }

    public void ResetData()
    {
        AlertDialog dialog = AlertDialog.CreateInstance();
        dialog.Title = "경고";
        dialog.Message = "데이터를 초기화하고 앱을 재실행할까요?";
        
        dialog.AddButton("네", () => {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });

        dialog.AddCancelButton("아니요", () => {
        });

        dialog.Show();
        sfxManager.PlaySfx(7);
    }
    
    public void OpenCocktailorPro()
    {
        if (DOTween.IsTweening(subscriptionPanel.transform)) return;
        originalPos = subscriptionPanel.transform.position;
        subscriptionPanel.transform.DOMoveY(-Screen.height, 0.5f)
            .From()
            .SetEase(Ease.OutCirc);
        subscriptionPanel.SetActive(true);
        sfxManager.PlaySfx(3);
        subscriptionOverviewCarousel.HighlightImage(0);
    }

    public void CloseCocktailorPro()
    {
        if (loadingIcon.activeSelf)
        {
            loadingIcon.SetActive(false);
            //return;
        }
        if (DOTween.IsTweening(subscriptionPanel.transform)) return;
        subscriptionPanel.transform.DOMoveY(-Screen.height, 0.75f)
            .SetEase(Ease.OutCirc)
            .OnComplete(DeactivePro);
        sfxManager.PlaySfx(3);
    }

    private void DeactivePro()
    {
        subscriptionPanel.SetActive(false);
        subscriptionPanel.transform.position = originalPos;
    }

    public void BugReport()
    {
        sfxManager.PlaySfx(3);

        string email = PrivateData.DevEmail;
        string subject = MyEscapeURL("[칵테일러] 문의/건의사항/버그신고");
        string body = MyEscapeURL("[ " + SystemInfo.deviceModel + " | " + SystemInfo.operatingSystem + " | " + Application.version + " ]\r\n\n 문의 건의사항을 언제나 환영합니다.\n버그의 경우 화면 캡쳐나 자세한 경로를 설명해주시면 빠른 해결이 가능합니다.");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
#if UNITY_ANDROID
        Application.OpenURL("PrivateData.PlayStoreURL");
#endif
    }

    public void Review()
    {
        sfxManager.PlaySfx(3);
        Utilities.RequestStoreReview();
#if UNITY_ANDROID
    Application.OpenURL("market://details?id=net.sundragon.cocktail");
#elif UNITY_IPHONE
        Application.OpenURL(PrivateData.AppStoreURL);
#endif
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void PrivacyPolicy()
    {
        sfxManager.PlaySfx(3);
        Application.OpenURL(PrivateData.PrivacyPolicyURL);
    }

    public void UserAgreement()
    {
        sfxManager.PlaySfx(3);
        Application.OpenURL(PrivateData.UserAgreementURL);
    }
}
