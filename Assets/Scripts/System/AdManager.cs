using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cocktailor
{
    /// <summary>
    /// Manages the display of ads and handles the loading and showing of both interstitial and rewarded ads.
    /// </summary>
    public class AdManager : MonoBehaviour
    {
        public enum AdType
        {
            InterstitialAd,
            RewardedAd
        }
        
        [SerializeField] private RecipeViewerPanel recipeViewerPanel; 
        [SerializeField] private QuizPanel quizPanel;

        private AdType adType;
        private InterstitialAd interstitial;
        private float lastAdShownTime;
        private Action onAdWatch;
        private int quizIndex;
        private RewardedAd rewardedAd;
        private RewardedInterstitialAd rewardedInterstitialAd;

        private void Start()
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
                if (rewardedAd.IsLoaded())
                {
                    rewardedAd.Show();
                }
                else
                {
                    OnRewardFinished();
                    LoadRewardAds();
                }
            }
            else if (adType == AdType.InterstitialAd)
            {
                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
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

        private void LoadRewardAds()
        {
#if UNITY_ANDROID
            string adUnitId = PrivateData.AndroidRewardUnitID;
#elif UNITY_IPHONE
            var adUnitId = PrivateData.IphoneRewardUnitID;
#else
            string adUnitId = "unexpected_platform";
#endif

            rewardedAd = new RewardedAd(adUnitId);
            rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            var request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
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
            var adUnitId = PrivateData.IphoneInterstitialUnitID;
#else
            string adUnitId = "unexpected_platform";
#endif

            interstitial = new InterstitialAd(adUnitId);
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdClosed += HandleOnAdClosed;

            var request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
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
}