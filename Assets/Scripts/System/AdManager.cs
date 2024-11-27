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
        private InterstitialAd interstitialAd;
        private Action onAdWatch;
        private int quizIndex;
        private RewardedAd rewardedAd;
        // private RewardedInterstitialAd rewardedInterstitialAd;
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
            
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            
            // Raised when the ad closed full screen content.
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardAds();
            };
            // Raised when the ad failed to open full screen content.
            rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardAds();
            };
            
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
                if (rewardedAd != null && rewardedAd.CanShowAd())
                {
                    PlayerPrefs.SetString("adShownTime", Utility.DateTimeToString(adShownTime));
                    rewardedAd.Show((Reward reward) =>
                    {
                        OnRewardFinished();
                    });
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

                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    UpdateAdShowCount();
                    interstitialAd.Show();
                    OnRewardFinished();
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

            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        HandleRewardedAdFailedToShow();
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                });
        }

        public void HandleRewardedAdFailedToShow()
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

            // Clean up the old ad before loading a new one.
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            
            
            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        HandleInterstitialFailedToShow();
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                });
        }

        private void HandleInterstitialFailedToShow()
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