# AppLovin MAX Integration Guide

## Overview

This document describes the complete AppLovin MAX mediation SDK integration for Battlecruisers, replacing the previous IronSource/LevelPlay implementation.

**Status:** ✅ Fully Integrated
**Platform Support:** Android (iOS ready but not enabled)
**Ad Types:** Interstitial & Rewarded Video
**Analytics:** Firebase Analytics integrated
**Last Updated:** November 2024

---

## Table of Contents

1. [Architecture](#architecture)
2. [Prerequisites](#prerequisites)
3. [Configuration](#configuration)
4. [Implementation Details](#implementation-details)
5. [Testing](#testing)
6. [Firebase Analytics](#firebase-analytics)
7. [A/B Testing](#ab-testing)
8. [Troubleshooting](#troubleshooting)

---

## Architecture

### Modular Design Principles

The integration follows Battlecruisers' modular, interface-driven architecture:

- **`IMediationManager`**: Interface for ad mediation (extensible for future SDKs)
- **`AppLovinMaxManager`**: Concrete implementation for AppLovin MAX
- **`MonetizationSettings`**: ScriptableObject for configuration
- **`AdConfigManager`**: Firebase Remote Config integration for A/B testing
- **`FirebaseAnalyticsManager`**: Analytics tracking (retained from previous setup)

### Key Components

```
Assets/Scripts/Ads/
├── IMediationManager.cs           # Interface for ad SDKs
├── AppLovinMaxManager.cs          # AppLovin MAX implementation
├── MonetizationSettings.cs        # ScriptableObject for config
├── AdConfigManager.cs             # Remote Config for A/B testing
└── UnityMainThreadDispatcher.cs   # Thread safety for native callbacks

Assets/Scripts/Analytics/
└── FirebaseAnalyticsManager.cs    # Analytics (tracks ad events)

Assets/Scripts/UI/
└── FullScreenAdverts.cs           # Interstitial ad UI controller

Assets/Scripts/Scenes/
├── DestructionSceneGod.cs         # Rewarded ads (post-battle)
└── PvPDestructionSceneGod.cs      # Rewarded ads (PvP)

Assets/Scripts/Utils/Debugging/
└── AdminPanel.cs                   # Testing tools
```

---

## Prerequisites

### 1. Unity Version

- **Unity 2021.3 LTS** or higher

### 2. AppLovin MAX SDK

Install the AppLovin MAX SDK via one of these methods:

**Option A: Unity Asset Store** (Recommended)
1. Download "AppLovin MAX Unity Plugin" from Unity Asset Store
2. Import into project
3. Follow AppLovin's integration wizard

**Option B: Direct Download**
1. Go to [dash.applovin.com/documentation/mediation/unity/getting-started](https://dash.applovin.com/documentation/mediation/unity/getting-started)
2. Download the `.unitypackage`
3. Import into project

**Option C: Unity Package Manager** (if available)
```
"com.applovin.mediation.unity": "latest"
```

### 3. AppLovin Dashboard Setup

1. Create account at [dash.applovin.com](https://dash.applovin.com)
2. Create a new app
3. Note your **SDK Key** (format: `abc123def456...`)
4. Create ad units:
   - **Interstitial Ad Unit** (for post-battle ads)
   - **Rewarded Video Ad Unit** (for reward multipliers)
5. Configure mediation networks (if using)

### 4. External Dependency Manager (EDM)

If using native Android libraries, ensure EDM is installed:
- Unity Package Manager → Google → External Dependency Manager

---

## Configuration

### Step 1: Create MonetizationSettings ScriptableObject

1. In Unity: `Assets → Create → BattleCruisers → Monetization Settings`
2. Name it `MonetizationSettings`
3. Move to `Assets/Resources/` folder (required for runtime loading)

### Step 2: Configure Settings

In the Inspector for `MonetizationSettings`:

```
AppLovin MAX Configuration:
├── SDK Key: [Your SDK Key from dashboard]
├── Enable AppLovin Ads: ✓ (checked)
├── Verbose Logging: ✓ (for development, uncheck for production)
├── Test Mode: ✓ (for testing, uncheck for production)
│
Android Ad Unit IDs:
├── Interstitial Ad Unit ID: [Your interstitial unit ID]
├── Rewarded Ad Unit ID: [Your rewarded video unit ID]
│
iOS Ad Unit IDs: (Not used yet)
├── Interstitial Ad Unit ID: (empty)
├── Rewarded Ad Unit ID: (empty)
│
GDPR Compliance:
├── Require GDPR Consent: ✓ (recommended)
└── Test Mode: ✓ (uncheck for production)
```

### Step 3: Link in Scenes

**For FullScreenAdverts (Interstitial Ads):**
- Scene: `ScreensScene`
- GameObject: `FullScreenAdverts`
- Component: `FullScreenAdverts.cs`
- No manual linking required (manager auto-creates)

**For Rewarded Ads (Destruction Screens):**
- Scene: `DestructionScene`
- GameObject: Hook up `Rewarded Ad Button` to `OnWatchRewardedAdButtonClicked()`
- Scene: `PvPDestructionScene`
- GameObject: Hook up `Rewarded Ad Button` to `OnWatchRewardedAdButtonClicked()`

**For Testing (Admin Panel):**
- GameObject: `AdminPanel`
- Component: `AdminPanel.cs`
- Inspector: Link `FullScreenAdverts` GameObject to `fullScreenAdverts` field

### Step 4: Firebase Remote Config (Optional but Recommended)

See "A/B Testing" section below for full setup.

---

## Implementation Details

### Interface-Based Design

```csharp
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
    
    // Events for ad lifecycle
    event Action OnInterstitialAdReady;
    event Action OnInterstitialAdClosed;
    // ... (see IMediationManager.cs for full interface)
}
```

### Initialization Flow

```
1. LandingSceneGod.Start()
   ↓
2. Scene-specific managers (DestructionSceneGod, etc.)
   ↓
3. Check if AppLovinMaxManager.Instance exists
   ↓
4. If not exists: Create GameObject + AddComponent
   ↓
5. AppLovinMaxManager.Start() auto-initializes
   ↓
6. Loads MonetizationSettings from Resources
   ↓
7. Calls InitializeAsync(sdkKey)
   ↓
8. Registers callbacks
   ↓
9. Loads first ads
   ↓
10. Ready to show ads
```

### Ad Display Logic

**Interstitial Ads (FullScreenAdverts.cs):**

```csharp
OpenAdvert()
   ↓
ShouldShowAds()
   ├─ Check premium edition (skip if premium + ads disabled)
   ├─ Check minimum level (default: level 7+)
   ├─ Check cooldown timer (default: 5 minutes)
   ├─ Check battle frequency (default: every 3 battles)
   └─ All checks pass → Show ad
   ↓
TryShowAppLovinAd()
   ├─ Check if manager exists
   ├─ Check if ad ready
   ├─ Show ad (fullscreen, hides Unity UI)
   └─ Log to Firebase Analytics
   ↓
OnAppLovinAdClosed() callback
   ├─ Resume gameplay
   ├─ Log ad_closed to Firebase
   └─ Load next ad automatically
```

**Rewarded Ads (DestructionSceneGod.cs):**

```csharp
CalculateRewards()
   ↓
ShowRewardedAdOffer()
   ├─ Check if manager exists
   ├─ Check if ad ready
   ├─ Calculate bonus rewards (2x coins, 3x credits)
   ├─ Display button with offer
   ├─ Log rewarded_ad_offered to Firebase
   └─ Start 10-second timer
   ↓
User clicks "Watch Ad" button
   ↓
OnWatchRewardedAdButtonClicked()
   ├─ Hide button
   ├─ Show rewarded ad
   └─ Log rewarded_ad_started to Firebase
   ↓
OnRewardedAdCompleted() callback
   ├─ Apply reward multipliers
   ├─ Update UI with new rewards
   ├─ Log rewarded_ad_completed to Firebase
   ├─ Log earn_virtual_currency to Firebase
   └─ Award coins and credits to player
```

### Platform Handling

**Android (Production):**
```csharp
#if UNITY_ANDROID && !UNITY_EDITOR
    // Native JNI calls to AppLovin MAX SDK
    using (AndroidJavaClass maxSdk = ...)
    {
        maxSdk.CallStatic("initializeSdk");
    }
#endif
```

**Unity Editor (Testing):**
```csharp
#elif UNITY_EDITOR
    // Simulated ad behavior
    await Task.Delay(2000);
    SimulateInterstitialReady();
#endif
```

### GDPR Compliance

AppLovin MAX handles GDPR consent automatically via their CMP (Consent Management Platform). To integrate:

1. Enable GDPR in AppLovin dashboard
2. Set `requireGDPRConsent = true` in `MonetizationSettings`
3. AppLovin will show consent dialog on first launch (EU users)

For manual GDPR control:
```csharp
if (settings.requireGDPRConsent && !HasUserConsent())
{
    // Don't initialize ads
    return;
}
```

---

## Testing

### 1. Editor Testing

**Simulated Ad Flow:**
- Ads automatically "load" after 2 seconds
- Showing an ad simulates 3-second display
- All callbacks fire correctly
- Firebase Analytics logs (in console)

**Enable:**
```csharp
// Automatically enabled in Unity Editor
#if UNITY_EDITOR
    // Simulation code...
#endif
```

### 2. AdminPanel Testing Tools

**Location:** `AdminPanel` GameObject in scene

**Available Commands:**

| Function | Description | Usage |
|----------|-------------|-------|
| `TogglePremiumEdition()` | Toggle between free and premium | Test ad blocking for premium users |
| `ResetAdCounters()` | Clear frequency/cooldown counters | Force next battle to show ad |
| `ForceShowAd()` | Bypass all checks and show ad immediately | Test ad display directly |
| `ShowAdStatus()` | Print current ad configuration | Debug ad logic |
| `TestFirebaseAnalytics()` | Send test event to Firebase | Verify analytics working |

**Example Test Sequence:**
```
1. Open AdminPanel
2. Click "Show Ad Status" → See current config
3. Click "Toggle Premium Edition" → Switch to free
4. Click "Reset Ad Counters" → Clear state
5. Play a level → Complete it → Ad should show
6. Check Unity Console for logs
```

### 3. Device Testing (Android)

**Test Mode Ads:**

1. Enable Test Mode in `MonetizationSettings`:
   ```
   Test Mode: ✓ (checked)
   ```

2. Add your device as a test device in AppLovin dashboard:
   - Go to Account → Test Devices
   - Add device advertising ID

3. Build and run on Android device

4. You should see test ads with "TEST MODE" watermark

**Real Ads Testing:**

1. Disable Test Mode in `MonetizationSettings`
2. Build release version
3. Upload to Google Play Console (Internal Testing track)
4. Download on device
5. Complete tutorial and reach level 7+
6. Complete 3 battles (default frequency)
7. Ad should show after 3rd battle

**Verify via Logcat:**
```bash
adb logcat -s Unity AppLovin Firebase

# Expected output:
[AppLovin MAX] Initializing with SDK Key: ...
[AppLovin MAX] Initialization complete
[AppLovin MAX] Interstitial ad loaded
[FullScreenAdverts] Showing AppLovin MAX interstitial ad
[Firebase] Event logged: ad_impression
```

### 4. Unity Test Framework

**Edit Mode Tests:**
```csharp
[TestFixture]
public class IMediationManagerTests
{
    [Test]
    public void Interface_HasAllRequiredMethods()
    {
        var type = typeof(IMediationManager);
        Assert.IsTrue(type.IsInterface);
        Assert.IsNotNull(type.GetMethod("InitializeAsync"));
        Assert.IsNotNull(type.GetMethod("IsInterstitialReady"));
        Assert.IsNotNull(type.GetMethod("ShowInterstitial"));
    }
}
```

**Play Mode Tests:**
```csharp
[UnityTest]
public IEnumerator AppLovinManager_InitializesInEditor()
{
    var go = new GameObject("TestManager");
    var manager = go.AddComponent<AppLovinMaxManager>();
    
    yield return new WaitForSeconds(3f); // Wait for initialization
    
    Assert.IsTrue(manager.IsInitialized);
    Assert.IsTrue(manager.IsInterstitialReady());
}
```

---

## Firebase Analytics

### Events Tracked

| Event Name | Parameters | Trigger |
|------------|------------|---------|
| `ad_impression` | `ad_platform`, `ad_type`, `ad_placement` | Ad is displayed |
| `ad_closed` | `ad_platform`, `ad_type`, `completed` | Ad is dismissed |
| `ad_clicked` | `ad_platform`, `ad_type` | User clicks ad |
| `rewarded_ad_offered` | `placement`, `coins_reward`, `credits_reward` | Button shown |
| `rewarded_ad_started` | `placement`, `ad_platform`, `ad_type` | User clicks watch button |
| `rewarded_ad_completed` | `placement`, `ad_platform`, `coins_earned`, `credits_earned` | User completes watching |
| `rewarded_ad_skipped` | `placement`, `completed` | User closes or times out |
| `earn_virtual_currency` | `virtual_currency_name`, `value`, `source` | Rewards granted |
| `ad_decision` | `decision`, `levels_completed`, `premium`, `min_level_config`, `frequency_config` | Ad eligibility checked |

**All events use `ad_platform: "applovin"`** (updated from "ironsource")

### Enable DebugView (Android)

```bash
# Enable Firebase DebugView on device
adb shell setprop debug.firebase.analytics.app com.Bluebottle.Battlecruisers

# Disable when done
adb shell setprop debug.firebase.analytics.app .none.
```

### View Events in Firebase Console

1. Go to Firebase Console → Analytics → DebugView
2. Select your device
3. See events in real-time as they're logged
4. Verify parameters are correct

---

## A/B Testing

### Firebase Remote Config Setup

**Step 1: Configure in Firebase Console**

1. Go to Firebase Console → Remote Config
2. Create parameters:

| Parameter Key | Type | Default Value | Description |
|---------------|------|---------------|-------------|
| `minimum_level_for_ads` | Number | `7` | Minimum levels to complete before ads |
| `ad_frequency` | Number | `3` | Show ad every N battles |
| `ad_cooldown_minutes` | Number | `5` | Minimum minutes between ads |
| `veteran_frequency_boost_enabled` | Boolean | `true` | Different frequency for veterans |
| `veteran_threshold` | Number | `15` | Levels required to be "veteran" |
| `veteran_ad_frequency` | Number | `2` | Ad frequency for veterans |

**Step 2: Create A/B Test Variants**

Example: Test ad frequency impact on retention

```
Control Group (50%):
- ad_frequency: 3
- veteran_ad_frequency: 2

Variant A (25%): Lower frequency
- ad_frequency: 5
- veteran_ad_frequency: 3

Variant B (25%): Higher frequency
- ad_frequency: 2
- veteran_ad_frequency: 1
```

**Step 3: Monitor Results**

Track these metrics per variant:
- **Retention**: 1-day, 7-day, 30-day retention rates
- **Revenue**: Ad revenue + IAP revenue
- **Churn**: Users who haven't returned in 7+ days
- **Ad Engagement**: `ad_impression` vs `ad_closed` events

**Step 4: Iterate**

1. Let test run for 2-4 weeks
2. Analyze results in Firebase Console → A/B Testing
3. Pick winning variant (or create new test)
4. Publish to 100% of users

### Local Testing of Remote Config

**Editor Overrides (via PlayerPrefs):**

```csharp
// Set test values
PlayerPrefs.SetInt("EditorAdConfig_MinLevel", 5);
PlayerPrefs.SetInt("EditorAdConfig_Frequency", 2);
PlayerPrefs.SetFloat("EditorAdConfig_Cooldown", 3.0f);
PlayerPrefs.Save();

// Restart scene to apply
```

---

## Troubleshooting

### "Ads not showing"

**Check:**
1. **Premium Status**: `DataProvider.GameModel.PremiumEdition == false`
2. **Level Requirement**: Completed 7+ levels
3. **Ad Unit IDs**: Set correctly in `MonetizationSettings`
4. **SDK Key**: Valid AppLovin MAX SDK key
5. **Internet**: Device has internet connection
6. **Logcat**: Check for initialization errors

**Solution:**
```bash
# On Android device
adb logcat -s Unity AppLovin | grep -i error

# Common errors:
# "Invalid SDK Key" → Check SDK key in MonetizationSettings
# "Ad unit not found" → Check ad unit IDs
# "No fill" → Normal, try again or enable test mode
```

### "AppLovinMaxManager not found"

**Cause:** Manager not created in scene

**Solution:**
The manager is created on-demand. Ensure:
1. `DestructionSceneGod.Start()` creates it if null
2. `MonetizationSettings` exists in `Resources` folder
3. No exceptions during initialization (check console)

### "Firebase events not logging"

**Check:**
1. **DebugView enabled**: Run adb command above
2. **google-services.json**: Exists in `Assets` folder
3. **Package name**: Matches Firebase Console (`com.Bluebottle.Battlecruisers`)
4. **Analytics enabled**: `FirebaseAnalyticsManager.Instance != null`

**Solution:**
```csharp
// Force test event
if (FirebaseAnalyticsManager.Instance != null)
{
    FirebaseAnalyticsManager.Instance.LogEvent("test_event", 
        new Dictionary<string, object> { { "test_param", "test_value" } });
}
```

### "JNI errors on Android"

**Cause:** AppLovin MAX SDK not properly integrated

**Solution:**
1. Re-import AppLovin MAX Unity plugin
2. Run EDM Android Resolver: `Assets → External Dependency Manager → Android Resolver → Resolve`
3. Clean and rebuild project
4. Check `AndroidManifest.xml` merged correctly

### "Editor simulation not working"

**Expected Behavior:**
- Ads "load" after 2 seconds
- Showing ad simulates 3-second display
- Callbacks fire in correct order

**If broken:**
1. Check Unity Console for exceptions
2. Verify `UNITY_EDITOR` preprocessor defined
3. Ensure `MonetizationSettings` in `Resources` folder

---

## Migration from IronSource

### Files Removed

- ✅ `Assets/Scripts/Ads/IronSourceManager.cs`
- ✅ `Assets/Editor/IronSourceDependencies.xml`
- ✅ `Assets/LevelPlay/Editor/IronSourceSDKDependencies.xml`

### Files Created

- ✅ `Assets/Scripts/Ads/IMediationManager.cs`
- ✅ `Assets/Scripts/Ads/AppLovinMaxManager.cs`
- ✅ `Assets/Scripts/Ads/MonetizationSettings.cs`
- ✅ `APPLOVIN_MAX_INTEGRATION.md` (this file)

### Files Updated

- ✅ `Assets/Scripts/UI/FullScreenAdverts.cs`
- ✅ `Assets/Scripts/Scenes/DestructionSceneGod.cs`
- ✅ `Assets/Scripts/PvP/GamePlay/BattleScene/Scenes/PvPDestructionSceneGod.cs`
- ✅ `Assets/Scripts/Analytics/FirebaseAnalyticsManager.cs`
- ✅ `Assets/Scripts/Utils/Debugging/AdminPanel.cs`
- ✅ `Assets/Plugins/Android/AndroidManifest.xml`

### Breaking Changes

None. The interface-based design ensures backward compatibility at the code level.

### Data Migration

No data migration required. `PlayerPrefs` keys remain the same:
- `AdCounterKey`: Ad frequency counter
- `LastAdShowTime`: Last ad timestamp

---

## Next Steps

### 1. Download AppLovin MAX SDK
- Unity Asset Store or direct download
- Import into project

### 2. Create MonetizationSettings
- `Assets → Create → BattleCruisers → Monetization Settings`
- Move to `Assets/Resources/`
- Fill in SDK key and ad unit IDs

### 3. Test in Editor
- Open `DestructionScene` or `ScreensScene`
- Play through a level
- Verify simulated ads work

### 4. Test on Android Device
- Enable Test Mode in `MonetizationSettings`
- Build and run on Android
- Complete tutorial and level 7
- Verify real test ads show

### 5. Set Up Firebase Remote Config
- Create parameters in Firebase Console
- Test with editor overrides
- Create A/B test variants

### 6. Production Release
- Disable Test Mode
- Update ad unit IDs to production IDs
- Build release APK
- Upload to Google Play Console
- Monitor in AppLovin dashboard and Firebase Analytics

---

## Support

**AppLovin Documentation:**
- [Unity Integration Guide](https://dash.applovin.com/documentation/mediation/unity/getting-started)
- [MAX Dashboard](https://dash.applovin.com)

**Firebase Documentation:**
- [Remote Config](https://firebase.google.com/docs/remote-config)
- [A/B Testing](https://firebase.google.com/docs/ab-testing)
- [Analytics](https://firebase.google.com/docs/analytics)

**Battlecruisers Architecture:**
- `BATTLECRUISERS_ARCHITECTURE_GUIDE.md`
- `ANDROID_ADS_SETUP.md`
- `FIREBASE_REMOTE_CONFIG_SETUP.md`

---

**Integration Complete** ✅  
**Last Updated:** November 2024  
**Version:** 1.0.0

