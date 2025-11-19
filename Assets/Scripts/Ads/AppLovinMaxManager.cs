using System;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages AppLovin MAX mediation SDK for interstitial and rewarded ads
    /// Implements IMediationManager for modular ad SDK integration
    /// </summary>
    public class AppLovinMaxManager : MonoBehaviour, IMediationManager
    {
        public static AppLovinMaxManager Instance { get; private set; }
        
        [SerializeField] private MonetizationSettings settings;
        
        private bool _isInitialized = false;
        private bool _isInterstitialReady = false;
        private bool _isRewardedAdReady = false;
        
        private string _interstitialAdUnitId;
        private string _rewardedAdUnitId;
        
        // Interface properties
        public bool IsInitialized => _isInitialized;
        
        // Interstitial Events
        public event Action OnInterstitialAdReady;
        public event Action OnInterstitialAdLoadFailed;
        public event Action OnInterstitialAdShown;
        public event Action OnInterstitialAdClosed;
        public event Action OnInterstitialAdShowFailed;
        public event Action OnInterstitialAdClicked;
        
        // Rewarded Ad Events
        public event Action OnRewardedAdReady;
        public event Action OnRewardedAdLoadFailed;
        public event Action OnRewardedAdShown;
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
            
            // Load settings from Resources if not assigned
            if (settings == null)
            {
                settings = Resources.Load<MonetizationSettings>("MonetizationSettings");
                if (settings == null)
                {
                    Debug.LogError("[AppLovin MAX] MonetizationSettings not found in Resources folder! Create one via Assets -> Create -> BattleCruisers -> Monetization Settings");
                }
            }
        }
        
        private async void Start()
        {
            if (settings != null && settings.enableAppLovinAds && settings.IsValid())
            {
                await InitializeAsync(settings.appLovinSdkKey);
            }
            else
            {
                Debug.LogWarning("[AppLovin MAX] Ads disabled or invalid configuration");
            }
        }
        
        /// <summary>
        /// Initialize AppLovin MAX SDK
        /// </summary>
        public async Task InitializeAsync(string sdkKey)
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[AppLovin MAX] Already initialized");
                return;
            }
            
            if (string.IsNullOrEmpty(sdkKey))
            {
                Debug.LogError("[AppLovin MAX] SDK Key is empty!");
                return;
            }
            
            _interstitialAdUnitId = settings?.GetInterstitialAdUnitId() ?? "";
            _rewardedAdUnitId = settings?.GetRewardedAdUnitId() ?? "";
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[AppLovin MAX] Initializing with SDK Key: {sdkKey}");
                
                // Initialize via Android JNI
                using (AndroidJavaClass maxSdk = new AndroidJavaClass("com.applovin.sdk.AppLovinSdk"))
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Get builder
                    using (AndroidJavaObject sdkSettings = maxSdk.CallStatic<AndroidJavaObject>("getInstance", currentActivity))
                    {
                        // Set verbose logging if enabled
                        if (settings != null && settings.verboseLogging)
                        {
                            sdkSettings.Call("setVerboseLogging", true);
                        }
                        
                        // Set test mode if enabled
                        if (settings != null && settings.testMode)
                        {
                            sdkSettings.Call("setTestDeviceAdvertisingIds", new string[] { "test_device_id" });
                        }
                        
                        // Initialize SDK
                        sdkSettings.Call("initializeSdk");
                    }
                }
                
                // Register callbacks
                RegisterInterstitialCallbacks();
                RegisterRewardedAdCallbacks();
                
                _isInitialized = true;
                Debug.Log("[AppLovin MAX] Initialization complete");
                
                // Load first ads
                LoadInterstitial(_interstitialAdUnitId);
                LoadRewardedAd(_rewardedAdUnitId);
                
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Initialization failed: {e.Message}\n{e.StackTrace}");
            }
#elif UNITY_EDITOR
            Debug.Log($"[AppLovin MAX] [Editor] Initializing with SDK Key: {sdkKey}");
            _isInitialized = true;
            
            // Simulate ads ready after short delay for testing
            await Task.Delay(2000);
            SimulateInterstitialReady();
            await Task.Delay(500);
            SimulateRewardedAdReady();
            
            Debug.Log("[AppLovin MAX] [Editor] Initialization complete (simulated)");
#else
            Debug.LogWarning("[AppLovin MAX] Platform not supported");
            await Task.CompletedTask;
#endif
        }
        
        #region Interstitial Ads
        
        public bool IsInterstitialReady(string adUnitId = null)
        {
            string unitId = adUnitId ?? _interstitialAdUnitId;
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass maxInterstitial = new AndroidJavaClass("com.applovin.mediation.ads.MaxInterstitialAd"))
                {
                    // Note: This is a simplified check. Actual implementation needs instance management
                    return _isInterstitialReady;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to check interstitial ready: {e.Message}");
                return false;
            }
#elif UNITY_EDITOR
            return _isInterstitialReady;
#else
            return false;
#endif
        }
        
        public void LoadInterstitial(string adUnitId = null)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[AppLovin MAX] SDK not initialized. Cannot load interstitial.");
                return;
            }
            
            string unitId = adUnitId ?? _interstitialAdUnitId;
            
            if (string.IsNullOrEmpty(unitId))
            {
                Debug.LogError("[AppLovin MAX] Interstitial Ad Unit ID is empty!");
                return;
            }
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[AppLovin MAX] Loading interstitial ad: {unitId}");
                
                using (AndroidJavaClass maxInterstitial = new AndroidJavaClass("com.applovin.mediation.ads.MaxInterstitialAd"))
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Create interstitial instance
                    using (AndroidJavaObject interstitial = new AndroidJavaObject("com.applovin.mediation.ads.MaxInterstitialAd", unitId, currentActivity))
                    {
                        // Load ad
                        interstitial.Call("loadAd");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to load interstitial: {e.Message}");
                OnInterstitialAdLoadFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log($"[AppLovin MAX] [Editor] Loading interstitial ad: {unitId}");
            Invoke(nameof(SimulateInterstitialReady), 1f);
#endif
        }
        
        public void ShowInterstitial(string adUnitId = null)
        {
            if (!IsInterstitialReady(adUnitId))
            {
                Debug.LogWarning("[AppLovin MAX] Interstitial not ready to show");
                OnInterstitialAdShowFailed?.Invoke();
                return;
            }
            
            string unitId = adUnitId ?? _interstitialAdUnitId;
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[AppLovin MAX] Showing interstitial ad: {unitId}");
                
                using (AndroidJavaClass maxInterstitial = new AndroidJavaClass("com.applovin.mediation.ads.MaxInterstitialAd"))
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Get interstitial instance and show
                    using (AndroidJavaObject interstitial = new AndroidJavaObject("com.applovin.mediation.ads.MaxInterstitialAd", unitId, currentActivity))
                    {
                        if (interstitial.Call<bool>("isReady"))
                        {
                            interstitial.Call("showAd");
                            OnInterstitialAdShown?.Invoke();
                            
                            // Log to Firebase Analytics (fail silently if not available)
                            try
                            {
                                if (FirebaseAnalyticsManager.Instance != null)
                                {
                                    FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning($"[AppLovin MAX] Failed to log analytics: {e.Message}");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("[AppLovin MAX] Interstitial not ready despite check");
                            OnInterstitialAdShowFailed?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to show interstitial: {e.Message}");
                OnInterstitialAdShowFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log($"[AppLovin MAX] [Editor] Showing interstitial ad: {unitId}");
            _isInterstitialReady = false;
            OnInterstitialAdShown?.Invoke();
            
            // Simulate ad closed after 3 seconds
            Invoke(nameof(SimulateInterstitialClosed), 3f);
            
            // Log to Firebase Analytics (fail silently if not available)
            try
            {
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AppLovin MAX] Failed to log analytics: {e.Message}");
            }
#endif
        }
        
        private void RegisterInterstitialCallbacks()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[AppLovin MAX] Registering interstitial callbacks");
                
                // TODO: Implement proper MaxAdListener using AndroidJavaProxy
                // This requires creating a listener class similar to IronSource implementation
                // For now, using simplified callback handling
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to register interstitial callbacks: {e.Message}");
            }
#endif
        }
        
        // Editor simulation methods
        private void SimulateInterstitialReady()
        {
            _isInterstitialReady = true;
            OnInterstitialAdReady?.Invoke();
            Debug.Log("[AppLovin MAX] [Editor] Interstitial ad ready (simulated)");
        }
        
        private void SimulateInterstitialClosed()
        {
            OnInterstitialAdClosed?.Invoke();
            Debug.Log("[AppLovin MAX] [Editor] Interstitial ad closed (simulated)");
            
            // Log to Firebase Analytics (fail silently if not available)
            try
            {
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[AppLovin MAX] Failed to log analytics: {e.Message}");
            }
            
            // Load next ad
            LoadInterstitial(_interstitialAdUnitId);
        }
        
        #endregion
        
        #region Rewarded Ads
        
        public bool IsRewardedAdReady(string adUnitId = null)
        {
            string unitId = adUnitId ?? _rewardedAdUnitId;
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                return _isRewardedAdReady;
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to check rewarded ad ready: {e.Message}");
                return false;
            }
#elif UNITY_EDITOR
            return _isRewardedAdReady;
#else
            return false;
#endif
        }
        
        public void LoadRewardedAd(string adUnitId = null)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[AppLovin MAX] SDK not initialized. Cannot load rewarded ad.");
                return;
            }
            
            string unitId = adUnitId ?? _rewardedAdUnitId;
            
            if (string.IsNullOrEmpty(unitId))
            {
                Debug.LogError("[AppLovin MAX] Rewarded Ad Unit ID is empty!");
                return;
            }
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[AppLovin MAX] Loading rewarded ad: {unitId}");
                
                using (AndroidJavaClass maxRewardedAd = new AndroidJavaClass("com.applovin.mediation.ads.MaxRewardedAd"))
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Create rewarded ad instance
                    using (AndroidJavaObject rewardedAd = maxRewardedAd.CallStatic<AndroidJavaObject>("getInstance", unitId, currentActivity))
                    {
                        // Load ad
                        rewardedAd.Call("loadAd");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to load rewarded ad: {e.Message}");
                OnRewardedAdLoadFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log($"[AppLovin MAX] [Editor] Loading rewarded ad: {unitId}");
            Invoke(nameof(SimulateRewardedAdReady), 1f);
#endif
        }
        
        public void ShowRewardedAd(string adUnitId = null)
        {
            if (!IsRewardedAdReady(adUnitId))
            {
                Debug.LogWarning("[AppLovin MAX] Rewarded ad not ready to show");
                OnRewardedAdShowFailed?.Invoke();
                return;
            }
            
            string unitId = adUnitId ?? _rewardedAdUnitId;
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[AppLovin MAX] Showing rewarded ad: {unitId}");
                
                using (AndroidJavaClass maxRewardedAd = new AndroidJavaClass("com.applovin.mediation.ads.MaxRewardedAd"))
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Get rewarded ad instance and show
                    using (AndroidJavaObject rewardedAd = maxRewardedAd.CallStatic<AndroidJavaObject>("getInstance", unitId, currentActivity))
                    {
                        if (rewardedAd.Call<bool>("isReady"))
                        {
                            rewardedAd.Call("showAd");
                            OnRewardedAdShown?.Invoke();
                        }
                        else
                        {
                            Debug.LogWarning("[AppLovin MAX] Rewarded ad not ready despite check");
                            OnRewardedAdShowFailed?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to show rewarded ad: {e.Message}");
                OnRewardedAdShowFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log($"[AppLovin MAX] [Editor] Showing rewarded ad: {unitId}");
            _isRewardedAdReady = false;
            OnRewardedAdShown?.Invoke();
            
            // Simulate ad completed (user watched it) after 3 seconds
            Invoke(nameof(SimulateRewardedAdCompleted), 3f);
#endif
        }
        
        private void RegisterRewardedAdCallbacks()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[AppLovin MAX] Registering rewarded ad callbacks");
                
                // TODO: Implement proper MaxRewardedAdListener using AndroidJavaProxy
                // This requires creating a listener class similar to IronSource implementation
            }
            catch (Exception e)
            {
                Debug.LogError($"[AppLovin MAX] Failed to register rewarded ad callbacks: {e.Message}");
            }
#endif
        }
        
        // Editor simulation methods
        private void SimulateRewardedAdReady()
        {
            _isRewardedAdReady = true;
            OnRewardedAdReady?.Invoke();
            Debug.Log("[AppLovin MAX] [Editor] Rewarded ad ready (simulated)");
        }
        
        private void SimulateRewardedAdCompleted()
        {
            Debug.Log("[AppLovin MAX] [Editor] Rewarded ad completed - granting reward (simulated)");
            OnRewardedAdRewarded?.Invoke();
            Invoke(nameof(SimulateRewardedAdClosed), 0.5f);
        }
        
        private void SimulateRewardedAdClosed()
        {
            OnRewardedAdClosed?.Invoke();
            Debug.Log("[AppLovin MAX] [Editor] Rewarded ad closed (simulated)");
            
            // Load next ad
            LoadRewardedAd(_rewardedAdUnitId);
        }
        
        #endregion
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}

