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
        
        public event Action OnInterstitialAdReady;
        public event Action OnInterstitialAdLoadFailed;
        public event Action OnInterstitialAdShown;
        public event Action OnInterstitialAdClosed;
        public event Action OnInterstitialAdShowFailed;

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
                            ironSourceClass.CallStatic("init", currentActivity, androidAppKey, "INTERSTITIAL");
                        }
                    }
                }
                
                // Register for interstitial callbacks
                RegisterInterstitialCallbacks();
                
                isInitialized = true;
                Debug.Log("[IronSource] Initialization complete");
                
                // Load first interstitial
                LoadInterstitial();
            }
            catch (Exception e)
            {
                Debug.LogError($"[IronSource] Initialization failed: {e.Message}");
            }
#elif UNITY_EDITOR
            Debug.Log("[IronSource] Running in Editor - SDK not initialized");
            isInitialized = true;
            // Simulate ad ready after short delay for testing
            Invoke(nameof(SimulateAdReady), 2f);
#else
            Debug.Log("[IronSource] Platform not supported - SDK not initialized");
#endif
        }

        private void SimulateAdReady()
        {
            isInterstitialReady = true;
            OnInterstitialAdReady?.Invoke();
            Debug.Log("[IronSource] [Editor] Simulated ad ready");
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
            Invoke(nameof(SimulateAdReady), 1f);
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
#endif
}

