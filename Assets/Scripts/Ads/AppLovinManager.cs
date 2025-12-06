using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX SDK for interstitial and rewarded ads.
    /// Simplified to match the official AppLovin demo project pattern.
    /// </summary>
    public class AppLovinManager : MonoBehaviour
    {
        public static AppLovinManager Instance { get; private set; }

        [Header("AppLovin MAX Configuration")]
        [Tooltip("Your AppLovin SDK Key from the dashboard")]
#pragma warning disable 0414 // Field assigned but not used (false positive - used in platform-specific code)
        [SerializeField] private string sdkKey = "G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0";
        
        [Tooltip("Interstitial Ad Unit ID from AppLovin dashboard")]
        [SerializeField] private string interstitialAdUnitId = "9375d1dbeb211048";
        
        [Tooltip("Rewarded Ad Unit ID from AppLovin dashboard")]
        [SerializeField] private string rewardedAdUnitId = "c96bd6d70b3804fa";
#pragma warning restore 0414

        [Header("Debug Settings")]
        [SerializeField] private bool enableDebugLogs = true;
        
        [Header("Safety Settings")]
        [Tooltip("Maximum time (seconds) to wait for an ad to close before forcing callbacks")]
        [SerializeField] private float adWatchdogTimeout = 31f;
        [Tooltip("When watchdog triggers on Android, also try sending a back press to close the ad UI")]
#pragma warning disable 0414 // Field assigned but not used (false positive - used in platform-specific code)
        [SerializeField] private bool sendAndroidBackOnWatchdog = true;
#pragma warning restore 0414
        
        [Header("Kill Switch UI (Assign Your Own)")]
        [Tooltip("Canvas that shows above ads when they're stuck")]
        public Canvas killSwitchCanvas;
        [Tooltip("Button that force-closes stuck ads")]
        public Button killSwitchButton;
        [Tooltip("Text showing countdown until kill switch appears")]
        public Text killSwitchTimerText;

        private bool isInitialized = false;
        private int interstitialRetryAttempt = 0;
        private int rewardedRetryAttempt = 0;
        
        // Watchdog tracking
        private bool isInterstitialShowing = false;
        private bool isRewardedShowing = false;
        private float adShowStartTime = 0f;

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
            
            // Setup kill switch button
            if (killSwitchButton != null)
            {
                killSwitchButton.onClick.AddListener(OnKillSwitchPressed);
            }
            
            // Hide kill switch initially
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            // Watchdog: if an ad has been showing too long, show kill switch UI
            if ((isInterstitialShowing || isRewardedShowing) && adShowStartTime > 0)
            {
                float elapsed = Time.realtimeSinceStartup - adShowStartTime;
                float timeRemaining = adWatchdogTimeout - elapsed;
                
                // Update kill switch UI
                if (killSwitchCanvas != null)
                {
                    if (elapsed >= adWatchdogTimeout)
                    {
                        // Show kill switch button
                        killSwitchCanvas.gameObject.SetActive(true);
                        if (killSwitchTimerText != null)
                        {
                            killSwitchTimerText.text = "AD STUCK - TAP TO CLOSE";
                        }
                    }
                    else if (elapsed >= adWatchdogTimeout * 0.5f) // Show countdown when halfway to timeout
                    {
                        killSwitchCanvas.gameObject.SetActive(true);
                        if (killSwitchTimerText != null)
                        {
                            killSwitchTimerText.text = $"Kill switch in {Mathf.CeilToInt(timeRemaining)}s";
                        }
                    }
                }
                
                // Auto-trigger watchdog as backup
                if (elapsed > adWatchdogTimeout + 5f) // Give user 5s to manually use kill switch
                {
                    Debug.LogWarning($"[AppLovin] WATCHDOG: Ad has been showing for {elapsed:F0}s - auto-triggering close");
                    TriggerAdKillSwitch();
                }
            }
        }
        
        private void OnKillSwitchPressed()
        {
            Debug.Log("[AppLovin] Kill switch pressed by user - force closing ad");
            TriggerAdKillSwitch();
        }
        
        private void TriggerAdKillSwitch()
        {
            // Hide kill switch UI
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
            
            // Force close the ad
            if (isInterstitialShowing)
            {
                isInterstitialShowing = false;
                adShowStartTime = 0f;
                OnInterstitialAdClosed?.Invoke();
                LoadInterstitial();
            }
            
            if (isRewardedShowing)
            {
                isRewardedShowing = false;
                adShowStartTime = 0f;
                // Note: We do NOT fire OnRewardedAdRewarded here - user didn't complete the ad
                OnRewardedAdShowFailed?.Invoke();
                LoadRewardedAd();
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            if (sendAndroidBackOnWatchdog)
            {
                TrySendAndroidBack();
            }
#endif
        }

        private void InitializeAppLovin()
        {
#if UNITY_EDITOR
            // Editor simulation
            LogDebug("Running in Editor - simulating SDK initialization");
            Invoke(nameof(SimulateInitSuccess), 1f);
#elif UNITY_ANDROID || UNITY_IOS
            if (string.IsNullOrEmpty(sdkKey) || sdkKey == "YOUR_SDK_KEY")
            {
                Debug.LogError("[AppLovin] SDK Key not set! Please set it in the Inspector.");
                return;
            }

            LogDebug($"Initializing AppLovin MAX SDK...");

            // Register SDK initialization callback BEFORE initialization (matches demo pattern)
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;

            MaxSdk.SetSdkKey(sdkKey);
            MaxSdk.InitializeSdk();
#else
            Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            LogDebug("MAX SDK Initialized");
            isInitialized = true;

            // Initialize ad types AFTER SDK is fully initialized (matches demo pattern)
            InitializeInterstitialAds();
            InitializeRewardedAds();
        }

        private void InitializeInterstitialAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private void InitializeRewardedAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

            // Load the first rewarded ad
            LoadRewardedAd();
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Interstitial loaded");
            interstitialRetryAttempt = 0;
            OnInterstitialAdReady?.Invoke();
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Retry with exponential backoff (matches demo pattern)
            interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            
            LogDebug($"Interstitial load failed: {errorInfo.Code}. Retrying in {retryDelay}s...");
            Invoke(nameof(LoadInterstitial), (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial displayed (network={adInfo.NetworkName}, creativeId={adInfo.CreativeIdentifier}, placement={adInfo.Placement})");
            isInterstitialShowing = true;
            adShowStartTime = Time.realtimeSinceStartup;
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial failed to display: {errorInfo.Code}");
            isInterstitialShowing = false;
            adShowStartTime = 0f;
            
            // Hide kill switch UI
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
            
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Interstitial dismissed");
            isInterstitialShowing = false;
            adShowStartTime = 0f;
            
            // Hide kill switch UI
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
            
            // Log to Firebase
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
            }
            
            OnInterstitialAdClosed?.Invoke();
            LoadInterstitial(); // Pre-load next ad
        }

        private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
            
            // Log to Firebase
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Rewarded ad loaded");
            rewardedRetryAttempt = 0;
            OnRewardedAdReady?.Invoke();
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Retry with exponential backoff (matches demo pattern)
            rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
            
            LogDebug($"Rewarded ad load failed: {errorInfo.Code}. Retrying in {retryDelay}s...");
            Invoke(nameof(LoadRewardedAd), (float)retryDelay);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad failed to display: {errorInfo.Code}");
            isRewardedShowing = false;
            adShowStartTime = 0f;
            
            // Hide kill switch UI
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
            
            OnRewardedAdShowFailed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad displayed (network={adInfo.NetworkName}, creativeId={adInfo.CreativeIdentifier}, placement={adInfo.Placement})");
            isRewardedShowing = true;
            adShowStartTime = Time.realtimeSinceStartup;
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Rewarded ad dismissed");
            isRewardedShowing = false;
            adShowStartTime = 0f;
            
            // Hide kill switch UI
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }
            
            OnRewardedAdClosed?.Invoke();
            LoadRewardedAd(); // Pre-load next ad
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Best-effort: ask the current Activity to execute onBackPressed, which can close some stuck ad UIs.
        /// </summary>
        private void TrySendAndroidBack()
        {
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        activity.Call("onBackPressed");
                    }));
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AppLovin] Failed to send back press on watchdog: {e.Message}");
            }
        }
#endif

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad received reward: {reward.Label} x{reward.Amount}");
            OnRewardedAdRewarded?.Invoke();
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Rewarded ad revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
        }
#endif

        #region Public Methods (work on all platforms)

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

        #region Editor Simulation Callbacks

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
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
            }
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
