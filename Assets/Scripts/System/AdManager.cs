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

        public static AdManager Instance;
        
        [SerializeField] private RecipeViewerPanel recipeViewerPanel; 
        [SerializeField] private QuizPanel quizPanel;

        private AdType adType;
        private InterstitialAd interstitial;
        private Action onAdWatch;
        private int quizIndex;
        private RewardedAd rewardedAd;
        private RewardedInterstitialAd rewardedInterstitialAd;
        private int dailyAdCount, useDateCount;
        private DateTime adShownTime;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MobileAds.Initialize(initStatus => { });
            LoadInterstitialAd();
            LoadRewardAds();
        }

        private void InitAdCount()
        {
            //initial launch
            if (!PlayerPrefs.HasKey("lastUpdateTime"))
            {
                dailyAdCount = 0;
                useDateCount = 1;
                PlayerPrefs.SetString("lastUpdateTime", Utility.DateTimeToString(DateTime.Now));
            }

            DateTime lastUpdateTime = Utility.StringToDateTime(PlayerPrefs.GetString("lastUpdateTime"));
            if (lastUpdateTime.Date != DateTime.Now.Date)
            {
                dailyAdCount = 0;
                useDateCount = PlayerPrefs.GetInt("useDateCount", 1);
                useDateCount += 1;
                PlayerPrefs.SetString("lastUpdateTime", Utility.DateTimeToString(DateTime.Now));
            }
        }

        public void ShowAds(AdType adType, Action onAdWatch)
        {
            this.onAdWatch = onAdWatch;

            if (PlayerPrefs.GetInt("subscribed") == 1)
            {
                OnRewardFinished();
                return;
            }
            
            if (adType == AdType.RewardedAd)
            {
                if (rewardedAd.IsLoaded())
                {
                    PlayerPrefs.SetString("adShownTime", Utility.DateTimeToString(adShownTime));
                    rewardedAd.Show();
                }
                else
                {
                    OnRewardFinished();
                    LoadRewardAds();
                }
                return;
            }
            
            if (adType == AdType.InterstitialAd)
            {
                if (!ShouldShowAd())
                {
                    OnRewardFinished();
                    return;
                }
                
                if (interstitial.IsLoaded())
                {
                    UpdateAdShowCount();
                    interstitial.Show();
                }
                else
                {
                    OnRewardFinished();
                    LoadInterstitialAd();
                }
            }
        }

        private void UpdateAdShowCount()
        {
            this.onAdWatch = onAdWatch;
            adShownTime = DateTime.Now;
            dailyAdCount += 1;
            
            PlayerPrefs.SetString("adShownTime", Utility.DateTimeToString(adShownTime));
            PlayerPrefs.SetInt("dailyAdCount",dailyAdCount);
            PlayerPrefs.SetInt("useDateCount",useDateCount);
            PlayerPrefs.Save();
        }

        private bool ShouldShowAd()
        {
            int minimumInterval = GetMinimumInterval();
            int maxADCount = GetMaxADCount();

            if (Time.time < 60)
                return false;
            
            TimeSpan timespan = DateTime.Now - adShownTime;
            if (timespan.Minutes < minimumInterval)
                return false;

            if (dailyAdCount >= maxADCount)
                return false;
            
            return true;
        }

        private int GetMaxADCount()
        {
            float dateNormal = useDateCount / 10;
            dateNormal = Mathf.Clamp(dateNormal, 0, 1);
            return Mathf.RoundToInt(Mathf.Lerp(10, 50, dateNormal));
        }

        private int GetMinimumInterval()
        {
            float dateNormal = useDateCount / 10;
            dateNormal = Mathf.Clamp(dateNormal, 0, 1);
            return Mathf.RoundToInt(Mathf.Lerp(10, 1, dateNormal));
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