using System;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages IronSource interstitial ads for Android platform
    /// </summary>
    public class IronSourceManager : MonoBehaviour
    {
        public static IronSourceManager Instance { get; private set; }

        [SerializeField] private string androidAppKey = "23fbe1e55";
        
        private bool isInitialized = false;
        private bool isInterstitialReady = false;
        private bool isRewardedAdReady = false;
        
        // Interstitial ad events
        public event Action OnInterstitialAdReady;
        public event Action OnInterstitialAdLoadFailed;
        public event Action OnInterstitialAdShown;
        public event Action OnInterstitialAdClosed;
        public event Action OnInterstitialAdShowFailed;
        
        // Rewarded ad events
        public event Action OnRewardedAdReady;
        public event Action OnRewardedAdLoadFailed;
        public event Action OnRewardedAdShown;
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
            InitializeIronSource();
        }

        /// <summary>
        /// Initialize IronSource SDK
        /// </summary>
        public void InitializeIronSource()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log($"[IronSource] Initializing with App Key: {androidAppKey}");
                
                // Get the IronSource class
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    // Set user consent for data privacy (GDPR, CCPA compliance)
                    ironSourceClass.CallStatic("setConsent", true);
                    
                    // Initialize the SDK
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            ironSourceClass.CallStatic("init", currentActivity, androidAppKey, "INTERSTITIAL,REWARDED_VIDEO");
                        }
                    }
                }
                
                // Register for interstitial and rewarded ad callbacks
                RegisterInterstitialCallbacks();
                RegisterRewardedAdCallbacks();
                
                isInitialized = true;
                Debug.Log("[IronSource] Initialization complete");
                
                // Load first interstitial and rewarded ad
                LoadInterstitial();
                LoadRewardedAd();
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Initialization failed: {e.Message}");
            }
#elif UNITY_EDITOR
            Debug.Log("[IronSource] Running in Editor - SDK not initialized");
            isInitialized = true;
            // Simulate ads ready after short delay for testing
            Invoke(nameof(SimulateInterstitialAdReady), 2f);
            Invoke(nameof(SimulateRewardedAdReady), 2.5f);
#else
            Debug.Log("[IronSource] Platform not supported - SDK not initialized");
#endif
        }

        private void SimulateInterstitialAdReady()
        {
            isInterstitialReady = true;
            OnInterstitialAdReady?.Invoke();
            Debug.Log("[IronSource] [Editor] Simulated interstitial ad ready");
        }
        
        private void SimulateRewardedAdReady()
        {
            isRewardedAdReady = true;
            OnRewardedAdReady?.Invoke();
            Debug.Log("[IronSource] [Editor] Simulated rewarded ad ready");
        }

        /// <summary>
        /// Register callbacks for interstitial ad events
        /// </summary>
        private void RegisterInterstitialCallbacks()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    // Create callback listener
                    IronSourceInterstitialListener listener = new IronSourceInterstitialListener(this);
                    ironSourceClass.CallStatic("setInterstitialListener", listener);
                }
                Debug.Log("[IronSource] Interstitial callbacks registered");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to register callbacks: {e.Message}");
            }
#endif
        }

        /// <summary>
        /// Load an interstitial ad
        /// </summary>
        public void LoadInterstitial()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[IronSource] SDK not initialized. Cannot load interstitial.");
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    ironSourceClass.CallStatic("loadInterstitial");
                }
                Debug.Log("[IronSource] Loading interstitial ad...");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to load interstitial: {e.Message}");
            }
#elif UNITY_EDITOR
            Debug.Log("[IronSource] [Editor] Simulating interstitial load");
            Invoke(nameof(SimulateInterstitialAdReady), 1f);
#endif
        }

        /// <summary>
        /// Check if interstitial ad is ready to show
        /// </summary>
        public bool IsInterstitialReady()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    bool ready = ironSourceClass.CallStatic<bool>("isInterstitialReady");
                    return ready;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to check if interstitial ready: {e.Message}");
                return false;
            }
#elif UNITY_EDITOR
            return isInterstitialReady;
#else
            return false;
#endif
        }

        /// <summary>
        /// Show interstitial ad
        /// </summary>
        public void ShowInterstitial()
        {
            if (!IsInterstitialReady())
            {
                Debug.LogWarning("[IronSource] Interstitial not ready to show");
                OnInterstitialAdShowFailed?.Invoke();
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    ironSourceClass.CallStatic("showInterstitial");
                }
                Debug.Log("[IronSource] Showing interstitial ad");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to show interstitial: {e.Message}");
                OnInterstitialAdShowFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log("[IronSource] [Editor] Simulating interstitial show");
            OnInterstitialAdShown?.Invoke();
            // Simulate ad closed after 3 seconds
            Invoke(nameof(SimulateAdClosed), 3f);
#endif
        }

        private void SimulateAdClosed()
        {
            isInterstitialReady = false;
            OnInterstitialAdClosed?.Invoke();
            Debug.Log("[IronSource] [Editor] Simulated ad closed");
            // Load next ad
            LoadInterstitial();
        }

        // Callback methods called from native listener
        internal void OnInterstitialAdReadyCallback()
        {
            isInterstitialReady = true;
            Debug.Log("[IronSource] Interstitial ad ready");
            OnInterstitialAdReady?.Invoke();
        }

        internal void OnInterstitialAdLoadFailedCallback(string error)
        {
            isInterstitialReady = false;
            Debug.LogError($"[IronSource] Interstitial ad load failed: {error}");
            OnInterstitialAdLoadFailed?.Invoke();
            
            // Retry loading after delay
            Invoke(nameof(LoadInterstitial), 30f);
        }

        internal void OnInterstitialAdShownCallback()
        {
            Debug.Log("[IronSource] Interstitial ad shown");
            OnInterstitialAdShown?.Invoke();
        }

        internal void OnInterstitialAdClosedCallback()
        {
            isInterstitialReady = false;
            Debug.Log("[IronSource] Interstitial ad closed");
            OnInterstitialAdClosed?.Invoke();
            
            // Load next ad
            LoadInterstitial();
        }

        internal void OnInterstitialAdShowFailedCallback(string error)
        {
            Debug.LogError($"[IronSource] Interstitial ad show failed: {error}");
            OnInterstitialAdShowFailed?.Invoke();
        }

        internal void OnInterstitialAdClickedCallback()
        {
            Debug.Log("[IronSource] Interstitial ad clicked");
        }

        // ========== REWARDED AD METHODS ==========

        /// <summary>
        /// Register callbacks for rewarded ad events
        /// </summary>
        private void RegisterRewardedAdCallbacks()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    // Create callback listener
                    IronSourceRewardedVideoListener listener = new IronSourceRewardedVideoListener(this);
                    ironSourceClass.CallStatic("setRewardedVideoListener", listener);
                }
                Debug.Log("[IronSource] Rewarded video callbacks registered");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to register rewarded callbacks: {e.Message}");
            }
#endif
        }

        /// <summary>
        /// Load a rewarded ad
        /// </summary>
        public void LoadRewardedAd()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[IronSource] SDK not initialized. Cannot load rewarded ad.");
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            // Rewarded ads auto-load with IronSource, just log
            Debug.Log("[IronSource] Rewarded ad will auto-load");
#elif UNITY_EDITOR
            Debug.Log("[IronSource] [Editor] Simulating rewarded ad load");
            Invoke(nameof(SimulateRewardedAdReady), 1f);
#endif
        }

        /// <summary>
        /// Check if rewarded ad is ready to show
        /// </summary>
        public bool IsRewardedAdReady()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    bool ready = ironSourceClass.CallStatic<bool>("isRewardedVideoAvailable");
                    return ready;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to check if rewarded ad ready: {e.Message}");
                return false;
            }
#elif UNITY_EDITOR
            return isRewardedAdReady;
#else
            return false;
#endif
        }

        /// <summary>
        /// Show rewarded ad
        /// </summary>
        public void ShowRewardedAd(string placementName = null)
        {
            if (!IsRewardedAdReady())
            {
                Debug.LogWarning("[IronSource] Rewarded ad not ready to show");
                OnRewardedAdShowFailed?.Invoke();
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass ironSourceClass = new AndroidJavaClass("com.ironsource.mediationsdk.IronSource"))
                {
                    if (string.IsNullOrEmpty(placementName))
                    {
                        ironSourceClass.CallStatic("showRewardedVideo");
                    }
                    else
                    {
                        ironSourceClass.CallStatic("showRewardedVideo", placementName);
                    }
                }
                Debug.Log("[IronSource] Showing rewarded ad");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Failed to show rewarded ad: {e.Message}");
                OnRewardedAdShowFailed?.Invoke();
            }
#elif UNITY_EDITOR
            Debug.Log("[IronSource] [Editor] Simulating rewarded ad show");
            OnRewardedAdShown?.Invoke();
            // Simulate ad completed (user watched it) after 3 seconds
            Invoke(nameof(SimulateRewardedAdCompleted), 3f);
#endif
        }

        private void SimulateRewardedAdCompleted()
        {
            Debug.Log("[IronSource] [Editor] Simulated rewarded ad completed - granting reward");
            OnRewardedAdRewarded?.Invoke();
            Invoke(nameof(SimulateRewardedAdClosed), 0.5f);
        }

        private void SimulateRewardedAdClosed()
        {
            isRewardedAdReady = false;
            OnRewardedAdClosed?.Invoke();
            Debug.Log("[IronSource] [Editor] Simulated rewarded ad closed");
            // Load next ad
            LoadRewardedAd();
        }

        // Callback methods called from native listener
        internal void OnRewardedAdAvailableCallback()
        {
            isRewardedAdReady = true;
            Debug.Log("[IronSource] Rewarded ad ready");
            OnRewardedAdReady?.Invoke();
        }

        internal void OnRewardedAdUnavailableCallback()
        {
            isRewardedAdReady = false;
            Debug.LogWarning("[IronSource] Rewarded ad unavailable");
        }

        internal void OnRewardedAdShownCallback()
        {
            Debug.Log("[IronSource] Rewarded ad shown");
            OnRewardedAdShown?.Invoke();
        }

        internal void OnRewardedAdRewardedCallback(string placementName, int amount)
        {
            Debug.Log($"[IronSource] Rewarded ad completed - User earned reward: {amount} from {placementName}");
            OnRewardedAdRewarded?.Invoke();
        }

        internal void OnRewardedAdClosedCallback()
        {
            isRewardedAdReady = false;
            Debug.Log("[IronSource] Rewarded ad closed");
            OnRewardedAdClosed?.Invoke();
        }

        internal void OnRewardedAdShowFailedCallback(string error)
        {
            Debug.LogError($"[IronSource] Rewarded ad show failed: {error}");
            OnRewardedAdShowFailed?.Invoke();
        }

        internal void OnRewardedAdClickedCallback()
        {
            Debug.Log("[IronSource] Rewarded ad clicked");
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    /// <summary>
    /// Listener for IronSource interstitial callbacks
    /// </summary>
    public class IronSourceInterstitialListener : AndroidJavaProxy
    {
        private IronSourceManager manager;

        public IronSourceInterstitialListener(IronSourceManager manager) 
            : base("com.ironsource.mediationsdk.sdk.InterstitialListener")
        {
            this.manager = manager;
        }

        public void onInterstitialAdReady()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdReadyCallback();
            });
        }

        public void onInterstitialAdLoadFailed(AndroidJavaObject error)
        {
            string errorMessage = error.Call<string>("getErrorMessage");
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdLoadFailedCallback(errorMessage);
            });
        }

        public void onInterstitialAdOpened()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdShownCallback();
            });
        }

        public void onInterstitialAdClosed()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdClosedCallback();
            });
        }

        public void onInterstitialAdShowFailed(AndroidJavaObject error)
        {
            string errorMessage = error.Call<string>("getErrorMessage");
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdShowFailedCallback(errorMessage);
            });
        }

        public void onInterstitialAdClicked()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnInterstitialAdClickedCallback();
            });
        }

        public void onInterstitialAdShowSucceeded()
        {
            // Ad show succeeded callback
        }
    }
    
    /// <summary>
    /// Listener for IronSource rewarded video callbacks
    /// </summary>
    public class IronSourceRewardedVideoListener : AndroidJavaProxy
    {
        private IronSourceManager manager;

        public IronSourceRewardedVideoListener(IronSourceManager manager) 
            : base("com.ironsource.mediationsdk.sdk.RewardedVideoListener")
        {
            this.manager = manager;
        }

        public void onRewardedVideoAdOpened()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnRewardedAdShownCallback();
            });
        }

        public void onRewardedVideoAdClosed()
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnRewardedAdClosedCallback();
            });
        }

        public void onRewardedVideoAvailabilityChanged(bool available)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                if (available)
                {
                    manager.OnRewardedAdAvailableCallback();
                }
                else
                {
                    manager.OnRewardedAdUnavailableCallback();
                }
            });
        }

        public void onRewardedVideoAdRewarded(AndroidJavaObject placement)
        {
            string placementName = placement.Call<string>("getPlacementName");
            int rewardAmount = placement.Call<int>("getRewardAmount");
            
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnRewardedAdRewardedCallback(placementName, rewardAmount);
            });
        }

        public void onRewardedVideoAdShowFailed(AndroidJavaObject error)
        {
            string errorMessage = error.Call<string>("getErrorMessage");
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnRewardedAdShowFailedCallback(errorMessage);
            });
        }

        public void onRewardedVideoAdClicked(AndroidJavaObject placement)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() => 
            {
                manager.OnRewardedAdClickedCallback();
            });
        }

        public void onRewardedVideoAdStarted()
        {
            // Video started playing
        }

        public void onRewardedVideoAdEnded()
        {
            // Video finished playing
        }
    }
#endif
}

