using System;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX SDK for interstitial and rewarded ads.
    /// Simplified version for Unity 2022+ with AppLovin SDK 13.x
    /// </summary>
    public class AppLovinManager : MonoBehaviour
    {
        public static AppLovinManager Instance { get; private set; }

        [Header("AppLovin MAX Configuration")]
        [Tooltip("Your AppLovin SDK Key from the dashboard")]
        [SerializeField] private string sdkKey = "G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0";
        
        [Tooltip("Interstitial Ad Unit ID from AppLovin dashboard")]
        [SerializeField] private string interstitialAdUnitId = "9375d1dbeb211048";
        
        [Tooltip("Rewarded Ad Unit ID from AppLovin dashboard")]
        [SerializeField] private string rewardedAdUnitId = "c96bd6d70b3804fa";

        [Header("Debug Settings")]
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
            gameObject.name = "AppLovinManager";
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
            if (string.IsNullOrEmpty(sdkKey) || sdkKey == "YOUR_SDK_KEY")
            {
                Debug.LogError("[AppLovin] SDK Key not set! Please set it in the Inspector.");
                return;
            }

            LogDebug("Initializing AppLovin MAX SDK...");

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.SetSdkKey(sdkKey);
            
            if (Debug.isDebugBuild)
            {
                MaxSdk.SetVerboseLogging(true);
            }

            MaxSdk.InitializeSdk();
#else
            Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            LogDebug($"MAX SDK Initialized - Country: {config.CountryCode}, TestMode: {config.IsTestModeEnabled}");
            isInitialized = true;

            InitializeInterstitialAds();
            InitializeRewardedAds();
        }

        #region Interstitial Ads

        private void InitializeInterstitialAds()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

            LoadInterstitial();
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial loaded - Network: {adInfo.NetworkName}");
            interstitialRetryAttempt = 0;
            OnInterstitialAdReady?.Invoke();
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            LogDebug($"Interstitial load failed: {errorInfo.Code}. Retrying in {retryDelay}s...");
            Invoke(nameof(LoadInterstitial), (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial displayed - Network: {adInfo.NetworkName}");
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial failed to display: {errorInfo.Code}");
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Interstitial dismissed");
            FirebaseAnalyticsManager.Instance?.LogAdClosed("applovin", "interstitial");
            OnInterstitialAdClosed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
            FirebaseAnalyticsManager.Instance?.LogAdImpression("applovin", "interstitial");
        }

        #endregion

        #region Rewarded Ads

        private void InitializeRewardedAds()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

            LoadRewardedAd();
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad loaded - Network: {adInfo.NetworkName}");
            rewardedRetryAttempt = 0;
            OnRewardedAdReady?.Invoke();
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
            LogDebug($"Rewarded ad load failed: {errorInfo.Code}. Retrying in {retryDelay}s...");
            Invoke(nameof(LoadRewardedAd), (float)retryDelay);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad failed to display: {errorInfo.Code}");
            OnRewardedAdShowFailed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad displayed - Network: {adInfo.NetworkName}");
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Rewarded ad dismissed");
            OnRewardedAdClosed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad received reward: {reward.Label} x{reward.Amount}");
            OnRewardedAdRewarded?.Invoke();
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
        }

        #endregion
#endif

        #region Public Methods

        public void LoadInterstitial()
        {
#if UNITY_EDITOR
            LogDebug("[EDITOR] Simulating interstitial load");
            Invoke(nameof(SimulateInterstitialReady), 0.5f);
#elif UNITY_ANDROID || UNITY_IOS
            LogDebug("Loading interstitial...");
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
                LogDebug("[EDITOR] Simulating interstitial show");
                Invoke(nameof(SimulateInterstitialClosed), 1f);
            }
            else
            {
                OnInterstitialAdShowFailed?.Invoke();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (IsInterstitialReady())
            {
                LogDebug("Showing interstitial");
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
            LogDebug("[EDITOR] Simulating rewarded ad load");
            Invoke(nameof(SimulateRewardedReady), 0.5f);
#elif UNITY_ANDROID || UNITY_IOS
            LogDebug("Loading rewarded ad...");
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
                LogDebug("[EDITOR] Simulating rewarded ad show");
                Invoke(nameof(SimulateRewardGranted), 1f);
            }
            else
            {
                OnRewardedAdShowFailed?.Invoke();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (IsRewardedAdReady())
            {
                LogDebug("Showing rewarded ad");
                MaxSdk.ShowRewardedAd(rewardedAdUnitId);
            }
            else
            {
                LogDebug("Rewarded ad not ready");
                OnRewardedAdShowFailed?.Invoke();
            }
#endif
        }

        /// <summary>
        /// Show Mediation Debugger UI (for testing/debugging)
        /// </summary>
        public void ShowMediationDebugger()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (isInitialized)
            {
                MaxSdk.ShowMediationDebugger();
            }
#else
            LogDebug("Mediation Debugger only available on Android/iOS");
#endif
        }

        #endregion

        #region Editor Simulation

#if UNITY_EDITOR
        private void SimulateInitSuccess()
        {
            isInitialized = true;
            LogDebug("[EDITOR] Simulated initialization successful");
            OnInterstitialAdReady?.Invoke();
            OnRewardedAdReady?.Invoke();
        }

        private void SimulateInterstitialReady()
        {
            OnInterstitialAdReady?.Invoke();
        }

        private void SimulateInterstitialClosed()
        {
            FirebaseAnalyticsManager.Instance?.LogAdClosed("applovin", "interstitial");
            OnInterstitialAdClosed?.Invoke();
        }

        private void SimulateRewardedReady()
        {
            OnRewardedAdReady?.Invoke();
        }

        private void SimulateRewardGranted()
        {
            OnRewardedAdRewarded?.Invoke();
            Invoke(nameof(SimulateRewardedClosed), 0.5f);
        }

        private void SimulateRewardedClosed()
        {
            OnRewardedAdClosed?.Invoke();
        }
#endif

        #endregion

        #region Utility

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[AppLovin] {message}");
            }
        }

        #endregion
    }
}
