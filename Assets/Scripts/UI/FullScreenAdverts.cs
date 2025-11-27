using BattleCruisers.Ads;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FullScreenAdverts : MonoBehaviour
{
    public DefaultAdvertController defaultAd;
    public Button closeButton;
    private SettingsManager settingsManager; // For fullscreen ads on premium :)
    private bool useAppLovin = true; // Toggle between AppLovin and default ads
    private bool isShowingAd = false;
    
    // Ad frequency control keys
    private const string AD_COUNTER_KEY = "AdCounterKey";
    private const string LAST_AD_TIME_KEY = "LastAdShowTime";

    // Start is called before the first frame update
    void Start()
    {
#if THIRD_PARTY_PUBLISHER
        StartPlatformSpecficAds();
#endif
        Button btn = closeButton.GetComponent<Button>();
        btn.onClick.AddListener(CloseAdvert);
        
        // Register AppLovin callbacks
        if (useAppLovin && AppLovinManager.Instance != null)
        {
            AppLovinManager.Instance.OnInterstitialAdReady += OnAppLovinAdReady;
            AppLovinManager.Instance.OnInterstitialAdClosed += OnAppLovinAdClosed;
            AppLovinManager.Instance.OnInterstitialAdShowFailed += OnAppLovinAdFailed;
        }
    }
    public void CloseAdvert()
    {
        isShowingAd = false;
        gameObject.SetActive(false);
    }

    public void OpenAdvert()
    {
        // Try to show AppLovin ad first (if available and not premium)
        if (useAppLovin && ShouldShowAds())
        {
            TryShowAppLovinAd();
        }
        else
        {
            // Fallback to default ad display
            defaultAd.UpdateImage();
            StartPlatformSpecficAds();
        }
    }
    
    private bool ShouldShowAds()
    {
        settingsManager = DataProvider.SettingsManager;
        
        // Don't show ads for premium users unless they have ads enabled in settings
        if (DataProvider.GameModel.PremiumEdition)
        {
            bool showForPremium = settingsManager != null && settingsManager.ShowAds;
            if (!showForPremium)
            {
                Debug.Log("[Ads] Skipped - Premium user with ads disabled");
            }
            return showForPremium;
        }
        
        // Get dynamic config (fallback to defaults if not available)
        int minLevel = AdConfigManager.Instance?.MinimumLevelForAds ?? 7;
        int levelsCompleted = DataProvider.GameModel.NumOfLevelsCompleted;
        
        // Minimum level requirement
        if (levelsCompleted < minLevel)
        {
            Debug.Log($"[Ads] Skipped - Only {levelsCompleted}/{minLevel} levels completed");
            LogAdDecision("skipped_min_level", levelsCompleted);
            return false;
        }
        
        // Get player-specific ad frequency (veteran vs new player)
        int frequency = AdConfigManager.Instance?.GetAdFrequencyForPlayer(levelsCompleted) ?? 3;
        float cooldown = AdConfigManager.Instance?.AdCooldownMinutes ?? 5f;
        
        // Time-based cooldown
        string lastAdTimeStr = PlayerPrefs.GetString(LAST_AD_TIME_KEY, "");
        if (!string.IsNullOrEmpty(lastAdTimeStr) && DateTime.TryParse(lastAdTimeStr, out DateTime lastAdTime))
        {
            double minutesSince = (DateTime.Now - lastAdTime).TotalMinutes;
            if (minutesSince < cooldown)
            {
                Debug.Log($"[Ads] Skipped - Cooldown active ({minutesSince:F1}/{cooldown} min)");
                LogAdDecision("skipped_cooldown", levelsCompleted);
                return false;
            }
        }
        
        // Battle frequency check
        int adCounter = PlayerPrefs.GetInt(AD_COUNTER_KEY, 0);
        adCounter++;
        
        if (adCounter < frequency)
        {
            PlayerPrefs.SetInt(AD_COUNTER_KEY, adCounter);
            PlayerPrefs.Save();
            Debug.Log($"[Ads] Skipped - Frequency counter ({adCounter}/{frequency})");
            LogAdDecision("skipped_frequency", levelsCompleted);
            return false;
        }
        
        // All checks passed - show ad
        PlayerPrefs.SetInt(AD_COUNTER_KEY, 0);
        PlayerPrefs.SetString(LAST_AD_TIME_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();
        
        bool isVeteran = AdConfigManager.Instance?.IsVeteranPlayer(levelsCompleted) ?? false;
        Debug.Log($"[Ads] Showing ad (levels: {levelsCompleted}, veteran: {isVeteran}, frequency: {frequency})");
        LogAdDecision("show", levelsCompleted);
        
        return true;
    }
    
    /// <summary>
    /// Log ad decision to Firebase for analysis
    /// </summary>
    private void LogAdDecision(string decision, int levelsCompleted)
    {
        if (FirebaseAnalyticsManager.Instance != null)
        {
            var configSnapshot = AdConfigManager.Instance?.GetConfigSnapshot() 
                ?? new System.Collections.Generic.Dictionary<string, object>();
            
            FirebaseAnalyticsManager.Instance.LogEvent("ad_decision", new System.Collections.Generic.Dictionary<string, object>
            {
                { "decision", decision },
                { "levels_completed", levelsCompleted },
                { "premium", DataProvider.GameModel.PremiumEdition },
                { "min_level_config", configSnapshot.ContainsKey("minimum_level") ? configSnapshot["minimum_level"] : 7 },
                { "frequency_config", configSnapshot.ContainsKey("ad_frequency") ? configSnapshot["ad_frequency"] : 3 }
            });
        }
    }
    
    private void TryShowAppLovinAd()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if (AppLovinManager.Instance != null && AppLovinManager.Instance.IsInterstitialReady())
        {
            Debug.Log("[FullScreenAdverts] Showing AppLovin interstitial ad");
            isShowingAd = true;
            
            // Hide the UI panel since AppLovin will show fullscreen
            gameObject.SetActive(false);
            
            // Show the ad
            AppLovinManager.Instance.ShowInterstitial();
            
            // Track ad impression with Firebase
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogAdImpression("applovin", "interstitial");
            }
        }
        else
        {
            Debug.LogWarning("[FullScreenAdverts] AppLovin ad not ready, showing default ad");
            ShowDefaultAd();
        }
#else
        ShowDefaultAd();
#endif
    }
    
    private void ShowDefaultAd()
    {
        defaultAd.UpdateImage();
        StartPlatformSpecficAds();
    }
    
    // AppLovin callbacks
    private void OnAppLovinAdReady()
    {
        Debug.Log("[FullScreenAdverts] AppLovin ad ready");
    }
    
    private void OnAppLovinAdClosed()
    {
        Debug.Log("[FullScreenAdverts] AppLovin ad closed");
        isShowingAd = false;
        
        // Track ad completion
        if (FirebaseAnalyticsManager.Instance != null)
        {
            FirebaseAnalyticsManager.Instance.LogAdClosed("applovin", "interstitial");
        }
        
        // Resume music if needed
        if (LandingSceneGod.MusicPlayer != null)
        {
            // Music will resume automatically
        }
    }
    
    private void OnAppLovinAdFailed()
    {
        Debug.LogWarning("[FullScreenAdverts] AppLovin ad failed to show, showing default ad");
        isShowingAd = false;
        ShowDefaultAd();
    }

    void StartPlatformSpecficAds()
    {
        settingsManager = DataProvider.SettingsManager;
#if THIRD_PARTY_PUBLISHER
        gameObject.SetActive(false);
#endif
        // #if FREE_EDITION && (UNITY_ANDROID || UNITY_IOS)
#if UNITY_ANDROID || UNITY_IOS
        if (!DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
            LandingSceneGod.MusicPlayer.PlayAdsMusic();
        }
        else
        {
            Assert.IsNotNull(settingsManager);
            // For premium users, show ads only if ShowAds setting is enabled
            gameObject.SetActive(settingsManager.ShowAds);
        }
// #elif UNITY_EDITOR && FREE_EDITION
#elif UNITY_EDITOR
        if (!DataProvider.GameModel.PremiumEdition)
        {
            gameObject.SetActive(true);
            LandingSceneGod.MusicPlayer.PlayAdsMusic();
        }
        else
        {
            Assert.IsNotNull(settingsManager);
            // For premium users, show ads only if ShowAds setting is enabled
            gameObject.SetActive(settingsManager.ShowAds);
        }
#else
        Assert.IsNotNull(settingsManager);
        gameObject.SetActive(settingsManager.ShowAds);
    //gameObject.SetActive(false);
#endif
    }

    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }
    
    /// <summary>
    /// Reset ad counters for testing
    /// </summary>
    public void ResetAdCounters()
    {
        PlayerPrefs.DeleteKey(AD_COUNTER_KEY);
        PlayerPrefs.DeleteKey(LAST_AD_TIME_KEY);
        PlayerPrefs.Save();
        Debug.Log("[Ads] Counters reset");
    }
    
    /// <summary>
    /// Force show ad (for testing)
    /// </summary>
    public void ForceShowAd()
    {
        ResetAdCounters();
        OpenAdvert();
    }
    
    /// <summary>
    /// Get current ad counter status (for debugging)
    /// </summary>
    public string GetAdCounterStatus()
    {
        int counter = PlayerPrefs.GetInt(AD_COUNTER_KEY, 0);
        string lastTime = PlayerPrefs.GetString(LAST_AD_TIME_KEY, "Never");
        int frequency = AdConfigManager.Instance?.GetAdFrequencyForPlayer(DataProvider.GameModel.NumOfLevelsCompleted) ?? 3;
        
        return $"Counter: {counter}/{frequency}, Last: {lastTime}";
    }
    
    private void OnDestroy()
    {
        // Unregister AppLovin callbacks
        if (useAppLovin && AppLovinManager.Instance != null)
        {
            AppLovinManager.Instance.OnInterstitialAdReady -= OnAppLovinAdReady;
            AppLovinManager.Instance.OnInterstitialAdClosed -= OnAppLovinAdClosed;
            AppLovinManager.Instance.OnInterstitialAdShowFailed -= OnAppLovinAdFailed;
        }
    }
}
