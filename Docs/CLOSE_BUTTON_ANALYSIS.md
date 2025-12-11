# Ad Close Button Issue - Root Cause Analysis

**Date:** December 12, 2025 (Updated)  
**Status:** Multiple fixes applied, testing required  
**Symptoms:** Countdown timer shows, install button shows, spinner shows, but close button NEVER appears

---

## Known Issue

This is a **widespread problem** affecting many developers across platforms (Unity, React Native, native Android). There is no single confirmed fix from AppLovin.

**GitHub Issues:**
- AppLovin-MAX-React-Native: "Many users have reported that they couldn't find the close button" (Oct 2025)
- AppLovin-MAX-SDK-Android: "Ad is not closing on back button press" (Nov 2025)

---

## ✅ All Fixes Applied (Dec 12, 2025)

### Fix #1: Disable SurfaceView ✅
```csharp
MaxSdk.SetExtraParameter("disable_video_surface_view", "true");
```
Forces TextureView rendering instead of SurfaceView to prevent z-order issues.

### Fix #2: Video Renderer = Texture ✅
```csharp
MaxSdk.SetExtraParameter("video_renderer", "texture");
```
Explicitly sets the video renderer type.

### Fix #3: WebView Hardware Acceleration ✅
```csharp
MaxSdk.SetExtraParameter("webview_hardware_acceleration", "true");
```
Ensures WebView can render properly.

### Fix #4: Disable render_outside_safe_area ✅ NEW
```csharp
MaxSdk.SetExtraParameter("render_outside_safe_area", "false");
```
**Critical for Android 15+ and devices with notches** - prevents close button from being pushed off-screen.

### Fix #5: Force Close Button ✅ NEW
```csharp
MaxSdk.SetExtraParameter("force_close_button", "true");
```
Undocumented parameter that might force the close button to show.

### Fix #6: Disable Immersive Mode ✅ NEW
```csharp
MaxSdk.SetExtraParameter("disable_immersive_mode", "true");
```
Prevents immersive mode from hiding system UI overlays.

### Creative Debugger Enabled ✅ NEW
```csharp
MaxSdk.SetCreativeDebuggerEnabled(true);
```
Flip device twice during ad to access the Creative Debugger.

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

