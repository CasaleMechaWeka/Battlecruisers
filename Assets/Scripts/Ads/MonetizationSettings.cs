using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// ScriptableObject for storing ad monetization configuration
    /// Create via: Assets -> Create -> BattleCruisers -> Monetization Settings
    /// </summary>
    [CreateAssetMenu(fileName = "MonetizationSettings", menuName = "BattleCruisers/Monetization Settings", order = 1)]
    public class MonetizationSettings : ScriptableObject
    {
        [Header("AppLovin MAX Configuration")]
        [Tooltip("AppLovin SDK Key from dashboard")]
        public string appLovinSdkKey = "G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0";
        
        [Tooltip("Enable AppLovin MAX ads")]
        public bool enableAppLovinAds = true;
        
        [Tooltip("Enable verbose logging for debugging")]
        public bool verboseLogging = true;
        
        [Header("Android Ad Unit IDs")]
        [Tooltip("Interstitial ad unit ID for Android")]
        public string androidInterstitialAdUnitId = "9375d1dbeb211048";
        
        [Tooltip("Rewarded ad unit ID for Android")]
        public string androidRewardedAdUnitId = "c96bd6d70b3804fa";
        
        [Header("iOS Ad Unit IDs (Not Yet Used)")]
        [Tooltip("Interstitial ad unit ID for iOS")]
        public string iosInterstitialAdUnitId = "";
        
        [Tooltip("Rewarded ad unit ID for iOS")]
        public string iosRewardedAdUnitId = "";
        
        [Header("GDPR Compliance")]
        [Tooltip("Require user consent before showing ads (GDPR)")]
        public bool requireGDPRConsent = true;
        
        [Tooltip("Test mode - use AppLovin test ads")]
        public bool testMode = false;
        
        /// <summary>
        /// Get the appropriate interstitial ad unit ID for current platform
        /// </summary>
        public string GetInterstitialAdUnitId()
        {
#if UNITY_ANDROID
            return androidInterstitialAdUnitId;
#elif UNITY_IOS
            return iosInterstitialAdUnitId;
#else
            return "";
#endif
        }
        
        /// <summary>
        /// Get the appropriate rewarded ad unit ID for current platform
        /// </summary>
        public string GetRewardedAdUnitId()
        {
#if UNITY_ANDROID
            return androidRewardedAdUnitId;
#elif UNITY_IOS
            return iosRewardedAdUnitId;
#else
            return "";
#endif
        }
        
        /// <summary>
        /// Validate configuration
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(appLovinSdkKey))
            {
                Debug.LogError("[MonetizationSettings] Invalid SDK Key! Set it in the ScriptableObject.");
                return false;
            }
            
#if UNITY_ANDROID
            if (string.IsNullOrEmpty(androidInterstitialAdUnitId))
            {
                Debug.LogWarning("[MonetizationSettings] Android Interstitial Ad Unit ID not set.");
            }
            
            if (string.IsNullOrEmpty(androidRewardedAdUnitId))
            {
                Debug.LogWarning("[MonetizationSettings] Android Rewarded Ad Unit ID not set.");
            }
#endif
            
            return true;
        }
    }
}

