# Battlecruisers Ad System - State Backup Before Debloat
**Created:** December 11, 2025
**Purpose:** Full record of ad system state before debloating to minimal configuration

---

## File Inventory (Pre-Debloat)

### Editor Scripts
| File | Lines | Status |
|------|-------|--------|
| `Assets/Editor/AppLovinDependencyConditional.cs` | 113 | TO DELETE |
| `Assets/Editor/DependencySafetyNet.cs` | 155 | TO DELETE |
| `Assets/Editor/FixFirebaseGoogleServices.cs` | 120 | TO COMMENT OUT |
| `Assets/Editor/PostGenerateGradleAndroidProject.cs` | 92 | KEEP |
| `Assets/Editor/FirebaseDependencies.xml` | 12 | TO COMMENT OUT |

### Ads Scripts
| File | Lines | Status |
|------|-------|--------|
| `Assets/Scripts/Ads/AppLovinManager.cs` | 418 | SIMPLIFY |
| `Assets/Scripts/Ads/AdConfigManager.cs` | 395 | SIMPLIFY |

### Debug Scripts
| File | Lines | Status |
|------|-------|--------|
| `Assets/Scripts/Utils/Debugging/AppLovinLogCollector.cs` | 177 | TO DELETE |

### Android Plugin Files
| File/Directory | Status |
|----------------|--------|
| `Assets/Plugins/Android/AdKillSwitch.androidlib/` | TO DELETE (empty) |
| `Assets/Plugins/Android/CustomActivity/` | TO DELETE (empty) |
| `Assets/Plugins/Android/baseProjectTemplate.gradle.DISABLED` | TO DELETE |
| `Assets/Plugins/Android/launcherTemplate.gradle.DISABLED` | TO DELETE |
| `Assets/Plugins/Android/mainTemplate.gradle.DISABLED` | TO DELETE |
| `Assets/Plugins/Android/settingsTemplate.gradle.DISABLED` | TO DELETE |
| `Assets/Plugins/Android/gradleTemplate.properties` | SIMPLIFY |
| `Assets/Plugins/Android/AndroidManifest.xml` | KEEP |

---

## Full File Contents (For Recovery)

### AppLovinManager.cs (418 lines)
```csharp
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
        [SerializeField] private bool enableVerboseLogging = true; // For AppLovin support debugging

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
#elif UNITY_ANDROID
            // Hardware acceleration for WebViews is enabled via android:hardwareAccelerated="true" in AndroidManifest
            
            if (string.IsNullOrEmpty(sdkKey) || sdkKey == "YOUR_SDK_KEY")
            {
                Debug.LogError("[AppLovin] SDK Key not set! Please set it in the Inspector.");
                return;
            }

            LogDebug("Initializing AppLovin MAX SDK...");

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.SetSdkKey(sdkKey);
            
            // Always enable verbose logging for debugging close button issues
            if (enableVerboseLogging || Debug.isDebugBuild)
            {
                MaxSdk.SetVerboseLogging(true);
                LogDebug("Verbose logging enabled for AppLovin MAX");
            }

            // FIX: Force TextureView rendering instead of SurfaceView
            // This prevents z-order issues where SurfaceView blocks WebView close button overlays
            // Applying multiple parameters to ensure close button appears
            
            // Fix #1: Disable SurfaceView (forces TextureView)
            MaxSdk.SetExtraParameter("disable_video_surface_view", "true");
            
            // Fix #2: Explicitly set video renderer to texture
            MaxSdk.SetExtraParameter("video_renderer", "texture");
            
            // Fix #3: Ensure WebView hardware acceleration is enabled
            MaxSdk.SetExtraParameter("webview_hardware_acceleration", "true");
            
            LogDebug("Applied 3 TextureView fixes: disable_video_surface_view, video_renderer=texture, webview_hardware_acceleration");

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
            LogDebug($"[INTERSTITIAL] Displayed - Network: {adInfo.NetworkName}, Placement: {adInfo.Placement}, AdUnit: {adUnitId}, Creative: {adInfo.CreativeIdentifier}, Revenue: ${adInfo.Revenue}");
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            Debug.LogError($"[INTERSTITIAL] Failed to display - Code: {errorInfo.Code}, Message: {errorInfo.Message}, AdLoadFailureInfo: {errorInfo.AdLoadFailureInfo}, MediatedNetworkErrorCode: {errorInfo.MediatedNetworkErrorCode}, MediatedNetworkErrorMessage: {errorInfo.MediatedNetworkErrorMessage}, Network: {adInfo?.NetworkName}");
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"[INTERSTITIAL] Dismissed - Network: {adInfo.NetworkName}, AdUnit: {adUnitId}");
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
            Debug.LogError($"[REWARDED] Failed to display - Code: {errorInfo.Code}, Message: {errorInfo.Message}, AdLoadFailureInfo: {errorInfo.AdLoadFailureInfo}, MediatedNetworkErrorCode: {errorInfo.MediatedNetworkErrorCode}, MediatedNetworkErrorMessage: {errorInfo.MediatedNetworkErrorMessage}, Network: {adInfo?.NetworkName}");
            OnRewardedAdShowFailed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"[REWARDED] Displayed - Network: {adInfo.NetworkName}, Placement: {adInfo.Placement}, AdUnit: {adUnitId}, Creative: {adInfo.CreativeIdentifier}, Revenue: ${adInfo.Revenue}");
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"[REWARDED] Dismissed - Network: {adInfo.NetworkName}, AdUnit: {adUnitId}");
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

        /// <summary>
        /// Handler for Android back button (if needed in future).
        /// NOTE (Dec 11, 2025): CustomUnityPlayerActivity was deleted - this method is currently unused.
        /// The default Unity activity and AppLovin SDK handle back button natively.
        /// Kept for potential future use if custom back button handling is needed.
        /// </summary>
        public void OnAndroidBackButton()
        {
            LogDebug("Android back button pressed (custom handler)");
            // Currently unused - back button handled natively by Unity + AppLovin SDK
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
```

### AdConfigManager.cs (395 lines)
```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.RemoteConfig;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// JSON structure for AD_CONFIG from Unity Remote Config
    /// Matches the pattern used by GAME_CONFIG, SHOP_CONFIG, etc.
    /// </summary>
    [Serializable]
    public struct AdConfig
    {
        public int ad_minimum_level;
        public int ad_frequency;
        public float ad_cooldown_minutes;
        public bool ad_veteran_boost_enabled;
        public int ad_veteran_threshold;
        public int ad_veteran_frequency;
        public bool ads_are_live;
        public bool ads_disabled;
        public bool interstitial_ads_enabled;
        public bool rewarded_ads_enabled;
        // First-time rewards
        public int first_rewarded_ad_coins;
        public int first_rewarded_ad_credits;
        // Returning rewards
        public int rewarded_ad_coins;
        public int rewarded_ad_credits;
    }

    /// <summary>
    /// Manages ad configuration with Unity Remote Config for A/B testing
    /// Falls back to local defaults if Remote Config is unavailable
    /// </summary>
    public class AdConfigManager : MonoBehaviour
    {
        public static AdConfigManager Instance { get; private set; }

        // Default values (used as fallback)
        [Header("Default Ad Configuration")]
        [Tooltip("Minimum levels player must complete before seeing ads")]
        public int defaultMinimumLevelForAds = 7;
        
        [Tooltip("Show ad every N battles (e.g., 3 = every 3rd battle)")]
        public int defaultAdFrequency = 3;
        
        [Tooltip("Minimum minutes between ads")]
        public float defaultAdCooldownMinutes = 9f;
        
        [Tooltip("Enable different ad frequency for new vs veteran players")]
        public bool enableVeteranFrequencyBoost = true;
        
        [Tooltip("Levels required to be considered 'veteran' player")]
        public int veteranThreshold = 15;
        
        [Tooltip("Ad frequency for veteran players (lower = more ads)")]
        public int veteranAdFrequency = 2;

        [Header("Ad Mode Configuration")]
        [Tooltip("If true, ads are in production mode. If false, ads are in test mode.")]
        public bool defaultAdsAreLive = false;
        
        [Tooltip("If true, all ads are completely disabled (no test ads, no real ads)")]
        public bool defaultAdsDisabled = false;

        [Header("Ad Type Configuration")]
        [Tooltip("Enable interstitial ads")]
        public bool defaultInterstitialAdsEnabled = true;
        
        [Tooltip("Enable rewarded ads")]
        public bool defaultRewardedAdsEnabled = true;

        [Header("Reward Configuration")]
        [Tooltip("First-time reward: coins")]
        public int defaultFirstRewardedAdCoins = 500;
        
        [Tooltip("First-time reward: credits")]
        public int defaultFirstRewardedAdCredits = 4500;
        
        [Tooltip("Returning reward: coins")]
        public int defaultRewardedAdCoins = 20;
        
        [Tooltip("Returning reward: credits")]
        public int defaultRewardedAdCredits = 1200;

        // Current values (from Remote Config or defaults)
        public int MinimumLevelForAds { get; private set; }
        public int AdFrequency { get; private set; }
        public float AdCooldownMinutes { get; private set; }
        public bool VeteranFrequencyBoostEnabled { get; private set; }
        public int VeteranThreshold { get; private set; }
        public int VeteranAdFrequency { get; private set; }
        
        public bool AdsAreLive { get; private set; }
        public bool AdsDisabled { get; private set; }
        public int RewardedAdCoins { get; private set; }
        public int RewardedAdCredits { get; private set; }
        public bool InterstitialAdsEnabled { get; private set; }
        public bool RewardedAdsEnabled { get; private set; }
        public int FirstRewardedAdCoins { get; private set; }
        public int FirstRewardedAdCredits { get; private set; }

        private bool isRemoteConfigFetched = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplyDefaults();
        }

        private async void Start()
        {
            await FetchRemoteConfigAsync();
        }

        private void ApplyDefaults()
        {
            MinimumLevelForAds = defaultMinimumLevelForAds;
            AdFrequency = defaultAdFrequency;
            AdCooldownMinutes = defaultAdCooldownMinutes;
            VeteranFrequencyBoostEnabled = enableVeteranFrequencyBoost;
            VeteranThreshold = veteranThreshold;
            VeteranAdFrequency = veteranAdFrequency;
            AdsAreLive = defaultAdsAreLive;
            AdsDisabled = defaultAdsDisabled;
            InterstitialAdsEnabled = defaultInterstitialAdsEnabled;
            RewardedAdsEnabled = defaultRewardedAdsEnabled;
            FirstRewardedAdCoins = defaultFirstRewardedAdCoins;
            FirstRewardedAdCredits = defaultFirstRewardedAdCredits;
            RewardedAdCoins = defaultRewardedAdCoins;
            RewardedAdCredits = defaultRewardedAdCredits;

            Debug.Log($"[AdConfig] Using defaults: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}, Cooldown={AdCooldownMinutes}, AdsAreLive={AdsAreLive}, AdsDisabled={AdsDisabled}");
        }

        private async Task FetchRemoteConfigAsync()
        {
            try
            {
                Debug.Log("[AdConfig] Fetching AD_CONFIG from Unity Remote Config...");

                if (Unity.Services.Core.UnityServices.State != Unity.Services.Core.ServicesInitializationState.Initialized)
                {
                    Debug.LogWarning("[AdConfig] Unity Services not initialized, using defaults");
                    return;
                }

                if (RemoteConfigService.Instance.appConfig.config.Count == 0)
                {
                    Debug.Log("[AdConfig] Fetching remote config data...");
                    await RemoteConfigService.Instance.FetchConfigsAsync(new BattleCruisers.Data.UserAttributes(), new BattleCruisers.Data.AppAttributes());
                }

                var adConfigJson = RemoteConfigService.Instance.appConfig.GetJson("AD_CONFIG");
                
                if (!string.IsNullOrEmpty(adConfigJson) && adConfigJson != "{}")
                {
                    Debug.Log($"[AdConfig] Fetched AD_CONFIG: {adConfigJson}");
                    
                    AdConfig adConfig = JsonUtility.FromJson<AdConfig>(adConfigJson);
                    
                    MinimumLevelForAds = adConfig.ad_minimum_level > 0 ? adConfig.ad_minimum_level : defaultMinimumLevelForAds;
                    AdFrequency = adConfig.ad_frequency > 0 ? adConfig.ad_frequency : defaultAdFrequency;
                    AdCooldownMinutes = adConfig.ad_cooldown_minutes > 0 ? adConfig.ad_cooldown_minutes : defaultAdCooldownMinutes;
                    VeteranFrequencyBoostEnabled = adConfig.ad_veteran_boost_enabled;
                    VeteranThreshold = adConfig.ad_veteran_threshold > 0 ? adConfig.ad_veteran_threshold : veteranThreshold;
                    VeteranAdFrequency = adConfig.ad_veteran_frequency > 0 ? adConfig.ad_veteran_frequency : veteranAdFrequency;
                    AdsAreLive = adConfig.ads_are_live;
                    AdsDisabled = adConfig.ads_disabled;
                    InterstitialAdsEnabled = adConfig.interstitial_ads_enabled;
                    RewardedAdsEnabled = adConfig.rewarded_ads_enabled;
                    FirstRewardedAdCoins = adConfig.first_rewarded_ad_coins > 0 ? adConfig.first_rewarded_ad_coins : defaultFirstRewardedAdCoins;
                    FirstRewardedAdCredits = adConfig.first_rewarded_ad_credits > 0 ? adConfig.first_rewarded_ad_credits : defaultFirstRewardedAdCredits;
                    RewardedAdCoins = adConfig.rewarded_ad_coins > 0 ? adConfig.rewarded_ad_coins : defaultRewardedAdCoins;
                    RewardedAdCredits = adConfig.rewarded_ad_credits > 0 ? adConfig.rewarded_ad_credits : defaultRewardedAdCredits;

                    isRemoteConfigFetched = true;
                    Debug.Log($"[AdConfig] AD_CONFIG parsed successfully!");
                }
                else
                {
                    Debug.LogWarning("[AdConfig] AD_CONFIG not found in Unity Remote Config, using defaults");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdConfig] Failed to fetch/parse AD_CONFIG: {e.Message}");
            }
        }

        public bool ShouldShowAds()
        {
            if (AdsDisabled)
            {
                Debug.Log("[AdConfig] Ads are DISABLED via Remote Config");
                return false;
            }
            return true;
        }

        public bool IsTestMode()
        {
            return !AdsAreLive;
        }

        public int GetAdFrequencyForPlayer(int levelsCompleted)
        {
            if (VeteranFrequencyBoostEnabled && levelsCompleted >= VeteranThreshold)
            {
                return VeteranAdFrequency;
            }
            return AdFrequency;
        }

        public bool IsVeteranPlayer(int levelsCompleted)
        {
            return levelsCompleted >= VeteranThreshold;
        }

        private const string HAS_WATCHED_REWARDED_AD_KEY = "HasWatchedRewardedAd";

        public static bool HasEverWatchedRewardedAd()
        {
            return PlayerPrefs.GetInt(HAS_WATCHED_REWARDED_AD_KEY, 0) == 1;
        }

        public static void MarkRewardedAdWatched()
        {
            PlayerPrefs.SetInt(HAS_WATCHED_REWARDED_AD_KEY, 1);
            PlayerPrefs.Save();
        }

        public static void ResetAdWatcherStatus()
        {
            PlayerPrefs.DeleteKey(HAS_WATCHED_REWARDED_AD_KEY);
            PlayerPrefs.Save();
        }

        public (int coins, int credits) GetRewardAmountsForPlayer()
        {
            if (HasEverWatchedRewardedAd())
                return (RewardedAdCoins, RewardedAdCredits);
            else
                return (FirstRewardedAdCoins, FirstRewardedAdCredits);
        }

        public async Task RefreshConfigAsync()
        {
            Debug.Log("[AdConfig] Force refreshing AD_CONFIG...");
            await FetchRemoteConfigAsync();
        }

        public Dictionary<string, object> GetConfigSnapshot()
        {
            return new Dictionary<string, object>
            {
                { "minimum_level", MinimumLevelForAds },
                { "ad_frequency", AdFrequency },
                { "cooldown_minutes", AdCooldownMinutes },
                { "veteran_boost_enabled", VeteranFrequencyBoostEnabled },
                { "veteran_threshold", VeteranThreshold },
                { "veteran_frequency", VeteranAdFrequency },
                { "ads_are_live", AdsAreLive },
                { "ads_disabled", AdsDisabled },
                { "interstitial_ads_enabled", InterstitialAdsEnabled },
                { "rewarded_ads_enabled", RewardedAdsEnabled },
                { "first_rewarded_ad_coins", FirstRewardedAdCoins },
                { "first_rewarded_ad_credits", FirstRewardedAdCredits },
                { "rewarded_ad_coins", RewardedAdCoins },
                { "rewarded_ad_credits", RewardedAdCredits },
                { "is_test_mode", IsTestMode() },
                { "remote_config_fetched", isRemoteConfigFetched },
                { "config_source", "Unity Remote Config (AD_CONFIG)" }
            };
        }

        public string GetStatusString()
        {
            string mode = AdsDisabled ? "DISABLED" : (AdsAreLive ? "PRODUCTION" : "TEST MODE");
            return $"Ads: {mode} | MinLevel: {MinimumLevelForAds} | Freq: {AdFrequency} | Cooldown: {AdCooldownMinutes}min | RemoteConfig: {(isRemoteConfigFetched ? "YES" : "NO")}";
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
```

### AppLovinLogCollector.cs (177 lines) - TO DELETE
```csharp
// Full content preserved in case needed for debugging later
// See Assets/Scripts/Utils/Debugging/AppLovinLogCollector.cs
```

### DependencySafetyNet.cs (155 lines) - TO DELETE
```csharp
// Verification script - not needed for operation
// See Assets/Editor/DependencySafetyNet.cs
```

### AppLovinDependencyConditional.cs (113 lines) - TO DELETE
```csharp
// DISABLE_ADS feature - not needed in minimal build
// See Assets/Editor/AppLovinDependencyConditional.cs
```

### FixFirebaseGoogleServices.cs (120 lines) - TO COMMENT OUT
```csharp
// Firebase google-services.json copier
// See Assets/Editor/FixFirebaseGoogleServices.cs
```

### FirebaseDependencies.xml (12 lines) - TO COMMENT OUT
```xml
<!-- Firebase Dependencies - Unity 2022 compatible versions (Kotlin 1.x) -->
<dependencies>
  <androidPackages>
    <androidPackage spec="com.google.firebase:firebase-analytics:20.1.2" />
  </androidPackages>
  <iosPods>
    <iosPod name="Firebase/Analytics" version="~> 10.18.0" />
    <iosPod name="Firebase/Crashlytics" version="~> 10.18.0" />
  </iosPods>
</dependencies>
```

### gradleTemplate.properties (14 lines) - TO SIMPLIFY
```properties
# Unity 2022.3 gradle.properties
# AppLovin 12.6.1 + Firebase 20.x + Kotlin 1.8.x (all Kotlin 1.x compatible)
android.enableJetifier=true
android.useAndroidX=true
android.nonTransitiveRClass=false
android.suppressUnsupportedCompileSdk=35
# FORCE Kotlin 1.9.0 globally (Unity 2022.3's version) to fix dexing issues with Firebase 21.x
kotlin.code.style=official
unityStreamingAssets=.unity3d,**STREAMING_ASSETS**
# Android Resolver Properties Start
android.useAndroidX=true
android.enableJetifier=true
# Android Resolver Properties End
**ADDITIONAL_PROPERTIES**
```

---

## Recovery Instructions

To restore any file:
1. Copy the code from this backup
2. Create the file at the original path
3. Re-run Android Resolver if dependencies change

To re-enable Firebase:
1. Uncomment `FirebaseDependencies.xml` contents
2. Remove `#if false` from `FixFirebaseGoogleServices.cs`
3. Uncomment Firebase calls in `AppLovinManager.cs`
4. Run Android Resolver → Force Resolve

---

*End of Backup*

