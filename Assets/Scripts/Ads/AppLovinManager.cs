using System;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX SDK for interstitial and rewarded ads.
    /// Follows official AppLovin Unity integration pattern.
    /// </summary>
    public class AppLovinManager : MonoBehaviour
    {
        public static AppLovinManager Instance { get; private set; }

        [Header("Ad Unit IDs")]
        [SerializeField] private string interstitialAdUnitId = "9375d1dbeb211048";
        [SerializeField] private string rewardedAdUnitId = "c96bd6d70b3804fa";

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        private bool isInitialized = false;
        private int interstitialRetryAttempt = 0;
        private int rewardedRetryAttempt = 0;

        // Public events for ad lifecycle
        public event Action OnInterstitialAdReady;
        public event Action OnInterstitialAdClosed;
        public event Action OnInterstitialAdShowFailed;
        
        public event Action OnRewardedAdReady;
        public event Action OnRewardedAdRewarded;
        public event Action OnRewardedAdClosed;
        public event Action OnRewardedAdShowFailed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeAppLovin();
        }

        private void InitializeAppLovin()
        {
#if UNITY_EDITOR
            LogDebug("Running in Editor - simulating SDK initialization");
            Invoke(nameof(SimulateInitSuccess), 1f);
#elif UNITY_ANDROID || UNITY_IOS
            LogDebug("Initializing AppLovin MAX SDK...");

            // Attach callback and initialize - SDK key is in AppLovinSettings.asset
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.InitializeSdk();
#else
            Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            LogDebug($"MAX SDK Initialized - Country: {config.CountryCode}");
            isInitialized = true;

            InitializeInterstitialAds();
            InitializeRewardedAds();
        }

        #region Interstitial Ads

        private void InitializeInterstitialAds()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaid;

            LoadInterstitial();
        }

        private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial loaded - Network: {adInfo.NetworkName}");
            interstitialRetryAttempt = 0;
            OnInterstitialAdReady?.Invoke();
        }

        private void OnInterstitialLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            LogDebug($"Interstitial load failed: {errorInfo.Message}. Retry in {retryDelay}s");
            Invoke(nameof(LoadInterstitial), (float)retryDelay);
        }

        private void OnInterstitialDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial display failed: {errorInfo.Message}");
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Interstitial hidden");
            FirebaseAnalyticsManager.Instance?.LogAdClosed("applovin", "interstitial");
            OnInterstitialAdClosed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
            FirebaseAnalyticsManager.Instance?.LogAdImpression("applovin", "interstitial");
        }

        #endregion

        #region Rewarded Ads

        private void InitializeRewardedAds()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedHidden;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedReceived;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedRevenuePaid;

            LoadRewardedAd();
        }

        private void OnRewardedLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded loaded - Network: {adInfo.NetworkName}");
            rewardedRetryAttempt = 0;
            OnRewardedAdReady?.Invoke();
        }

        private void OnRewardedLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
            LogDebug($"Rewarded load failed: {errorInfo.Message}. Retry in {retryDelay}s");
            Invoke(nameof(LoadRewardedAd), (float)retryDelay);
        }

        private void OnRewardedDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded display failed: {errorInfo.Message}");
            OnRewardedAdShowFailed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Rewarded hidden");
            OnRewardedAdClosed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedReceived(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded received: {reward.Label} x{reward.Amount}");
            OnRewardedAdRewarded?.Invoke();
        }

        private void OnRewardedRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
        }

        #endregion
#endif

        #region Public Methods

        public void LoadInterstitial()
        {
#if UNITY_EDITOR
            LogDebug("[Editor] Simulating interstitial load");
            Invoke(nameof(SimulateInterstitialReady), 0.5f);
#elif UNITY_ANDROID || UNITY_IOS
            MaxSdk.LoadInterstitial(interstitialAdUnitId);
#endif
        }

        public bool IsInterstitialReady()
        {
#if UNITY_EDITOR
            return isInitialized;
#elif UNITY_ANDROID || UNITY_IOS
            return isInitialized && MaxSdk.IsInterstitialReady(interstitialAdUnitId);
#else
            return false;
#endif
        }

        public void ShowInterstitial()
        {
#if UNITY_EDITOR
            if (isInitialized)
            {
                LogDebug("[Editor] Simulating interstitial");
                Invoke(nameof(SimulateInterstitialClosed), 1f);
            }
            else
            {
                OnInterstitialAdShowFailed?.Invoke();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (IsInterstitialReady())
            {
                MaxSdk.ShowInterstitial(interstitialAdUnitId);
            }
            else
            {
                LogDebug("Interstitial not ready");
                OnInterstitialAdShowFailed?.Invoke();
            }
#endif
        }

        public void LoadRewardedAd()
        {
#if UNITY_EDITOR
            LogDebug("[Editor] Simulating rewarded load");
            Invoke(nameof(SimulateRewardedReady), 0.5f);
#elif UNITY_ANDROID || UNITY_IOS
            MaxSdk.LoadRewardedAd(rewardedAdUnitId);
#endif
        }

        public bool IsRewardedAdReady()
        {
#if UNITY_EDITOR
            return isInitialized;
#elif UNITY_ANDROID || UNITY_IOS
            return isInitialized && MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
#else
            return false;
#endif
        }

        public void ShowRewardedAd()
        {
#if UNITY_EDITOR
            if (isInitialized)
            {
                LogDebug("[Editor] Simulating rewarded");
                Invoke(nameof(SimulateRewardGranted), 1f);
            }
            else
            {
                OnRewardedAdShowFailed?.Invoke();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (IsRewardedAdReady())
            {
                MaxSdk.ShowRewardedAd(rewardedAdUnitId);
            }
            else
            {
                LogDebug("Rewarded not ready");
                OnRewardedAdShowFailed?.Invoke();
            }
#endif
        }

        public void ShowMediationDebugger()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (isInitialized)
            {
                MaxSdk.ShowMediationDebugger();
            }
#endif
        }

        public string GetDebugInfo()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!isInitialized) return "SDK not initialized";
            return $"Initialized: {isInitialized}\n" +
                   $"Interstitial Ready: {IsInterstitialReady()}\n" +
                   $"Rewarded Ready: {IsRewardedAdReady()}";
#else
            return "Debug info only on device";
#endif
        }

        #endregion

        #region Editor Simulation

#if UNITY_EDITOR
        private void SimulateInitSuccess()
        {
            isInitialized = true;
            LogDebug("[Editor] SDK initialized");
            OnInterstitialAdReady?.Invoke();
            OnRewardedAdReady?.Invoke();
        }

        private void SimulateInterstitialReady() => OnInterstitialAdReady?.Invoke();
        
        private void SimulateInterstitialClosed()
        {
            FirebaseAnalyticsManager.Instance?.LogAdClosed("applovin", "interstitial");
            OnInterstitialAdClosed?.Invoke();
        }

        private void SimulateRewardedReady() => OnRewardedAdReady?.Invoke();
        
        private void SimulateRewardGranted()
        {
            OnRewardedAdRewarded?.Invoke();
            Invoke(nameof(SimulateRewardedClosed), 0.5f);
        }

        private void SimulateRewardedClosed() => OnRewardedAdClosed?.Invoke();
#endif

        #endregion

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[AppLovin] {message}");
            }
        }
    }
}
