using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Manages Firebase Analytics for tracking player behavior and churn
    /// </summary>
    public class FirebaseAnalyticsManager : MonoBehaviour
    {
        public static FirebaseAnalyticsManager Instance { get; private set; }

        private bool isInitialized = false;
        private DateTime sessionStartTime;
        private int sessionEventCount = 0;

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
            InitializeFirebase();
        }

        /// <summary>
        /// Initialize Firebase Analytics
        /// </summary>
        public void InitializeFirebase()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[Firebase] Initializing Firebase Analytics...");
                
                // Get Firebase Analytics instance
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaClass firebaseAnalytics = new AndroidJavaClass("com.google.firebase.analytics.FirebaseAnalytics"))
                {
                    AndroidJavaObject analyticsInstance = firebaseAnalytics.CallStatic<AndroidJavaObject>("getInstance", currentActivity);
                    
                    // Enable analytics collection
                    analyticsInstance.Call("setAnalyticsCollectionEnabled", true);
                    
                    Debug.Log("[Firebase] Firebase Analytics initialized successfully");
                }
                
                isInitialized = true;
                
                // Log session start
                sessionStartTime = DateTime.Now;
                LogSessionStart();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Firebase] Initialization failed: {e.Message}");
            }
#elif UNITY_EDITOR
            Debug.Log("[Firebase] Running in Editor - Analytics simulated");
            isInitialized = true;
            sessionStartTime = DateTime.Now;
            LogSessionStart();
#else
            Debug.Log("[Firebase] Platform not supported - Analytics not initialized");
#endif
        }

        /// <summary>
        /// Log custom event with parameters
        /// </summary>
        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"[Firebase] Cannot log event '{eventName}' - not initialized");
                return;
            }

            sessionEventCount++;

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaClass firebaseAnalytics = new AndroidJavaClass("com.google.firebase.analytics.FirebaseAnalytics"))
                {
                    AndroidJavaObject analyticsInstance = firebaseAnalytics.CallStatic<AndroidJavaObject>("getInstance", currentActivity);
                    
                    if (parameters != null && parameters.Count > 0)
                    {
                        using (AndroidJavaObject bundle = new AndroidJavaObject("android.os.Bundle"))
                        {
                            foreach (var param in parameters)
                            {
                                if (param.Value is string strValue)
                                {
                                    bundle.Call("putString", param.Key, strValue);
                                }
                                else if (param.Value is int intValue)
                                {
                                    bundle.Call("putInt", param.Key, intValue);
                                }
                                else if (param.Value is long longValue)
                                {
                                    bundle.Call("putLong", param.Key, longValue);
                                }
                                else if (param.Value is float floatValue)
                                {
                                    bundle.Call("putFloat", param.Key, floatValue);
                                }
                                else if (param.Value is double doubleValue)
                                {
                                    bundle.Call("putDouble", param.Key, doubleValue);
                                }
                                else if (param.Value is bool boolValue)
                                {
                                    bundle.Call("putBoolean", param.Key, boolValue);
                                }
                            }
                            
                            analyticsInstance.Call("logEvent", eventName, bundle);
                        }
                    }
                    else
                    {
                        analyticsInstance.Call("logEvent", eventName, null);
                    }
                }
                
                Debug.Log($"[Firebase] Event logged: {eventName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Firebase] Failed to log event '{eventName}': {e.Message}");
            }
#elif UNITY_EDITOR
            string paramStr = parameters != null ? $" with {parameters.Count} params" : "";
            Debug.Log($"[Firebase] [Editor] Event: {eventName}{paramStr}");
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    Debug.Log($"  - {param.Key}: {param.Value}");
                }
            }
#endif
        }

        /// <summary>
        /// Set user property for segmentation
        /// </summary>
        public void SetUserProperty(string propertyName, string value)
        {
            if (!isInitialized) return;

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaClass firebaseAnalytics = new AndroidJavaClass("com.google.firebase.analytics.FirebaseAnalytics"))
                {
                    AndroidJavaObject analyticsInstance = firebaseAnalytics.CallStatic<AndroidJavaObject>("getInstance", currentActivity);
                    analyticsInstance.Call("setUserProperty", propertyName, value);
                }
                
                Debug.Log($"[Firebase] User property set: {propertyName} = {value}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Firebase] Failed to set user property: {e.Message}");
            }
#elif UNITY_EDITOR
            Debug.Log($"[Firebase] [Editor] User Property: {propertyName} = {value}");
#endif
        }

        #region Session Events

        public void LogSessionStart()
        {
            // Note: "session_start" is a reserved Firebase event name, use custom name
            LogEvent("game_session_begin", new Dictionary<string, object>
            {
                { "platform", Application.platform.ToString() },
                { "app_version", Application.version },
                { "unity_version", Application.unityVersion }
            });
        }

        public void LogSessionEnd()
        {
            var sessionLength = (DateTime.Now - sessionStartTime).TotalSeconds;
            
            LogEvent("session_end", new Dictionary<string, object>
            {
                { "session_length_seconds", (long)sessionLength },
                { "events_in_session", sessionEventCount }
            });
        }

        #endregion

        #region Level Events (Key for Churn Analysis)

        public void LogLevelStart(string levelId, int levelNumber, string gameMode, int attemptNumber = 1)
        {
            LogEvent("level_start", new Dictionary<string, object>
            {
                { "level_id", levelId },
                { "level_number", levelNumber },
                { "game_mode", gameMode },
                { "attempt_number", attemptNumber }
            });
        }

        public void LogLevelComplete(string levelId, int levelNumber, string gameMode, float timeSpent, int score)
        {
            LogEvent("level_complete", new Dictionary<string, object>
            {
                { "level_id", levelId },
                { "level_number", levelNumber },
                { "game_mode", gameMode },
                { "time_spent_seconds", timeSpent },
                { "score", score },
                { "success", true }
            });
        }

        public void LogLevelFail(string levelId, int levelNumber, string gameMode, float timeSpent, string failReason)
        {
            LogEvent("level_fail", new Dictionary<string, object>
            {
                { "level_id", levelId },
                { "level_number", levelNumber },
                { "game_mode", gameMode },
                { "time_spent_seconds", timeSpent },
                { "fail_reason", failReason }
            });
        }

        #endregion

        #region Monetization Events

        public void LogIAPAttempt(string productId, string productType, double price, string currency)
        {
            LogEvent("iap_attempt", new Dictionary<string, object>
            {
                { "product_id", productId },
                { "product_type", productType },
                { "price", price },
                { "currency", currency }
            });
        }

        public void LogIAPSuccess(string productId, string productType, double price, string currency)
        {
            LogEvent("in_app_purchase", new Dictionary<string, object>
            {
                { "product_id", productId },
                { "product_type", productType },
                { "value", price },
                { "currency", currency }
            });
        }

        public void LogIAPFailed(string productId, string reason)
        {
            LogEvent("iap_failed", new Dictionary<string, object>
            {
                { "product_id", productId },
                { "fail_reason", reason }
            });
        }

        #endregion

        #region Ad Events

        public void LogAdImpression(string adSource, string adType, string placement = "")
        {
            LogEvent("ad_impression", new Dictionary<string, object>
            {
                { "ad_platform", adSource },
                { "ad_type", adType },
                { "ad_placement", placement }
            });
        }

        public void LogAdClosed(string adSource, string adType, bool completed = true)
        {
            LogEvent("ad_closed", new Dictionary<string, object>
            {
                { "ad_platform", adSource },
                { "ad_type", adType },
                { "completed", completed }
            });
        }

        public void LogAdClicked(string adSource, string adType)
        {
            LogEvent("ad_clicked", new Dictionary<string, object>
            {
                { "ad_platform", adSource },
                { "ad_type", adType }
            });
        }

        public void LogRewardedAdOffered(string placement, int coinsReward, int creditsReward)
        {
            LogEvent("rewarded_ad_offered", new Dictionary<string, object>
            {
                { "placement", placement },
                { "coins_reward", coinsReward },
                { "credits_reward", creditsReward }
            });
        }

        public void LogRewardedAdStarted(string placement)
        {
            LogEvent("rewarded_ad_started", new Dictionary<string, object>
            {
                { "placement", placement },
                { "ad_platform", "applovin" },
                { "ad_type", "rewarded" }
            });
        }

        public void LogRewardedAdCompleted(string placement, int coinsEarned, int creditsEarned)
        {
            LogEvent("rewarded_ad_completed", new Dictionary<string, object>
            {
                { "placement", placement },
                { "ad_platform", "applovin" },
                { "ad_type", "rewarded" },
                { "coins_earned", coinsEarned },
                { "credits_earned", creditsEarned },
                { "completed", true }
            });
        }

        public void LogRewardedAdSkipped(string placement)
        {
            LogEvent("rewarded_ad_skipped", new Dictionary<string, object>
            {
                { "placement", placement },
                { "completed", false }
            });
        }

        #endregion

        #region Player Progress (Churn Indicators)

        public void LogPlayerProgression(int currentLevel, int totalLevelsCompleted, int totalPlayTime, bool isPremium)
        {
            LogEvent("player_progression", new Dictionary<string, object>
            {
                { "current_level", currentLevel },
                { "total_levels_completed", totalLevelsCompleted },
                { "total_play_time_minutes", totalPlayTime },
                { "is_premium", isPremium }
            });
            
            // Set as user properties for segmentation
            SetUserProperty("current_level", currentLevel.ToString());
            SetUserProperty("is_premium", isPremium.ToString());
        }

        public void LogTutorialComplete(string tutorialId)
        {
            LogEvent("tutorial_complete", new Dictionary<string, object>
            {
                { "tutorial_id", tutorialId }
            });
        }

        public void LogTutorialSkip(string tutorialId)
        {
            LogEvent("tutorial_skip", new Dictionary<string, object>
            {
                { "tutorial_id", tutorialId }
            });
        }

        public void LogUnlockAchievement(string achievementId, string achievementType)
        {
            LogEvent("unlock_achievement", new Dictionary<string, object>
            {
                { "achievement_id", achievementId },
                { "achievement_type", achievementType }
            });
        }

        #endregion

        #region Economy Events

        public void LogEarnVirtualCurrency(string currencyName, int amount, string source)
        {
            LogEvent("earn_virtual_currency", new Dictionary<string, object>
            {
                { "virtual_currency_name", currencyName },
                { "value", amount },
                { "source", source }
            });
        }

        public void LogSpendVirtualCurrency(string currencyName, int amount, string itemName)
        {
            LogEvent("spend_virtual_currency", new Dictionary<string, object>
            {
                { "virtual_currency_name", currencyName },
                { "value", amount },
                { "item_name", itemName }
            });
        }

        #endregion

        #region UI/Menu Events

        public void LogScreenView(string screenName, string screenClass = "")
        {
            LogEvent("screen_view", new Dictionary<string, object>
            {
                { "screen_name", screenName },
                { "screen_class", string.IsNullOrEmpty(screenClass) ? screenName : screenClass }
            });
        }

        public void LogButtonClick(string buttonName, string screenName)
        {
            LogEvent("button_click", new Dictionary<string, object>
            {
                { "button_name", buttonName },
                { "screen_name", screenName }
            });
        }

        #endregion

        #region Engagement/Retention Events

        public void LogDailyLogin(int daysSinceInstall, int consecutiveDays)
        {
            LogEvent("daily_login", new Dictionary<string, object>
            {
                { "days_since_install", daysSinceInstall },
                { "consecutive_days", consecutiveDays }
            });
        }

        public void LogReturnUser(int daysSinceLastSession)
        {
            LogEvent("user_return", new Dictionary<string, object>
            {
                { "days_since_last_session", daysSinceLastSession }
            });
            
            // This is a key churn indicator
            if (daysSinceLastSession >= 7)
            {
                SetUserProperty("user_category", "returning_after_week");
            }
            else if (daysSinceLastSession >= 3)
            {
                SetUserProperty("user_category", "returning_casual");
            }
            else
            {
                SetUserProperty("user_category", "active");
            }
        }

        #endregion

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                LogSessionEnd();
            }
            else
            {
                sessionStartTime = DateTime.Now;
                sessionEventCount = 0;
                LogSessionStart();
            }
        }

        private void OnApplicationQuit()
        {
            LogSessionEnd();
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

