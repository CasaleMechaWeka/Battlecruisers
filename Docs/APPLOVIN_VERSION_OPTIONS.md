# AppLovin MAX SDK Version Options

## Current Environment (Unity 2022.3)
- Unity: **2022.3.62f3** (LTS)
- Android targetSdkVersion: **35** (Android 15)
- AppLovin MAX Unity plugin: **12.6.1** (Kotlin 1.x)
- Firebase Unity SDK: **13.5.0** (Android analytics 20.1.2)
- Google Play Games plugin: **0.11.01**
- UGS packages (selected): Authentication 3.3.3, Remote Config 4.1.1, Purchasing 4.13.0, Netcode 1.12.0
  (see `Packages/manifest.json` for full list)

## AppLovin MAX Versions (Unity 2022.3)
- **12.6.1 (current)** — Kotlin 1.x, known stable with our project
- **13.x (latest)** — Kotlin 2.0, compatible with Unity 2022.3, includes bug fixes and better Android 14/15 support

### Compatibility Matrix (Conservative view)
| Component | Current | Known Compatible With | Notes |
|-----------|---------|-----------------------|-------|
| AppLovin MAX 13.x | Kotlin 2.0 | Unity 2022.3+ | Must retest Mali/immersive fixes; update manifests if regenerated |
| Firebase Unity 13.5.0 | GMS 20.1.2 | Unity 2022.3 | Consider bumping Android analytics lib if AppLovin 13 pulls newer play-services |
| Google Play Games 0.11.01 | Play Services v2 | Unity 2022.3 | Keep unless sign-in issues arise |
| UGS packages | see manifest | Unity 2022.3 | Update incrementally via UPM |

## Key Issues to Track (still apply after upgrade)
- **Mali GPU / Samsung**: Stuck rewarded ads, invisible close button; requires immersive re-apply in callbacks.
- **Immersive mode breakage**: Ads reset system UI flags; mitigated by `RestoreImmersiveMode()` and periodic re-apply.
- **Back button consumption**: Solved with `CustomUnityPlayerActivity` + Unity callback.
- **Manifest merges**: AppLovin activities need `android:exported="false"` and `tools:replace="android:configChanges"`.
- **AD_ID permission**: Keep `com.google.android.gms.permission.AD_ID` with `tools:replace` to prevent stripping.

## Upgrade Path (Unity 2022.3 → AppLovin 13.x)
1) Update `Assets/MaxSdk/AppLovin/Editor/Dependencies.xml` to AppLovin 13.x (Kotlin 2.0).
2) Resolve Android dependencies (EDM4U) and ensure Gradle plugin/AGP remain compatible (Unity 2022.3 supports AGP 7.1.2+).
3) Verify `Assets/Plugins/Android/AndroidManifest.xml` still contains:
   - `android:exported="false"` on AppLovin activities
   - `tools:replace="android:configChanges"` and full flags: `orientation|screenSize|keyboardHidden|screenLayout|uiMode|smallestScreenSize`
   - AD_ID permission with `tools:replace`
4) Retest immersive mode fix on rewarded/interstitial callbacks (Samsung/Mali devices).
5) Smoke test Google Play Games sign-in and Firebase events.

## Known Issues (architectural)
- Native Android video layer renders above Unity UI; overlay UI can be hidden by ad view.
- Some networks delay/omit close button; must rely on immersive re-apply + back handling.
- Mali decoding quirks can hang; periodic immersive restore often unsticks layout/close button.

## Testing Checklist (post-upgrade)
- Rewarded & interstitial: close button visible, back button works, immersive restored.
- Samsung Tab A8 (Mali): no stuck video, no infinite hangs; nuclear timer does not fire.
- Google Play Games: sign-in ok on launch.
- Firebase: events flowing; no AD_ID zeroing.
- Build with targetSdk 33–35 without manifest merge errors.

## Recommendation (now on Unity 2022.3)
- Proceed to **AppLovin MAX 13.x** with careful regression on Mali devices.
- Keep Firebase Unity SDK 13.5.0; consider bumping Android analytics lib only if resolver pulls newer play-services and tests pass.
- Leave Google Play Games at 0.11.01 unless sign-in issues appear.

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
