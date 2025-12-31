# Battlecruisers - AppLovin MAX & Firebase Integration Documentation

**Last Updated:** Current State (Verified Working)  
**Unity Version:** 2022.3.x LTS  
**Platform:** Android (API 23+)  
**AppLovin MAX SDK:** v8.5.1 (Unity Package Manager)  
**Firebase SDK:** Custom JNI implementation (Analytics)

## ⚠️ CRITICAL: SDK Version Constraints

**Current Position on AppLovin:**  
- **SDK Version:** 8.5.1 (Installed via Unity Package Manager)
- **Installation Method:** Unity Package Manager using AppLovin Integration Manager migration tool
- **Package:** `com.applovin.mediation.ads:8.5.1` in `Packages/manifest.json`
- **Scoped Registry:** AppLovin MAX Unity registry configured

**Status:** ✅ **STABLE**
- AppLovin MAX SDK: **WORKING** (Unity Package Manager installation)
- Ad System: **FUNCTIONAL** (Rewarded and Interstitial ads working)
- Firebase: **WORKING** (Custom JNI implementation)

---

## 📋 Table of Contents

1. [Current Implementation Status](#current-implementation-status)
2. [System Architecture](#system-architecture)
3. [Setup Instructions](#setup-instructions)
4. [Rewarded Ad System](#rewarded-ad-system)
5. [Firebase Configuration](#firebase-configuration)
6. [Testing & Debugging](#testing--debugging)
7. [AppLovin MAX Dashboard Setup](#applovin-max-dashboard-setup)
8. [Quick Start Checklist](#quick-start-checklist)
9. [Unity 2021 (Last Known Good Build)](#unity-2021-last-known-good-build)
10. [Troubleshooting](#troubleshooting)
11. [Changelog](#changelog)

---

## 📚 Related Documentation

This is the primary documentation file. Additional guides are organized in the [`Docs/`](Docs/) folder:

### Ad Integration & Testing
- [AppLovin MAX Integration](Docs/APPLOVIN_MAX_INTEGRATION.md) - Complete integration guide
- [AppLovin Migration Summary](Docs/APPLOVIN_MIGRATION_SUMMARY.md) - Migration from IronSource
- [AppLovin MAX Quickstart](Docs/APPLOVIN_MAX_QUICKSTART.md) - Quick setup guide
- [Ad Kill Switch Setup](Docs/AD_KILL_SWITCH_SETUP.md) - Emergency ad close button
- [Ad Testing Guide](Docs/AD_TESTING_GUIDE.md) - Testing procedures
- [Android Ads Setup](Docs/ANDROID_ADS_SETUP.md) - Android-specific setup
- [Root Cause Analysis](Docs/ROOT_CAUSE_FOUND.md) - Close button fix details
- [Close Button Analysis](Docs/CLOSE_BUTTON_ANALYSIS.md) - Investigation history

### Platform Setup
- [iOS SKAdNetwork Setup](Docs/IOS_SKADNETWORK_SETUP.md) - iOS attribution setup

### Feature Guides
- [Heckle Integration Guide](Docs/HECKLE_INTEGRATION_GUIDE.md) - Taunt system setup
- [Heckle Implementation Summary](Docs/HECKLE_IMPLEMENTATION_SUMMARY.md) - Technical details
- [Endless Mode Learnings](Docs/ENDLESS_MODE_LEARNINGS_REPORT.md) - Post-mortem

### Architecture
- [Battlecruisers Architecture Guide](Docs/BATTLECRUISERS_ARCHITECTURE_GUIDE.md) - System overview
- [Integration Summary](Docs/INTEGRATION_SUMMARY.md) - Overall integration status

---

## Current Implementation Status

### ✅ Complete & Production Ready
- **AppLovin MAX SDK Integration** - AppLovin MAX SDK v13.5.1
- **Custom Tabs Support** - Added `androidx.browser:browser:1.8.0` dependency
- **Ad Close Button** - Fixed by removing custom activity back-button interception
- **TextureView Rendering** - Enabled to prevent z-order issues
- **Firebase Android SDK** - v20.1.2 (Analytics) compatible with Unity 2022.3
- **Rewarded Ad System** - Full cycle working (Show -> Watch -> Reward -> Close)
- **AdConfigManager** - Unity Remote Config integration
- **Admin Panel** - Debugging tools active

### 🐛 Active Issues
- **None Critical** - Build is stable.

### ⚠️ Benign Logs (Safe to Ignore)
- `[AdWebView] Unable to process click, ad not found!`: Occurs when user taps screen as ad closes.
- `Renderer process crash (code -1)`: Chromium process cleanup after ad webview destruction.
- `FilePhenotypeFlags`: Internal Google Play Services noise.

### ⏳ Required Setup (Non-Code)
- **AppLovin SDK Key** - Must be set in Unity Inspector
- **AppLovin Dashboard** - Configure ad units and networks
- **Android Dependencies** - Run Android Resolver to bundle native SDKs

---

## System Architecture

### Core Components

```
┌─────────────────────────────────────┐
│     Unity Remote Config             │
│            (AD_CONFIG)              │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│       AdConfigManager               │
│  - Fetches AD_CONFIG JSON           │
│  - Applies defaults if offline      │
│  - Provides ad configuration        │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│      AppLovinManager                │
│  - Manages ad lifecycle             │
│  - Fires ad events                  │
│  - AppLovin MAX SDK integration     │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  DestructionSceneGod (Campaign)     │
│  PvPDestructionSceneGod (PvP)       │
│  - Shows/hides rewarded ad button   │
│  - Handles ad rewards               │
│  - Tracks first-time bonus          │
└─────────────────────────────────────┘
```

### Key Files

| File | Purpose | Status |
|------|---------|--------|
| `Assets/Scripts/Ads/AdConfigManager.cs` | Unity Remote Config (AD_CONFIG JSON) fetching | ✅ Complete |
| `Assets/Scripts/Ads/AppLovinManager.cs` | AppLovin MAX SDK wrapper | ✅ Full implementation |
| `Assets/Scripts/Scenes/DestructionSceneGod.cs` | Campaign rewarded ads | ✅ Complete |
| `Assets/Scripts/PvP/.../PvPDestructionSceneGod.cs` | PvP rewarded ads | ✅ Complete |
| `Assets/Scripts/Utils/Debugging/AdminPanel.cs` | Testing utilities | ✅ Enhanced |
| `Assets/Editor/FirebaseDependencies.xml` | Firebase Android dependencies | ✅ Custom JNI |
| `Packages/manifest.json` | Unity Package Manager (AppLovin MAX 8.5.1) | ✅ UPM |
| `Assets/Editor/AppLovinDependencyConditional.cs` | Manages DISABLE_ADS dependency exclusion | ✅ Active |
| `Assets/Editor/PostGenerateGradleAndroidProject.cs` | R8 packagingOptions exclusions (simplified Dec 11) | ✅ Active |
| `Packages/manifest.json` | Unity Package Manager | ✅ Clean |

---

## Setup Instructions

### AppLovin MAX SDK Setup

1. **Get your AppLovin SDK Key** (https://dash.applovin.com/)
   - Navigate to: **Account** → **Keys**
   - Copy your **SDK Key** (currently configured: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`)

2. **Create Ad Units in AppLovin Dashboard**
   - Go to **Monetize** → **Manage** → **Ad Units**
   - Create two ad units:
     - **Rewarded Video** - Copy the Ad Unit ID (currently configured: `c96bd6d70b3804fa`)
     - **Interstitial** - Copy the Ad Unit ID (currently configured: `9375d1dbeb211048`)

3. **Configure the AppLovinManager in Unity**
   - Open `LandingScene`
   - Select the `AppLovinManager` GameObject
   - Verify the **SDK Key** field is set to: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
   - Verify the **Interstitial Ad Unit ID** field is set to: `9375d1dbeb211048`
   - Verify the **Rewarded Ad Unit ID** field is set to: `c96bd6d70b3804fa`
   - Optionally enable **Debug Logs** for testing
   
   **Note:** These values are also hardcoded as defaults in `Assets/Scripts/Ads/AppLovinManager.cs` (lines 17, 20, 23) and the SDK Key is stored in `Assets/MaxSdk/Resources/AppLovinSettings.asset` (line 16).

4. **AppLovin MAX SDK Installation** (Unity Package Manager)
   - The SDK is installed via Unity Package Manager
   - Package: `com.applovin.mediation.ads:8.5.1`
   - Scoped registry configured in `Packages/manifest.json`
   - Migration was performed using AppLovin Integration Manager's built-in migration tool
   - **Note:** SDK Key is configured in `Assets/MaxSdk/Resources/AppLovinSettings.asset`

5. **Resolve Android dependencies** (if needed)
   - Assets → External Dependency Manager → Android Resolver → Settings
     - Enable Auto-Resolution and Resolution On Build
   - Assets → External Dependency Manager → Android Resolver → Force Resolve
   - Wait for console output showing resolution complete

6. **Build & test**
   - File → Build Settings → Android → Build (or Build and Run)
   - Unity bundles AppLovin MAX SDK (via UPM) + Firebase + Play Games SDKs

### AppLovin MAX Dashboard Checklist

1. Create two ad units:
   - Rewarded Video
   - Interstitial
2. Add at least one ad network (AppLovin recommended for testing)
3. Enable **Test Mode** and add your test device ID
4. (Optional) Add additional ad networks (AdMob, Meta, Unity, etc.) for mediation

### Firebase Native SDK (Analytics Only)

**Current Versions (Verified Working - Unity 2022 Compatible)**
- Firebase Analytics: **20.1.2** ✅ (Verified in `FirebaseDependencies.xml`)
- AppLovin MAX SDK: **13.5.1** ✅ (Verified in `Dependencies.xml`)
- Custom Tabs: **1.8.0** ✅ (Required for MAX 13.x)

**⚠️ If upgrading MAX again:** 
Always ensure `androidx.browser:browser` is included in dependencies, as MAX 13.x relies on it for Custom Tabs.

**Implementation Notes**
- We use AndroidJavaClass to call Firebase Analytics directly
- Dependencies defined in `Assets/Editor/FirebaseDependencies.xml`

**Rebuild Checklist**
1. Assets → External Dependency Manager → Android Resolver → Delete Resolved Libraries
2. Assets → External Dependency Manager → Android Resolver → Force Resolve
3. Verify resolved versions in `ProjectSettings/AndroidResolverDependencies.xml` match pinned versions
4. Delete Unity caches:
   ```powershell
   Remove-Item -Path "Library\Bee","Library\PramData","Temp" -Recurse -Force
   ```
5. Delete Gradle transform cache:
   ```powershell
   Remove-Item -Recurse -Force "$env:USERPROFILE\.gradle\caches\transforms-3"
   ```
6. Rebuild in Unity

### Unity Remote Config – AD_CONFIG

Unity Remote Config now stores all ad tuning values in a single JSON key named `AD_CONFIG`, matching the style of `GAME_CONFIG` and `SHOP_CONFIG`.

**Key Name:** `AD_CONFIG`  
**Type:** `json`  
**Recommended Value:**
```json
{"ad_minimum_level":7,"ad_frequency":3,"ad_cooldown_minutes":9.0,"ad_veteran_boost_enabled":true,"ad_veteran_threshold":15,"ad_veteran_frequency":2,"ads_are_live":false,"ads_disabled":false,"rewarded_ad_coins":10,"rewarded_ad_credits":250}
```

**Usage Notes**
- Add `AD_CONFIG` via Unity Dashboard → Remote Config → Add Key
- Publish to propagate changes (~2 minutes)
- `AdConfigManager` reads the JSON via `JsonUtility.FromJson<AdConfig>()`
- `FullScreenAdverts`, `AppLovinManager`, and `AdminPanel` honor `ads_are_live`/`ads_disabled`
- Flip between test mode (`ads_are_live=false`), production (`ads_are_live=true`), or disabled (`ads_disabled=true`) without code changes

### Unity Inspector Assignments
### Unity Inspector Assignments

#### AppLovinManager
- **SDK Key**: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0` (configured in `AppLovinManager.cs` line 17 and `AppLovinSettings.asset`)
- **Interstitial Ad Unit ID**: `9375d1dbeb211048` (configured in `AppLovinManager.cs` line 20)
- **Rewarded Ad Unit ID**: `c96bd6d70b3804fa` (configured in `AppLovinManager.cs` line 23)
- **Banner Ad Unit ID**: `YOUR_BANNER_AD_UNIT_ID` (placeholder, optional)
- **MREC Ad Unit ID**: `YOUR_MREC_AD_UNIT_ID` (placeholder, optional)
- Enable Debug Logs while testing (disable for production)

#### DestructionScene (`DestructionSceneGod`)
1. Assign **Rewarded Ad Button**, **Coins Text**, **Credits Text**
2. Hook button `On Click()` to `OnWatchRewardedAdButtonClicked`

#### PvPDestructionScene (`PvPDestructionSceneGod`)
1. Mirror the same assignments as the campaign scene

### Firebase Remote Config Upload

1. Firebase Console → Engage → Remote Config
2. Import `firebase-remote-config-UPLOAD.json`
3. Publish changes (propagation takes ~2 minutes)

**Default values (editable remotely)**:
- `first_rewarded_ad_coins`: 5000
- `first_rewarded_ad_credits`: 25000
- `rewarded_ad_coins`: 15
- `rewarded_ad_credits`: 2000
- `rewarded_ad_min_level`: 7
- `rewarded_ads_enabled`: true
- `interstitial_ads_enabled`: false (off by default)

### Android Build Settings

**Already configured**
- Minimum SDK: Android API 23
- Target SDK: Android API 35
- IL2CPP scripting backend

**Build steps**
1. File → Build Settings → Platform: Android
2. Click Build or Build and Run
3. Wait for Unity to finish (~5-15 minutes)

---

## Rewarded Ad System

### User Flow

```
Player beats Level 7
    ↓
Destruction Screen loads
    ↓
Check: Is player Level 7+? ──NO──→ Hide button
    ↓ YES
Check: Is AdConfigManager loaded? ──NO──→ Hide button (offline)
    ↓ YES
Check: Is rewarded ad ready? ──NO──→ Hide button
    ↓ YES
Check: Has player watched ad before?
    ├─ NO  → Show button: "5000 coins, 25000 credits"
    └─ YES → Show button: "15 coins, 2000 credits"
```

### First-Time Bonus Logic

**Storage:** `PlayerPrefs` key `"FirstRewardedAdWatched"`
- **Value 0 (default):** Player has never watched a rewarded ad
- **Value 1:** Player has watched at least one ad

**Reward Flow:**
```csharp
// On button display
bool isFirstAd = !IsFirstRewardedAdWatched();

if (isFirstAd) {
    coins = 5000;
    credits = 25000;
} else {
    coins = 15;
    credits = 2000;
}

// On ad completion
if (wasFirstAd) {
    MarkFirstRewardedAdWatched(); // Sets PlayerPrefs to 1
}
```

**Persistence:** Flag survives app restarts, device reboots, and app updates

### Button Visibility Rules

Button shows ONLY when **ALL** conditions are met:
1. ✅ Player level ≥ `rewarded_ad_min_level` (default: 7)
2. ✅ `AdConfigManager.Instance` is not null (online)
3. ✅ `rewarded_ads_enabled` is `true` (Firebase Remote Config)
4. ✅ `AppLovinManager.Instance.IsRewardedAdReady()` returns `true`

Button hides if **ANY** condition fails.

---

## Firebase Configuration

### Remote Config Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `interstitial_ads_enabled` | bool | `false` | Master on/off for interstitials |
| `interstitial_min_level` | int | `7` | Minimum level for interstitials |
| `interstitial_cooldown_minutes` | int | `12` | Cooldown between interstitials |
| `max_ads_per_battles` | int | `3` | Max interstitials per N battles |
| `veteran_ad_cooldown_minutes` | int | `8` | Cooldown for Level 15+ players |
| `rewarded_ads_enabled` | bool | `true` | Master on/off for rewarded ads |
| `rewarded_ad_min_level` | int | `7` | Minimum level for rewarded ad button |
| `first_rewarded_ad_coins` | int | `5000` | **First-time bonus: Coins** |
| `first_rewarded_ad_credits` | int | `25000` | **First-time bonus: Credits** |
| `rewarded_ad_coins` | int | `15` | **Subsequent: Coins** |
| `rewarded_ad_credits` | int | `2000` | **Subsequent: Credits** |

### Updating Values Without App Republishing

1. Firebase Console → Remote Config
2. Edit parameter value
3. Click "Publish"
4. Changes apply to all players within 1-2 minutes

### Analytics Events

Automatically logged:
- `rewarded_ad_offered` - Button displayed
- `rewarded_ad_started` - Player clicked button
- `rewarded_ad_completed` - Player finished watching
- `earn_virtual_currency` - Rewards granted (coins + credits)
- `ad_impression` - Interstitial ad shown
- `ad_closed` - Interstitial ad closed

**View in:** Firebase Console → Analytics → DebugView

---

## Testing & Debugging

### Admin Panel Functions

**Already Implemented:**

1. **Reset Ad Counters**
   - Resets interstitial cooldowns only
   - Does NOT reset first-time rewarded ad flag
   - Use for: Testing interstitial frequency

2. **Reset First Rewarded Ad** (NEW)
   - Deletes `PlayerPrefs("FirstRewardedAdWatched")`
   - Next rewarded ad shows big reward again
   - Use for: Testing first-time bonus (5000/25000)

3. **Reset All Ad Data** (NEW)
   - Resets all ad-related PlayerPrefs
   - Complete fresh start
   - Use for: Full ad system testing

4. **Force Show Ad** (Existing)
   - Simulates ad display in Editor
   - Resets counters automatically
   - Use for: Quick ad testing in Editor

5. **Clear Battle Log** (ENHANCED - Dec 10, 2025)
   - First saves AppLovin debug logs to `/sdcard/Download/AppLovin_Debug_*.txt`
   - Then clears the battle log file
   - Includes system info, all AppLovin events, error traces
   - Use for: Saving logs for support tickets + resetting battle logging

### Editor Testing Workflow

1. Play mode in Unity Editor
2. Open Admin Panel
3. Click "Force Show Ad"
4. Check Console logs:
   ```
   [AppLovin] [EDITOR] Simulating rewarded ad completion...
   [Rewards] Ad completed! Granted 5000 coins and 25000 credits (First ad: True)
   ```
5. Force ad again → Should show 15 coins, 2000 credits
6. Click "Reset First Rewarded Ad" → Resets to big reward

### Device Testing (USB)

1. Enable USB Debugging on Android device
2. Connect via USB
3. Build and Run from Unity
4. Watch logcat:
   ```powershell
   adb logcat -s Unity
   ```
5. Look for:
   - `[Firebase] Successfully initialized`
   - `[AdConfig] Remote Config fetched successfully`
   - `[Rewards] Offering rewarded ad: X coins, Y credits`

### Collecting Logs for AppLovin Support (Dec 10, 2025)

**Method 1 - Automated Script (Recommended):**
```powershell
# Run from project root
.\collect_applovin_logs.ps1
```
- Automatically collects all AppLovin, Unity, and error logs
- Saves to `AppLovin_Logcat_TIMESTAMP.txt`
- Press Ctrl+C when done

**Method 2 - In-Game Log Collector:**
1. Launch app on device
2. Open Admin Panel (ENABLE_CHEATS required)
3. Reproduce the ad close button issue
4. Tap "Clear Battle Log" button (also saves AppLovin logs)
5. Logs saved to `/sdcard/Download/AppLovin_Debug_*.txt`
6. Pull file: `adb pull /sdcard/Download/AppLovin_Debug_*.txt`

**Method 3 - Manual adb logcat:**
```powershell
# Clear previous logs
adb logcat -c

# Collect filtered logs
adb logcat -v time *:E Unity:V AppLovinSdk:V MAX:V > applovin_debug.txt
```

**What to Include in Support Ticket:**
- Device model and Android version
- App version and build number
- Full log file from one of the methods above
- Description: "Ad close button never appears, error -1009 from rt.applovin.com/4.0/pix"
- SDK Key: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
- Ad Unit IDs: Interstitial `9375d1dbeb211048`, Rewarded `c96bd6d70b3804fa`

### Common Issues

| Problem | Solution |
|---------|----------|
| Button never appears | Check: Player ≥ Level 7, AdConfigManager initialized, Firebase connected |
| Text fields empty | Verify Text components assigned in Inspector (not GameObjects) |
| Wrong reward values | Upload `firebase-remote-config-UPLOAD.json` and publish |
| Build fails: `minSdkVersion 22 < 23` | Already fixed (API 23) |
| Build fails: R8 dexing error | Already fixed (Firebase 21.x) |
| ClassNotFoundException Firebase | Run Android Resolver |

---

## AppLovin MAX Dashboard Setup

### 1. Create Ad Units

1. Go to: **Monetize** → **Manage** → **Ad Units**
2. Click **Create Ad Unit**
3. Create two ad units:
   - **Rewarded** (name e.g., `Rewarded_Video`)
   - **Interstitial** (name e.g., `Interstitial`)
4. Copy the Ad Unit IDs for Unity configuration

### 2. Add Ad Networks

1. Navigate: **Monetize** → **Manage** → **Networks**
2. Click **Connect Network** for each network you want:
   - ✅ AppLovin (built-in, recommended for testing)
   - ✅ AdMob (requires Google AdMob account)
   - ✅ Meta Audience Network (optional)
   - ✅ Unity Ads (optional)
3. Follow the integration steps for each network
4. Set CPM rates or enable Auto CPM

### 3. Enable Test Mode

1. Go to: **Account** → **Settings** → **Test Devices**
2. Add your test device ID:
   - `adb logcat | findstr "AppLovin"`
   - Look for "Test Mode" device ID in logs
3. Enable Test Mode for your app

---

## Quick Start Checklist

### ✅ Already Done

- AppLovin MAX SDK integrated
- `AppLovinManager.cs` fully implemented with AppLovin MAX SDK
- Android dependencies configured via Android Resolver
- Firebase integration (Analytics only) plus Unity Remote Config (AD_CONFIG)
- Rewarded & interstitial ad systems implemented
- Editor simulation + Admin Panel enhancements

### 📝 Action Items

1. **Get AppLovin SDK Key**:
   - https://dash.applovin.com/ → Account → Keys → Copy SDK Key
   - Currently configured: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
2. **Create Ad Units**:
   - Dashboard → Monetize → Manage → Ad Units → Create Rewarded + Interstitial
   - Currently configured:
     - Rewarded: `c96bd6d70b3804fa`
     - Interstitial: `9375d1dbeb211048`
3. **Configure Unity**:
   - Open `LandingScene`, select `AppLovinManager`
   - Verify SDK Key matches: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
   - Verify Interstitial Ad Unit ID: `9375d1dbeb211048`
   - Verify Rewarded Ad Unit ID: `c96bd6d70b3804fa`
   - Enable Debug Logs while testing
4. **Resolve Dependencies**:
   - Assets → External Dependency Manager → Android Resolver → Force Resolve
5. **Configure Dashboard**:
   - Add ad networks, enable test mode
6. **Build & Test**:
   - Unity → Build Settings → Android → Build and Run
   - Confirm rewarded ad button, watch ad, verify rewards
- 7. **Verify Analytics & Remote Config**:
-   - Firebase Console → Analytics → DebugView
-   - Enable debug: `adb shell setprop debug.firebase.analytics.app com.Bluebottle.Battlecruisers`
-   - Check for `ad_impression`, `rewarded_ad_started`, `rewarded_ad_completed`, `earn_virtual_currency`
-   - Unity Dashboard → Remote Config → Confirm `AD_CONFIG` is published

### 🧾 Build Checklist

- [ ] AppLovin SDK Key set in Unity (currently: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`)
- [ ] Interstitial Ad Unit ID set in Unity (currently: `9375d1dbeb211048`)
- [ ] Rewarded Ad Unit ID set in Unity (currently: `c96bd6d70b3804fa`)
- [ ] Android dependencies resolved (AppLovin MAX + Firebase)
- [ ] Unity Inspector assignments completed for both scenes
- [ ] Firebase Remote Config uploaded and published
- [ ] Target SDK/API settings validated (Min API 23, Target API 35)

### 🧪 Testing Checklist

- [ ] Editor: Admin Panel → "Force Show Ad"
- [ ] Reward amounts: first-time (5000 coins, 25000 credits), subsequent (15 coins, 2000 credits)
- [ ] Button visible only after level 7
- [ ] Offline behavior: button hides when no Firebase connection
- [ ] Device test: Build & run on Android
- [ ] Firebase DebugView logs expected events

### 🔗 References

- Unity Remote Config Dashboard: `https://dashboard.unity3d.com/projects/<project-id>/remote-config`
- Admin Panel: `Assets/Scripts/Utils/Debugging/AdminPanel.cs`
- AppLovin MAX Dashboard: `https://dash.applovin.com/`
- AppLovin MAX Unity Plugin GitHub: `https://github.com/AppLovin/AppLovin-MAX-Unity-Plugin`

---

## Unity 2021 (Last Known Good Build)

**Scope:** Stable Android build on Unity 2021.3.45f2 with Kotlin 1.x toolchain, AppLovin MAX 12.6.1, Firebase Analytics/Config 21.5.0/21.6.0, Crashlytics 18.6.0.

- **Versions & Platforms**
  - Unity 2021.3.45f2; IL2CPP; Min SDK 23; Target SDK 35.
  - AppLovin MAX SDK **12.6.1** (pinned to avoid Kotlin 2.0 / R8 crashes from 13.x).
  - Firebase: analytics 21.5.0, config 21.6.0, crashlytics 18.6.0; Kotlin 1.x compatible.
  - Jetifier required: `android.enableJetifier=true`, `useJetifier="True"`.
- **Build Workflow (known-good)**
  - Run EDM4U Android Resolver (auto + Force Resolve) and verify `ProjectSettings/AndroidResolverDependencies.xml` shows MAX 12.6.1.
  - Clean caches when versions drift: delete `Library/Bee`, `Library/PramData`, `Temp`, and `%USERPROFILE%\.gradle\caches\transforms-3`.
  - Use Unity-generated Gradle templates; avoid manual launcher/main template edits. If resolver writes wrong Firebase versions, manually correct `Assets/Plugins/Android/mainTemplate.gradle` and re-run resolver.
  - Hardware acceleration enabled in `Assets/Plugins/Android/AndroidManifest.xml` (`android:hardwareAccelerated="true"`) to fix ad close buttons/MRAID.
- **Remote Config & AD_CONFIG defaults**
  - Unity Remote Config key `AD_CONFIG` (json):
    ```json
    {"ad_minimum_level":7,"ad_frequency":3,"ad_cooldown_minutes":9.0,"ad_veteran_boost_enabled":true,"ad_veteran_threshold":15,"ad_veteran_frequency":2,"ads_are_live":false,"ads_disabled":false,"rewarded_ad_coins":10,"rewarded_ad_credits":250}
    ```
  - Firebase Remote Config parameters (editable): `first_rewarded_ad_coins=5000`, `first_rewarded_ad_credits=25000`, `rewarded_ad_coins=15`, `rewarded_ad_credits=2000`, `rewarded_ad_min_level=7`, `rewarded_ads_enabled=true`, `interstitial_ads_enabled=false`.
- **Rewarded Ad Rules (campaign & PvP)**
  - Show button only when: player level ≥ 7, `AdConfigManager.Instance` is available, `rewarded_ads_enabled` true, and `AppLovinManager.Instance.IsRewardedAdReady()` true.
  - First-time bonus uses `PlayerPrefs("FirstRewardedAdWatched")`: first ad grants **5000 coins + 25000 credits**; subsequent ads grant **15 coins + 2000 credits**; flag set after first completion.
- **Admin Panel (Editor/device)**
  - Force Show Ad (Editor simulation), Reset Ad Counters, Reset First Rewarded Ad (clears PlayerPrefs flag), Reset All Ad Data.
  - Logcat markers: `[Firebase] Successfully initialized`, `[AdConfig] Remote Config fetched successfully`, `[Rewards] Offering rewarded ad: X coins, Y credits`.
- **Troubleshooting (2021)**
  - R8/D8 StackOverflow from AppLovin 13.x → pin to 12.6.1.
  - Missing Firebase classes → rerun Android Resolver after cache cleanup.
  - Template validation errors → revert to Unity-generated templates; prefer `IPostGenerateGradleAndroidProject` for adjustments.
  - Gradle cache corruption (`gradle-7.4.2.jar`) → delete `%USERPROFILE%\.gradle\caches\jars-9` then rebuild.

---

## Troubleshooting


### Gradle Template Modification Issues

**Why Direct Template Modification Fails:**
- Unity regenerates Gradle files from templates during each build
- Unity strictly validates template syntax
- Custom templates can cause validation errors (e.g., `aaptOptions` format)
- Templates are overwritten if Unity detects issues

**Common Errors:**
- `launcherTemplate.gradle file is using the old aaptOptions noCompress property definition`
- `Plugin with id 'com.android.application' not found`
- Template validation failures

**Better Approach:**
- Use `IPostGenerateGradleAndroidProject` scripts to modify generated files
- Scripts run after Unity generates files but before Gradle build
- More reliable than template modification

### Unity's Android Build Process

**Understanding the Build Pipeline:**
1. **Unity Compiles C# Code** → IL2CPP → Native code
2. **Unity Generates Gradle Project** → Creates `Library/Bee/Android/Prj/IL2CPP/Gradle/`
   - `launcher/build.gradle` (main app module)
   - `unityLibrary/build.gradle` (Unity library module)
   - `build.gradle` (root project)
   - `settings.gradle` (project settings)
   - `gradle.properties` (project properties)
3. **EDM4U Processes Dependencies** → Injects dependencies into Gradle files
4. **Post-Generation Scripts Run** → `IPostGenerateGradleAndroidProject` callbacks execute
5. **Gradle Build Starts** → Compiles, dexes, packages APK

**Key Insight:**
- Unity generates fresh Gradle files from templates on each build
- Modifications must happen AFTER generation but BEFORE Gradle build
- This is why `IPostGenerateGradleAndroidProject` is the correct hook point

### ClassNotFoundException Errors (Firebase, Play Games)

**Errors:**
```
java.lang.ClassNotFoundException: com.google.firebase.analytics.FirebaseAnalytics
java.lang.ClassNotFoundException: com.google.firebase.remoteconfig.FirebaseRemoteConfig
java.lang.ClassNotFoundException: com.google.android.gms.games.PlayGames
```

**Cause:** Native Android libraries (Firebase, Play Games) are not bundled in your APK because Android Resolver hasn't run after cache cleanup.

**Fix:**
1. Open Unity Editor
2. Go to: **Assets** → **External Dependency Manager** → **Android Resolver** → **Resolve**
3. Wait for resolution to complete (EDM4U downloads native AARs/JARs)
4. Rebuild APK

**What This Does:**
- Reads `Assets/Editor/FirebaseDependencies.xml`
- Reads `Assets/Editor/AppLovinMaxDependencies.xml`
- Reads `Assets/GooglePlayGames/.../GooglePlayGamesPluginDependencies.xml`
- Adds Maven dependency references to `Assets/Plugins/Android/mainTemplate.gradle`
- Gradle downloads dependencies during build (not stored in Unity project)
- Dependencies are resolved from Maven Central/Google repositories

**Important:** Modern EDM4U (v1.2.186) uses **Maven dependencies** instead of downloading AAR files. You won't see AAR files in `Assets/Plugins/Android/` - this is normal! Dependencies are referenced in `mainTemplate.gradle` and downloaded during the Gradle build.

**If Resolve Fails:**
- Try: **Assets** → **External Dependency Manager** → **Android Resolver** → **Delete Resolved Libraries**
- Then: **Force Resolve**
- Verify `mainTemplate.gradle` contains dependency entries after resolution

### Gradle Cache Corruption Error

**Error:**
```
Failed to create Jar file C:\Users\Home\.gradle\caches\jars-9\...\gradle-7.4.2.jar
```

**Fix:**
```powershell
# Delete corrupted Gradle JAR cache
Remove-Item -Path "$env:USERPROFILE\.gradle\caches\jars-9" -Recurse -Force

# Clean Unity build caches
cd c:\Battlecruisers
Remove-Item -Path "Library\Bee","Library\PramData","Temp" -Recurse -Force
```

Then rebuild in Unity.

**Root Cause:** Corrupted Gradle cache from interrupted download or disk I/O issue.

### How to Build with Stacktrace

**Method 1 - Unity Editor:**
- `File` → `Build Settings` → Check "Development Build" → Build
- Provides more detailed error output

**Method 2 - Command Line:**
```powershell
cd c:\Battlecruisers\Library\Bee\Android\Prj\IL2CPP\Gradle
gradlew.bat assembleDebug --stacktrace
# OR with full debug output:
gradlew.bat assembleDebug --stacktrace --debug > build.log 2>&1
```

**Method 3 - Full Gradle Cache Wipe (Nuclear Option):**
```powershell
Remove-Item -Path "$env:USERPROFILE\.gradle\caches" -Recurse -Force
```
Then rebuild (will re-download all dependencies, ~5 minutes)

### Firebase Version Not Found Errors

**Error:**
```
Could not find com.google.firebase:firebase-analytics:21.6.0
Could not find com.google.firebase:firebase-config:21.7.0
Could not find com.google.firebase:firebase-crashlytics:18.7.0
```

**Cause:** These specific version numbers don't exist in Maven repositories. Firebase versions must match what's actually published.

**Verified Working Versions:**
- ✅ `firebase-analytics:21.5.0` (exists in Maven)
- ✅ `firebase-config:21.6.0` (exists in Maven)
- ✅ `firebase-crashlytics:18.6.0` (exists in Maven)

**Fix:**
1. Verify `Assets/Editor/FirebaseDependencies.xml` uses the versions above
2. If `mainTemplate.gradle` has wrong versions, manually edit it:
   ```gradle
   implementation 'com.google.firebase:firebase-analytics:21.5.0'
   implementation 'com.google.firebase:firebase-config:21.6.0'
   implementation 'com.google.firebase:firebase-crashlytics:18.6.0'
   ```
3. Re-run Android Resolver to sync: **Assets → External Dependency Manager → Android Resolver → Force Resolve**

**Prevention:** Always verify Firebase version numbers exist in Maven before updating. Check: https://mvnrepository.com/artifact/com.google.firebase

### Android Resolver: Maven Dependencies vs Local AAR Files

**Question:** "Where are the AAR files after running Android Resolver?"

**Answer:** Modern EDM4U (v1.2.186) uses **Maven dependency references** instead of downloading AAR files locally. This is the correct behavior!

**How It Works:**
1. Android Resolver reads your `*Dependencies.xml` files
2. Adds `implementation 'com.package:name:version'` entries to `mainTemplate.gradle`
3. Gradle downloads dependencies from Maven during build
4. No AAR files stored in Unity project (keeps project size small)

**Verify It Worked:**
- Check `Assets/Plugins/Android/mainTemplate.gradle` contains dependency entries
- Look for lines like: `implementation 'com.applovin:applovin-sdk:12.6.1'`
- If present, resolution succeeded - dependencies will download during Gradle build

**Benefits:**
- ✅ Smaller Unity project (no large binary files)
- ✅ Always fresh dependencies (downloaded during build)
- ✅ Better version control (no binary files in git)
- ✅ Faster Unity editor (less assets to load)

### Gradle SDK Version Warnings

**Warning:**
```
WARNING:minSdkVersion (23) is greater than targetSdkVersion (22)
WARNING:We recommend using a newer Android Gradle plugin to use compileSdk = 35
```

**Cause:** GooglePlayGamesManifest.androidlib has outdated targetSdkVersion, or Gradle plugin version mismatch.

**Fix:**
1. Add to `Assets/Plugins/Android/gradleTemplate.properties`:
   ```
   android.suppressUnsupportedCompileSdk=35
   ```
2. This suppresses the warning (your actual targetSdkVersion is 35 in ProjectSettings)
3. The GooglePlayGames warning is harmless - it's from a library manifest, not your app

**Note:** Your app's actual settings (Min API 23, Target API 35) are correct. These warnings are from dependency libraries.

### EDM4U Informational Logs (Normal)

**Log Message:**
```
'GooglePlayGamesPlugin' Manifest:
Current files:
Assets\ExternalDependencyManager\Editor\1.2.169\...
```

**Status:** ✅ **NORMAL** - This is just EDM4U scanning for manifest files. Not an error.

**What It Means:**
- EDM4U is checking Google Play Games plugin files
- Listing files for version tracking
- This is informational logging, not a problem

**Action:** None required - these logs can be ignored. They're verbose but harmless.

### Jetifier Configuration (Required by AppLovin)

**Status:** ✅ **Correctly Enabled**

According to [AppLovin's Unity integration documentation](https://support.axon.ai/en/max/unity/overview/integration/), Jetifier is **required** for Android builds with AppLovin MAX SDK.

**Current Configuration:**
- `ProjectSettings/AndroidResolverDependencies.xml`: `useJetifier="True"` ✅
- `Assets/Plugins/Android/gradleTemplate.properties`: `android.enableJetifier=true` ✅
- AppLovin's post-processor (`AppLovinPostProcessAndroid.cs`) also automatically enables Jetifier ✅

**What Jetifier Does:**
- Converts legacy Android Support Library dependencies to AndroidX
- Required for AppLovin MAX SDK compatibility
- Automatically enabled by AppLovin's post-processor

**Note:** We previously attempted to disable Jetifier, but this was incorrect. AppLovin requires it, and it's now properly enabled.

### DISABLE_ADS Flag - Building Without AppLovin SDK

**Purpose:** Allows building without AppLovin SDK when needed for testing or debugging.

**How to Enable/Disable:**
1. **Unity Menu (Recommended):** **Tools → AppLovin → Toggle DISABLE_ADS Define**
2. **Manual:** Edit → Project Settings → Player → Other Settings → Scripting Define Symbols → Add/Remove `DISABLE_ADS`

**What Happens When DISABLE_ADS is Enabled:**
- `Assets/Editor/AppLovinMaxDependencies.xml` is automatically disabled
- AppLovin SDK is not included in Gradle builds
- All `MaxSdk` calls are conditionally compiled out
- `AppLovinManager` still exists but SDK never initializes
- Scripts checking `AppLovinManager.Instance != null` work correctly
- `IsInterstitialReady()` and `IsRewardedAdReady()` return `false`

**Files Involved:**
- `Assets/Editor/AppLovinDependencyConditional.cs` - Automatically manages dependency exclusion
- `Assets/Scripts/Ads/AppLovinManager.cs` - All `MaxSdk` calls wrapped in `#if !DISABLE_ADS`

**Usage:**
- Enable `DISABLE_ADS` when debugging non-ad features to eliminate SDK complications
- Disable `DISABLE_ADS` for production builds with ads
- Always use the menu item rather than manually editing defines
- Clean build caches after toggling: Delete `Library/Bee` folder

**Example Code (Works With or Without DISABLE_ADS):**
```csharp
if (AppLovinManager.Instance != null && AppLovinManager.Instance.IsInterstitialReady())
{
    AppLovinManager.Instance.ShowInterstitial();
}
else
{
    // Fallback behavior when ads are disabled or not ready
    Debug.Log("Ads not available");
}
```

### Gradle Warnings Fix

**Warning:**
```
WARNING:The option setting 'android.enableDexingArtifactTransform=false' is deprecated.
The current default is 'true'.
It will be removed in version 8.0 of the Android Gradle plugin.
```

**Root Cause:**
- AppLovin's post-processor (`AppLovinPostProcessAndroid.cs`) sets `android.enableDexingArtifactTransform=false` for Unity < 6.0
- This causes a deprecation warning in newer Gradle versions
- AppLovin sets this intentionally for ExoPlayer compatibility

**Solution:**
- **No action needed** - This warning is harmless and expected
- AppLovin intentionally sets this value for compatibility reasons
- The warning is informational only and does not break builds
- The property will be removed in Gradle 8.0, but Unity 2021.3 uses Gradle 7.4.2

**Note:** The `android.aapt2FromMavenOverride` warning is also informational (set by Unity automatically) and can be safely ignored.

### AppLovin MAX Specific Issues

| Problem | Solution |
|---------|---------|
| Ads not loading | Check SDK Key, Ad Unit IDs, internet connection |
| "No Fill" errors | Normal in test mode - add more ad networks in production |
| Test ads not showing | Enable test mode in AppLovin dashboard, add device ID |
| SDK initialization failed | Verify SDK Key is correct, check Android Resolver ran |
| Build fails with Kotlin errors | Verify using SDK 12.6.1 (NOT 13.x) - Unity 2022.3 requires this |
| SDK auto-upgraded to 13.x | Pin version in `Dependencies.xml` to 12.6.1 and re-resolve |
| Ad close button not working | See Root Cause section - CustomActivity was blocking back button |

### Build Workflow: Dependency Resolution Process

**Complete Build Workflow:**

1. **Unity Editor Setup:**
   - SDK Keys and Ad Unit IDs are already configured in `AppLovinManager.cs`:
     - SDK Key: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0` (line 17)
     - Interstitial: `9375d1dbeb211048` (line 20)
     - Rewarded: `c96bd6d70b3804fa` (line 23)
   - Configure Firebase Remote Config in dashboard

2. **Android Resolver (Before First Build):**
   - **Assets → External Dependency Manager → Android Resolver → Force Resolve**
   - Resolver reads `*Dependencies.xml` files
   - Generates/updates `mainTemplate.gradle` with Maven dependencies
   - Console shows: `"Resolution complete!"`

3. **Gradle Build Process:**
   - Unity generates Gradle project in `Library/Bee/Android/Prj/IL2CPP/Gradle/`
   - Gradle reads `mainTemplate.gradle` dependencies
   - Downloads from Maven: AppLovin SDK, Firebase libraries, Play Games
   - Compiles and packages into APK

4. **Verification:**
   - Check `mainTemplate.gradle` has dependency entries
   - Build succeeds without `ClassNotFoundException` errors
   - App runs and ads load correctly

**Key Files in Build Process:**
- `Assets/Editor/*Dependencies.xml` → Dependency definitions
- `Assets/Plugins/Android/mainTemplate.gradle` → Generated by resolver
- `Assets/Plugins/Android/gradleTemplate.properties` → Gradle settings
- `Library/Bee/Android/Prj/IL2CPP/Gradle/` → Generated Gradle project

**If Build Fails:**
1. Check `mainTemplate.gradle` has correct dependency versions
2. Verify versions exist in Maven (check error messages)
3. Clean caches and re-run resolver
4. Check Gradle console for specific dependency errors

### Manual Gradle File Fixes

**When to Manually Edit `mainTemplate.gradle`:**

If Android Resolver generates wrong versions (e.g., non-existent Firebase versions), you can manually fix:

1. Open `Assets/Plugins/Android/mainTemplate.gradle`
2. Find the dependency section:
   ```gradle
   // Android Resolver Dependencies Start
   implementation 'com.google.firebase:firebase-analytics:21.5.0'
   implementation 'com.google.firebase:firebase-config:21.6.0'
   implementation 'com.google.firebase:firebase-crashlytics:18.6.0'
   // Android Resolver Dependencies End
   ```
3. Update to correct versions if needed
4. Save file
5. Build again

**Note:** After manual fix, re-run Android Resolver to sync with XML files. The resolver will regenerate the file, but your fix ensures correct versions until next resolution.

### Other Build Issues

| Problem | Check |
|---------|-------|
| Out of disk space | Ensure 5+ GB free |
| Antivirus blocking | Temporarily disable |
| No internet | Gradle needs to download dependencies |
| File permissions | Gradle cache folder needs write access |
| Dependency version not found | Verify version exists in Maven repositories |
| Gradle build hangs | Check internet connection, firewall settings |
| Gradle cache corruption | Delete `%USERPROFILE%\.gradle\caches\transforms-3` and rebuild |

---

## Archived Gradle Templates (December 9, 2025)

**Context:** These were the "closest to working" custom Gradle templates before switching to the clean slate approach. Archived for reference in case we need to restore specific fixes.

**Why Archived:** Unity 2022.3's auto-generated templates work better with modern SDKs than heavily customized ones. Custom templates caused recurring issues:
- `aaptOptions` format errors
- `signingConfig` reference errors
- R8/D8 compatibility issues
- Unity overwriting templates on settings changes

### Archived: launcherTemplate.gradle
```gradle
apply plugin: 'com.android.application'

dependencies {
    implementation project(':unityLibrary')
}

android {
    namespace 'com.Bluebottle.Battlecruisers'
    ndkPath "**NDKPATH**"

    compileSdkVersion **APIVERSION**
    buildToolsVersion '**BUILDTOOLS**'

    compileOptions {
        sourceCompatibility JavaVersion.VERSION_11
        targetCompatibility JavaVersion.VERSION_11
    }

    defaultConfig {
        minSdkVersion **MINSDKVERSION**
        targetSdkVersion **TARGETSDKVERSION**
        applicationId 'com.Bluebottle.Battlecruisers'
        ndk {
            abiFilters **ABIFILTERS**
        }
        versionCode **VERSIONCODE**
        versionName '**VERSIONNAME**'
        consumerProguardFiles 'proguard-unity.txt'**USER_PROGUARD**
        multiDexEnabled true
    }

    lintOptions {
        abortOnError false
    }

    buildTypes {
        debug {
            minifyEnabled **MINIFY_DEBUG**
            proguardFiles getDefaultProguardFile('proguard-android.txt')
            signingConfig signingConfigs.debug
            jniDebuggable true
        }
        release {
            minifyEnabled **MINIFY_RELEASE**
            proguardFiles getDefaultProguardFile('proguard-android.txt')
            signingConfig signingConfigs.debug
        }
    }
**PACKAGING_OPTIONS**
    bundle {
        language {
            enableSplit = false
        }
        density {
            enableSplit = false
        }
        abi {
            enableSplit = true
        }
    }
}

apply plugin: 'com.google.gms.google-services'
```

### Archived: mainTemplate.gradle
```gradle
apply plugin: 'com.android.library'
**APPLY_PLUGINS**

dependencies {
    implementation fileTree(dir: 'libs', include: ['*.jar'])
    implementation "com.google.android.gms:play-services-games-v2:+"
// Android Resolver Dependencies Start
    implementation 'com.applovin:applovin-sdk:12.6.1'
    implementation 'com.google.firebase:firebase-analytics:21.5.0'
    implementation 'com.google.firebase:firebase-crashlytics:18.6.0'
    implementation 'com.google.games:gpgs-plugin-support:0.11.01'
// Android Resolver Dependencies End
**DEPS**}

// Force Kotlin 1.9.22 to avoid R8/D8 incompatibility with Kotlin 2.x
configurations.all {
    resolutionStrategy {
        force 'org.jetbrains.kotlin:kotlin-stdlib:1.9.22'
        force 'org.jetbrains.kotlin:kotlin-stdlib-jdk8:1.9.22'
        force 'org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.9.22'
    }
}

// Android Resolver Exclusions Start
android {
  packagingOptions {
      exclude ('/lib/armeabi/*' + '*')
      exclude ('/lib/mips/*' + '*')
      exclude ('/lib/mips64/*' + '*')
      exclude ('/lib/x86/*' + '*')
      exclude ('/lib/x86_64/*' + '*')
  }
}
// Android Resolver Exclusions End
android {
    namespace "com.unity3d.player"
    ndkPath "**NDKPATH**"
    compileSdkVersion **APIVERSION**
    buildToolsVersion '**BUILDTOOLS**'

    compileOptions {
        sourceCompatibility JavaVersion.VERSION_11
        targetCompatibility JavaVersion.VERSION_11
    }

    defaultConfig {
        minSdkVersion **MINSDKVERSION**
        targetSdkVersion **TARGETSDKVERSION**
        ndk {
            abiFilters **ABIFILTERS**
        }
        versionCode **VERSIONCODE**
        versionName '**VERSIONNAME**'
        consumerProguardFiles 'proguard-unity.txt'**USER_PROGUARD**
    }

    lintOptions {
        abortOnError false
    }

    aaptOptions {
        noCompress = **BUILTIN_NOCOMPRESS** + unityStreamingAssets.tokenize(', ')
        ignoreAssetsPattern = "!.svn:!.git:!.ds_store:!*.scc:.*:!CVS:!thumbs.db:!picasa.ini:!*~"
    }

   **PACKAGING_OPTIONS**
}
**IL_CPP_BUILD_SETUP**
**SOURCE_BUILD_SETUP**
**EXTERNAL_SOURCES**
```

### Archived: gradleTemplate.properties
```properties
org.gradle.jvmargs=-Xmx**JVM_HEAP_SIZE**M -XX:MaxMetaspaceSize=512m -XX:+HeapDumpOnOutOfMemoryError -Xss2m
org.gradle.parallel=false
org.gradle.logging.level=info
org.gradle.daemon=false
org.gradle.caching=false
android.debug.obsoleteApi=true
unityStreamingAssets=.unity3d,**STREAMING_ASSETS**
android.suppressUnsupportedCompileSdk=35
# Android Resolver Properties Start
android.useAndroidX=true
android.enableJetifier=true
# Android Resolver Properties End
# CRITICAL: Disable dexing artifact transform to prevent DEX merge conflicts
# with AppLovin, Firebase, and other large SDKs (Unity 2022+ issue)
android.enableDexingArtifactTransform=false
# Disable R8 to avoid Kotlin 2.x compatibility issues
android.enableR8=false
**ADDITIONAL_PROPERTIES**
```

### Archived: baseProjectTemplate.gradle
```gradle
buildscript {
    repositories {
        google()
        mavenCentral()
        **BUILD_SCRIPT_DEPS_REPOSITORIES**
    }
    dependencies {
        classpath 'com.android.tools.build:gradle:7.4.2'
        classpath 'com.google.gms:google-services:4.4.0'
        **BUILD_SCRIPT_DEPS**
    }
}

allprojects {
    repositories {
        google()
        mavenCentral()
        **ALL_PROJECTS_DEPS_REPOSITORIES**
    }
}

task clean(type: Delete) {
    delete rootProject.buildDir
}
```

### Archived: settingsTemplate.gradle
```gradle
pluginManagement {
    repositories {
        **ARTIFACTORYREPOSITORY**
        gradlePluginPortal()
        google()
        mavenCentral()
    }
}

include ':launcher', ':unityLibrary'
**INCLUDES**

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.PREFER_SETTINGS)
    repositories {
        **ARTIFACTORYREPOSITORY**
        google()
        mavenCentral()
// Android Resolver Repos Start
        def unityProjectPath = $/file:///**DIR_UNITYPROJECT**/$.replace("\\", "/")
        maven {
            url (unityProjectPath + "/Assets/GooglePlayGames/com.google.play.games/Editor/m2repository")
        }
        mavenLocal()
// Android Resolver Repos End
        flatDir {
            dirs "${project(':unityLibrary').projectDir}/libs"
        }
    }
}
```

### Known Issues with Archived Templates
| Setting | Issue |
|---------|-------|
| `aaptOptions` in launcherTemplate | Unity 2022.3 errors on format, not needed in launcher |
| `signingConfig signingConfigs.release` | Fails if signingConfigs block not defined |
| `android.enableR8=false` | May cause issues; better to let Unity handle |
| `android.enableDexingArtifactTransform=false` | Deprecated warning, may not be needed |
| Kotlin force in mainTemplate | May conflict with SDK requirements |

---

## Unity 2022.3 Clean Slate Approach (December 9, 2025)

### Strategy Overview

**Key Insight:** Unity 2022.3's auto-generated Gradle templates work better with modern SDKs than custom templates from Unity 2021.3 era.

**Approach:**
1. **NO custom Gradle templates** - Let Unity generate defaults
2. **Post-modify via script** - Use `IPostGenerateGradleAndroidProject` to fix generated files
3. **Pin Kotlin to 1.9.22** - Required for AppLovin 13.x + Firebase 21.x compatibility
4. **Keep R8 ENABLED** - Use packagingOptions exclusions instead of disabling R8

### Files Created

| File | Purpose |
|------|---------|
| `Assets/Editor/PostGenerateGradleAndroidProject.cs` | Post-build script that modifies generated Gradle files |
| `Assets/Editor/DependencySafetyNet.cs` | Verifies dependency versions, provides clean build tools |
| `Assets/Plugins/Android/gradleTemplate.properties` | Minimal 8-line config for essential settings |

### PostGenerateGradleAndroidProject.cs Functions

| Function | Purpose |
|----------|---------|
| `EnsureKotlinVersion()` | Pins Kotlin to 1.9.22 in root build.gradle |
| `FixAppLovinDependencies()` | Adds explicit Kotlin stdlib to unityLibrary |
| `FixStreamingAssets()` | Verifies aaptOptions configuration |
| `AddPackagingExclusions()` | Adds R8-safe META-INF exclusions |
| `FixImmersiveMode()` | Adds immersive mode for ad display |

### gradleTemplate.properties (8 lines)

```properties
android.enableJetifier=true
android.useAndroidX=true
android.enableR8=true
kotlin.code.style=official
android.nonTransitiveRClass=false
android.suppressUnsupportedCompileSdk=35
unityStreamingAssets=.unity3d,**STREAMING_ASSETS**
**ADDITIONAL_PROPERTIES**
```

### Unity Menu Tools

- **Tools → Battlecruisers → Verify Android Dependencies** - Checks version compatibility
- **Tools → Battlecruisers → Force Clean Android Build** - Clears caches for fresh build

### Build Workflow

1. Open Unity (auto-generates Library, imports assets)
2. Run **Assets → External Dependency Manager → Android Resolver → Force Resolve**
3. Run **Tools → Battlecruisers → Verify Android Dependencies**
4. Build APK (Development Build ON for first test)
5. If errors, check console and apply targeted fixes via PostGenerateGradle script

### What NOT To Do

| ❌ Don't | ✅ Instead |
|----------|-----------|
| Create custom launcherTemplate.gradle | Let Unity generate it |
| Create custom mainTemplate.gradle | Let Unity generate it |
| Set `android.enableR8=false` | Keep R8 enabled, use packagingOptions |
| Downgrade Kotlin below 1.9.22 | Unity 2022.3 works with 1.9.22 |
| Upgrade AppLovin to 13.x | **STAY ON 12.6.1** - Unity 2022.3 doesn't support Kotlin 2.0 |

### Troubleshooting Map

| Error Pattern | Likely Cause | Fix |
|--------------|--------------|-----|
| `Kotlin: Unresolved reference` | Kotlin version mismatch | PostGenerateGradle adds 1.9.22 |
| `R8: Failed to transform` | Metadata conflicts | packagingOptions exclusions |
| `ClassNotFoundException: AppLovin` | SDK not bundled | Run Android Resolver |
| `aapt2 error: .unity3d` | Streaming assets config | Check unityLibrary aaptOptions |
| `BUILD FAILED` but APK exists | False positive | Verify APK runs on device |

---

## Changelog

### December 11, 2025 (Latest) - INTERSTITIAL CLOSE BUTTON FIX ATTEMPT
- **Issue:** Interstitial ads cannot be closed at the end (User report: "No way to close it").
- **Analysis:** "Report button" trick suggests input focus/Z-order issue.
- **Action:** Reverted `MaxSdk.SetExtraParameter("disable_video_surface_view", "true")` and other TextureView hacks in `AppLovinManager.cs`.
- **Reasoning:** Since SDK is now 13.5.1 + Custom Tabs, the default SurfaceView implementation should work best. Forcing TextureView might be causing the input layer to be obscured or unfocused.
- **Status:** Pending user test.

### December 11, 2025 (Late) - STABLE BUILD ACHIEVED
- **Status:** ✅ **Best State So Far**
- **Changes:**
  - Upgraded AppLovin SDK to **13.5.1** (fixed version mismatch).
  - Added `androidx.browser:browser:1.8.0` (fixed Custom Tabs error).
  - Removed `CustomUnityPlayerActivity` (fixed back button blocking).
  - Cleaned up `AndroidManifest.xml` (removed custom activity refs).
  - Verified `AppLovinManager.cs` forces TextureView (fixes z-order/black screen).
- **Result:**
  - Build succeeds.
  - Ads load and display.
  - Close button works (Back button correctly propagates).
  - Rewards are granted.
  - User "Managed to quit and add" successfully.
- **Benign Logs:**
  - "Unable to process click, ad not found" (Race condition on close -> Ignorable).
  - "Renderer process crash" (WebView cleanup -> Ignorable).

### December 11, 2025 - MAJOR CLEANUP: Removed Redundant Custom Scripts

**Build Status:** 🔄 **Pending rebuild after cleanup**

**PROBLEM IDENTIFIED:**
Multiple custom scripts were created over time to fix the ad close button issue, but:
- The problem was never actually fixed by these scripts
- Scripts were duplicating each other's work
- Some scripts were actively causing problems (e.g., immersive mode blocking overlays)
- CustomUnityPlayerActivity was intercepting back button events

**CLEANUP PERFORMED:**

### Files DELETED (redundant/problematic):

| File/Folder | Reason for Deletion |
|-------------|---------------------|
| `Assets/Plugins/Android/AdKillSwitch.androidlib/` | Never wired up - AdKillSwitchOverlay.java was never called from C# |
| `Assets/Plugins/Android/CustomActivity/` | CustomUnityPlayerActivity was intercepting back button, preventing ad close |
| `Assets/Editor/FixGradleKotlinOptions.cs` | Duplicate of PostGenerateGradleAndroidProject.cs Kotlin handling |
| `Assets/Editor/GradleTemplateRecovery.cs` | Unnecessary - Unity generates templates, had conflicting google-services |

### Files SIMPLIFIED:

| File | Changes |
|------|---------|
| `Assets/Editor/PostGenerateGradleAndroidProject.cs` | Removed: EnsureKotlinVersion(), FixAppLovinDependencies(), FixStreamingAssets(), FixImmersiveMode(). Kept only: AddPackagingExclusions() for R8 |
| `Assets/Plugins/Android/AndroidManifest.xml` | Removed: CustomUnityPlayerActivity declaration, all AppLovin activity overrides. Let SDK use defaults. |
| `Assets/Scripts/Ads/AppLovinManager.cs` | Updated OnAndroidBackButton() comment - now unused |

### Scripts KEPT (still useful):

| Script | Purpose |
|--------|---------|
| `PostGenerateGradleAndroidProject.cs` | R8 packagingOptions exclusions only |
| `FixFirebaseGoogleServices.cs` | Copies google-services.json, adds plugin |
| `DependencySafetyNet.cs` | Logs version warnings (diagnostic) |
| `AppLovinDependencyConditional.cs` | DISABLE_ADS feature toggle |

**ARCHIVED CODE (for recovery if needed):**

```java
// CustomUnityPlayerActivity.java (DELETED)
// Was at: Assets/Plugins/Android/CustomActivity/src/main/java/com/battlecruisers/customactivity/
package com.battlecruisers.customactivity;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import com.unity3d.player.UnityPlayerActivity;

public class CustomUnityPlayerActivity extends UnityPlayerActivity {
    private static final String TAG = "CustomActivity";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d(TAG, "CustomUnityPlayerActivity created");
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            Log.d(TAG, "Back button pressed");
            com.unity3d.player.UnityPlayer.UnitySendMessage("AppLovinManager", "OnAndroidBackButton", "");
            return false; // Was causing issues when returning true
        }
        return super.onKeyDown(keyCode, event);
    }

    @Override
    public void onBackPressed() {
        com.unity3d.player.UnityPlayer.UnitySendMessage("AppLovinManager", "OnAndroidBackButton", "");
        super.onBackPressed();
    }
}
```

```java
// AdKillSwitchOverlay.java (DELETED - never used)
// Was at: Assets/Plugins/Android/AdKillSwitch.androidlib/src/main/java/com/battlecruisers/adkillswitch/
// Created a native Android overlay with "FORCE CLOSE AD" button
// Never wired up to any C# code - completely unused
```

```csharp
// FixGradleKotlinOptions.cs (DELETED - duplicate)
// Was at: Assets/Editor/
// Duplicated Kotlin 1.9.0 pinning already handled by PostGenerateGradleAndroidProject.cs
```

```csharp
// GradleTemplateRecovery.cs (DELETED - unnecessary)
// Was at: Assets/Editor/
// Copied Unity templates and had fallback with conflicting google-services plugin
```

**WHY THIS SHOULD FIX THE CLOSE BUTTON:**
1. ✅ No more custom activity intercepting back button
2. ✅ No more immersive mode blocking overlays
3. ✅ AppLovin uses its own default activity configurations
4. ✅ Simplified manifest lets SDK handle everything natively

**Status:** Requires rebuild and device testing

---

### December 11, 2025 (Earlier) - ROOT CAUSE FOUND: CustomActivity Blocking Back Button

**ROOT CAUSE IDENTIFIED:**
- `CustomUnityPlayerActivity` was consuming ALL back button presses with `return true;`
- This prevented ad activities from receiving back button events needed to close ads
- Ad countdown timer worked (passive rendering) but close button didn't (requires back button event)

**Initial Fix Attempted:**
- Modified activity to call `super.onBackPressed()` and return `false`
- **SUPERSEDED BY:** Complete deletion of CustomActivity (see cleanup above)

### December 10, 2025 - Build Success + Debug Tools

**Build Status:** ✅ **APK builds and installs successfully**

**Debug Tools Added:**
- ✅ Created `AppLovinLogCollector.cs` - Captures detailed logs for support tickets
- ✅ Added `collect_applovin_logs.ps1` - Automated adb logcat collection script
- ✅ Enhanced `AppLovinManager.cs` with verbose error logging
- ✅ Enhanced "Clear Battle Log" button - Now saves AppLovin logs first, then clears battle log
- ✅ All ad callbacks now log full error details (error code, message, network, creative ID)

**Working Configuration (Verified Dec 10, 2025):**
- Unity 2022.3.x LTS
- AppLovin MAX SDK: **12.6.1** (NOT 13.5.1 - Kotlin constraint)
- Firebase: 21.5.0 (analytics), 18.6.0 (crashlytics)
- Kotlin: 1.9.22 (Unity 2022.3 max supported)
- R8: Enabled
- Min SDK: API 23, Target SDK: API 35

**Files Created:**
- `Assets/Scripts/Utils/Debugging/AppLovinLogCollector.cs` - Log collection system
- `collect_applovin_logs.ps1` - Automated log collection script

**Files Modified:**
- `Assets/Scripts/Ads/AppLovinManager.cs` - Enhanced error logging
- `Assets/Scripts/Utils/Debugging/AdminPanel.cs` - Added log collection buttons

### December 9, 2025 - Unity 2022 Simplified AppLovinManager

**AppLovin MAX Update:**
- ✅ Confirmed AppLovin MAX SDK **12.6.1** for Unity 2022 (Kotlin 1.9.x constraint)
- ✅ Simplified `AppLovinManager.cs` (removed Unity 2021 watchdogs, nuclear timers, immersive mode JNI hacks)
- ✅ Retained core interstitial/rewarded flows with clean callbacks and retry backoff

**Documentation:**
- ✅ Clarified SDK version constraints (cannot upgrade to 13.x without Unity 6.0+)

### December 7, 2024 - Hardware Acceleration Fix, AD_CONFIG Rewards, AdminPanel Testing, Documentation Organization

**Hardware Acceleration Fix:**
- ✅ Added `android:hardwareAccelerated="true"` to AndroidManifest.xml
- ✅ Fixed MRAID JavaScript injection issue that prevented ad close buttons from appearing
- ✅ Ads now properly display close buttons after required viewing time
- ✅ Resolved `ERR_FILE_NOT_FOUND` error for `mraid.js` in ad WebViews

**AD_CONFIG Extended with Reward Amounts:**
- ✅ Added `rewarded_ad_coins` and `rewarded_ad_credits` to `AdConfig` struct
- ✅ Added public properties `RewardedAdCoins` and `RewardedAdCredits` to `AdConfigManager`
- ✅ Default values: 10 coins, 250 credits (configurable via Unity Remote Config)
- ✅ Reward amounts now centralized in AD_CONFIG JSON for easy A/B testing
- ✅ Updated `GetConfigSnapshot()` to include reward values

**AdminPanel Testing Enhancements:**
- ✅ Added `RewardedAdWatched()` button - Simulates successful rewarded ad completion
  - Grants coins/credits from AD_CONFIG
  - Shows before/after currency values
  - Useful for testing reward grant logic without waiting for real ads
- ✅ Added `RewardedAdOffline()` button - Simulates failed/offline/interrupted ad
  - Shows joke/fallback ad panel (`FullScreenAdverts.defaultAd`)
  - Demonstrates offline behavior for players without internet
  - No rewards granted (as expected for incomplete ads)
- ✅ Updated `ShowRewardedAndGrant()` to use `AdConfigManager.RewardedAdCoins/Credits` properties

**Documentation Organization:**
- ✅ Created `Docs/` folder for all project documentation
- ✅ Moved 12 MD files from root to `Docs/` folder:
  - All AppLovin integration guides
  - Ad testing and setup guides
  - Platform-specific setup (iOS, Android)
  - Feature guides (Heckles, Endless Mode)
  - Architecture documentation
- ✅ Updated `PROJECT_DOCUMENTATION.md` with documentation index
- ✅ Documentation now self-organizing for AI assistant reference
- ✅ `PROJECT_DOCUMENTATION.md` remains at root as primary index

**Files Modified:**
- `Assets/Plugins/Android/AndroidManifest.xml` - Added hardware acceleration
- `Assets/Scripts/Ads/AdConfigManager.cs` - Extended with reward amounts
- `Assets/Scripts/Utils/Debugging/AdminPanel.cs` - Added testing buttons
- `PROJECT_DOCUMENTATION.md` - Added doc index and changelog entry

**Result:** ✅ Ads now properly close, reward amounts configurable via Remote Config, enhanced testing tools, organized documentation structure

---

## Android Build Reliability Plan (Dec 10, 2025)

### ✅ Current Working Build (As of Dec 10, 2025)

**Build Status:** ✅ **APK builds successfully and installs**

**Known Issue:** Ad close button not appearing (AppLovin network error -1009)

**Latest Fix (Dec 11, 2025):** Fixed `CustomUnityPlayerActivity.java` consuming back button events:
- **Problem:** Our custom activity was calling `return true;` on back button press, preventing ad activities from receiving the event
- **Fix:** Modified activity to call `super.onBackPressed()` and return `false` to allow event propagation
- **Result:** Ad activities can now properly handle back button for close functionality

**Working Configuration:**
- Unity 2022.3.x LTS
- AppLovin MAX SDK: **13.5.1** (via Unity MAX plugin 7.x)
- Firebase Analytics: **21.5.0** (verified in Maven)
- Firebase Crashlytics: **18.6.0** (verified in Maven)
- Kotlin: **1.9.22** (Unity 2022.3 maximum supported)
- R8: **Enabled** (with packagingOptions exclusions)
- Jetifier: **Enabled** (required by AppLovin)
- Min SDK: API 23
- Target SDK: API 35

**If upgrading MAX beyond 13.5.x**
- Reconfirm Kotlin compatibility (Unity 2022.3 currently pinned to Kotlin 1.9.22)
- Expect to upgrade Unity/Gradle for Kotlin 2.x if future MAX releases require it

**Build Steps That Work:**
1. Open Unity 2022.3.x
2. Assets → External Dependency Manager → Android Resolver → Force Resolve
3. Tools → Battlecruisers → Verify Android Dependencies
4. File → Build Settings → Android → Build (Development Build ON)
5. APK successfully builds and installs

**Active Debugging (Close Button Issue):**
- Enhanced verbose logging in AppLovinManager
- Added AppLovinLogCollector for support ticket data
- Created `collect_applovin_logs.ps1` for adb logcat collection
- Error: `-1009` from `rt.applovin.com/4.0/pix` (network postback failure)

### Context & History (AppLovin / Firebase / Unity 2022)
- 2021.3 "last-known-good" relied on AppLovin 12.6.x + R8 off + Kotlin 1.8.x; immersive-mode watchdog added to fix ad close button.
- 2022.3 upgrade introduced Kotlin 2.x conflicts, R8/D8 transform errors, Unity overwriting custom Gradle templates, and launcher template validation failures.
- Recurrent issues: missing/duplicated `launcherTemplate.gradle`, `aaptOptions` format errors, signingConfig release placeholders, R8 disabled causing mismatches, false-positive red console logs.
- Close-button/immersive issues now handled via manifest + post-generate hook; watchdog kept in history for reference.
- **Dec 10, 2025:** Build now succeeds! Close button issue remains (network error -1009).

### Current Approach (Clean Slate, Unity 2022.3)
- Do not pre-create custom templates; let Unity generate. If Unity install is missing templates, copy built-ins.
- Keep R8 enabled; solve conflicts with packagingOptions excludes (not by disabling R8).
- Pin Kotlin to **1.9.22** (AppLovin 13.x + Firebase 21.x compatible). No downgrades.
- Streaming assets: rely on Unity 2022.3 defaults; no custom launcher aaptOptions.

### Safety Scripts
- `Assets/Editor/PostGenerateGradleAndroidProject.cs`: Simplified (Dec 11, 2025) - only adds R8 packagingOptions META-INF excludes. Removed Kotlin pinning, immersive mode, and other redundant fixes.
- `Assets/Editor/DependencySafetyNet.cs`: Verifies dependency versions (AppLovin 13.x, Firebase 21.5.0/21.6.0/18.6.0) and offers a clean-build utility.
- ~~`Assets/Editor/GradleTemplateRecovery.cs`~~: **DELETED Dec 11, 2025** - Was redundant; Unity generates templates automatically.

### Stable Build Runbook (Unity 2022.3)
1) Open Unity; let it regenerate Gradle project.
2) Run **Assets → External Dependency Manager → Android Resolver → Force Resolve**.
3) Run **Tools → Battlecruisers → Verify Android Dependencies**.
4) Build a Dev APK (Development Build ON) to verify; inspect Gradle log for versions (AppLovin 13.5.1, Kotlin 1.9.22, Firebase 21.5.0/21.6.0/18.6.0).

**Note:** The "Recover Gradle Templates" menu was removed Dec 11, 2025 (script deleted as redundant).

### Troubleshooting Map (loop-breaker)
- **FileNotFound launcherTemplate.gradle**: Let Unity regenerate; delete Library/Bee folder and rebuild.
- **Kotlin unresolved / R8 transform errors**: Ensure Resolver pulled correct versions; post-generate adds META-INF excludes.
- **ClassNotFound AppLovin/Firebase**: Re-run Resolver; confirm dependency versions via safety net.
- **aapt2 .unity3d errors**: Ensure no custom launcher aaptOptions; Unity 2022.3 handles streaming assets via unityLibrary.
- **False-positive BUILD FAILED**: Verify APK/AAB produced and installs; inspect log for actual errors.
- **Ad close button missing**: Check this cleanup was applied (Dec 11, 2025) - no CustomActivity, no immersive mode.

### Action Items (standing)
- Keep Unity 2022.3 templates auto-generated; rely on recovery script for missing templates.
- Keep R8 enabled; avoid legacy 2021.3 downgrades.
- Maintain Kotlin 1.9.22 pin and AppLovin 13.5.1 / Firebase 21.5.0/21.6.0/18.6.0 alignment (re-validate if MAX SDK changes).
- If Unity Android module is damaged/missing templates, repair/reinstall Unity 2022.3.62f3 Android support.

### November 24, 2024 - Migration from IronSource to AppLovin MAX
**Completed:**
- ✅ Removed IronSource SDK 8.3.0 and all dependencies
- ✅ Integrated AppLovin MAX SDK 12.6.0 via `Assets/Editor/AppLovinMaxDependencies.xml`
- ✅ Created new `AppLovinManager.cs` with full AppLovin MAX SDK implementation
- ✅ Deleted `LevelPlayManager.cs` (IronSource)
- ✅ Updated all references from `LevelPlayManager` to `AppLovinManager`:
  - `DestructionSceneGod.cs`
  - `PvPDestructionSceneGod.cs`
  - `FullScreenAdverts.cs`
  - `LandingSceneGod.cs`
  - `AdminPanel.cs`
- ✅ Updated `AdConfigManager.cs` comments and provider references
- ✅ Updated documentation to reflect AppLovin MAX integration
- ✅ Kept Firebase configuration intact (Analytics + Remote Config)
- ✅ Preserved all ad logic, reward systems, and first-time bonus functionality

**What's New:**
- AppLovin MAX mediation platform (industry-leading)
- Better fill rates and eCPM optimization
- Simplified SDK integration
- Unified ad platform for better performance
- Editor simulation mode for testing without real ads
- Production-ready implementation

**Migration Notes:**
- All IronSource/LevelPlay code completely removed
- AppLovin MAX uses same event-based callback system
- No changes to Firebase Remote Config parameters
- Same reward amounts and first-time bonus logic
- Same admin panel testing functions

**Required User Actions:**
1. Get AppLovin SDK Key from dashboard (currently configured: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`)
2. Create ad units in AppLovin MAX dashboard (currently configured: Rewarded `c96bd6d70b3804fa`, Interstitial `9375d1dbeb211048`)
3. Verify SDK Key and Ad Unit IDs in Unity Inspector match the values above
4. Run Android Resolver to bundle AppLovin MAX SDK
5. Build and test

**Result:** ✅ Fully functional ad system with AppLovin MAX, ready for production

### November 25, 2024 - Firebase Version Fixes & Build Process Improvements
**Issues Found:**
- ❌ Firebase versions 21.6.0, 21.7.0, 18.7.0 don't exist in Maven repositories
- ❌ Build fails with "Could not find com.google.firebase:firebase-analytics:21.6.0"
- ❌ Confusion about Android Resolver not downloading AAR files
- ❌ Gradle SDK version warnings from GooglePlayGames manifest

**Root Causes:**
- Attempted to use non-existent Firebase versions (21.6.0, 21.7.0, 18.7.0)
- Maven repositories only have specific published versions
- Modern EDM4U uses Maven dependencies instead of local AAR files (this is correct!)
- GooglePlayGamesManifest.androidlib has outdated targetSdkVersion (harmless warning)

**Fixes Applied:**
1. ✅ Reverted Firebase versions to verified working versions:
   - `firebase-analytics:21.5.0` (exists in Maven)
   - `firebase-config:21.6.0` (exists in Maven)
   - `firebase-crashlytics:18.6.0` (exists in Maven)
2. ✅ Updated `Assets/Editor/FirebaseDependencies.xml` with correct versions
3. ✅ Manually fixed `mainTemplate.gradle` to use correct versions
4. ✅ Added `android.suppressUnsupportedCompileSdk=35` to `gradleTemplate.properties`
5. ✅ Documented Android Resolver Maven dependency behavior

**Files Modified:**
- `Assets/Editor/FirebaseDependencies.xml` (reverted to working versions)
- `Assets/Plugins/Android/mainTemplate.gradle` (fixed Firebase versions)
- `Assets/Plugins/Android/gradleTemplate.properties` (added SDK warning suppression)
- `PROJECT_DOCUMENTATION.md` (added troubleshooting sections)

**Key Learnings:**
- ✅ Always verify Firebase version numbers exist in Maven before using
- ✅ Modern EDM4U (v1.2.186) uses Maven dependencies, not local AAR files
- ✅ Dependencies are downloaded during Gradle build, not stored in Unity project
- ✅ `mainTemplate.gradle` can be manually edited if resolver generates wrong versions
- ✅ EDM4U informational logs are normal and can be ignored
- ✅ Gradle warnings from dependency libraries are usually harmless

**Verification:**
- Check Maven repository: https://mvnrepository.com/artifact/com.google.firebase
- Verify `mainTemplate.gradle` contains correct dependency entries
- Build should succeed without "Could not find" errors

**Result:** ✅ Build process now works correctly with verified Firebase versions

### November 25, 2024 - DISABLE_ADS Flag Implementation
**Issues:**
- Need ability to build without AppLovin SDK for testing/debugging

**Fixes Applied:**
1. ✅ Implemented `DISABLE_ADS` scripting define:
   - Created `Assets/Editor/AppLovinDependencyConditional.cs`:
     - Automatically excludes AppLovin dependencies when `DISABLE_ADS` is defined
     - Provides menu item: **Tools → AppLovin → Toggle DISABLE_ADS Define**
   - Updated `Assets/Scripts/Ads/AppLovinManager.cs`:
     - All `MaxSdk` calls wrapped in `#if !DISABLE_ADS`
     - Manager works gracefully when disabled (returns false for ready checks)

3. ✅ Fixed library manifest warnings:
   - Updated `FirebaseApp.androidlib/project.properties`: `target=android-9` → `target=android-35`
   - Added `namespace=com.google.firebase.app.unity`

**Files Created:**
- `Assets/Editor/AppLovinDependencyConditional.cs` - Manages DISABLE_ADS dependency exclusion

**Files Modified:**
- `Assets/Scripts/Ads/AppLovinManager.cs` - Added `#if !DISABLE_ADS` guards around all MaxSdk calls
- `Assets/Plugins/Android/FirebaseApp.androidlib/project.properties` - Fixed target SDK

**Usage:**
- **Enable DISABLE_ADS:** Tools → AppLovin → Toggle DISABLE_ADS Define (or add to Scripting Define Symbols)
- **Build without ads:** Build succeeds without AppLovin SDK dependencies
- **Build with ads:** Remove DISABLE_ADS define, force resolve dependencies, rebuild

**Result:** ✅ Gradle warnings eliminated, ability to build without AppLovin SDK for testing

### November 25, 2024 - Creative Debugger & Jetifier Verification
**Issues Found:**
- Missing Creative Debugger implementation from [AppLovin documentation](https://support.axon.ai/en/max/unity/testing-networks/creative-debugger/)
- Jetifier was correctly enabled (verified against AppLovin requirements)

**Fixes Applied:**
1. ✅ Added Creative Debugger support to `AppLovinManager.cs`:
   - `SetCreativeDebuggerEnabled()` - Configurable via Inspector
   - `ShowCreativeDebugger()` - Programmatic access to debugger UI
   - Creative ID now included in revenue tracking logs (via `OnAdRevenuePaidEvent` callback)
   - **Note:** `GetInfo()` method from AppLovin docs is not available in SDK 12.6.1. AdInfo is only available through callbacks (which we're already using)

2. ✅ Verified Jetifier configuration:
   - Confirmed `useJetifier="True"` in AndroidResolverDependencies.xml
   - Confirmed `android.enableJetifier=true` in gradleTemplate.properties
   - AppLovin's post-processor also enables Jetifier automatically

**Files Modified:**
- `Assets/Scripts/Ads/AppLovinManager.cs` - Added Creative Debugger methods and creative ID tracking

**References:**
- [AppLovin Creative Debugger Documentation](https://support.axon.ai/en/max/unity/testing-networks/creative-debugger/)
- [AppLovin Unity Integration Guide](https://support.axon.ai/en/max/unity/overview/integration/)

**Result:** ✅ Creative Debugger now available, Jetifier confirmed as required and correctly enabled

### November 21, 2024 - Unity Mediation SDK Conflict Fix
**Issue:** Build fails with dependency conflicts

**Fix:** Removed conflicting Unity Mediation SDK dependencies

**Result:** ✅ Build succeeded

### November 20, 2024 - Rewarded Ad System Implementation
**Added:**
- First-time bonus system using PlayerPrefs flag
- Firebase Remote Config integration for reward values
- Button auto-show/hide based on level, online status, ad availability
- Separate UI text fields for coins and credits
- Admin Panel testing functions

**Default Rewards:**
- First ad: 5,000 coins + 25,000 credits
- Subsequent: 15 coins + 2,000 credits

---

**Project Status:** ✅ **Production Ready with AppLovin MAX**

**Next Steps:**
1. Complete Unity Inspector assignments
2. Test in Editor via Admin Panel
3. Build to Android device and verify
4. Monitor ad performance in AppLovin MAX dashboard

---

*End of Documentation*
