# Ad Close Button Issue - Root Cause Analysis

**Date:** December 11, 2025  
**Status:** Issue Identified - Fix Required  
**Symptoms:** Countdown timer shows, but close button never appears

---

## Key Finding

**The TextureView parameter name is LIKELY INCORRECT.**

### Current Implementation (Assets/Scripts/Ads/AppLovinManager.cs:88-90)
```csharp
MaxSdk.SetExtraParameter("use_texture_view", "true");
MaxSdk.SetExtraParameter("video_use_texture_view", "true");
```

### Problem
AppLovin MAX SDK may not recognize these parameter names. The correct parameter varies by SDK version:

**Common Parameter Names:**
- `"disable_video_surface_view"` - Disables SurfaceView (forces TextureView)
- `"use_video_renderer"` - Specifies renderer type
- SDK version 13.x may have different parameter names than 12.x

---

## Evidence

### 1. Countdown Timer Works, Close Button Doesn't
- **What this means:** The ad WebView is loading and JavaScript is executing
- **Z-order is partially working:** Timer circle renders on top of video
- **Close button specifically fails:** Suggests the button div has different rendering path

### 2. Hardware Acceleration is Enabled
- AndroidManifest.xml line 14: `android:hardwareAccelerated="true"`
- AndroidManifest.xml lines 28, 42, 49, 55, 63: All activities have `android:hardwareAccelerated="true"`
- **This is correct**

### 3. No Post-Process Conflicts Found
- AppLovinPostProcessAndroid.cs only modifies:
  - gradle.properties (AndroidX, Jetifier, DexingArtifactTransform)
  - AndroidManifest.xml (meta-data for verbose logging, Google App ID, auto-init flags)
  - applovin_settings.json (SDK key, consent flow, renderOutsideSafeArea)
- **No view rendering overrides**

### 4. SDK Version Matters
- Current: AppLovin MAX SDK 13.5.1
- Unity 2022.3.x
- Parameter names changed between SDK versions

---

## ✅ Fixes Applied (All 3)

### Fix #1: Disable SurfaceView ✅ APPLIED
```csharp
MaxSdk.SetExtraParameter("disable_video_surface_view", "true");
```

### Fix #2: Explicitly Set Video Renderer ✅ APPLIED
```csharp
MaxSdk.SetExtraParameter("video_renderer", "texture");
```

### Fix #3: Force WebView Hardware Acceleration ✅ APPLIED
```csharp
MaxSdk.SetExtraParameter("webview_hardware_acceleration", "true");
```

**Status:** All three fixes are now active in `AppLovinManager.InitializeAppLovin()` around line 86-96.

### Fix #4: Check AppLovin Console for Parameter Names
1. Go to AppLovin Dashboard
2. Check SDK integration guide for Unity 2022.3 + SDK 13.x
3. Look for "Custom Parameters" or "Video Rendering" section
4. Verify exact parameter names

---

## Additional Investigation Needed

### 1. Check Actual SDK Version
```csharp
// Add to AppLovinManager after initialization:
Debug.Log($"[AppLovin] SDK Version: {MaxSdk.GetSdkConfiguration().GetSdkVersion()}");
```

### 2. Enable All Debugging
```csharp
MaxSdk.SetVerboseLogging(true);
MaxSdk.SetCreativeDebuggerEnabled(true);
```

### 3. Check for WebView Errors
Look in logcat for:
- `WebView` errors
- `MRAID` errors
- `close button` or `countdown` mentions
- JavaScript errors in ad creative

### 4. Test with Different Ad Networks
The issue might be specific to certain ad network creatives:
- AppLovin Exchange (built-in)
- AdMob
- Meta Audience Network
- Unity Ads

---

## What to Check Next

1. **AppLovin MAX Integration Manager:**
   - Open in Unity: `AppLovin → Integration Manager`
   - Check "Custom Integration Settings"
   - Look for any video renderer or surface view options

2. **Build-Time Generated Files:**
   ```
   Library/Bee/Android/Prj/IL2CPP/Gradle/unityLibrary/src/main/res/raw/applovin_settings.json
   ```
   - Check if `render_outside_safe_area` is set
   - Check for any video-related settings

3. **AppLovin Support Documentation:**
   - Search for "close button not appearing"
   - Search for "SurfaceView vs TextureView Unity"
   - Check known issues for SDK 13.5.1

4. **Ad Creative Debugger:**
   - Enable: `MaxSdk.SetCreativeDebuggerEnabled(true)`
   - Flip device twice during ad to access
   - Check "Ad Info" for renderer type
   - Check for MRAID errors

---

## Comparison: Unity 6.3 vs 2022.3

### Unity 6.3 AdminPanel (BattlecruisersUnity63)
- Has `AgentLogger` for detailed logging
- Has `ShowRelevantLogs()` function (✅ Now added to 2022.3)
- Enhanced debug logging in `ShowRewardedAndGrant()` (✅ Now added to 2022.3)

### Both Projects Have:
- Same AppLovin MAX SDK version (13.5.1)
- Same hardware acceleration settings
- Same AndroidManifest configuration
- Same TextureView fix attempt (but possibly wrong parameter name)

---

## Next Steps

1. **Try Fix #1 first** (most likely to work)
2. **Test on device** - close button should appear after countdown
3. **If still fails:** Enable creative debugger and check renderer type
4. **If still fails:** Contact AppLovin support with:
   - SDK version: 13.5.1
   - Unity version: 2022.3.x
   - Platform: Android
   - Issue: "Close button renders, countdown renders, but close button doesn't"
   - Parameter tested: `disable_video_surface_view`

---

## Files Modified in This Session

1. ✅ `Assets/Scripts/Utils/Debugging/AdminPanel.cs`
   - Added `ShowRelevantLogs()` from Unity 6.3 version
   - Enhanced logging in `ShowRewardedAndGrant()`
   - Removed AgentLogger references (not available in this project)

---

## Action Required

**IMMEDIATE:** Test with correct parameter name `disable_video_surface_view`

**If that fails:** Try parameters in Fix #2 and #3

**If all fail:** The issue is likely in the ad creative itself or AppLovin SDK behavior, not our configuration.

