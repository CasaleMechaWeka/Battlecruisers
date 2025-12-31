using System;
using System.Threading.Tasks;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Interface for ad mediation managers (AppLovin MAX, etc.)
    /// Provides abstraction for interstitial and rewarded video ads
    /// Note: Currently unused - AppLovinManager doesn't implement this interface
    /// </summary>
    public interface IMediationManager
    {
        // Lifecycle
        Task InitializeAsync(string sdkKey);
        bool IsInitialized { get; }
        
        // Interstitial Ads
        bool IsInterstitialReady(string adUnitId = null);
        void ShowInterstitial(string adUnitId = null);
        void LoadInterstitial(string adUnitId = null);
        
        // Rewarded Ads
        bool IsRewardedAdReady(string adUnitId = null);
        void ShowRewardedAd(string adUnitId = null);
        void LoadRewardedAd(string adUnitId = null);
        
        // Interstitial Events
        event Action OnInterstitialAdReady;
        event Action OnInterstitialAdLoadFailed;
        event Action OnInterstitialAdShown;
        event Action OnInterstitialAdClosed;
        event Action OnInterstitialAdShowFailed;
        event Action OnInterstitialAdClicked;
        
        // Rewarded Ad Events
        event Action OnRewardedAdReady;
        event Action OnRewardedAdLoadFailed;
        event Action OnRewardedAdShown;
        event Action OnRewardedAdRewarded; // User earned the reward
        event Action OnRewardedAdClosed;
        event Action OnRewardedAdShowFailed;
    }
}

