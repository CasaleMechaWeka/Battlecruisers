# Questions for AppLovin MAX SDK Developers

**Context:** Unity 2022.3.x LTS, AppLovin MAX SDK 13.5.1, Android (API 23+)  
**Issue:** Ad countdown timer shows but close button never appears  
**Current Setup:** Hardware acceleration enabled, 3 TextureView parameters applied

---

## 🔴 CRITICAL - Close Button Issue

### 1. TextureView/SurfaceView Parameters
**Q:** What are the **official, correct parameter names** for forcing TextureView rendering in AppLovin MAX SDK **13.5.1** for Unity?

We've tried:
- `"disable_video_surface_view"` = true
- `"video_renderer"` = "texture"  
- `"webview_hardware_acceleration"` = true
- Previously tried (didn't work): `"use_texture_view"`, `"video_use_texture_view"`

**Which of these are actually recognized by SDK 13.5.1?**

### 2. Demo App Implementation
**Q:** In your official Unity demo app, how do you handle the close button rendering? 
- What parameters do you use in `MaxSdk.SetExtraParameter()`?
- Do you set them before or after `MaxSdk.InitializeSdk()`?
- Are there any additional Unity project settings required?

### 3. Close Button Z-Order
**Q:** Our countdown timer shows correctly but close button doesn't. What does this indicate?
- Does this mean the WebView is partially rendering but the close button div specifically is blocked?
- Is this a known issue with certain ad networks or creative types?
- Should the close button be part of the video renderer or the WebView overlay?

### 4. Hardware Acceleration
**Q:** We have `android:hardwareAccelerated="true"` everywhere in AndroidManifest. Is this sufficient, or are there additional settings needed?
- Should we set anything in Unity's Player Settings?
- Are there specific activity theme requirements?
- Does `PlayerSettings.Android.renderOutsideSafeArea` affect ad rendering?

---

## 🟡 SDK Configuration

### 5. Initialization Sequence
**Q:** What is the correct initialization sequence for MaxSdk parameters?

Our current order:
```csharp
MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
MaxSdk.SetSdkKey(sdkKey);
MaxSdk.SetVerboseLogging(true);
MaxSdk.SetExtraParameter("disable_video_surface_view", "true");
MaxSdk.SetExtraParameter("video_renderer", "texture");
MaxSdk.SetExtraParameter("webview_hardware_acceleration", "true");
MaxSdk.InitializeSdk();
```

**Is this order correct? Should any parameters be set AFTER InitializeSdk()?**

### 6. Unity 2022.3 Compatibility
**Q:** Are there any known issues or special configurations for Unity 2022.3.x LTS?
- Does it require different parameters than Unity 2021.3?
- Are there Gradle or Android build settings specific to 2022.3?
- Should we use a different SDK version for Unity 2022.3?

### 7. SDK Version Recommendations
**Q:** We're using AppLovin MAX SDK **13.5.1**. 
- Is this the recommended version for Unity 2022.3 + Android?
- Are there newer versions that fix close button issues?
- Have there been changes to parameter names between 12.x and 13.x?

---

## 🟢 Ad Networks & Mediation

### 8. Network-Specific Issues
**Q:** Does the close button issue occur with specific ad networks?
- Does AppLovin Exchange (your own network) work correctly?
- Are there known issues with AdMob, Meta, Unity Ads regarding close buttons?
- Should we test with only AppLovin Exchange first to isolate the issue?

### 9. MRAID & WebView
**Q:** How does the close button integrate with MRAID?
- Is the close button injected by the SDK or provided by the ad creative?
- Should we see MRAID initialization logs in logcat?
- What WebView errors indicate close button issues?

### 10. Creative Debugger
**Q:** When using `MaxSdk.SetCreativeDebuggerEnabled(true)`:
- What should "Renderer Type" show (TextureView vs SurfaceView)?
- Are there specific error codes related to close button rendering?
- What logs indicate a z-order or view rendering problem?

---

## 🔵 Best Practices

### 11. AndroidManifest Configuration
**Q:** Our AndroidManifest has these AppLovin activities:
```xml
<activity android:name="com.applovin.adview.AppLovinInterstitialActivity"
          android:hardwareAccelerated="true"
          android:theme="@android:style/Theme.NoTitleBar.Fullscreen" />
          
<activity android:name="com.applovin.mediation.ads.MaxFullscreenActivity"
          android:hardwareAccelerated="true" />
```

**Are these configurations optimal? Should we set any additional attributes?**

### 12. Gradle Configuration
**Q:** Our gradle.properties has:
```properties
android.useAndroidX=true
android.enableJetifier=true
android.enableDexingArtifactTransform=false
```

**Are there Gradle settings that affect ad rendering or WebView behavior?**

### 13. Proguard/R8
**Q:** We have R8 enabled. Are there specific ProGuard rules needed for:
- WebView rendering in ads?
- Close button overlays?
- MRAID functionality?

---

## 🟣 Debugging & Diagnostics

### 14. Logcat Keywords
**Q:** What should we search for in logcat to diagnose close button issues?

Current searches:
- `*:E Unity:V AppLovinSdk:V MAX:V`
- `"close button"`, `"MRAID"`, `"WebView"`, `"SurfaceView"`, `"TextureView"`

**Are there specific log tags or error codes that indicate close button problems?**

### 15. Network Error -1009
**Q:** We see this error when ads play:
```
Failed POST returned -1009 in 0.034 s over wifi to rt.applovin.com/4.0/pix
```

**Does this postback failure affect close button rendering? Is this related to our issue?**

### 16. Testing Configuration
**Q:** How should we test close button rendering?
- Should we use test mode or production mode?
- Does test mode use different ad creatives that might work differently?
- Are there specific ad unit IDs for testing close button functionality?

---

## 🟤 Unity-Specific Integration

### 17. Unity Player Settings
**Q:** Are there Unity Player Settings that affect ad rendering?

Current settings:
- Min SDK: API 23
- Target SDK: API 35  
- IL2CPP scripting backend
- Render outside safe area: TRUE

**Should any of these be different for optimal ad rendering?**

### 18. Canvas & UI Overlays
**Q:** We have a kill-switch UI (Canvas with sort order 32767) for stuck ads.
- Does a high-z-order Unity Canvas interfere with ad WebView overlays?
- Should we disable our kill-switch Canvas when ads are showing?
- How does Unity's Canvas system interact with native Android Views?

### 19. Scene Management
**Q:** Our AppLovinManager is DontDestroyOnLoad and persists across scenes.
- Does scene loading affect ad rendering or WebView state?
- Should we reinitialize anything when loading new scenes?
- Are there Unity lifecycle events we should handle?

---

## 🔶 Common Gotchas

### 20. Known Issues List
**Q:** What are the most common reasons for close button not appearing?

Our checklist so far:
- ✅ Hardware acceleration enabled
- ✅ Correct SDK version
- ✅ Activity configurations in manifest
- ✅ TextureView parameters set (we think)
- ✅ Verbose logging enabled
- ❓ Correct parameter names?
- ❓ Missing configuration?

**What else should we check?**

### 21. Device/OS Variations
**Q:** Does this issue occur on specific:
- Android versions? (we're on Android 12+)
- Device manufacturers? (Samsung, Pixel, etc.)
- Screen sizes or densities?
- GPU types?

### 22. Timing Issues
**Q:** Could this be a timing/race condition?
- Does the close button require a network callback to appear?
- Should we wait for specific SDK events before showing ads?
- Is there a minimum ad display time before close button shows?

---

## 🟠 Documentation & Examples

### 23. Official Code Example
**Q:** Can you provide the **exact code from your Unity demo app** for:
- AppLovin manager initialization with correct parameters
- Showing interstitial ads with proper callbacks
- AndroidManifest configuration
- Any custom Unity editor scripts or post-processors

### 24. Integration Documentation
**Q:** Is there Unity 2022.3-specific documentation?
- Are there blog posts or guides about TextureView vs SurfaceView?
- Are there known issues documented for SDK 13.x?
- Is there a troubleshooting guide specifically for close button issues?

### 25. Demo App Access
**Q:** Can we access your official Unity demo app source code?
- GitHub repository link?
- Unity package with working example?
- Video walkthrough of proper setup?

---

## 📋 Quick Summary for Support Ticket

If asking via support ticket, include:

**Environment:**
- Unity: 2022.3.x LTS
- AppLovin MAX SDK: 13.5.1
- Android: API 23-35
- Kotlin: 1.9.22
- R8: Enabled
- Jetifier: Enabled

**Issue:**
- Ad video plays ✅
- Countdown timer shows ✅  
- Close button never appears ❌
- Network error: -1009 from rt.applovin.com/4.0/pix

**Attempts:**
1. Hardware acceleration enabled everywhere
2. Applied 3 TextureView parameters (may be wrong names)
3. No Proguard rules blocking WebView
4. Verbose logging enabled
5. All activities configured correctly

**Question:** What are the correct `MaxSdk.SetExtraParameter()` names for forcing TextureView in SDK 13.5.1?

---

## 🎯 Priority Ranking

**Ask these FIRST (most critical):**
1. Question 1 - Official parameter names for SDK 13.5.1
2. Question 2 - Demo app implementation
3. Question 5 - Initialization sequence
4. Question 23 - Exact code example

**Ask these SECOND (diagnosis):**
5. Question 3 - Close button z-order explanation
6. Question 14 - Logcat keywords  
7. Question 15 - Network error -1009 relationship
8. Question 20 - Known issues checklist

**Ask these THIRD (optimization):**
9. Question 6 - Unity 2022.3 compatibility
10. Question 11 - AndroidManifest best practices
11. Question 17 - Unity Player Settings

---

## 📎 Attachments to Include

If submitting via email/ticket, attach:
1. `AppLovinManager.cs` (our implementation)
2. `AndroidManifest.xml` (our configuration)
3. Logcat output showing the issue
4. Screenshot of ad with countdown but no close button
5. `PROJECT_DOCUMENTATION.md` (our full setup)

---

**Last Updated:** December 11, 2025  
**Status:** Awaiting AppLovin developer feedback

