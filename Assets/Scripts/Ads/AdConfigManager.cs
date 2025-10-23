using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages ad configuration with Firebase Remote Config for A/B testing
    /// Falls back to local defaults if Firebase is unavailable
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
        public float defaultAdCooldownMinutes = 5f;
        
        [Tooltip("Enable different ad frequency for new vs veteran players")]
        public bool enableVeteranFrequencyBoost = true;
        
        [Tooltip("Levels required to be considered 'veteran' player")]
        public int veteranThreshold = 15;
        
        [Tooltip("Ad frequency for veteran players (lower = more ads)")]
        public int veteranAdFrequency = 2;

        // Current values (from Remote Config or defaults)
        public int MinimumLevelForAds { get; private set; }
        public int AdFrequency { get; private set; }
        public float AdCooldownMinutes { get; private set; }
        public bool VeteranFrequencyBoostEnabled { get; private set; }
        public int VeteranThreshold { get; private set; }
        public int VeteranAdFrequency { get; private set; }

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

            // Set defaults immediately
            ApplyDefaults();
        }

        private async void Start()
        {
            await FetchRemoteConfigAsync();
        }

        /// <summary>
        /// Apply default configuration values
        /// </summary>
        private void ApplyDefaults()
        {
            MinimumLevelForAds = defaultMinimumLevelForAds;
            AdFrequency = defaultAdFrequency;
            AdCooldownMinutes = defaultAdCooldownMinutes;
            VeteranFrequencyBoostEnabled = enableVeteranFrequencyBoost;
            VeteranThreshold = veteranThreshold;
            VeteranAdFrequency = veteranAdFrequency;

            Debug.Log($"[AdConfig] Using defaults: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}, Cooldown={AdCooldownMinutes}");
        }

        /// <summary>
        /// Fetch configuration from Firebase Remote Config
        /// </summary>
        private async Task FetchRemoteConfigAsync()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[AdConfig] Fetching Remote Config from Firebase...");
                
                // Get Firebase Remote Config instance via Android JNI
                using (AndroidJavaClass firebaseRemoteConfig = new AndroidJavaClass("com.google.firebase.remoteconfig.FirebaseRemoteConfig"))
                using (AndroidJavaObject remoteConfigInstance = firebaseRemoteConfig.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    // Set default values
                    using (AndroidJavaObject defaultsMap = new AndroidJavaObject("java.util.HashMap"))
                    {
                        defaultsMap.Call<AndroidJavaObject>("put", "minimum_level_for_ads", new AndroidJavaObject("java.lang.Long", (long)defaultMinimumLevelForAds));
                        defaultsMap.Call<AndroidJavaObject>("put", "ad_frequency", new AndroidJavaObject("java.lang.Long", (long)defaultAdFrequency));
                        defaultsMap.Call<AndroidJavaObject>("put", "ad_cooldown_minutes", new AndroidJavaObject("java.lang.Double", (double)defaultAdCooldownMinutes));
                        defaultsMap.Call<AndroidJavaObject>("put", "veteran_frequency_boost_enabled", new AndroidJavaObject("java.lang.Boolean", enableVeteranFrequencyBoost));
                        defaultsMap.Call<AndroidJavaObject>("put", "veteran_threshold", new AndroidJavaObject("java.lang.Long", (long)veteranThreshold));
                        defaultsMap.Call<AndroidJavaObject>("put", "veteran_ad_frequency", new AndroidJavaObject("java.lang.Long", (long)veteranAdFrequency));
                        
                        remoteConfigInstance.Call("setDefaultsAsync", defaultsMap);
                    }

                    // Fetch and activate
                    // Set cache expiration to 1 hour for production, 0 for testing
                    long cacheExpiration = 3600; // 1 hour
                    using (AndroidJavaObject fetchTask = remoteConfigInstance.Call<AndroidJavaObject>("fetch", cacheExpiration))
                    {
                        // Wait for fetch to complete (simplified - in production use proper async)
                        await Task.Delay(2000);
                    }

                    // Activate fetched values
                    bool activated = remoteConfigInstance.Call<bool>("activate");
                    
                    if (activated)
                    {
                        // Get values
                        MinimumLevelForAds = (int)remoteConfigInstance.Call<AndroidJavaObject>("getLong", "minimum_level_for_ads").Call<long>("longValue");
                        AdFrequency = (int)remoteConfigInstance.Call<AndroidJavaObject>("getLong", "ad_frequency").Call<long>("longValue");
                        AdCooldownMinutes = (float)remoteConfigInstance.Call<AndroidJavaObject>("getDouble", "ad_cooldown_minutes").Call<double>("doubleValue");
                        VeteranFrequencyBoostEnabled = remoteConfigInstance.Call<AndroidJavaObject>("getBoolean", "veteran_frequency_boost_enabled").Call<bool>("booleanValue");
                        VeteranThreshold = (int)remoteConfigInstance.Call<AndroidJavaObject>("getLong", "veteran_threshold").Call<long>("longValue");
                        VeteranAdFrequency = (int)remoteConfigInstance.Call<AndroidJavaObject>("getLong", "veteran_ad_frequency").Call<long>("longValue");

                        isRemoteConfigFetched = true;
                        
                        Debug.Log($"[AdConfig] Remote Config fetched: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}, Cooldown={AdCooldownMinutes}, VeteranBoost={VeteranFrequencyBoostEnabled}");
                    }
                    else
                    {
                        Debug.LogWarning("[AdConfig] Remote Config activation failed, using defaults");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdConfig] Failed to fetch Remote Config: {e.Message}");
                Debug.LogWarning("[AdConfig] Using default values");
            }
#elif UNITY_EDITOR
            // Simulate remote config fetch in Editor
            Debug.Log("[AdConfig] [Editor] Simulating Remote Config fetch...");
            await Task.Delay(500);
            
            // Check if we have editor overrides in PlayerPrefs
            if (PlayerPrefs.HasKey("EditorAdConfig_MinLevel"))
            {
                MinimumLevelForAds = PlayerPrefs.GetInt("EditorAdConfig_MinLevel", defaultMinimumLevelForAds);
                AdFrequency = PlayerPrefs.GetInt("EditorAdConfig_Frequency", defaultAdFrequency);
                AdCooldownMinutes = PlayerPrefs.GetFloat("EditorAdConfig_Cooldown", defaultAdCooldownMinutes);
                Debug.Log($"[AdConfig] [Editor] Using editor overrides: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}");
            }
            else
            {
                Debug.Log($"[AdConfig] [Editor] Using defaults (set overrides via PlayerPrefs)");
            }
            
            isRemoteConfigFetched = true;
#else
            Debug.Log("[AdConfig] Platform not supported for Remote Config");
            await Task.CompletedTask;
#endif
        }

        /// <summary>
        /// Get ad frequency based on player progression (veteran vs new player)
        /// </summary>
        public int GetAdFrequencyForPlayer(int levelsCompleted)
        {
            if (VeteranFrequencyBoostEnabled && levelsCompleted >= VeteranThreshold)
            {
                return VeteranAdFrequency;
            }
            return AdFrequency;
        }

        /// <summary>
        /// Check if player is considered a veteran
        /// </summary>
        public bool IsVeteranPlayer(int levelsCompleted)
        {
            return levelsCompleted >= VeteranThreshold;
        }

        /// <summary>
        /// Force refresh remote config (for testing)
        /// </summary>
        public async Task RefreshConfigAsync()
        {
            Debug.Log("[AdConfig] Force refreshing config...");
            await FetchRemoteConfigAsync();
        }

        /// <summary>
        /// Get current config as dictionary for logging
        /// </summary>
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
                { "remote_config_fetched", isRemoteConfigFetched }
            };
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

