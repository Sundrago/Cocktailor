using System;
using System.Collections.Generic;
using Cocktailor.Utility;
using Features.Quiz;
using Features.Quize;
using Features.RecipeViewer;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Serialization;

public class AdManager : MonoBehaviour
{
    [FormerlySerializedAs("mainControl")] [SerializeField] private RecipeViewerManager recipeViewerManager;
    [SerializeField] private QuizManager quizManager;
    
    private RewardedAd rewardedAd;
    private InterstitialAd interstitial;
    private RewardedInterstitialAd rewardedInterstitialAd;
    
    private AdType adType;
    private int quizIndex;
    private float lastAdShownTime;

    private Action onAdWatch;
    
    public enum AdType
    {
        InterstitialAd, RewardedAd
    }
    
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadInterstitialAd();
        LoadRewardAds();
        
        lastAdShownTime = Time.time;
    }

    public void ShowAds(AdType adType, Action onAdWatch)
    {
        this.onAdWatch = onAdWatch;
        
        if (adType == AdType.RewardedAd)
        {
            if (this.rewardedAd.IsLoaded())
            {
                this.rewardedAd.Show();
            }
            else
            {
                OnRewardFinished();
                LoadRewardAds();
            }
        }
        else if (adType == AdType.InterstitialAd)
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                OnRewardFinished();
                LoadInterstitialAd();
            }
        }
    }
    
    private void OnRewardFinished()
    {
        onAdWatch?.Invoke();
        onAdWatch = null;
    }

    private void LoadRewardAds(){

        #if UNITY_ANDROID
            string adUnitId = PrivateData.AndroidRewardUnitID;
        #elif UNITY_IPHONE
            string adUnitId = PrivateData.IphoneRewardUnitID;
        #else
            string adUnitId = "unexpected_platform";
        #endif
        
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        OnRewardFinished();
        LoadRewardAds();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        OnRewardFinished();
        LoadRewardAds();
    }

    private void LoadInterstitialAd()
    {
        #if UNITY_ANDROID
            string adUnitId = PrivateData.AndroidInterstitialUnitID;
        #elif UNITY_IPHONE
            string adUnitId = PrivateData.IphoneInterstitialUnitID;
        #else
            string adUnitId = "unexpected_platform";
        #endif
        
        this.interstitial = new InterstitialAd(adUnitId);
        this.interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    private void HandleInterstitialFailedToShow(object sender, AdErrorEventArgs args)
    {
        OnRewardFinished();
        LoadInterstitialAd();
    }
    
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        OnRewardFinished();
        LoadInterstitialAd();
    }
}
