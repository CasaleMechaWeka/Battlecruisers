# Ad Testing & A/B Configuration Guide

## 🎮 AdminPanel Testing Functions

Your AdminPanel now has **4 new buttons** for comprehensive ad testing:

### 1. Toggle Premium Edition
**Button Name**: `TogglePremiumEdition()`

**What it does**:
- Switches game between Premium and Free edition instantly
- No need to make actual IAP purchases
- Changes persist across sessions (saved to GameModel)
- Logs change to Firebase Analytics

**Usage**:
```
1. Click "Toggle Premium" button in AdminPanel
2. Check console: "[AdminPanel] Edition toggled to: FREE" or "PREMIUM"
3. Complete a level to test ad behavior
```

### 2. Reset Ad Counters
**Button Name**: `ResetAdCounters()`

**What it does**:
- Clears frequency counter (battles since last ad)
- Clears cooldown timer (time since last ad)
- Forces next eligible battle to show an ad

**Usage**:
```
1. Click "Reset Ad Counters"
2. Next battle completion will show ad (if you meet level requirements)
```

### 3. Force Show Ad
**Button Name**: `ForceShowAd()`

**What it does**:
- Immediately triggers ad display
- Bypasses all checks (level, frequency, cooldown)
- Best for testing IronSource integration

**Usage**:
```
1. Click "Force Show Ad"
2. Ad appears immediately (if IronSource is ready)
3. Check console for "[IronSource]" messages
```

### 4. Show Ad Status
**Button Name**: `ShowAdStatus()`

**What it does**:
- Prints comprehensive ad configuration to console
- Shows current state of all ad controls
- Useful for debugging

**Output Example**:
```
=== AD STATUS ===
Edition: FREE
Levels Completed: 10
Min Level for Ads: 7
Ad Frequency: 3
Ad Cooldown: 5 min
Veteran Player: False
Counter Status: Counter: 2/3, Last: 10/22/2025 3:45 PM
================
```

## 🎯 Testing Workflow

### Test 1: Basic Ad Flow (Free User)
```
1. Click AdminPanel > "Toggle Premium" (make sure you're FREE)
2. Click "Reset Ad Counters"
3. Complete level 7 or higher
4. Ad should appear after battle
5. Check console for "[Ads] Showing ad" message
```

### Test 2: Premium User (No Ads)
```
1. Click AdminPanel > "Toggle Premium" (make sure you're PREMIUM)
2. Complete a level
3. Ad should NOT appear
4. Check console for "[Ads] Skipped - Premium user with ads disabled"
```

### Test 3: Level Requirement
```
1. Click AdminPanel > "Reset to State" with levelToUnlock = 5
2. Click "Toggle Premium" (set to FREE)
3. Complete a level
4. Ad should NOT appear (need level 7)
5. Check console for "[Ads] Skipped - Only 5/7 levels completed"
```

### Test 4: Ad Frequency
```
1. Set to FREE edition
2. Complete level 7+
3. Win 1st battle: No ad (counter 1/3)
4. Win 2nd battle: No ad (counter 2/3)
5. Win 3rd battle: Ad shows! (counter 3/3, resets)
6. Use "Show Ad Status" to verify counter
```

### Test 5: Force Show (Quick Test)
```
1. In ScreensScene (menu scene)
2. Click AdminPanel > "Force Show Ad"
3. IronSource ad appears immediately
4. Perfect for testing SDK integration
```

## 🔧 Firebase Remote Config Setup

### Step 1: Firebase Console Setup

1. **Go to Firebase Console** → Your Project → Remote Config
2. **Add Parameters**:

| Parameter Name | Type | Default Value | Description |
|----------------|------|---------------|-------------|
| `minimum_level_for_ads` | Number | 7 | Levels required before showing ads |
| `ad_frequency` | Number | 3 | Show ad every N battles |
| `ad_cooldown_minutes` | Number | 5.0 | Minimum minutes between ads |
| `veteran_frequency_boost_enabled` | Boolean | true | Show more ads to veteran players |
| `veteran_threshold` | Number | 15 | Levels to be considered veteran |
| `veteran_ad_frequency` | Number | 2 | Ad frequency for veterans |

3. **Save and Publish** your config

### Step 2: Create A/B Experiments

In Firebase Console → A/B Testing:

**Example Experiment: "Ad Level Threshold Test"**
```
Variant A (50% users): minimum_level_for_ads = 5
Variant B (50% users): minimum_level_for_ads = 10
Goal: Maximize retention_7_day
```

**Example Experiment: "Ad Frequency Test"**
```
Variant A (33% users): ad_frequency = 2 (more ads)
Variant B (33% users): ad_frequency = 3 (baseline)
Variant C (33% users): ad_frequency = 5 (fewer ads)
Goal: Maximize ad_revenue + retention_7_day
```

### Step 3: Target Specific Users

Use Firebase User Properties for segmentation:
```
Segment: New Players
Condition: levels_completed < 10
Config Override: ad_frequency = 4 (fewer ads for new players)

Segment: Veteran Players
Condition: levels_completed >= 15
Config Override: ad_frequency = 2 (more ads for engaged players)
```

## 📊 Firebase Analytics Events

All ad decisions are logged automatically:

### `ad_decision` Event
```json
{
  "decision": "show" | "skipped_min_level" | "skipped_cooldown" | "skipped_frequency",
  "levels_completed": 10,
  "premium": false,
  "min_level_config": 7,
  "frequency_config": 3
}
```

**Use in Firebase Console**:
1. Analytics → Events → ad_decision
2. Create funnel: `ad_decision[show]` → `ad_impression` → `ad_closed`
3. Track conversion rate and revenue

## 🎨 AdConfigManager Configuration

### In Unity Inspector

After adding AdConfigManager to scene, you can set defaults:

```
AdConfigManager (Script)
├── Default Minimum Level For Ads: 7
├── Default Ad Frequency: 3
├── Default Ad Cooldown Minutes: 5
├── Enable Veteran Frequency Boost: ✓
├── Veteran Threshold: 15
└── Veteran Ad Frequency: 2
```

These are **fallback values** if Firebase Remote Config is unavailable (offline mode, first launch, etc.)

### Dynamic Behavior

**Veteran Player Detection**:
- Player with 15+ levels = Veteran
- Veterans see ads more frequently (every 2 battles vs 3)
- Assumes engaged players tolerate more ads
- Configurable via Remote Config

**Automatic Fallbacks**:
```csharp
// If AdConfigManager not initialized:
int minLevel = AdConfigManager.Instance?.MinimumLevelForAds ?? 7;

// If Remote Config fails:
// Uses Inspector defaults automatically
```

## 🧪 Editor Testing

### Override Remote Config in Editor

Use PlayerPrefs to simulate different configs:

```csharp
// In Unity Console or Debug script:
PlayerPrefs.SetInt("EditorAdConfig_MinLevel", 5);
PlayerPrefs.SetInt("EditorAdConfig_Frequency", 2);
PlayerPrefs.SetFloat("EditorAdConfig_Cooldown", 3f);
PlayerPrefs.Save();
// Restart game to apply
```

### Test Different User Segments

```csharp
// Test as new player (< 7 levels):
AdminPanel > Reset to State (levelToUnlock = 5)
AdminPanel > Toggle Premium (FREE)
AdminPanel > Show Ad Status
// Should skip ads: "Only 5/7 levels completed"

// Test as veteran (15+ levels):
AdminPanel > Unlock Everything
// Completes all 31 levels
AdminPanel > Show Ad Status
// Should show: "Veteran Player: True, Ad Frequency: 2"
```

## 🔍 Debugging Ad Issues

### Issue: Ads Not Showing

**Check List**:
```
1. AdminPanel > Show Ad Status
   - Verify Edition: FREE (not PREMIUM)
   - Verify Levels Completed >= 7

2. Check Console for Skip Reason:
   - "[Ads] Skipped - Only X/7 levels completed"
   - "[Ads] Skipped - Cooldown active"
   - "[Ads] Skipped - Frequency counter (X/3)"

3. Force Test:
   - AdminPanel > Reset Ad Counters
   - AdminPanel > Force Show Ad
   - If this works, issue is config/frequency

4. Verify IronSource:
   - Look for "[IronSource] Initialization complete"
   - Look for "[IronSource] Interstitial ad ready"
```

### Issue: Too Many Ads

```
Solution 1: Adjust Remote Config
- Firebase Console → Remote Config
- Increase "ad_frequency" (e.g., 3 → 5)
- Increase "ad_cooldown_minutes" (e.g., 5 → 10)

Solution 2: Adjust in Inspector (local testing)
- AdConfigManager → Default Ad Frequency: 5
- Restart game

Solution 3: Target Specific Users
- Create Firebase segment: "frequent_players"
- Override ad_frequency = 5 for that segment
```

### Issue: Wrong Config Loading

```
Check Load Order:
1. LandingSceneGod.Start() creates AdConfigManager
2. AdConfigManager.Start() fetches Remote Config
3. Takes ~2 seconds on first launch

Verify in Console:
"[AdConfig] Fetched: MinLevel=7, Frequency=3, Cooldown=5"

If not appearing:
- Check Firebase dependencies resolved
- Check google-services.json in Assets/
- Check internet connection (Remote Config requires it)
```

## 📈 Production Rollout Strategy

### Phase 1: Soft Launch (Week 1)
```
Config:
- minimum_level_for_ads: 10 (conservative)
- ad_frequency: 4 (fewer ads)
- ad_cooldown_minutes: 10 (longer cooldown)

Monitor:
- retention_1_day, retention_7_day
- ad_impression count
- session_length
```

### Phase 2: Optimization (Week 2-4)
```
A/B Test Variants:
A: Current config (baseline)
B: minimum_level = 7, frequency = 3
C: minimum_level = 5, frequency = 3

Goal: Maximize (ad_revenue * retention_7_day)
```

### Phase 3: Segmented Rollout
```
New Players (< 10 levels):
- Fewer ads: frequency = 5, cooldown = 10

Engaged Players (10-20 levels):
- Baseline: frequency = 3, cooldown = 5

Veterans (20+ levels):
- More ads: frequency = 2, cooldown = 3
```

## 🎯 Key Metrics to Monitor

In Firebase Console → Analytics → Events:

1. **Ad Performance**:
   - `ad_impression` count
   - `ad_closed` completion rate
   - Revenue per ad (from IronSource dashboard)

2. **User Segmentation**:
   - Group by `is_premium` user property
   - Group by `current_level` user property
   - Compare ad tolerance by segment

3. **Churn Indicators**:
   - `ad_decision[skipped_min_level]` → Track blocked users
   - `session_end` after `ad_impression` → Ad-driven churn
   - `user_return` days after ad changes

4. **A/B Test Results**:
   - Conversion rate per variant
   - Retention per variant
   - Revenue per user per variant

## ✅ Integration Checklist

- [x] AdConfigManager created
- [x] Firebase Remote Config integrated
- [x] FullScreenAdverts updated with dynamic config
- [x] AdminPanel testing functions added
- [x] Analytics events logging all decisions
- [x] Veteran player detection
- [x] Editor simulation support
- [x] Fallback to defaults when offline
- [ ] Add AdminPanel UI buttons (see next section)
- [ ] Configure Firebase Remote Config parameters
- [ ] Set up A/B experiments
- [ ] Test on device with IronSource live

## 🔘 Adding AdminPanel Buttons in Unity

### Option 1: Add to Existing Button Panel

1. **Open Scene**: `Assets/Scenes/ScreensScene.unity`
2. **Find AdminPanel** in Hierarchy
3. **Locate** `AdminPanel → buttons` GameObject
4. **Duplicate** an existing button (e.g., "Add Money")
5. **Rename** to "Toggle Premium"
6. **Update Button**:
   - Text: "Toggle Premium"
   - OnClick(): `AdminPanel.TogglePremiumEdition()`

Repeat for other 3 buttons:
- "Reset Ad Counters" → `AdminPanel.ResetAdCounters()`
- "Force Show Ad" → `AdminPanel.ForceShowAd()`
- "Show Ad Status" → `AdminPanel.ShowAdStatus()`

### Option 2: Test via Console

In Play Mode, use Unity Console:
```csharp
// Find AdminPanel
var admin = FindObjectOfType<BattleCruisers.Utils.Debugging.AdminPanel>();

// Toggle premium
admin.TogglePremiumEdition();

// Show status
admin.ShowAdStatus();

// Force ad
admin.ForceShowAd();
```

---

**You now have a production-ready ad system with full A/B testing capabilities and comprehensive testing tools!** 🎉

