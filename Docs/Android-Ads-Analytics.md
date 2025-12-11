# Android Ads & Analytics Snapshot (current repo state)

Documented from the contents of this workspace on 2025-12-11 (Windows). No assumptions beyond files listed here. AppLovin MAX Maven dependency currently resolves to **13.5.1** while the Unity MAX plugin stays on the **7.x** release line.

## Build & Gradle Pipeline (Unity Android export)
- `Assets/Plugins/Android/baseProjectTemplate.gradle`: uses Android Gradle Plugin 7.4.2 for both application and library modules with placeholder `**BUILD_SCRIPT_DEPS**`.
- `Assets/Plugins/Android/settingsTemplate.gradle`: includes `:launcher` and `:unityLibrary`; repositories are Google, Maven Central, mavenLocal, and the Google Play Games local m2repo injected by the Android Resolver.
- `Assets/Plugins/Android/gradleTemplate.properties`: AndroidX/Jetifier enabled, `android.suppressUnsupportedCompileSdk=35`, Kotlin style set to official; Unity streaming assets token present; no extra custom properties beyond resolver placeholders.
- `Assets/Plugins/Android/mainTemplate.gradle`: library module (`com.unity3d.player`) targets Java 11; dependencies injected by the resolver include AppLovin SDK `13.5.1`, Firebase Analytics `20.1.2`, AndroidX Browser `1.8.0`, and Google Play Games support `0.11.01`. Packaging options exclude x86/mips ABIs. Compile/target/min SDK, ABI filters, and ProGuard are driven by Unity placeholders.
- `Assets/Plugins/Android/launcherTemplate.gradle`: application module depends on `:unityLibrary`, sets Java 11, bundles split ABI only (language/density splits disabled). Minify flags are templated; signing injected via Unity placeholders.
- `Assets/Plugins/Android/libTemplate.gradle`: template for additional android libraries (namespace placeholder, Java res/assets/jni dirs configured).
- `Assets/Plugins/Android/proguard-user.txt`: present but empty (no custom ProGuard rules).

## Android Manifests & Activities
- `Assets/Plugins/Android/AndroidManifest.xml`: declares permissions `INTERNET`, `ACCESS_NETWORK_STATE`, and `com.google.android.gms.permission.AD_ID`; application is hardware-accelerated and disallows cleartext traffic. Marks Google Advertising ID property `com.google.android.gms.ads.AD_MANAGER_APP=true`. Sets `com.battlecruisers.customactivity.CustomUnityPlayerActivity` as the launcher activity (hardware-accelerated, exported). Explicit AppLovin activities declared with `android:exported="false"` and `tools:replace="android:configChanges"` for Android 12+ compliance: `AppLovinInterstitialActivity`, `AppLovinFullscreenActivity`, `MaxFullscreenActivity`, and `AppLovinRewardedInterstitialActivity`.
- `Assets/Plugins/Android/CustomActivity/AndroidManifest.xml`: placeholder manifest for the custom activity package.
- `Assets/Plugins/Android/CustomActivity/src/main/java/com/battlecruisers/customactivity/CustomUnityPlayerActivity.java`: extends `UnityPlayerActivity`; logs creation; on back key, forwards a Unity message to `AppLovinManager` method `OnAndroidBackButton` then defers to default behavior; also forwards in `onBackPressed()`.
- `Assets/Plugins/Android/AdKillSwitch.androidlib/AndroidManifest.xml`: empty application manifest for the overlay library.
- `Assets/Plugins/Android/AdKillSwitch.androidlib/src/main/java/com/battlecruisers/adkillswitch/AdKillSwitchOverlay.java`: native overlay that renders above all views; shows dimmed overlay with timer text and a “FORCE CLOSE AD” button, invoking a Unity-provided callback on close.
- Firebase manifests: `Assets/Plugins/Android/FirebaseApp.androidlib/AndroidManifest.xml` and Crashlytics variant are minimal package/version declarations; Google Play Games manifest adds metadata keys and `NativeBridgeActivity` (exported=false).
- `app-ads.txt`: populated with AppLovin DIRECT entry and multiple reseller entries.

## Unity Ads Stack (AppLovin MAX)
- `Assets/Scripts/Ads/AppLovinManager.cs`: MonoBehaviour singleton (`AppLovinManager`) with serialized SDK key and ad unit IDs (interstitial `9375d1dbeb211048`, rewarded `c96bd6d70b3804fa`). On Android, sets verbose logging (flag + `Debug.isDebugBuild`), applies AppLovin texture-view fixes via `MaxSdk.SetExtraParameter` (`disable_video_surface_view`, `video_renderer=texture`, `webview_hardware_acceleration`), then initializes MAX. Registers interstitial/rewarded callbacks, exponential backoff reload, logs impressions/revenue via `FirebaseAnalyticsManager`, and exposes events for game code. Editor simulation stubs replicate ad lifecycle. No implementation of `OnAndroidBackButton` exists in this class (the native activity sends the message but no receiver is present).
- `Assets/Scripts/Ads/AdConfigManager.cs`: Singleton fetching `AD_CONFIG` JSON from Unity Remote Config (after `UnityServices` initialized); defaults defined in inspector (min level 7, frequency 3, cooldown 9 min, veteran boost on at level 15 with frequency 2, ads live flag false, ads disabled flag false, interstitial/rewarded enabled true, reward amounts). Parses remote JSON into strongly typed struct and exposes getters and snapshot/status helpers. Stores first-rewarded-ad state in `PlayerPrefs`.
- `Assets/Scripts/Ads/MonetizationSettings.cs`: ScriptableObject defining AppLovin SDK key and platform ad unit IDs (Android IDs match those serialized in `AppLovinManager`), test mode and GDPR consent flags; includes validators.
- `Assets/Scripts/Ads/AdDebugLogger.cs`: Persistent logger writing to `Application.persistentDataPath/AdDebugLog.txt`, duplicating output to Unity/Logcat with `[AdLog]` tag; starts with device/system info header.
- `Assets/Scripts/Ads/UnityMainThreadDispatcher.cs`: queue to marshal actions onto Unity main thread from native callbacks.
- `Assets/Scripts/Ads/IMediationManager.cs`: interface placeholder for mediation abstraction (unused; `AppLovinManager` does not implement it).

## Analytics Stack
- `Assets/Scripts/Analytics/FirebaseAnalyticsManager.cs`: MonoBehaviour singleton using AndroidJava calls to `com.google.firebase.analytics.FirebaseAnalytics` (Android only; Editor simulated). Initializes in `Start()`, enables collection, logs session start/end on app pause/resume and quit, and exposes a wide set of event helpers (level lifecycle, IAP attempts/success/fail, ad events including impressions/closed/clicked/rewarded, player progression, economy, UI, retention). Sets user properties for segmentation. Uses `BattleCruisers.Ads` namespace; no iOS-specific path implemented.
- Firebase config: `Assets/Plugins/Android/FirebaseApp.androidlib/res/values/google-services.xml` contains project `battlecruisers-34d87` with app id `1:137527829113:android:2bae6e75193cd8b1a3eaa8` and API key `AIzaSyBU8ZKVnT3XRNE_hUb9LAdkz3COn21wURQ`.

## Remote Config Assets (JSON)
- `firebase-remote-config-template.json`: documents baseline ad knobs (min level 7, frequency 3, cooldown 5, veteran boost enabled, threshold 15, veteran frequency 2) with descriptions.
- `firebase-remote-config-UPLOAD.json`: upload-ready config with master switches (`ads_enabled` true, interstitials disabled by default, rewarded enabled), min level 7, frequency 3 (veterans frequency 1, cooldown 8), rewarded reward amounts (first-time 5000 coins/25000 credits, recurring 15/2000), provider flags (`ad_provider` 0 meaning LevelPlay only; AppLovin test percentage 0).
- `firebase-remote-config-variants.json`: describes A/B variants and segmentation strategies for monetization (conservative/balanced/aggressive/etc.) with recommended test setups and metrics; informational only, not consumed by code.

## Diagnostics & Tooling
- `collect_applovin_logs.ps1`: PowerShell helper to clear logcat and capture verbose Unity/AppLovin/MAX logs to `AppLovin_Logcat_<timestamp>.txt` while reproducing issues (filters Unity/AppLovin tags).

## Observations, Gaps, Risks (from code state)
- The native activity sends `UnityPlayer.UnitySendMessage("AppLovinManager", "OnAndroidBackButton", "")`, but `AppLovinManager` does not implement that method; handling for back presses during ads is unclear.
- AppLovin keys/ad unit IDs are hard-coded in scripts/ScriptableObject assets; no runtime switch for test/live beyond verbose logging and `AdConfigManager` flags, and `AppLovinManager` does not currently read `AdConfigManager` values.
- Remote Config in code expects `AD_CONFIG` (JSON) via Unity Remote Config, while the Firebase Remote Config JSON files use different keys (e.g., `ads_enabled`, `interstitial_ads_enabled`), indicating these assets are documentation/templates rather than directly consumed.
- ProGuard user file is empty; no custom keep rules beyond Unity defaults and resolver-generated entries.

## Active Assets Relevant to Ads/Analytics
- AppLovin SDK: declared in Gradle templates and configured via `AppLovinManager` and `MonetizationSettings`.
- Firebase Analytics: included via Gradle dependency `20.1.2` and manual AndroidJava integration in `FirebaseAnalyticsManager`; Google services values present.
- Google Play Games: dependency and manifest present (activity exported=false).
- AdKillSwitch overlay: native Java overlay available; manifest present but integration point in C# not located in reviewed files.


