# iOS SKAdNetwork Setup

## Overview
iOS 14+ requires SKAdNetwork IDs in your app's Info.plist to enable ad attribution tracking. This is required for Unity Ads, IronSource, and all major ad networks.

## ✅ Solution Implemented

### Automated Post-Process Build Script
**File:** `Assets/Editor/iOSSKAdNetworkPostProcess.cs`

This script automatically adds all required SKAdNetwork IDs to your iOS Info.plist during the build process.

### What It Does:
1. **Runs after every iOS build** (callback order = 1, after DisableBitcode)
2. **Finds/creates** the `SKAdNetworkItems` array in Info.plist
3. **Adds 200+ SKAdNetwork IDs** for:
   - Unity Ads
   - IronSource & all mediation partners
   - Google AdMob
   - Major ad networks (Facebook, AppLovin, Vungle, etc.)
4. **Prevents duplicates** - only adds IDs that don't already exist
5. **Logs results** to Unity console

## How to Use

### No manual setup required!

The script runs automatically when you build for iOS:

1. **Build & Run** → File → Build Settings → iOS → Build (or Build and Run)
2. **Check Unity Console** for output:
   ```
   [SKAdNetwork] ✅ Post-process complete!
   [SKAdNetwork] Added 200 new SKAdNetwork IDs
   [SKAdNetwork] Skipped 0 existing IDs
   [SKAdNetwork] Total SKAdNetwork IDs in Info.plist: 200
   ```

### Verify in Xcode

After building, you can verify in Xcode:

1. Open the generated Xcode project: `[BuildFolder]/Unity-iPhone.xcodeproj`
2. Select the Unity-iPhone target
3. Go to Info tab
4. Find `SKAdNetworkItems` array
5. Should contain 200+ entries with `SKAdNetworkIdentifier` keys

## What's Included

### Unity Ads (2 IDs)
- `4dzt52r2t5.skadnetwork`
- `bvpn9ufa9b.skadnetwork`

### IronSource + Mediation Partners (27 IDs)
- `su67r6k2v3.skadnetwork`
- `c6k4g5qg8m.skadnetwork`
- `44jx6755aq.skadnetwork`
- ...and 24 more

### Google AdMob + Partners (170+ IDs)
- Full list of Google's recommended SKAdNetwork IDs
- Includes major networks: Facebook, AppLovin, Vungle, Chartboost, etc.

## Updating IDs

As Unity Ads dashboard warned: **"Check this list monthly as it might change"**

### To update:
1. Check latest IDs from:
   - [IronSource SKAdNetwork List](https://developers.is.com/ironsource-mobile/ios/ios-14-network-support/)
   - [Unity Ads Requirements](https://docs.unity.com/ads/en/manual/SKAdNetworkIDs)
   - [Google AdMob List](https://developers.google.com/admob/ios/3p-skadnetworks)
2. Edit `Assets/Editor/iOSSKAdNetworkPostProcess.cs`
3. Add new IDs to the `skAdNetworkIds` array
4. Rebuild for iOS

## Troubleshooting

### "SKAdNetworkItems not added"
- Check Unity Console for errors during build
- Verify `UnityEditor.iOS.Xcode` package is available
- Make sure you're building for iOS platform

### "Unity Ads dashboard still shows warning"
- Do a **clean build** (delete existing build folder)
- Rebuild for iOS
- Check Xcode project's Info.plist manually
- Resubmit to App Store Connect

### "Build error in post-process"
- Check Unity Console for specific error
- Verify Info.plist exists in build output
- Try setting callback order to higher number if conflicts exist

## Notes

- **Automatic:** Runs on every iOS build
- **Safe:** Only adds missing IDs, never removes existing ones
- **Performance:** Adds ~0.5 seconds to build time
- **Maintenance:** Update list monthly as ad networks add/change IDs
- **Xcode version:** Requires Xcode 12+ for iOS 14+ SKAdNetwork support

## Related Files

- `Assets/Editor/iOSSKAdNetworkPostProcess.cs` - Main script
- `Assets/Editor/DisableBitcode.cs` - Other iOS post-process (runs first)
- Generated: `[BuildFolder]/Info.plist` - Modified by this script

---

**Status:** ⚠️ iOS ads NOT enabled yet (Android only for now)
**Purpose:** Pre-configured for when iOS ads are enabled in the future
**Last Updated:** November 2024
**Next Review:** When iOS ads are enabled

