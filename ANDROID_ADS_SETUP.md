# Android Ads Setup

## Overview
Configuration for IronSource ads and Firebase Analytics on Android.

## ✅ What's Configured

### 1. AndroidManifest.xml
**File:** `Assets/Plugins/Android/AndroidManifest.xml`

#### Permissions Added:
```xml
<!-- Required for ads -->
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />

<!-- Optional: For ad targeting (Android 13+) -->
<uses-permission android:name="com.google.android.gms.permission.AD_ID" />
```

#### Application Metadata:
```xml
<!-- Google Advertising ID declaration (Android 13+ requirement) -->
<property
    android:name="com.google.android.gms.ads.AD_MANAGER_APP"
    android:value="true" />

<!-- IronSource App Key -->
<meta-data
    android:name="ironsource_app_key"
    android:value="23fbe1e55" />
```

#### IronSource Activities:
Three activities declared for fullscreen ads:
- `ControllerActivity` - Main ad controller
- `InterstitialActivity` - Interstitial ads
- `OpenUrlActivity` - Ad clickthrough handling

### 2. Firebase Configuration
**File:** `Assets/google-services.json`
- Package name: `com.Bluebottle.Battlecruisers`
- Already configured for Android app
- Unity automatically includes this in Android builds

### 3. IronSource Integration
**App Key:** `23fbe1e55`
- Set in AndroidManifest.xml (line 23)
- Also set in code via `IronSourceManager.cs`

## Platform-Specific Notes

### Android vs iOS Differences

| Feature | Android | iOS |
|---------|---------|-----|
| **Ads Enabled** | ✅ Yes | ❌ No (not yet) |
| **Ad SDK** | IronSource | N/A |
| **Ad Types** | Interstitial + Rewarded | N/A |
| **Attribution Setup** | Google Ad ID | SKAdNetwork IDs |
| **Config File** | google-services.json | GoogleService-Info.plist |
| **Manifest** | AndroidManifest.xml | Info.plist |

### Why This Setup?

1. **`INTERNET` permission** - Required for downloading ads
2. **`ACCESS_NETWORK_STATE` permission** - Check connectivity before loading ads
3. **`AD_ID` permission** - Allows Google Advertising ID access for attribution (Android 13+)
4. **Google Ad Manager property** - Declares app uses Google ads for proper attribution
5. **IronSource activities** - Required for fullscreen ad display
6. **`configChanges` attribute** - Prevents activity restart on rotation during ads

## Testing on Android

### 1. Build and Install
```bash
# In Unity: File → Build Settings → Android → Build and Run
```

### 2. Check Logcat
```bash
# Enable logcat filtering
adb logcat -s Unity IronSource Firebase

# Watch for:
# [IronSource] SDK initialized
# [IronSource] Interstitial ad loaded
# [Firebase] Analytics initialized
```

### 3. Expected Console Output
```
[IronSourceManager] Initializing IronSource with App Key: 23fbe1e55
[IronSourceManager] IronSource initialized successfully
[FirebaseAnalyticsManager] Firebase Analytics initialized
[FullScreenAdverts] Showing IronSource interstitial ad
```

### 4. Test Ads
- Play through level 7+ (minimum level for ads)
- Complete a battle
- Ad should show on post-battle screen
- Premium users: Toggle premium off in AdminPanel to test

## Firebase Analytics on Android

### Enable Debug Mode
```bash
# Enable DebugView for your device
adb shell setprop debug.firebase.analytics.app com.Bluebottle.Battlecruisers

# Disable when done
adb shell setprop debug.firebase.analytics.app .none.
```

### Events to Monitor
- `ad_impression` - When ad is shown
- `ad_closed` - When ad is dismissed
- `level_complete` - When level beaten
- `iap_attempt` / `iap_success` - Purchase events
- `rewarded_ad_completed` - When rewarded ad watched

## Troubleshooting

### "Ads not showing"
1. **Check Premium Status:** `DataProvider.GameModel.PremiumEdition` should be `false`
2. **Check Level:** Must have completed level 7+
3. **Check Logcat:** Look for IronSource initialization errors
4. **Check App Key:** Verify `23fbe1e55` is correct in AndroidManifest.xml

### "Google Ad ID not accessible"
- Ensure `com.google.android.gms.permission.AD_ID` permission is in manifest
- User may have denied permission in Android 13+ settings
- Check `Settings → Google → Ads → Ad privacy` on device

### "IronSource activities not found"
- Clean build folder
- Rebuild for Android
- Verify AndroidManifest.xml merged correctly in `Temp/StagingArea/AndroidManifest.xml`

### "Firebase not tracking events"
- Check `Assets/google-services.json` exists
- Verify package name matches: `com.Bluebottle.Battlecruisers`
- Enable DebugView with adb command (see above)
- Wait 1-2 seconds for events to appear in Firebase Console
- Check Unity console for Firebase initialization logs

## Android 13+ (API 33+) Requirements

### Google Advertising ID
Starting with Android 13 (API 33), apps must declare usage of the Advertising ID:

✅ **Already configured in AndroidManifest.xml:**
```xml
<uses-permission android:name="com.google.android.gms.permission.AD_ID" />
```

### User Permission
- Users can now opt-out of Ad ID tracking in system settings
- Your app will still work, but ad targeting may be less effective
- IronSource SDK handles this automatically

## Google Play Store Requirements

### App Ads Declaration
When submitting to Google Play Console:

1. **Declare Ad Content:**
   - App content → Ads → Yes, my app contains ads
   
2. **Declare Ad Types:**
   - ☑ Interstitial ads
   - ☑ Rewarded ads
   
3. **Declare Ad Networks:**
   - IronSource
   - (Any mediation partners IronSource uses)

4. **Data Safety:**
   - Declare you collect Advertising ID
   - Purpose: Advertising and Analytics
   - Data is shared with third parties (IronSource, Firebase)

## Related Files

### Android-Specific
- `Assets/Plugins/Android/AndroidManifest.xml` - Main manifest with permissions
- `Assets/google-services.json` - Firebase configuration
- `Assets/Scripts/Ads/IronSourceManager.cs` - Ad initialization
- `Assets/Scripts/Analytics/FirebaseAnalyticsManager.cs` - Analytics

### Cross-Platform
- `Assets/Scripts/UI/FullScreenAdverts.cs` - Interstitial ad display
- `Assets/Scripts/Scenes/DestructionSceneGod.cs` - Rewarded ads
- `Assets/Scripts/Ads/AdConfigManager.cs` - Remote Config for A/B testing

### iOS-Specific (Not Yet Used)
- `GoogleService-Info.plist` - Firebase config for iOS
- `Assets/Editor/iOSSKAdNetworkPostProcess.cs` - SKAdNetwork setup (iOS only)
- `IOS_SKADNETWORK_SETUP.md` - iOS attribution setup (for future)

---

**Status:** ✅ Android ads fully configured
**Current Platform:** Android only
**iOS Ads:** Not enabled yet
**Last Updated:** November 2024

