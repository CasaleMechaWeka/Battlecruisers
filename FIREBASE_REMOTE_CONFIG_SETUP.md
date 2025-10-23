# Firebase Remote Config Setup Guide

## 📤 Upload Configuration to Firebase

### Method 1: Firebase Console (Recommended for First Setup)

1. **Open Firebase Console**: https://console.firebase.google.com
2. **Select your project**: battlecruisers-948ef
3. **Navigate to**: Remote Config (left sidebar)
4. **Click**: "Add parameter" or "Get started"

Add these 6 parameters:

#### Parameter 1: minimum_level_for_ads
```
Name: minimum_level_for_ads
Type: Number
Default value: 7
Description: Minimum levels before showing ads
```

#### Parameter 2: ad_frequency
```
Name: ad_frequency
Type: Number
Default value: 3
Description: Show ad every N battles
```

#### Parameter 3: ad_cooldown_minutes
```
Name: ad_cooldown_minutes
Type: Number
Default value: 5.0
Description: Minimum minutes between ads
```

#### Parameter 4: veteran_frequency_boost_enabled
```
Name: veteran_frequency_boost_enabled
Type: Boolean
Default value: true
Description: Enable different frequency for veterans
```

#### Parameter 5: veteran_threshold
```
Name: veteran_threshold
Type: Number
Default value: 15
Description: Levels required to be veteran
```

#### Parameter 6: veteran_ad_frequency
```
Name: veteran_ad_frequency
Type: Number
Default value: 2
Description: Ad frequency for veteran players
```

5. **Click**: "Publish changes"

### Method 2: Import JSON File

1. **Download**: `firebase-remote-config-template.json` from your project root
2. **Firebase Console** → Remote Config
3. **Click** the three-dot menu (⋮) → "Download config file"
4. **Edit** the downloaded file or replace with template
5. **Click** three-dot menu → "Publish from file"
6. **Upload** your edited JSON
7. **Review** and **Publish**

### Method 3: Firebase CLI (Advanced)

```bash
# Install Firebase CLI
npm install -g firebase-tools

# Login
firebase login

# Select project
firebase use battlecruisers-948ef

# Deploy Remote Config
firebase deploy --only remoteconfig
```

## 🧪 Setting Up A/B Tests

### Test 1: Level Threshold Test

1. **Firebase Console** → A/B Testing → Create experiment
2. **Configure**:
   ```
   Name: Ad Level Threshold Test
   Goal: retention_7_day
   Targeting: All users, free edition only
   
   Variants:
   - Baseline (33%): minimum_level_for_ads = 7
   - Variant A (33%): minimum_level_for_ads = 5
   - Variant B (34%): minimum_level_for_ads = 10
   
   Duration: 14 days minimum
   ```

### Test 2: Ad Frequency Test

1. **Create experiment**: "Ad Frequency Optimization"
2. **Configure**:
   ```
   Goal: Maximize(ad_revenue × retention_7_day)
   
   Variants:
   - Baseline (25%): ad_frequency = 3
   - More Ads (25%): ad_frequency = 2
   - Fewer Ads (25%): ad_frequency = 4
   - Very Few (25%): ad_frequency = 5
   
   Duration: 21 days minimum
   ```

### Test 3: Veteran Boost Test

1. **Create experiment**: "Veteran Player Monetization"
2. **Target**: Users with user_property "current_level" >= 10
3. **Configure**:
   ```
   Goal: ad_revenue (from veterans)
   
   Variants:
   - Boost ON (50%): 
       veteran_frequency_boost_enabled = true
       veteran_threshold = 15
       veteran_ad_frequency = 2
   
   - Boost OFF (50%):
       veteran_frequency_boost_enabled = false
       ad_frequency = 3
   
   Duration: 30 days
   ```

## 🎯 User Segmentation Examples

### By User Property

**Target Free Users Only**:
```
Condition: user_property "is_premium" equals "false"
Override: Use standard ad config
```

**Target New Players**:
```
Condition: user_property "current_level" < 10
Override:
  minimum_level_for_ads = 10
  ad_frequency = 5
```

**Target Veterans**:
```
Condition: user_property "current_level" >= 20
Override:
  ad_frequency = 2
  ad_cooldown_minutes = 3
```

### By Platform

**Android**:
```
Condition: Platform equals "android"
Override: Standard config (balanced_monetization)
```

**iOS**:
```
Condition: Platform equals "ios"
Override: Conservative config
  minimum_level_for_ads = 10
  ad_frequency = 4
```

### By Region

**High-Value Markets** (US, CA, GB, AU, DE, JP):
```
Condition: Region in [US, CA, GB, AU, DE, JP, KR]
Override:
  ad_frequency = 4
  ad_cooldown_minutes = 7
Note: Higher CPM = need fewer impressions
```

**Emerging Markets** (IN, BR, MX, ID):
```
Condition: Region in [IN, BR, MX, ID, PH, VN, TH]
Override:
  ad_frequency = 2
  ad_cooldown_minutes = 3
Note: Lower CPM = need more impressions
```

## 📊 Recommended Testing Roadmap

### Week 1-2: Baseline Collection
```
Config: Conservative
- minimum_level_for_ads: 10
- ad_frequency: 4
- ad_cooldown_minutes: 8

Goals:
- Establish baseline retention
- Collect initial ad revenue data
- Monitor session length impact
```

### Week 3-4: Level Threshold Test
```
Test: When to introduce ads
Variants: Level 5 vs 7 vs 10
Success Metric: retention_7_day × ad_impressions_per_user
```

### Week 5-7: Frequency Optimization
```
Test: How often to show ads
Variants: Every 2, 3, 4, or 5 battles
Success Metric: ad_revenue_per_dau × retention_7_day
```

### Week 8-11: Veteran Boost Validation
```
Test: Should veterans see more ads?
Variants: Boost ON vs OFF
Success Metric: veteran_ltv (lifetime value)
```

### Week 12+: Segmented Rollout
```
Apply learnings with user segmentation:
- New players: Gentle intro
- Regular players: Optimized frequency
- Veterans: Maximum monetization
- By region: Adjust for CPM
```

## 🔍 Monitoring & Iteration

### Key Metrics Dashboard

**In Firebase Console → Analytics → Custom Dashboards**:

1. **Ad Performance**:
   - `ad_impression` count (daily)
   - `ad_closed` completion rate
   - Revenue per ad (from IronSource)
   - eCPM by region

2. **User Impact**:
   - `retention_1_day`, `retention_7_day`
   - `session_length` before/after ad
   - `session_count` per DAU
   - Churn rate by cohort

3. **Segmentation Analysis**:
   - Performance by `current_level` property
   - Performance by `is_premium` property
   - Performance by region
   - Performance by Remote Config variant

### When to Iterate

**Increase ad frequency** if:
- ✅ retention_7_day > 35%
- ✅ session_length unchanged
- ✅ Low ad revenue per DAU

**Decrease ad frequency** if:
- ⚠️ retention_7_day drops > 5%
- ⚠️ session_length decreases
- ⚠️ High `session_end` after `ad_impression`

**Adjust level threshold** if:
- Early churn (< level 7): Increase threshold
- Low ad impressions: Decrease threshold
- Specific level drop-off: Set threshold after that level

## 🚀 Quick Start Commands

### Set Production Config (Recommended First Launch)
```json
{
  "minimum_level_for_ads": 7,
  "ad_frequency": 3,
  "ad_cooldown_minutes": 5.0,
  "veteran_frequency_boost_enabled": true,
  "veteran_threshold": 15,
  "veteran_ad_frequency": 2
}
```

### Test Config in Editor
```csharp
// In Unity Console:
PlayerPrefs.SetInt("EditorAdConfig_MinLevel", 5);
PlayerPrefs.SetInt("EditorAdConfig_Frequency", 2);
PlayerPrefs.SetFloat("EditorAdConfig_Cooldown", 3f);
PlayerPrefs.Save();
// Restart game
```

### Verify Config is Working
1. Build Android APK
2. Install on device
3. Enable Firebase DebugView:
   ```bash
   adb shell setprop debug.firebase.analytics.app com.bluebottle.battlecruisers
   ```
4. Open game
5. Check Firebase Console → DebugView
6. Look for `ad_decision` events with `min_level_config` parameter

## ✅ Launch Checklist

- [ ] All 6 parameters added to Firebase Remote Config
- [ ] Default values set (balanced_monetization)
- [ ] Published to production
- [ ] Tested with DebugView on device
- [ ] Analytics events tracking (`ad_decision`, `ad_impression`)
- [ ] User properties set (`is_premium`, `current_level`)
- [ ] A/B test created for optimization
- [ ] Monitoring dashboard configured
- [ ] Alert thresholds set for retention drops

---

**Files Created**:
1. `firebase-remote-config-template.json` - Upload to Firebase
2. `firebase-remote-config-variants.json` - Reference for A/B tests
3. `FIREBASE_REMOTE_CONFIG_SETUP.md` - This guide

**Next Steps**:
1. Upload config to Firebase Console
2. Publish changes
3. Build and test on device
4. Monitor metrics for 1 week
5. Start A/B testing in Week 2

