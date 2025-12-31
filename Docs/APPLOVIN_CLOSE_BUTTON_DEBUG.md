# AppLovin Close Button Issue - Debug Guide

**Issue:** Ad close button never appears when ads are displayed  
**Error:** `-1009` from `rt.applovin.com/4.0/pix`  
**Root Cause:** Likely SurfaceView z-order blocking WebView close button overlay  
**Fix Applied:** Force TextureView rendering via `MaxSdk.SetExtraParameter("use_text_view", "true")`  
**Status:** Build works ✅ | Close button fix testing 🔄  
**Date:** December 10, 2025

---

## Quick Symptom Check

- ✅ **Build succeeds** - APK builds and installs
- ✅ **App launches** - No crashes
- ✅ **Ads load** - Ad displays on screen
- ❌ **Close button** - Never appears, user stuck watching ad
- ❌ **Network error** - `Failed POST returned -1009 to rt.applovin.com/4.0/pix`

---

## Log Collection (For AppLovin Support)

### Option 1: Automated Script (Fastest)
```powershell
# From project root
.\collect_applovin_logs.ps1
```
- Reproduce the issue
- Press Ctrl+C when done
- Find: `AppLovin_Logcat_TIMESTAMP.txt`

### Option 2: In-App Collector (Best Detail)
1. Launch app with Admin Panel enabled
2. Reproduce ad close button issue
3. Open Admin Panel → "Clear Battle Log" (also saves AppLovin logs)
4. Logs saved to: `/sdcard/Download/AppLovin_Debug_*.txt`
5. Pull file: `adb pull /sdcard/Download/AppLovin_Debug_*.txt`

### Option 3: Manual adb
```powershell
adb logcat -c
adb logcat -v time *:E Unity:V AppLovinSdk:V MAX:V > applovin_debug.txt
```

---

## What to Send to AppLovin Support

**Subject:** Ad close button not appearing - Error -1009 postback failure

**Include:**
1. Log file from collection method above
2. Device model: `_____________`
3. Android version: `_____________`
4. AppLovin MAX SDK: **13.5.1**
5. Unity Version: **2022.3.x**
6. SDK Key: `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
7. Ad Units:
   - Interstitial: `9375d1dbeb211048`
   - Rewarded: `c96bd6d70b3804fa`

**Description:**
```
Ads display correctly but the close button never appears. User is stuck 
watching the ad indefinitely. Logcat shows repeated error:

"Failed POST returned -1009 in 0.034 s over wifi to rt.applovin.com/4.0/pix"

Hardware acceleration is enabled in AndroidManifest.xml. Verbose logging is 
enabled. Full logs attached.
```

---

## Current Working Build Configuration

**These settings produce a working APK:**
- Unity 2022.3.x LTS
- AppLovin MAX SDK: **13.5.1**
- Firebase Analytics: **21.5.0**
- Firebase Crashlytics: **18.6.0**
- Kotlin: **1.9.22** (pinned via PostGenerateGradleAndroidProject)
- R8: **Enabled** (with packagingOptions exclusions)
- Jetifier: **Enabled** (required by AppLovin)
- Min SDK: API 23
- Target SDK: API 35

**Build steps that work:**
1. Open Unity 2022.3.x
2. Assets → External Dependency Manager → Android Resolver → Force Resolve
3. Tools → Battlecruisers → Verify Android Dependencies
4. File → Build Settings → Android → Build (Development Build ON)
5. ✅ APK builds successfully

---

## Error Analysis

**Error Code:** `-1009`  
**Endpoint:** `rt.applovin.com/4.0/pix`  
**Type:** Network postback failure

**What `-1009` means:**
- Network request failed to complete
- Could be: DNS issue, SSL/TLS problem, proxy/firewall, or server timeout
- Ad displays but tracking postback fails
- Without successful postback, close button logic may not trigger

**Potential Causes:**
1. ⭐ **SurfaceView z-order blocking close button** (LIKELY - fix applied)
   - Video renders in SurfaceView using compositor "hole punching"
   - WebView close button overlay gets blocked despite higher z-order
   - Solution: Force TextureView rendering instead
2. Network timing issue with postback server (error -1009)
3. SSL certificate pinning or validation failure
4. Proxy/firewall blocking `rt.applovin.com`
5. Race condition between ad display and postback completion

---

## Files with Enhanced Logging

| File | Changes |
|------|---------|
| `AppLovinManager.cs` | Added verbose error logging to all callbacks |
| `AppLovinLogCollector.cs` | NEW - Captures all AppLovin events and errors |
| `AdminPanel.cs` | Added Save/Clear/Show log buttons |
| `collect_applovin_logs.ps1` | NEW - Automated log collection script |

---

## Testing Checklist

Before collecting logs for support:

- [ ] Verbose logging enabled in AppLovinManager Inspector
- [ ] Development Build enabled (for full log output)
- [ ] USB debugging enabled on device
- [ ] Device connected and authorized (`adb devices`)
- [ ] App installed and running
- [ ] Admin Panel accessible (ENABLE_CHEATS defined)
- [ ] Reproduce issue 2-3 times for consistent logs
- [ ] Check both interstitial and rewarded ads
- [ ] Note exact timestamp when close button should appear

---

## References

- AppLovin Support: https://support.applovin.com/
- Creative Debugger: https://support.axon.ai/en/max/unity/testing-networks/creative-debugger/
- Unity Integration: https://support.axon.ai/en/max/unity/overview/integration/
- PROJECT_DOCUMENTATION.md - Full project documentation

---

*This guide is specifically for the close button issue. Build process is working!*

