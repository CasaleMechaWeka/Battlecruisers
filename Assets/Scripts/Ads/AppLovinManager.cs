using System;
using System.Collections;
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
        [SerializeField] private float adWatchdogTimeout = 30f; // Force close after 30s no matter what
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
        private Coroutine watchdogCoroutine = null;

        // Nuclear timer - guarantees closure after 30s no matter what
        private System.Threading.Timer nuclearTimer = null;

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
            
            // Set object name so Android can find it via UnitySendMessage
            gameObject.name = "AppLovinManager";
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

        private float lastWatchdogLogTime = 0f;
        
        private void Update()
        {
            // Check for Android back button when ad is showing (allow user to force close IMMEDIATELY)
#if UNITY_ANDROID && !UNITY_EDITOR
            // Log every frame to verify Update() is running
            if (isInterstitialShowing || isRewardedShowing)
            {
                if (Time.frameCount % 60 == 0) // Every 60 frames (~1 second)
                {
                    Debug.Log($"[AppLovin] Update() running - ad showing for {Time.realtimeSinceStartup - adShowStartTime:F1}s");
                }
            }
            
            if ((isInterstitialShowing || isRewardedShowing) && Input.GetKeyDown(KeyCode.Escape))
            {
                float elapsed = Time.realtimeSinceStartup - adShowStartTime;
                Debug.LogWarning($"[AppLovin] *** BACK BUTTON PRESSED *** - closing ad after {elapsed:F1}s");
                AdDebugLogger.Instance?.LogWarning($"*** BACK BUTTON DETECTED *** User pressed back after {elapsed:F1}s");
                TriggerAdKillSwitch();
                return;
            }
#endif

            // Watchdog: if an ad has been showing too long, show kill switch UI
            if ((isInterstitialShowing || isRewardedShowing) && adShowStartTime > 0)
            {
                float elapsed = Time.realtimeSinceStartup - adShowStartTime;
                float timeRemaining = adWatchdogTimeout - elapsed;

                // Debug logging every 2 seconds
                if (elapsed > 2f && Time.realtimeSinceStartup - lastWatchdogLogTime >= 2f)
                {
                    lastWatchdogLogTime = Time.realtimeSinceStartup;
                    Debug.Log($"[AppLovin] WATCHDOG: Ad showing for {elapsed:F1}s, isRewardedShowing={isRewardedShowing}, isInterstitialShowing={isInterstitialShowing}");
                    AdDebugLogger.Instance?.Log($"Watchdog tick: {elapsed:F1}s elapsed, {timeRemaining:F1}s remaining");
                }

                // Update kill switch UI (should already be visible from ShowKillSwitchUIImmediate)
                if (killSwitchCanvas != null)
                {
                    // Ensure canvas is always active when ad is showing
                    if (!killSwitchCanvas.gameObject.activeSelf)
                    {
                        killSwitchCanvas.gameObject.SetActive(true);
                        Debug.LogWarning($"[AppLovin] WATCHDOG: Reactivating kill switch UI at {elapsed:F1}s");
                    }
                    
                    // Update text based on elapsed time
                    if (killSwitchTimerText != null)
                    {
                        if (elapsed >= adWatchdogTimeout)
                        {
                            killSwitchTimerText.text = "AD STUCK - TAP TO CLOSE";
                        }
                        else if (elapsed >= 5f)
                        {
                            killSwitchTimerText.text = $"Back button or wait {Mathf.CeilToInt(timeRemaining)}s";
                        }
                        else
                        {
                            killSwitchTimerText.text = "Ad loading... (Back button to exit)";
                        }
                    }
                }

                // Auto-trigger watchdog as backup
                if (elapsed > adWatchdogTimeout + 3f) // Give user 3s to manually use kill switch
                {
                    Debug.LogWarning($"[AppLovin] WATCHDOG: Ad has been showing for {elapsed:F0}s - AUTO-TRIGGERING CLOSE");
                    AdDebugLogger.Instance?.LogError($"AUTO-CLOSING stuck ad after {elapsed:F0}s");
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

            // Canvas - use absolute maximum sorting order
            GameObject canvasObj = new GameObject("KillSwitchCanvas_Fallback");
            DontDestroyOnLoad(canvasObj); // Ensure it persists across scene changes
            canvasObj.layer = LayerMask.NameToLayer("UI");
            killSwitchCanvas = canvasObj.AddComponent<Canvas>();
            killSwitchCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            killSwitchCanvas.sortingOrder = 32767; // Maximum value
            killSwitchCanvas.overrideSorting = true;
            killSwitchCanvas.planeDistance = 100f; // Put it in front of everything
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            canvasObj.AddComponent<GraphicRaycaster>();
            
            Debug.Log($"[AppLovin] Kill switch canvas created: {canvasObj.name}, sortingOrder={killSwitchCanvas.sortingOrder}");
            AdDebugLogger.Instance?.Log($"Canvas created: {canvasObj.name}, layer={canvasObj.layer}, sortingOrder={killSwitchCanvas.sortingOrder}");

            // Background (semi-transparent dark overlay)
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvasObj.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.7f); // More visible overlay
            bgImage.raycastTarget = true; // Block clicks to ads below
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

            // Button (larger and more prominent)
            GameObject buttonObj = new GameObject("KillSwitchButton");
            buttonObj.transform.SetParent(canvasObj.transform, false);
            killSwitchButton = buttonObj.AddComponent<Button>();
            Image btnImage = buttonObj.AddComponent<Image>();
            btnImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Bright red, fully opaque
            RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(700, 150); // Larger button
            btnRect.anchoredPosition = Vector2.zero;

            // Button text
            GameObject btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(buttonObj.transform, false);
            Text btnText = btnTextObj.AddComponent<Text>();
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.fontSize = 36; // Larger text
            btnText.fontStyle = FontStyle.Bold;
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
            
            Debug.Log($"[AppLovin] Kill switch UI fully created - Canvas: {killSwitchCanvas != null}, Button: {killSwitchButton != null}, Text: {killSwitchTimerText != null}");
            AdDebugLogger.Instance?.Log($"Kill switch UI components created: Canvas={killSwitchCanvas != null}, Button={killSwitchButton != null}, Text={killSwitchTimerText != null}");
        }

        private void OnKillSwitchPressed()
        {
            Debug.Log("[AppLovin] Kill switch pressed by user - force closing ad");
            TriggerAdKillSwitch();
        }
        
        private void HideKillSwitchUI()
        {
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(false);
            }

            // Stop coroutine watchdog
            if (watchdogCoroutine != null)
            {
                StopCoroutine(watchdogCoroutine);
                watchdogCoroutine = null;
            }

            // Stop nuclear timer
            StopNuclearTimer();
        }
        
        /// <summary>
        /// Show kill switch UI immediately when ad starts (before timeout)
        /// </summary>
        private void ShowKillSwitchUIImmediate()
        {
            Debug.Log($"[AppLovin] ShowKillSwitchUIImmediate called - killSwitchCanvas is {(killSwitchCanvas != null ? "not null" : "NULL")}");
            
            if (killSwitchCanvas != null)
            {
                killSwitchCanvas.gameObject.SetActive(true);
                
                Debug.Log($"[AppLovin] Canvas activated - isActiveAndEnabled={killSwitchCanvas.isActiveAndEnabled}, gameObject.activeSelf={killSwitchCanvas.gameObject.activeSelf}");
                AdDebugLogger.Instance?.Log($"Canvas state: active={killSwitchCanvas.gameObject.activeSelf}, enabled={killSwitchCanvas.enabled}, sortingOrder={killSwitchCanvas.sortingOrder}");
                
                if (killSwitchTimerText != null)
                {
                    killSwitchTimerText.text = "Ad loading... (Back button to exit)";
                    Debug.Log($"[AppLovin] Timer text set: '{killSwitchTimerText.text}'");
                }
                else
                {
                    Debug.LogWarning("[AppLovin] killSwitchTimerText is NULL!");
                }
                
                if (killSwitchButton != null)
                {
                    Debug.Log($"[AppLovin] Button state: active={killSwitchButton.gameObject.activeSelf}, interactable={killSwitchButton.interactable}");
                }
                else
                {
                    Debug.LogWarning("[AppLovin] killSwitchButton is NULL!");
                }
                
                Debug.Log("[AppLovin] Kill switch UI shown immediately - CHECK SCREEN NOW");
                AdDebugLogger.Instance?.LogWarning("KILL SWITCH UI SHOULD BE VISIBLE NOW");
            }
            else
            {
                Debug.LogError("[AppLovin] Cannot show kill switch UI - killSwitchCanvas is NULL!");
                AdDebugLogger.Instance?.LogError("CRITICAL: killSwitchCanvas is NULL - UI will not show!");
            }
        }
        
        /// <summary>
        /// Start a coroutine-based watchdog that runs independently of Update()
        /// This ensures the kill switch activates even if Update() is blocked
        /// </summary>
        private void StartWatchdogCoroutine()
        {
            // Stop any existing watchdog
            if (watchdogCoroutine != null)
            {
                StopCoroutine(watchdogCoroutine);
            }
            
            watchdogCoroutine = StartCoroutine(WatchdogCoroutine());
        }
        
        /// <summary>
        /// Coroutine-based watchdog that updates UI and triggers auto-close
        /// </summary>
        private IEnumerator WatchdogCoroutine()
        {
            Debug.Log("[AppLovin] Watchdog coroutine started");
            AdDebugLogger.Instance?.Log("Coroutine watchdog started");
            
            float elapsed = 0f;
            
            while (isInterstitialShowing || isRewardedShowing)
            {
                elapsed = Time.realtimeSinceStartup - adShowStartTime;
                float timeRemaining = adWatchdogTimeout - elapsed;
                
                // Update UI every frame
                if (killSwitchCanvas != null && killSwitchCanvas.gameObject.activeSelf)
                {
                    if (killSwitchTimerText != null)
                    {
                        if (elapsed >= adWatchdogTimeout)
                        {
                            killSwitchTimerText.text = "AD STUCK - TAP TO CLOSE";
                        }
                        else if (elapsed >= 5f)
                        {
                            killSwitchTimerText.text = $"Back button or wait {Mathf.CeilToInt(timeRemaining)}s";
                        }
                        else
                        {
                            killSwitchTimerText.text = "Ad loading... (Back button to exit)";
                        }
                    }
                }
                
                // Log progress every 2 seconds
                if ((int)elapsed % 2 == 0 && Time.frameCount % 60 == 0)
                {
                    Debug.Log($"[AppLovin] Coroutine watchdog: {elapsed:F1}s elapsed");
                    AdDebugLogger.Instance?.Log($"Coroutine tick: {elapsed:F1}s / {adWatchdogTimeout}s");
                }
                
                // Auto-trigger after timeout + grace period
                if (elapsed > adWatchdogTimeout + 3f)
                {
                    Debug.LogWarning($"[AppLovin] Coroutine watchdog AUTO-CLOSE at {elapsed:F1}s");
                    AdDebugLogger.Instance?.LogError($"Coroutine watchdog triggered auto-close");
                    TriggerAdKillSwitch();
                    yield break;
                }
                
                yield return null; // Wait one frame
            }
            
            Debug.Log("[AppLovin] Watchdog coroutine finished normally");
            AdDebugLogger.Instance?.Log("Coroutine watchdog finished");
        }
        
        private void TriggerAdKillSwitch()
        {
            Debug.LogWarning($"[AppLovin] FORCE-KILLING AD: isInterstitial={isInterstitialShowing}, isRewarded={isRewardedShowing}");
            AdDebugLogger.Instance?.LogWarning($"FORCE-KILLING AD - timeout reached");

            HideKillSwitchUI();

            // Force close the ad by invoking callbacks (this is what AppLovin does normally)
            if (isInterstitialShowing)
            {
                isInterstitialShowing = false;
                adShowStartTime = 0f;
                Debug.Log("[AppLovin] Force-closing interstitial ad");
                OnInterstitialAdClosed?.Invoke();
                LoadInterstitial();
            }

            if (isRewardedShowing)
            {
                isRewardedShowing = false;
                adShowStartTime = 0f;
                Debug.Log("[AppLovin] Force-closing rewarded ad (no reward granted)");
                // Note: We do NOT fire OnRewardedAdRewarded here - user didn't complete the ad
                OnRewardedAdShowFailed?.Invoke();
                LoadRewardedAd();
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            // Try multiple approaches to force-close stuck ad
            ForceCloseStuckAd();
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

            // Note: Test mode controlled via AppLovin dashboard (add test device) or mediation debugger
            // SetTestModeEnabled() not available in SDK v7.0.0
            if (isTestMode)
            {
                AdDebugLogger.Instance?.LogWarning("TEST MODE ENABLED in config - Add device as test device in AppLovin dashboard");
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
            
            // Watchdog already started in ShowInterstitial(), just log confirmation
            float elapsed = Time.realtimeSinceStartup - adShowStartTime;
            AdDebugLogger.Instance?.Log($"OnAdDisplayedEvent callback fired after {elapsed:F3}s");
            
            // Ensure flags are set (in case ShowInterstitial wasn't called)
            if (!isInterstitialShowing)
            {
                isInterstitialShowing = true;
                adShowStartTime = Time.realtimeSinceStartup;
                AdDebugLogger.Instance?.LogWarning("Watchdog wasn't started in ShowInterstitial - starting now");
            }
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial failed to display: {errorInfo.Code}");
            isInterstitialShowing = false;
            adShowStartTime = 0f;
            HideKillSwitchUI();
            
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Interstitial dismissed");
            isInterstitialShowing = false;
            adShowStartTime = 0f;
            HideKillSwitchUI();
            
            FirebaseAnalyticsManager.Instance?.LogAdClosed("applovin", "interstitial");
            
            OnInterstitialAdClosed?.Invoke();
            LoadInterstitial(); // Pre-load next ad
        }

        private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug($"Interstitial revenue: ${adInfo.Revenue:F4} from {adInfo.NetworkName}");
            FirebaseAnalyticsManager.Instance?.LogAdImpression("applovin", "interstitial");
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
            HideKillSwitchUI();
            
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
            
            // Watchdog already started in ShowRewardedAd(), just log confirmation
            float elapsed = Time.realtimeSinceStartup - adShowStartTime;
            AdDebugLogger.Instance?.Log($"OnAdDisplayedEvent callback fired after {elapsed:F3}s");
            
            // Ensure flags are set (in case ShowRewardedAd wasn't called)
            if (!isRewardedShowing)
            {
                isRewardedShowing = true;
                adShowStartTime = Time.realtimeSinceStartup;
                AdDebugLogger.Instance?.LogWarning("Watchdog wasn't started in ShowRewardedAd - starting now");
            }
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogDebug("Rewarded ad dismissed");
            isRewardedShowing = false;
            adShowStartTime = 0f;
            HideKillSwitchUI();
            
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

                // Start watchdog IMMEDIATELY (don't wait for OnAdDisplayedEvent callback)
                isInterstitialShowing = true;
                adShowStartTime = Time.realtimeSinceStartup;
                AdDebugLogger.Instance?.Log($"[Interstitial] Watchdog started immediately at {adShowStartTime}");

                // Start coroutine-based watchdog as backup (in case Update() gets blocked)
                StartWatchdogCoroutine();

                // Start NUCLEAR TIMER - guarantees closure in 30s no matter what
                StartNuclearTimer();

                // Show kill switch UI immediately (make it visible from the start)
                ShowKillSwitchUIImmediate();

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

                // Start watchdog IMMEDIATELY (don't wait for OnAdDisplayedEvent callback)
                isRewardedShowing = true;
                adShowStartTime = Time.realtimeSinceStartup;
                AdDebugLogger.Instance?.Log($"[Rewarded] Watchdog started immediately at {adShowStartTime}");

                // Start coroutine-based watchdog as backup (in case Update() gets blocked)
                StartWatchdogCoroutine();

                // Start NUCLEAR TIMER - guarantees closure in 30s no matter what
                StartNuclearTimer();

                // Show kill switch UI immediately (make it visible from the start)
                ShowKillSwitchUIImmediate();

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

        #region Android Back Button Handler

        /// <summary>
        /// Called by CustomUnityPlayerActivity when Android back button is pressed.
        /// This is a UnitySendMessage receiver - must be public and take string parameter.
        /// </summary>
        public void OnAndroidBackButton(string unused)
        {
            Debug.LogWarning("[AppLovin] *** ANDROID BACK BUTTON (via Activity) ***");
            AdDebugLogger.Instance?.LogWarning("*** Back button received from Android Activity ***");

            // Only trigger if an ad is showing
            if (isInterstitialShowing || isRewardedShowing)
            {
                float elapsed = Time.realtimeSinceStartup - adShowStartTime;
                Debug.LogWarning($"[AppLovin] Ad is showing - closing after {elapsed:F1}s");
                AdDebugLogger.Instance?.LogWarning($"Closing ad via back button - elapsed: {elapsed:F1}s");
                TriggerAdKillSwitch();
            }
            else
            {
                Debug.Log("[AppLovin] No ad showing - ignoring back button");

                // If no ad showing, let the app handle back button normally
                // You might want to show a quit confirmation here
#if UNITY_ANDROID && !UNITY_EDITOR
                // Check if we're on the main menu or should quit
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LandingScene")
                {
                    Debug.Log("[AppLovin] On main scene - quitting app");
                    Application.Quit();
                }
#endif
            }
        }

        /// <summary>
        /// Start a nuclear timer that guarantees ad closure after exactly 30 seconds
        /// This uses System.Threading.Timer which runs on its own thread and cannot be blocked
        /// </summary>
        private void StartNuclearTimer()
        {
            StopNuclearTimer(); // Clean up any existing timer

            Debug.Log("[AppLovin] Starting NUCLEAR TIMER - guaranteed 30s closure");
            AdDebugLogger.Instance?.LogWarning("NUCLEAR TIMER STARTED - Will force-close ad in exactly 30 seconds");

            nuclearTimer = new System.Threading.Timer(
                NuclearTimerCallback,     // Callback method
                null,                     // State object (unused)
                30000,                    // Due time (30 seconds)
                System.Threading.Timeout.Infinite); // Period (one-shot)
        }

        /// <summary>
        /// Stop the nuclear timer
        /// </summary>
        private void StopNuclearTimer()
        {
            if (nuclearTimer != null)
            {
                nuclearTimer.Dispose();
                nuclearTimer = null;
                Debug.Log("[AppLovin] Nuclear timer stopped");
            }
        }

        /// <summary>
        /// Callback fired by nuclear timer after exactly 30 seconds
        /// This cannot be blocked or prevented - it will execute no matter what
        /// </summary>
        private void NuclearTimerCallback(object state)
        {
            // This runs on a background thread, so we need to dispatch to main thread
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    ExecuteNuclearClose();
                }));
            }
            catch (System.Exception e)
            {
                // Last resort - try direct closure
                Debug.LogError($"[AppLovin] Nuclear timer callback failed: {e.Message} - attempting direct closure");
                ExecuteNuclearClose();
            }
#else
            // In editor, just execute directly
            ExecuteNuclearClose();
#endif
        }

        /// <summary>
        /// Execute the nuclear close logic (called from timer callback)
        /// </summary>
        private void ExecuteNuclearClose()
        {
            Debug.LogError("[AppLovin] *** NUCLEAR TIMER FIRED *** 30 seconds elapsed - FORCE CLOSING AD");
            AdDebugLogger.Instance?.LogError("NUCLEAR TIMER: 30 seconds reached - executing emergency closure");

            if (isInterstitialShowing || isRewardedShowing)
            {
                TriggerAdKillSwitch();
            }
            else
            {
                Debug.Log("[AppLovin] Nuclear timer fired but no ad showing (already closed)");
            }
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Forcefully close stuck ads using Android system calls - NUCLEAR OPTION
        /// Simplified approach: Back spam (limited) + finishAffinity() as last resort
        /// </summary>
        private void ForceCloseStuckAd()
        {
            Debug.LogWarning("[AppLovin] *** NUCLEAR FORCE-CLOSE ACTIVATED ***");

            try
            {
                // Layer 1: Send limited back presses (10 max to avoid ANR)
                Debug.Log("[AppLovin] Layer 1: Sending 10 back button presses");
                for (int i = 0; i < 10; i++)
                {
                    TrySendAndroidBack();
                    System.Threading.Thread.Sleep(50);
                }

                // Layer 2: Try ESC key (some WebView ads respond)
                Debug.Log("[AppLovin] Layer 2: Sending ESC key events");
                for (int i = 0; i < 3; i++)
                {
                    TrySendEscapeKey();
                    System.Threading.Thread.Sleep(50);
                }

                // Layer 3: finishAffinity() - ends entire task stack (ad + Unity)
                // This will close the app but ensures user escapes stuck ad
                Debug.LogWarning("[AppLovin] Layer 3: Calling finishAffinity() - will restart app");
                TryFinishAffinity();

                Debug.LogWarning("[AppLovin] *** ALL NUCLEAR METHODS ATTEMPTED ***");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AppLovin] Nuclear force-close attempt failed: {e.Message}");
            }
        }

        /// <summary>
        /// Call finishAffinity() to end the entire task stack
        /// This is the nuclear option - it will close the app entirely
        /// but guarantees escape from stuck ads
        /// </summary>
        private void TryFinishAffinity()
        {
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    Debug.LogWarning("[AppLovin] Calling activity.finishAffinity() - APP WILL CLOSE");
                    activity.Call("finishAffinity");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AppLovin] finishAffinity failed: {e.Message}");
                // Last resort - try regular finish
                try
                {
                    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        Debug.LogWarning("[AppLovin] Trying regular finish()");
                        activity.Call("finish");
                    }
                }
                catch (System.Exception e2)
                {
                    Debug.LogError($"[AppLovin] finish() also failed: {e2.Message}");
                }
            }
        }

        /// <summary>
        /// Try to send ESC key event (some WebView-based ads respond to ESC)
        /// </summary>
        private void TrySendEscapeKey()
        {
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Create KeyEvent for ESC key (KEYCODE_ESCAPE = 111)
                    using (var keyEventClass = new AndroidJavaClass("android.view.KeyEvent"))
                    {
                        // ACTION_DOWN
                        using (var downEvent = new AndroidJavaObject("android.view.KeyEvent", 0, 111))
                        {
                            currentActivity.Call<bool>("dispatchKeyEvent", downEvent);
                        }
                        // ACTION_UP
                        using (var upEvent = new AndroidJavaObject("android.view.KeyEvent", 1, 111))
                        {
                            currentActivity.Call<bool>("dispatchKeyEvent", upEvent);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log($"[AppLovin] SendEscapeKey failed: {e.Message}");
            }
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
