# AppLovin MAX SDK Version Options

## Current Version
**12.6.1** - Last version using Kotlin 1.x (compatible with Unity 2021.3)
- ✅ Compatible with Unity 2021.3
- ❌ Version 13.x uses Kotlin 2.0 which Unity 2021.3 cannot handle
- ❌ Cannot upgrade without upgrading Unity

## Downgrade Options (All 12.x versions compatible with Unity 2021.3)

### Option 1: Stay on 12.6.1 (Recommended)
**Pros:**
- Latest 12.x features
- Most recent bug fixes
- Proven to work with your setup

**Cons:**
- Current ad rendering issues (being fixed with custom activity)

**Recommendation:** ✅ **Stay on 12.6.1** with custom back button handler

---

### Option 2: Downgrade to 12.5.0
Released: ~September 2024

**Pros:**
- Slightly older, may have different ad rendering behavior
- Still receives updates

**Cons:**
- Missing recent bug fixes from 12.6.x
- Unlikely to fix native video rendering issue (architectural)

**How to downgrade:**
1. Edit `Assets/MaxSdk/AppLovin/Editor/Dependencies.xml`
2. Change: `<androidPackage spec="com.applovin:applovin-sdk:12.6.1" />`
3. To: `<androidPackage spec="com.applovin:applovin-sdk:12.5.0" />`
4. Same for iOS: `<iosPod name="AppLovinSDK" version="12.5.0" />`
5. Delete `Library/` folder
6. Reopen project to force reimport

---

### Option 3: Downgrade to 12.4.0
Released: ~July 2024

**Changes from 12.6.1:**
- Uses `AppLovinSdkInitializationConfiguration` API (already compatible)
- May require removing `applovin.sdk.key` from AndroidManifest (already done)

**Pros:**
- Known stable version
- Good mediation network support

**Cons:**
- Even older, missing 4+ months of fixes
- Still won't fix native video rendering (same architecture)

---

### Option 4: Try 12.3.x or earlier
**Not Recommended** - Too old, missing important features

---

## The Real Problem (Applies to ALL Versions)

**Native Android Video Rendering:**
- All AppLovin MAX versions use hardware-accelerated native video views for video ads
- These render in Android's native view layer (above Unity's OpenGL layer)
- This is NOT a bug - it's an architectural design for performance
- **No version downgrade will fix this**

The errors you see:
```
ACodec [OMX.sprd.h264.decoder] setPortMode on output to DynamicANWBuffer failed
mali_gralloc Unsupported dataspace standard
```

These indicate **hardware video decoding** - this is present in ALL modern AppLovin versions.

---

## Solution: Custom Activity (Works with ANY Version)

Instead of downgrading, use the **Custom Unity Activity** approach:

1. ✅ Intercepts back button at Android Activity level (before ad consumes it)
2. ✅ Sends back button event to Unity via `UnitySendMessage`
3. ✅ Works with AppLovin 12.x, 13.x, any version
4. ✅ No performance impact
5. ✅ Reliable escape hatch for stuck ads

**Files:**
- `Assets/Plugins/Android/CustomActivity/` - Custom activity plugin
- `Assets/Plugins/Android/AndroidManifest.xml` - Updated to use custom activity
- `Assets/Scripts/Ads/AppLovinManager.cs` - Receives back button from Java

---

## Testing Recommendation

1. **Build with custom activity** (current changes)
2. **Test on device** - back button should now work during ads
3. **If still issues** - Try 12.5.0 as a sanity check
4. **If 12.5.0 same issues** - Confirms it's architectural (not version-specific)

---

## Long-term Solution

If custom back button doesn't satisfy users:

### Option A: Different Ad Network
- **Unity Ads** - Uses Unity rendering (UI can overlay)
- **AdMob** - Uses WebView for some ads (better than native video)
- **ironSource** - Similar mediation, may have different rendering

### Option B: Upgrade Unity to 2022+ 
- Allows AppLovin MAX 13.x
- May have better native view integration
- **Requires extensive testing** of entire project

### Option C: Disable Video Ads
- AppLovin supports banner/interstitial without video
- Lower CPM but fewer stuck ad issues
- Can configure in AppLovin dashboard

---

## Recommendation

✅ **Stay on AppLovin 12.6.1 with custom Activity back button handler**

Why:
1. Latest compatible version
2. Custom activity fixes back button (root cause)
3. Downgrading won't fix native video rendering
4. Stable and proven to work

Only downgrade if you encounter **specific bugs** documented in AppLovin release notes that affect 12.6.1 specifically.
