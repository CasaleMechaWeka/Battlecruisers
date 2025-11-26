using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX SDK for interstitial and rewarded ads
    /// Includes Editor simulation for testing without real ads
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

        [Tooltip("Banner Ad Unit ID from AppLovin dashboard (optional)")]
        [SerializeField] private string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";

        [Tooltip("MREC Ad Unit ID from AppLovin dashboard (optional)")]
        [SerializeField] private string mrecAdUnitId = "YOUR_MREC_AD_UNIT_ID";

        [Header("Debug Settings")]
        [SerializeField] private bool enableDebugLogs = true;
        
        private bool isInitialized = false;

        // Retry logic for failed ad loads
        private int interstitialRetryCount = 0;
        private int rewardedRetryCount = 0;
        private const int MAX_RETRY_ATTEMPTS = 6; // 2^6 = 64 seconds max delay
        private const float BASE_RETRY_DELAY = 2f; // Start with 2 seconds

        // Public events for ad lifecycle
        public event Action OnInterstitialAdReady;
        public event Action OnInterstitialAdClosed;
        public event Action OnInterstitialAdShowFailed;
        
        public event Action OnRewardedAdReady;
        public event Action OnRewardedAdRewarded; // User earned the reward
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
            if (string.IsNullOrEmpty(sdkKey) || sdkKey == "YOUR_SDK_KEY")
            {
                Debug.LogError("[AppLovin] ⚠️ SDK Key not set! Please set it in the Inspector.");
                return;
            }

            try
            {
                LogDebug($"Initializing AppLovin MAX SDK with SDK Key: {sdkKey}");

                #if UNITY_EDITOR
                // Editor simulation
                Invoke(nameof(SimulateInitSuccess), 1f);

                #elif UNITY_ANDROID || UNITY_IOS
                // Register SDK initialization callback BEFORE initialization
                MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;

                // Register callbacks BEFORE initialization
                RegisterInterstitialCallbacks();
                RegisterRewardedCallbacks();
                RegisterRevenueTrackingCallbacks();

                // Initialize AppLovin MAX SDK
                MaxSdk.SetSdkKey(sdkKey);
                MaxSdk.InitializeSdk();

                // Set user ID if needed
                // MaxSdk.SetUserId("USER_ID");

                LogDebug("✅ AppLovin MAX SDK initialization started");

                #else
                Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
                #endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AppLovin] Initialization failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            isInitialized = true;
            LogDebug("✅ AppLovin MAX SDK fully initialized");

            // Load first ads AFTER SDK is fully initialized
            LoadInterstitial();
            LoadRewardedAd();
        }

        #region Interstitial Ad Implementation

        public void LoadInterstitial()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[AppLovin] Cannot load interstitial - SDK not initialized");
                return;
            }

            try
            {
                LogDebug("Loading interstitial ad...");
                
                #if UNITY_EDITOR
                Invoke(nameof(SimulateInterstitialReady), 1f);
                #elif UNITY_ANDROID || UNITY_IOS
                MaxSdk.LoadInterstitial(interstitialAdUnitId);
                #endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AppLovin] Failed to load interstitial: {ex.Message}");
            }
        }

        public bool IsInterstitialReady()
        {
            if (!isInitialized) return false;
            
            #if UNITY_EDITOR
            return true; // Always ready in editor
            #elif UNITY_ANDROID || UNITY_IOS
            return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
            #else
            return false;
            #endif
        }

        public void ShowInterstitial()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[AppLovin] Cannot show interstitial - SDK not initialized");
                OnInterstitialAdShowFailed?.Invoke();
                return;
            }

            if (!IsInterstitialReady())
            {
                Debug.LogWarning("[AppLovin] Interstitial ad not ready");
                OnInterstitialAdShowFailed?.Invoke();
                LoadInterstitial();
                return;
            }

            try
            {
                LogDebug("Showing interstitial ad");
                
                #if UNITY_EDITOR
                SimulateInterstitialCompleted();
                #elif UNITY_ANDROID || UNITY_IOS
                MaxSdk.ShowInterstitial(interstitialAdUnitId);
                #endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AppLovin] Failed to show interstitial: {ex.Message}");
                OnInterstitialAdShowFailed?.Invoke();
            }
        }

        #endregion

        #region Rewarded Ad Implementation

        public void LoadRewardedAd()
        {
            // AppLovin MAX auto-loads rewarded ads, but we can explicitly load too
            LogDebug("Loading rewarded ad...");
            
            #if UNITY_EDITOR
            // Already simulated in Init
            #elif UNITY_ANDROID || UNITY_IOS
            MaxSdk.LoadRewardedAd(rewardedAdUnitId);
            #endif
        }

        public bool IsRewardedAdReady()
        {
            if (!isInitialized) return false;
            
            #if UNITY_EDITOR
            return true; // Always ready in editor
            #elif UNITY_ANDROID || UNITY_IOS
            return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
            #else
            return false;
            #endif
        }

        public void ShowRewardedAd()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[AppLovin] Cannot show rewarded ad - SDK not initialized");
                OnRewardedAdShowFailed?.Invoke();
                return;
            }

            if (!IsRewardedAdReady())
            {
                Debug.LogWarning("[AppLovin] Rewarded ad not ready");
                OnRewardedAdShowFailed?.Invoke();
                return;
            }

            try
            {
                LogDebug("Showing rewarded ad");
                
                #if UNITY_EDITOR
                SimulateRewardedAdCompleted();
                #elif UNITY_ANDROID || UNITY_IOS
                MaxSdk.ShowRewardedAd(rewardedAdUnitId);
                #endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AppLovin] Failed to show rewarded ad: {ex.Message}");
                OnRewardedAdShowFailed?.Invoke();
            }
        }

        #endregion

        #region AppLovin MAX Callbacks (Android/iOS Only)

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        private void RegisterInterstitialCallbacks()
        {
            // Interstitial Ad Loaded
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Interstitial ad ready: {adUnitId}");
                interstitialRetryCount = 0; // Reset retry count on success
                OnInterstitialAdReady?.Invoke();
            };

            // Interstitial Ad Load Failed
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (adUnitId, errorInfo) =>
            {
                Debug.LogWarning($"[AppLovin] Interstitial load failed: {errorInfo.Message}");
                ScheduleRetryWithBackoff(LoadInterstitial, ref interstitialRetryCount);
            };

            // Interstitial Ad Displayed
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Interstitial ad displayed: {adUnitId}");
                
                // Log to Firebase
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
                }
            };

            // Interstitial Ad Hidden (Closed)
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Interstitial ad closed: {adUnitId}");
                
                // Log to Firebase
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
                }
                
                OnInterstitialAdClosed?.Invoke();
                LoadInterstitial(); // Load next ad
            };

            // Interstitial Ad Display Failed
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (adUnitId, errorInfo, adInfo) =>
            {
                Debug.LogError($"[AppLovin] Interstitial show failed: {errorInfo.Message}");
                OnInterstitialAdShowFailed?.Invoke();
                LoadInterstitial(); // Try loading again
            };
        }

        private void RegisterRewardedCallbacks()
        {
            // Rewarded Ad Loaded
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Rewarded ad ready: {adUnitId}");
                rewardedRetryCount = 0; // Reset retry count on success
                OnRewardedAdReady?.Invoke();
            };

            // Rewarded Ad Load Failed
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (adUnitId, errorInfo) =>
            {
                Debug.LogWarning($"[AppLovin] Rewarded ad load failed: {errorInfo.Message}");
                ScheduleRetryWithBackoff(LoadRewardedAd, ref rewardedRetryCount);
            };

            // Rewarded Ad Displayed
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Rewarded ad displayed: {adUnitId}");
            };

            // Rewarded Ad Received Reward
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (adUnitId, reward, adInfo) =>
            {
                LogDebug($"Rewarded ad granted reward: {reward.Label} x{reward.Amount}");
                OnRewardedAdRewarded?.Invoke();
            };

            // Rewarded Ad Hidden (Closed)
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (adUnitId, adInfo) =>
            {
                LogDebug($"Rewarded ad closed: {adUnitId}");
                OnRewardedAdClosed?.Invoke();
                LoadRewardedAd(); // Load next ad
            };

            // Rewarded Ad Display Failed
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (adUnitId, errorInfo, adInfo) =>
            {
                Debug.LogError($"[AppLovin] Rewarded ad show failed: {errorInfo.Message}");
                OnRewardedAdShowFailed?.Invoke();
                LoadRewardedAd(); // Try loading again
            };
        }

        private void RegisterRevenueTrackingCallbacks()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            try
            {
                // Track revenue for all ad formats
                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                // Add banner/MREC revenue tracking if you add those formats:
                // MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                // MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaid;

                LogDebug("[AppLovin] Revenue tracking callbacks registered");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin] Failed to register revenue tracking callbacks: {e.Message}");
            }
#endif
        }

        private void OnAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Extract revenue data
            double revenue = adInfo.Revenue;
            string networkName = adInfo.NetworkName;
            string adUnitIdentifier = adInfo.AdUnitIdentifier;
            string placement = adInfo.Placement ?? "default";

            LogDebug($"[Revenue] Ad revenue: ${revenue:F4} from {networkName} for {adUnitIdentifier} (placement: {placement})");

            // Log to Firebase Analytics
            if (FirebaseAnalyticsManager.Instance != null)
            {
                // Create revenue event with parameters
                var revenueParams = new Dictionary<string, object>
                {
                    { "ad_platform", "applovin" },
                    { "ad_format", GetAdFormatFromUnitId(adUnitId) },
                    { "ad_unit_id", adUnitId },
                    { "network_name", networkName },
                    { "placement", placement },
                    { "revenue", revenue },
                    { "currency", "USD" }
                };

                FirebaseAnalyticsManager.Instance.LogEvent("ad_revenue", revenueParams);
            }
        }

        private string GetAdFormatFromUnitId(string adUnitId)
        {
            // You can customize this logic based on your ad unit naming convention
            if (adUnitId.Contains("interstitial") || adUnitId.Contains("inter"))
                return "interstitial";
            else if (adUnitId.Contains("rewarded") || adUnitId.Contains("reward"))
                return "rewarded";
            else if (adUnitId.Contains("banner"))
                return "banner";
            else if (adUnitId.Contains("mrec"))
                return "mrec";
            else
                return "unknown";
        }
#endif

        #endregion

        #region Retry Logic

        private void ScheduleRetryWithBackoff(Action loadAction, ref int retryCount)
        {
            if (retryCount >= MAX_RETRY_ATTEMPTS)
            {
                Debug.LogWarning($"[AppLovin] Max retry attempts ({MAX_RETRY_ATTEMPTS}) reached. Stopping retries.");
                retryCount = 0; // Reset for next time
                return;
            }

            retryCount++;
            float delay = Mathf.Min(BASE_RETRY_DELAY * Mathf.Pow(2, retryCount - 1), 64f);
            LogDebug($"[Retry] Scheduling retry #{retryCount} in {delay} seconds");

            Invoke(loadAction.Method.Name, delay);
        }

        private void ResetRetryCounts()
        {
            interstitialRetryCount = 0;
            rewardedRetryCount = 0;
        }

        #endregion

        #region Editor Simulation

#if UNITY_EDITOR
        private void SimulateInitSuccess()
        {
            isInitialized = true;
            LogDebug("✅ [EDITOR] Simulated initialization successful");
            LoadInterstitial();
        }

        private void SimulateInterstitialReady()
        {
            LogDebug("[EDITOR] Simulating interstitial ad ready");
            OnInterstitialAdReady?.Invoke();
        }

        private void SimulateInterstitialCompleted()
        {
            LogDebug("[EDITOR] Simulating interstitial ad shown");
            
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
            }
            
            Invoke(nameof(SimulateInterstitialClosed), 2f);
        }

        private void SimulateInterstitialClosed()
        {
            LogDebug("[EDITOR] Simulating interstitial ad closed");
            
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
            }
            
            OnInterstitialAdClosed?.Invoke();
            LoadInterstitial();
        }

        private void SimulateRewardedAdCompleted()
        {
            LogDebug("[EDITOR] Simulating rewarded ad shown");
            Invoke(nameof(SimulateRewardedGranted), 2f);
        }

        private void SimulateRewardedGranted()
        {
            LogDebug("[EDITOR] Simulating rewarded ad reward granted");
            OnRewardedAdRewarded?.Invoke();
            Invoke(nameof(SimulateRewardedClosed), 0.5f);
        }

        private void SimulateRewardedClosed()
        {
            LogDebug("[EDITOR] Simulating rewarded ad closed");
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

        private void OnDestroy()
        {
            // Cleanup (AppLovin handles most cleanup automatically)
        }

        #endregion
    }
}

