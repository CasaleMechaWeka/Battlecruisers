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

        // Current values (from Remote Config or defaults)
        public int MinimumLevelForAds { get; private set; }
        public int AdFrequency { get; private set; }
        public float AdCooldownMinutes { get; private set; }
        public bool VeteranFrequencyBoostEnabled { get; private set; }
        public int VeteranThreshold { get; private set; }
        public int VeteranAdFrequency { get; private set; }
        
        /// <summary>
        /// If true, ads are in PRODUCTION mode (real ads, real revenue).
        /// If false, ads are in TEST mode (test ads only).
        /// </summary>
        public bool AdsAreLive { get; private set; }
        
        /// <summary>
        /// If true, ALL ads are disabled (no test ads, no real ads).
        /// Use this for maintenance or to turn off ads entirely.
        /// </summary>
        public bool AdsDisabled { get; private set; }

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
            AdsAreLive = defaultAdsAreLive;
            AdsDisabled = defaultAdsDisabled;

            Debug.Log($"[AdConfig] Using defaults: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}, Cooldown={AdCooldownMinutes}, AdsAreLive={AdsAreLive}, AdsDisabled={AdsDisabled}");
        }

        /// <summary>
        /// Fetch configuration from Unity Remote Config as single JSON (AD_CONFIG)
        /// Matches the pattern used by GAME_CONFIG, SHOP_CONFIG, etc.
        /// </summary>
        private async Task FetchRemoteConfigAsync()
        {
            try
            {
                Debug.Log("[AdConfig] Fetching AD_CONFIG from Unity Remote Config...");

                // Check if Unity Services are initialized
                if (Unity.Services.Core.UnityServices.State != Unity.Services.Core.ServicesInitializationState.Initialized)
                {
                    Debug.LogWarning("[AdConfig] Unity Services not initialized, using defaults");
                    return;
                }

                // Fetch remote config (if not already fetched by DataProvider)
                if (RemoteConfigService.Instance.appConfig.config.Count == 0)
                {
                    Debug.Log("[AdConfig] Fetching remote config data...");
                    await RemoteConfigService.Instance.FetchConfigsAsync(new BattleCruisers.Data.UserAttributes(), new BattleCruisers.Data.AppAttributes());
                }

                // Get AD_CONFIG as JSON (single key, like GAME_CONFIG pattern)
                var adConfigJson = RemoteConfigService.Instance.appConfig.GetJson("AD_CONFIG");
                
                if (!string.IsNullOrEmpty(adConfigJson) && adConfigJson != "{}")
                {
                    Debug.Log($"[AdConfig] Fetched AD_CONFIG: {adConfigJson}");
                    
                    // Parse JSON to struct
                    AdConfig adConfig = JsonUtility.FromJson<AdConfig>(adConfigJson);
                    
                    // Apply values from Remote Config
                    MinimumLevelForAds = adConfig.ad_minimum_level > 0 ? adConfig.ad_minimum_level : defaultMinimumLevelForAds;
                    AdFrequency = adConfig.ad_frequency > 0 ? adConfig.ad_frequency : defaultAdFrequency;
                    AdCooldownMinutes = adConfig.ad_cooldown_minutes > 0 ? adConfig.ad_cooldown_minutes : defaultAdCooldownMinutes;
                    VeteranFrequencyBoostEnabled = adConfig.ad_veteran_boost_enabled;
                    VeteranThreshold = adConfig.ad_veteran_threshold > 0 ? adConfig.ad_veteran_threshold : veteranThreshold;
                    VeteranAdFrequency = adConfig.ad_veteran_frequency > 0 ? adConfig.ad_veteran_frequency : veteranAdFrequency;
                    AdsAreLive = adConfig.ads_are_live;
                    AdsDisabled = adConfig.ads_disabled;

                    isRemoteConfigFetched = true;

                    Debug.Log($"[AdConfig] AD_CONFIG parsed successfully!");
                    Debug.Log($"[AdConfig] Ad Settings: MinLevel={MinimumLevelForAds}, Frequency={AdFrequency}, Cooldown={AdCooldownMinutes}min");
                    Debug.Log($"[AdConfig] Ad Mode: AdsAreLive={AdsAreLive} (production={AdsAreLive}), AdsDisabled={AdsDisabled}");
                    Debug.Log($"[AdConfig] Veteran Settings: Boost={VeteranFrequencyBoostEnabled}, Threshold={VeteranThreshold}, Frequency={VeteranAdFrequency}");
                }
                else
                {
                    Debug.LogWarning("[AdConfig] AD_CONFIG not found in Unity Remote Config, using defaults");
                    Debug.LogWarning("[AdConfig] Add key 'AD_CONFIG' (type: json) to Unity Remote Config dashboard");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdConfig] Failed to fetch/parse AD_CONFIG: {e.Message}");
                Debug.LogWarning("[AdConfig] Using default values");
            }
        }

        /// <summary>
        /// Check if ads should be shown at all
        /// </summary>
        public bool ShouldShowAds()
        {
            if (AdsDisabled)
            {
                Debug.Log("[AdConfig] Ads are DISABLED via Remote Config");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if we're in test mode (for AppLovin test ads)
        /// </summary>
        public bool IsTestMode()
        {
            return !AdsAreLive;
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
            Debug.Log("[AdConfig] Force refreshing AD_CONFIG...");
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
                { "ads_are_live", AdsAreLive },
                { "ads_disabled", AdsDisabled },
                { "is_test_mode", IsTestMode() },
                { "remote_config_fetched", isRemoteConfigFetched },
                { "config_source", "Unity Remote Config (AD_CONFIG)" }
            };
        }

        /// <summary>
        /// Get a formatted string of current ad status for display
        /// </summary>
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
