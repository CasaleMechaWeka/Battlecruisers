# AppLovin Support Ticket Draft

## Issue: Video Ads Stuck / No Close Button on Android (Native Rendering Issue)

### Environment
*   **Unity Version**: 2021.3.x (LTS)
*   **AppLovin MAX SDK Version**: 12.6.1 (Android)
*   **Platform**: Android
*   **Device Architecture**: ARM64 / Android
*   **Mediation**: AppLovin MAX (No other networks involved in reproduction)

### Problem Description
On certain Android devices, Interstitial and Rewarded video ads fail to display the close (X) button after the video completes. The ad renders in a native view on top of the Unity application, but because the close button is missing, the user is stuck in the ad indefinitely.

We have observed that:
1.  The `OnAdDisplayedEvent` callback fires.
2.  The video plays.
3.  **No close button appears** visually.
4.  The standard Android Back button is consumed by the ad activity and does not dismiss the ad.
5.  Watchdog timers implemented in Unity `Update()` continue to run (mostly), but since the ad view covers the entire screen (including Unity UI), our custom "Force Close" UI is hidden behind the native ad view.

### Logs / Errors
We are seeing specific native video decoding errors in `logcat` when the ad plays, which suggests a native rendering issue on specific hardware (Mali GPU / Spreadtrum decoders):

```text
Error ACodec [OMX.sprd.h264.decoder] setPortMode on output to DynamicANWBuffer failed w/ err -2147483648
Error mali_gralloc Unsupported dataspace standard (1159284880), format (100)
Error chromium Renderer process crash detected (code -1)
```

It appears the native video view is rendering, but the overlay layer (which typically contains the Close button and mute controls) fails to render or is obstructed.

### Steps to Reproduce
1.  Initialize AppLovin MAX SDK 12.6.1 in a Unity 2021.3 project.
2.  Load an Interstitial or Rewarded Ad.
3.  Call `MaxSdk.ShowInterstitial(adUnitId)` or `MaxSdk.ShowRewardedAd(adUnitId)`.
4.  On affected devices (e.g., devices using Mali GPU/Spreadtrum chipsets), the video ad plays but no close button appears.
5.  User is soft-locked.

### Questions
1.  Is there a known incompatibility with specific Android video decoders (OMX.sprd) in SDK 12.6.1?
2.  Is there a configuration to force ads to use `TextureView` or `WebView` instead of `SurfaceView` to avoid these hardware decoder overlays?
3.  Why does the ad activity consume the Back button event without closing the ad if the close button is missing?

### Attempted Workarounds
*   Implemented a custom Activity to intercept `onBackPressed` to force-close the ad (partial success).
*   Attempted to render Unity UI on top (failed due to Android View hierarchy limitations).
