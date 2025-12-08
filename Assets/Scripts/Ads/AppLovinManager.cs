using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX SDK for interstitial and rewarded ads.
    /// 
    /// CRITICAL ANDROID FIX (Samsung/Mali devices, Android 12-14):
    /// - RestoreImmersiveMode() in OnAdHidden callbacks fixes missing close buttons
    /// - Known AppLovin bug where ads break system UI flags on Samsung One UI
    /// - Also unsticks Mali GPU video decoding hangs
    /// 
    /// SAFETY FEATURES:
    /// - 30s timeout for interstitial ads
    /// - 60s timeout for rewarded ads (longer for video completion)
    /// - Nuclear timer on background thread (cannot be blocked)
    /// - Scene reload fallback if ad gets completely stuck
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
        [Tooltip("In debug builds, auto-show Mediation Debugger on SDK init")]
        [SerializeField] private bool autoShowMediationDebugger = false;
        
        [Header("Safety Settings")]
        [Tooltip("Maximum time (seconds) to wait for an INTERSTITIAL ad to close")]
        [SerializeField] private float interstitialWatchdogTimeout = 30f;
        [Tooltip("Maximum time (seconds) to wait for a REWARDED ad to close (longer for video completion)")]
        [SerializeField] private float rewardedWatchdogTimeout = 60f;
        [Tooltip("When watchdog triggers on Android, also try sending a back press to close the ad UI")]
#pragma warning disable 0414 // Field assigned but not used (false positive - used in platform-specific code)
        [SerializeField] private bool sendAndroidBackOnWatchdog = true;
#pragma warning restore 0414
        
        // Kill switch UI removed - doesn't work above native video ads
        // Using auto-timer kill only

        private bool isInitialized = false;
        private int interstitialRetryAttempt = 0;
        private int rewardedRetryAttempt = 0;
        
        // Watchdog tracking
        private bool isInterstitialShowing = false;
        private bool isRewardedShowing = false;
        private float adShowStartTime = 0f;

        // Nuclear timer - guarantees closure after 30s no matter what
        private System.Threading.Timer nuclearTimer = null;
        
        // Flag to signal main thread to handle nuclear close (avoids JNI on background thread)
        private volatile bool nuclearTimerFired = false;

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
            
            InitializeAppLovin();
        }

        private float lastWatchdogLogTime = 0f;
        
        private void Update()
        {
            // CHECK NUCLEAR TIMER FLAG - Handle on main thread to avoid JNI crash
            if (nuclearTimerFired)
            {
                nuclearTimerFired = false;
                Debug.LogError("[AppLovin] *** NUCLEAR TIMER DETECTED IN UPDATE *** Executing soft close");
                ExecuteNuclearClose();
                return;
            }

            // Watchdog: log progress for debugging
            if ((isInterstitialShowing || isRewardedShowing) && adShowStartTime > 0)
            {
                float elapsed = Time.realtimeSinceStartup - adShowStartTime;
                float timeout = isRewardedShowing ? rewardedWatchdogTimeout : interstitialWatchdogTimeout;
                float timeRemaining = timeout - elapsed;

                // Debug logging every 5 seconds
                if (elapsed > 2f && Time.realtimeSinceStartup - lastWatchdogLogTime >= 5f)
                {
                    lastWatchdogLogTime = Time.realtimeSinceStartup;
                    string adType = isRewardedShowing ? "Rewarded" : "Interstitial";
                    Debug.Log($"[AppLovin] WATCHDOG ({adType}): {elapsed:F1}s / {timeout}s, remaining: {timeRemaining:F1}s");
                    AdDebugLogger.Instance?.Log($"Watchdog tick: {elapsed:F1}s elapsed");
                }
            }
        }
        
        /// <summary>
        /// Stop timers and clean up
        /// </summary>
        private void StopAllWatchdogs()
        {
            // Stop nuclear timer
            StopNuclearTimer();
            
            // Reset flag
            nuclearTimerFired = false;
        }
        
        
        private void TriggerAdKillSwitch()
        {
            Debug.LogWarning($"[AppLovin] FORCE-KILLING AD: isInterstitial={isInterstitialShowing}, isRewarded={isRewardedShowing}");
            AdDebugLogger.Instance?.LogWarning($"FORCE-KILLING AD - timeout reached");

            // Stop all watchdogs first
            StopAllWatchdogs();
            
            // Stop periodic immersive mode restores
            CancelInvoke(nameof(RestoreImmersiveMode));
            
            bool wasInterstitial = isInterstitialShowing;
            bool wasRewarded = isRewardedShowing;

#if UNITY_ANDROID || UNITY_IOS
            // Layer 0: CRITICAL - Restore immersive mode FIRST (may make close button appear!)
            Debug.Log("[AppLovin] Layer 0: Restoring immersive mode to trigger close button");
            RestoreImmersiveMode();
            
            // Try multiple times with delays (sometimes needs a few attempts)
            Invoke(nameof(RestoreImmersiveMode), 0.1f);
            Invoke(nameof(RestoreImmersiveMode), 0.3f);
            Invoke(nameof(RestoreImmersiveMode), 0.5f);
            
            // Layer 1: Try SDK's official hide methods
            try
            {
                if (wasInterstitial)
                {
                    Debug.Log("[AppLovin] Layer 1: Calling MaxSdk.HideInterstitial()");
                    // Note: There's no HideInterstitial in MAX SDK - interstitials auto-hide
                    // Just invoke the callback
                }
                
                if (wasRewarded)
                {
                    Debug.Log("[AppLovin] Layer 1: Attempting to hide rewarded ad via SDK");
                    // Note: MAX SDK doesn't have HideRewarded either - rewarded ads must complete or be dismissed
                    // The SDK handles this internally, we just need to clean up our state
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AppLovin] SDK hide attempt failed: {e.Message}");
            }
#endif

            // Layer 2: Reset our state and invoke callbacks
            if (wasInterstitial)
            {
                isInterstitialShowing = false;
                adShowStartTime = 0f;
                Debug.Log("[AppLovin] Force-closing interstitial ad - invoking OnAdHidden");
                OnInterstitialAdClosed?.Invoke();
                LoadInterstitial();
            }

            if (wasRewarded)
            {
                isRewardedShowing = false;
                adShowStartTime = 0f;
                Debug.Log("[AppLovin] Force-closing rewarded ad - invoking OnAdHidden (no reward)");
                // Fire the hidden event so game flow continues
                OnRewardedAdClosed?.Invoke();
                // Also fire show failed since reward wasn't earned
                OnRewardedAdShowFailed?.Invoke();
                LoadRewardedAd();
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            // Layer 3: Try Android system methods to dismiss any stuck UI
            try
            {
                ForceCloseStuckAd();
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin] ForceCloseStuckAd failed: {e.Message}");
            }
            
            // Layer 4: If ad UI is still stuck, reload scene as last resort
            // Give a brief delay to let other methods work first
            Invoke(nameof(CheckAndSoftRestart), 0.5f);
#endif
        }
        
        /// <summary>
        /// Check if ad is still showing and do soft restart if needed
        /// </summary>
        private void CheckAndSoftRestart()
        {
            // If we somehow still have an ad showing after all cleanup, force restart
            if (isInterstitialShowing || isRewardedShowing)
            {
                Debug.LogWarning("[AppLovin] Ad still showing after cleanup - forcing soft restart");
                SoftRestart();
            }
        }
        
        /// <summary>
        /// Soft restart - reload current scene instead of crashing
        /// </summary>
        private void SoftRestart()
        {
            Debug.LogWarning("[AppLovin] Performing soft restart - reloading current scene");
            AdDebugLogger.Instance?.LogWarning("SOFT RESTART - Reloading scene to escape stuck ad");
            
            try
            {
                // Reset ad state
                isInterstitialShowing = false;
                isRewardedShowing = false;
                adShowStartTime = 0f;
                
                // Reload current scene
                var currentScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentScene);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin] Soft restart failed: {e.Message}");
                // Last resort - go to landing scene
                try
                {
                    SceneManager.LoadScene("LandingScene");
                }
                catch
                {
                    Debug.LogError("[AppLovin] Complete failure - cannot recover");
                }
            }
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

            // MUST set SDK key FIRST before any other calls
            MaxSdk.SetSdkKey(sdkKey);

            // Force test mode for development builds OR when config says test mode
            if (Debug.isDebugBuild || isTestMode)
            {
                Debug.Log("[AppLovin] ENABLING TEST MODE - Development build or test config");
                AdDebugLogger.Instance?.LogWarning("TEST MODE ENABLED - Forcing test ads");
                
                // Enable verbose logging for debugging
                MaxSdk.SetVerboseLogging(true);
                
                // Enable creative debugger (shows ad info overlay)
                MaxSdk.SetCreativeDebuggerEnabled(true);
                
                // Get device advertising ID and set as test device
                // This MUST be called BEFORE InitializeSdk()
                SetCurrentDeviceAsTestDevice();
            }
            else
            {
                AdDebugLogger.Instance?.LogWarning("PRODUCTION MODE - Will show real ads");
                MaxSdk.SetVerboseLogging(false);
            }

            // Register SDK initialization callback BEFORE initialization (matches demo pattern)
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;

            MaxSdk.InitializeSdk();
            
            AdDebugLogger.Instance?.Log("InitializeSdk() called");
#else
            Debug.LogWarning("[AppLovin] SDK only works on Android/iOS builds");
            AdDebugLogger.Instance?.LogWarning("Wrong platform - SDK only works on Android/iOS");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        /// <summary>
        /// Set the current device as a test device using its advertising ID.
        /// This enables test ads on this specific device.
        /// MUST be called BEFORE MaxSdk.InitializeSdk()
        /// </summary>
        private void SetCurrentDeviceAsTestDevice()
        {
            try
            {
#if UNITY_ANDROID
                // Get Android Advertising ID (GAID)
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (var resolver = activity.Call<AndroidJavaObject>("getContentResolver"))
                using (var settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                {
                    string androidId = settingsSecure.CallStatic<string>("getString", resolver, "android_id");
                    if (!string.IsNullOrEmpty(androidId))
                    {
                        Debug.Log($"[AppLovin] Setting device as test device: {androidId}");
                        AdDebugLogger.Instance?.Log($"Test Device ID (Android): {androidId}");
                        MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { androidId });
                    }
                    else
                    {
                        Debug.LogWarning("[AppLovin] Could not get Android ID for test device");
                    }
                }
#elif UNITY_IOS
                // On iOS, we need to use IDFA - but this requires ATT permission
                // For now, just log that test mode is enabled via dashboard
                Debug.Log("[AppLovin] iOS test mode - add device IDFA in AppLovin dashboard");
                AdDebugLogger.Instance?.Log("iOS: Add device IDFA in AppLovin dashboard for test ads");
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[AppLovin] Failed to set test device: {e.Message}");
                AdDebugLogger.Instance?.LogWarning($"Test device setup failed: {e.Message}");
            }
        }

        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration config)
        {
            LogDebug("MAX SDK Initialized");
            
            AdDebugLogger.Instance?.LogSection("SDK INITIALIZED SUCCESSFULLY");
            AdDebugLogger.Instance?.Log($"Country Code: {config.CountryCode}");
            AdDebugLogger.Instance?.Log($"Test Mode Enabled: {config.IsTestModeEnabled}");
            AdDebugLogger.Instance?.Log($"Consent Flow: {config.ConsentFlowUserGeography}");
            
            // Log test mode status prominently
            if (config.IsTestModeEnabled)
            {
                Debug.Log("[AppLovin] ✓ TEST MODE IS ACTIVE - Test ads will be shown");
                AdDebugLogger.Instance?.LogWarning("TEST MODE CONFIRMED ACTIVE");
            }
            else
            {
                Debug.LogWarning("[AppLovin] ✗ TEST MODE NOT ACTIVE - Live ads will be shown");
                AdDebugLogger.Instance?.LogWarning("WARNING: Test mode NOT active despite settings!");
            }
            
            isInitialized = true;

            // Initialize ad types AFTER SDK is fully initialized (matches demo pattern)
            InitializeInterstitialAds();
            InitializeRewardedAds();
            
            // In debug builds, auto-show mediation debugger so dev can verify test mode
            // Comment this out if you don't want it to auto-show
            if (Debug.isDebugBuild && autoShowMediationDebugger)
            {
                Debug.Log("[AppLovin] Debug build - auto-showing Mediation Debugger");
                // Delay slightly to let UI settle
                Invoke(nameof(ShowMediationDebugger), 1.5f);
            }
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
            // CRITICAL: Restore immersive mode when interstitial displays (same issue as rewarded)
            RestoreImmersiveMode();
            
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
            StopAllWatchdogs();
            
            OnInterstitialAdShowFailed?.Invoke();
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // CRITICAL: Restore immersive mode IMMEDIATELY to fix broken system UI flags
            // This fixes the "no close button" bug on Samsung/Mali devices (Android 12-14)
            RestoreImmersiveMode();
            
            LogDebug("Interstitial dismissed");
            isInterstitialShowing = false;
            adShowStartTime = 0f;
            StopAllWatchdogs();
            
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
            // Restore immersive mode even if ad failed
            RestoreImmersiveMode();
            CancelInvoke(nameof(RestoreImmersiveMode));
            
            LogDebug($"Rewarded ad failed to display: {errorInfo.Code}");
            isRewardedShowing = false;
            adShowStartTime = 0f;
            StopAllWatchdogs();
            
            OnRewardedAdShowFailed?.Invoke();
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // CRITICAL: Restore immersive mode IMMEDIATELY when ad displays
            // Prevents system UI from breaking and ensures close button will render
            RestoreImmersiveMode();
            
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
            
            // Schedule periodic immersive mode restore while ad is showing (every 2 seconds)
            // This ensures close button stays visible even if system UI gets broken again
            InvokeRepeating(nameof(RestoreImmersiveMode), 2f, 2f);
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // CRITICAL: Restore immersive mode IMMEDIATELY to fix broken system UI flags
            // This fixes the "no close button" bug on Samsung/Mali devices (Android 12-14)
            // This re-layout also unsticks Mali GPU video decoding hangs
            RestoreImmersiveMode();
            
            // Stop periodic immersive mode restores
            CancelInvoke(nameof(RestoreImmersiveMode));
            
            LogDebug("Rewarded ad dismissed");
            isRewardedShowing = false;
            adShowStartTime = 0f;
            StopAllWatchdogs();
            
            OnRewardedAdClosed?.Invoke();
            LoadRewardedAd(); // Pre-load next ad
        }

        /// <summary>
        /// Restore immersive fullscreen mode after ad dismissal.
        /// 
        /// CRITICAL FIX for Samsung/Mali devices (Android 12-14, especially One UI):
        /// AppLovin MAX ads destroy immersive mode when shown, causing:
        /// - System navigation bar to reappear
        /// - Ad close button to fail to render
        /// - Mali GPU video decoding to hang
        /// 
        /// Re-applying immersive mode forces Android to re-layout the ad container,
        /// which makes the close button appear and unsticks video playback.
        /// 
        /// Called at multiple points:
        /// - OnAdDisplayed: Prevent the issue proactively
        /// - OnRewardReceived: When video finishes (playbackEnded) - close button should appear
        /// - OnAdHidden: After ad closes
        /// - Periodically while ad is showing: Keep it fixed
        /// - Nuclear timer: Last resort force-fix
        /// 
        /// This is a known AppLovin + Android quirk affecting dozens of Unity devs in 2024-2025.
        /// </summary>
        private void RestoreImmersiveMode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    if (activity == null)
                    {
                        Debug.LogWarning("[AppLovin] Cannot restore immersive mode - activity is null");
                        return;
                    }
                    
                    // Run on UI thread to ensure it executes immediately
                    activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        try
                        {
                            using (var window = activity.Call<AndroidJavaObject>("getWindow"))
                            using (var decorView = window.Call<AndroidJavaObject>("getDecorView"))
                            {
                                // 5894 = View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY (2048) 
                                //        | View.SYSTEM_UI_FLAG_FULLSCREEN (4)
                                //        | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION (2)
                                //        | View.SYSTEM_UI_FLAG_LAYOUT_STABLE (256)
                                //        | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION (512)
                                //        | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN (1024)
                                const int IMMERSIVE_STICKY_FLAGS = 5894;
                                
                                decorView.Call("setSystemUiVisibility", IMMERSIVE_STICKY_FLAGS);
                                Debug.Log("[AppLovin] Immersive mode restored on UI thread");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning($"[AppLovin] Failed to restore immersive mode on UI thread: {e.Message}");
                        }
                    }));
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AppLovin] Failed to restore immersive mode: {e.Message}");
            }
#endif
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
            // CRITICAL: Restore immersive mode when video finishes (playbackEnded equivalent)
            // This is when the close button SHOULD appear - force re-layout to make it visible
            RestoreImmersiveMode();
            
            LogDebug($"Rewarded ad received reward: {reward.Label} x{reward.Amount}");
            OnRewardedAdRewarded?.Invoke();
            
            // Keep restoring immersive mode periodically until ad closes
            // Some ads delay showing close button, this ensures it appears
            InvokeRepeating(nameof(RestoreImmersiveMode), 0.5f, 0.5f);
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

                // Start NUCLEAR TIMER - guarantees closure after timeout
                StartNuclearTimer(interstitialWatchdogTimeout);

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

                // Start NUCLEAR TIMER - 60s for rewarded (longer for video completion)
                StartNuclearTimer(rewardedWatchdogTimeout);

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
        /// Start a nuclear timer that guarantees ad closure after specified seconds
        /// This uses System.Threading.Timer which runs on its own thread and cannot be blocked
        /// </summary>
        /// <param name="timeoutSeconds">Timeout in seconds (30 for interstitial, 60 for rewarded)</param>
        private void StartNuclearTimer(float timeoutSeconds)
        {
            StopNuclearTimer(); // Clean up any existing timer

            int timeoutMs = (int)(timeoutSeconds * 1000);
            Debug.Log($"[AppLovin] Starting NUCLEAR TIMER - guaranteed {timeoutSeconds}s closure");
            AdDebugLogger.Instance?.LogWarning($"NUCLEAR TIMER STARTED - Will force-close ad in {timeoutSeconds} seconds");

            nuclearTimer = new System.Threading.Timer(
                NuclearTimerCallback,     // Callback method
                null,                     // State object (unused)
                timeoutMs,                // Due time
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
        /// IMPORTANT: This runs on a background thread - DO NOT do JNI here!
        /// Just set a flag that Update() will check on the main thread.
        /// </summary>
        private void NuclearTimerCallback(object state)
        {
            // DO NOT DO JNI HERE - causes SIGABRT crash!
            // Just set flag, Update() will handle it on main thread
            Debug.Log("[AppLovin] Nuclear timer callback fired - setting flag for main thread");
            nuclearTimerFired = true;
        }

        /// <summary>
        /// Execute the nuclear close logic (called from Update on main thread)
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
        /// Forcefully close stuck ads using Android system calls
        /// Limited to back spam + ESC keys - no finishAffinity (causes crash)
        /// If this fails, SoftRestart (scene reload) will be called
        /// </summary>
        private void ForceCloseStuckAd()
        {
            Debug.LogWarning("[AppLovin] *** FORCE-CLOSE ACTIVATED ***");

            // Layer 1: Send limited back presses (10 max to avoid ANR)
            Debug.Log("[AppLovin] Layer 1: Sending 10 back button presses");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    TrySendAndroidBack();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[AppLovin] Back press {i} failed: {e.Message}");
                }
                System.Threading.Thread.Sleep(50);
            }

            // Layer 2: Try ESC key (some WebView ads respond)
            Debug.Log("[AppLovin] Layer 2: Sending ESC key events");
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    TrySendEscapeKey();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[AppLovin] ESC key {i} failed: {e.Message}");
                }
                System.Threading.Thread.Sleep(50);
            }

            Debug.LogWarning("[AppLovin] *** FORCE-CLOSE METHODS COMPLETED ***");
            
            // NOTE: If ad is still stuck after this, TriggerAdKillSwitch's catch block
            // will call SoftRestart() which reloads the scene
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
