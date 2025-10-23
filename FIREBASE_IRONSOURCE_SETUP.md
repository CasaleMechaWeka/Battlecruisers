# Firebase Analytics & IronSource Ads Integration

## Overview
This document describes the Firebase Analytics and IronSource Ads integration for Battlecruisers to track player churn and show interstitial ads.

## Components Installed

### 1. Firebase Analytics
- **Location**: `Assets/Scripts/Analytics/FirebaseAnalyticsManager.cs`
- **Purpose**: Track player behavior, progression, and churn indicators
- **Configuration**: `Assets/google-services.json` (Android)

### 2. IronSource Ads
- **Location**: `Assets/Scripts/Ads/IronSourceManager.cs`
- **Purpose**: Display interstitial ads for non-premium users
- **App Key**: `23fbe1e55` (Android)

### 3. Supporting Scripts
- `Assets/Scripts/Ads/UnityMainThreadDispatcher.cs` - Handles native callbacks on main thread
- `Assets/Scripts/UI/FullScreenAdverts.cs` - Updated to use IronSource

## Dependencies

### Native SDKs (via External Dependency Manager)
- **Firebase**: `Assets/Editor/FirebaseDependencies.xml`
  - Firebase Analytics 21.5.0
  - Firebase Crashlytics 18.6.0
  
- **IronSource**: `Assets/Editor/IronSourceDependencies.xml`
  - IronSource SDK 7.8.0

## Firebase Analytics Events

### Churn Tracking Events

#### Session Events
- `session_start` - When app starts
- `session_end` - When app closes (with session length)
- `user_return` - When user returns after days away

#### Level Events (Key for Churn)
- `level_start` - Level begins
- `level_complete` - Level completed successfully
- `level_fail` - Level failed (tracks fail reason)

#### Progression Events
- `player_progression` - Overall player progress snapshot
- `tutorial_complete` / `tutorial_skip` - Tutorial engagement
- `unlock_achievement` - Achievement unlocks

#### Monetization Events
- `iap_attempt` - IAP purchase initiated
- `in_app_purchase` - Successful IAP
- `iap_failed` - Failed IAP (with reason)

#### Ad Events
- `ad_impression` - Ad shown
- `ad_closed` - Ad closed/completed
- `ad_clicked` - Ad clicked

#### Economy Events
- `earn_virtual_currency` - Coins earned
- `spend_virtual_currency` - Coins spent

#### Engagement Events
- `daily_login` - Daily login tracking
- `screen_view` - Screen navigation
- `button_click` - UI interaction

### User Properties (for Segmentation)
- `current_level` - Player's current level
- `is_premium` - Premium status
- `user_category` - Active/Casual/Returning classification

## IronSource Integration

### Ad Flow
1. **Initialization** - Auto-initializes in `LandingSceneGod.Start()`
2. **Loading** - Automatically loads interstitial ads
3. **Display** - Shows on `FullScreenAdverts.OpenAdvert()`
4. **Respects Premium** - Checks `DataProvider.GameModel.PremiumEdition`

### Testing in Editor
- Simulates ad loading (2 second delay)
- Simulates ad display (3 second duration)
- All events logged to console with `[IronSource] [Editor]` prefix

## Analytics Integration Points

### Game Start
- **File**: `Assets/Scripts/Scenes/LandingSceneGod.cs`
- **Events**: `session_start`, `player_progression`, `user_return`

### Level Complete/Fail
- **File**: `Assets/Scripts/UI/ScreensScene/PostBattleScreen/PostBattleScreenController.cs`
- **Events**: `level_complete`, `level_fail`, `tutorial_complete`

### IAP
- **File**: `Assets/Scripts/UI/IAPManager.cs`
- **Events**: `iap_attempt`, `in_app_purchase`, `iap_failed`

### Ads
- **File**: `Assets/Scripts/UI/FullScreenAdverts.cs`
- **Events**: `ad_impression`, `ad_closed`

## Churn Analysis Recommendations

### Key Metrics to Monitor in Firebase Console

1. **Day 1 Retention** - Users returning after first day
2. **Day 7 Retention** - Users returning after week
3. **Level Completion Rate** - By level number
4. **Level Failure Rate** - Where players get stuck
5. **Session Length** - Average time per session
6. **Days Since Last Session** - Returning user patterns

### Churn Indicators
- **High churn levels**: Track which levels have high `level_fail` rates
- **Drop-off points**: Monitor where `session_end` occurs frequently
- **Stuck players**: Users with repeated `level_fail` on same level
- **Inactive users**: Users with `user_return` after 7+ days

### Cohort Analysis
- Group users by `is_premium` property
- Compare engagement between premium and free users
- Track IAP conversion rates

## Build Configuration

### Android Build Settings
1. **Minimum API Level**: 21 (Android 5.0)
2. **Target API Level**: 33+ (Android 13+)
3. **Permissions** (in AndroidManifest.xml):
   - `INTERNET` - Required for ads and analytics
   - `ACCESS_NETWORK_STATE` - Check connection status

### Proguard Rules (if using)
```proguard
# IronSource
-keepclassmembers class com.ironsource.** { *; }
-dontwarn com.ironsource.**

# Firebase
-keepattributes Signature
-keepattributes *Annotation*
-keepattributes EnclosingMethod
-keepattributes InnerClasses
```

## Testing

### Firebase Events
1. Open Firebase Console
2. Go to Analytics > DebugView
3. Enable debug mode on test device:
   ```bash
   adb shell setprop debug.firebase.analytics.app com.bluebottle.battlecruisers
   ```
4. Events appear in real-time

### IronSource Ads
1. **Test Mode**: Ads automatically use test ads during development
2. **Production**: Live ads require IronSource dashboard configuration
3. **Verify**: Check IronSource dashboard for impressions

## Troubleshooting

### Firebase Not Tracking
- Verify `google-services.json` is in `Assets/` folder
- Check Android build includes Firebase dependencies
- Enable debug logging in Firebase Console
- Verify internet connection on device

### IronSource Ads Not Showing
- Verify App Key: `23fbe1e55`
- Check internet connection
- Review IronSource dashboard for account status
- Check logs for `[IronSource]` messages
- Ensure not premium user or ShowAds setting enabled

### Build Errors
- Run "Assets > External Dependency Manager > Android Resolver > Resolve"
- Clean build folder
- Reimport dependencies

## Future Enhancements

### Potential Additions
1. **Rewarded Ads** - Give players coins for watching ads
2. **Banner Ads** - Bottom banner for free users
3. **Remote Config** - Adjust ad frequency dynamically
4. **A/B Testing** - Test different ad placements
5. **Predictive Analytics** - ML-based churn prediction
6. **Push Notifications** - Re-engage users at risk of churning

### Additional Analytics
1. **Custom Funnels** - Track onboarding flow
2. **User Lifetime Value** - Revenue per user
3. **Social Features** - Track PvP engagement
4. **Performance Metrics** - FPS, crashes, load times

## Support

### Documentation
- Firebase: https://firebase.google.com/docs/analytics
- IronSource: https://developers.is.com/ironsource-mobile/unity/

### Contact
For questions about this integration, check the implementation files:
- FirebaseAnalyticsManager.cs
- IronSourceManager.cs
- FullScreenAdverts.cs

