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
            // Initialize debug logger
            if (AdDebugLogger.Instance == null)
            {
                GameObject loggerObj = new GameObject("AdDebugLogger");
                loggerObj.AddComponent<AdDebugLogger>();
            }
            
            AdDebugLogger.Instance?.LogSection("APPLOVIN MANAGER START");

            // Ensure kill switch UI exists even if not wired in Inspector
            EnsureKillSwitchUiExists();
            
            InitializeAppLovin();
            
            // Setup kill switch button
            if (killSwitchButton != null)
            {
                killSwitchButton.onClick.AddListener(OnKillSwitchPressed);
                AdDebugLogger.Instance?.Log("Kill switch button listener added");
            }
            
            // Hide kill switch initially
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
                AdDebugLogger.Instance?.Log("Kill switch canvas hidden initially");
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
        
        /// <summary>
        /// Create a simple fallback kill switch UI if none is assigned in the Inspector.
        /// This prevents the watchdog from silently failing on device builds.
        /// </summary>
        private void EnsureKillSwitchUiExists()
        {
            if (killSwitchCanvas != null && killSwitchButton != null && killSwitchTimerText != null)
            {
                return; // already wired
            }

            AdDebugLogger.Instance?.LogWarning("Kill switch UI not assigned. Creating fallback UI (ScreenSpaceOverlay).");

            // Canvas
            GameObject canvasObj = new GameObject("KillSwitchCanvas_Fallback");
            canvasObj.layer = LayerMask.NameToLayer("UI");
            killSwitchCanvas = canvasObj.AddComponent<Canvas>();
            killSwitchCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            killSwitchCanvas.sortingOrder = 32767;
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            canvasObj.AddComponent<GraphicRaycaster>();

            // Background (transparent)
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvasObj.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.35f);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Timer text
            GameObject timerObj = new GameObject("KillSwitchTimerText");
            timerObj.transform.SetParent(canvasObj.transform, false);
            killSwitchTimerText = timerObj.AddComponent<Text>();
            killSwitchTimerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            killSwitchTimerText.fontSize = 32;
            killSwitchTimerText.alignment = TextAnchor.MiddleCenter;
            killSwitchTimerText.color = Color.white;
            RectTransform timerRect = timerObj.GetComponent<RectTransform>();
            timerRect.anchorMin = new Vector2(0.5f, 0.6f);
            timerRect.anchorMax = new Vector2(0.5f, 0.6f);
            timerRect.sizeDelta = new Vector2(700, 80);
            timerRect.anchoredPosition = Vector2.zero;

            // Button
            GameObject buttonObj = new GameObject("KillSwitchButton");
            buttonObj.transform.SetParent(canvasObj.transform, false);
            killSwitchButton = buttonObj.AddComponent<Button>();
            Image btnImage = buttonObj.AddComponent<Image>();
            btnImage.color = new Color(0.8f, 0.2f, 0.2f, 0.95f);
            RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.45f);
            btnRect.anchorMax = new Vector2(0.5f, 0.45f);
            btnRect.sizeDelta = new Vector2(500, 120);
            btnRect.anchoredPosition = Vector2.zero;

            // Button text
            GameObject btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(buttonObj.transform, false);
            Text btnText = btnTextObj.AddComponent<Text>();
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.fontSize = 30;
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.color = Color.white;
            btnText.text = "FORCE CLOSE AD";
            RectTransform btnTextRect = btnTextObj.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.offsetMin = Vector2.zero;
            btnTextRect.offsetMax = Vector2.zero;

            // Wire up button
            killSwitchButton.onClick.AddListener(OnKillSwitchPressed);

            // Start hidden
            killSwitchCanvas.gameObject.SetActive(false);
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
            AdDebugLogger.Instance?.LogWarning("EDITOR MODE - Simulated initialization");
            Invoke(nameof(SimulateInitSuccess), 1f);
#elif UNITY_ANDROID || UNITY_IOS
            if (string.IsNullOrEmpty(sdkKey) || sdkKey == "YOUR_SDK_KEY")
            {
                Debug.LogError("[AppLovin] SDK Key not set! Please set it in the Inspector.");
                AdDebugLogger.Instance?.LogError("SDK Key not set!");
                return;
            }

            AdDebugLogger.Instance?.LogSection("SDK INITIALIZATION");
            AdDebugLogger.Instance?.Log($"SDK Key: {sdkKey}");
            AdDebugLogger.Instance?.Log($"Interstitial Unit: {interstitialAdUnitId}");
            AdDebugLogger.Instance?.Log($"Rewarded Unit: {rewardedAdUnitId}");
            
            // Check test mode from AdConfigManager
            bool isTestMode = AdConfigManager.Instance?.IsTestMode() ?? true;
            AdDebugLogger.Instance?.Log($"AdConfigManager Found: {(AdConfigManager.Instance != null)}");
            AdDebugLogger.Instance?.Log($"Test Mode Setting: {isTestMode}");
            AdDebugLogger.Instance?.Log($"AdsAreLive: {(AdConfigManager.Instance?.AdsAreLive ?? false)}");

            LogDebug($"Initializing AppLovin MAX SDK...");

            // Register SDK initialization callback BEFORE initialization (matches demo pattern)
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;

            // Set test mode if needed
            if (isTestMode)
            {
                AdDebugLogger.Instance?.LogWarning("ENABLING TEST MODE - Should show test ads");
                MaxSdk.SetTestModeEnabled(true);
            }
            else
            {
                AdDebugLogger.Instance?.LogWarning("PRODUCTION MODE - Will show real ads");
            }

            MaxSdk.SetSdkKey(sdkKey);
            MaxSdk.InitializeSdk();
            
            AdDebugLogger.Instance?.Log("InitializeSdk() called");
#else
            Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
            AdDebugLogger.Instance?.LogWarning("Wrong platform - SDK only works on Android/iOS");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            LogDebug("MAX SDK Initialized");
            
            AdDebugLogger.Instance?.LogSection("SDK INITIALIZED SUCCESSFULLY");
            AdDebugLogger.Instance?.Log($"Country Code: {config.CountryCode}");
            AdDebugLogger.Instance?.Log($"Test Mode Enabled: {config.IsTestModeEnabled}");
            AdDebugLogger.Instance?.Log($"Consent Flow: {config.ConsentFlowUserGeography}");
            
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
            
            AdDebugLogger.Instance?.Log($"[Interstitial] AD LOADED - Network: {adInfo.NetworkName}, Creative: {adInfo.CreativeIdentifier}");
            
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
            
            AdDebugLogger.Instance?.LogSection("INTERSTITIAL AD DISPLAYED");
            AdDebugLogger.Instance?.Log($"Network: {adInfo.NetworkName}");
            AdDebugLogger.Instance?.Log($"Creative ID: {adInfo.CreativeIdentifier}");
            AdDebugLogger.Instance?.Log($"Placement: {adInfo.Placement}");
            AdDebugLogger.Instance?.Log($"Revenue: ${adInfo.Revenue}");
            AdDebugLogger.Instance?.Log($"Ad Unit: {adUnitId}");
            AdDebugLogger.Instance?.Log($"Watchdog started at {Time.realtimeSinceStartup}");
            
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
            
            AdDebugLogger.Instance?.LogSection("REWARDED AD DISPLAYED");
            AdDebugLogger.Instance?.Log($"Network: {adInfo.NetworkName}");
            AdDebugLogger.Instance?.Log($"Creative ID: {adInfo.CreativeIdentifier}");
            AdDebugLogger.Instance?.Log($"Placement: {adInfo.Placement}");
            AdDebugLogger.Instance?.Log($"Revenue: ${adInfo.Revenue}");
            AdDebugLogger.Instance?.Log($"Ad Unit: {adUnitId}");
            AdDebugLogger.Instance?.Log($"Watchdog started at {Time.realtimeSinceStartup}");
            
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
