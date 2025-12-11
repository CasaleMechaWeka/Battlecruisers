# Log Collection for AppLovin Support

Quick guide to collect logs for the ad close button issue.

## 🎯 Issue
Ad close button never appears. Error `-1009` from `rt.applovin.com/4.0/pix`

## 📋 Prerequisites
- Android device with USB debugging enabled
- Device connected via USB
- App installed on device
- `adb` in system PATH

Verify device connection:
```powershell
adb devices
```

---

## 🚀 Method 1: Automated Script (Easiest)

```powershell
.\collect_applovin_logs.ps1
```

**What it does:**
1. Clears old logs
2. Starts collecting AppLovin + Unity + error logs
3. Saves to `AppLovin_Logcat_TIMESTAMP.txt`
4. Press **Ctrl+C** when done

**When to use:** Best for capturing logs while reproducing the issue on device

---

## 📱 Method 2: In-App Collector (Most Detail)

**Setup (one-time):**
1. Open LandingScene in Unity
2. Create empty GameObject: `AppLovinLogCollector`
3. Add Component → `AppLovinLogCollector`
4. Save scene
5. Build to device

**Usage:**
1. Launch app on device
2. Open Admin Panel (requires `ENABLE_CHEATS` scripting define)
3. Reproduce ad close button issue
4. Tap **"Clear Battle Log"** button (also saves AppLovin logs)
5. Logs saved to `/sdcard/Download/AppLovin_Debug_*.txt`

**Pull log from device:**
```powershell
adb pull /sdcard/Download/AppLovin_Debug_*.txt
```

**When to use:** Best for detailed system info + filtered AppLovin events

---

## 🔧 Method 3: Manual adb (Most Control)

**Clear previous logs:**
```powershell
adb logcat -c
```

**Start collecting:**
```powershell
adb logcat -v time *:E Unity:V AppLovinSdk:V MAX:V > applovin_debug.txt
```

**Stop:** Press **Ctrl+C**

**When to use:** When you need custom log filtering

---

## 📤 What to Send to AppLovin Support

### Required Information:
- **Log file** from any method above
- **Device model:** *(check in Settings → About Phone)*
- **Android version:** *(check in Settings → About Phone)*
- **Unity version:** 2022.3.x LTS
- **AppLovin MAX SDK:** 13.5.1
- **SDK Key:** `G4pcLyqOtAarkEgzzsKcBiIQ8Mtx9mxARSfP_wfhnMtIyW5RwTdAZ2sZD5ToV03CELZoBHBXTX6_987r4ChTp0`
- **Ad Unit IDs:**
  - Interstitial: `9375d1dbeb211048`
  - Rewarded: `c96bd6d70b3804fa`

### Issue Description Template:
```
Subject: Ad close button not appearing - Error -1009 postback failure

Description:
Ads display correctly but the close button never appears, leaving the user 
stuck watching the ad indefinitely. 

Logcat consistently shows:
"Failed POST returned -1009 in 0.034 s over wifi to rt.applovin.com/4.0/pix"

Environment:
- Unity 2022.3.x LTS
- AppLovin MAX SDK 13.5.1
- Hardware acceleration enabled in AndroidManifest.xml
- Verbose logging enabled
- Min SDK: API 23, Target SDK: API 35
- Kotlin 1.9.22, R8 enabled, Jetifier enabled

Steps to reproduce:
1. Launch app
2. Show any ad (interstitial or rewarded)
3. Ad displays but close button never appears
4. Error -1009 appears in logs

Full logs attached.
```

---

## 🔍 What the Logs Show

**Key entries to look for:**
- `[INTERSTITIAL]` or `[REWARDED]` - Ad lifecycle events
- `AppLovinSdk` - SDK internal logs
- `Failed POST returned -1009` - Network error
- `rt.applovin.com/4.0/pix` - Postback endpoint
- Error traces with stack traces

**Normal flow should be:**
1. `[REWARDED] Displayed - Network: ...`
2. `OnAdRevenuePaidEvent` - Revenue tracked
3. *(close button should appear here)*
4. `[REWARDED] Dismissed - Network: ...`

**What we see instead:**
1. `[REWARDED] Displayed - Network: ...`
2. `Failed POST returned -1009` ← **Issue is here**
3. *(close button never appears - user stuck)*

---

## 🛠️ Troubleshooting

**Script won't run:**
```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
.\collect_applovin_logs.ps1
```

**adb not found:**
- Install Android SDK Platform Tools
- Add to PATH: `C:\Users\<YourUser>\AppData\Local\Android\Sdk\platform-tools`

**Device not authorized:**
- Check device screen for "Allow USB debugging" prompt
- Accept and check "Always allow from this computer"

**No logs appearing:**
- Verify app is running: `adb shell ps | findstr Battlecruisers`
- Check logcat is not paused
- Try clearing: `adb logcat -c` then restart collection

---

## 📚 Related Files

- `APPLOVIN_CLOSE_BUTTON_DEBUG.md` - Full debug guide
- `PROJECT_DOCUMENTATION.md` - Complete project documentation
- `Assets/Scripts/Utils/Debugging/AppLovinLogCollector.cs` - In-app collector
- `Assets/Scripts/Ads/AppLovinManager.cs` - Enhanced with verbose logging

---

## ✅ Testing Checklist

Before submitting logs:
- [ ] Reproduced issue 2-3 times
- [ ] Tested both interstitial and rewarded ads
- [ ] Noted exact time/timestamp when close button should appear
- [ ] Verified verbose logging enabled in AppLovinManager
- [ ] Collected full log file
- [ ] Noted device model and Android version
- [ ] Ready to submit to AppLovin support

---

*Build is working! Close button issue is isolated and ready for support ticket.*

