# Battlecruisers QA Testing Guide

## 🎯 Overview
This guide provides comprehensive testing instructions for Battlecruisers, a real-time strategy (RTS) mobile game where players build and manage battlecruisers equipped with various buildings and units. The game features multiple game modes, extensive progression systems, and AppLovin MAX monetization.

**Game Link**: [Internal Test Release](https://play.google.com/apps/internaltest/4701718057790780450)

---

## 📖 What is QA Testing?

**QA (Quality Assurance) testing** is the process of checking that the game works correctly before players see it. Think of it like checking a car before selling it - you test all the buttons, make sure nothing breaks, and verify everything works as expected.

### Why We Do QA Testing
- **Find bugs early**: Catch problems before players do
- **Ensure quality**: Make sure the game is fun and works well
- **Protect revenue**: Ads and purchases must work correctly
- **Player satisfaction**: Players expect a smooth experience

### What the Process Looks Like
1. **Open the app** on your test device
2. **Follow the test checklist** - try each feature step by step
3. **Take screenshots** or write notes when you find problems
4. **Report bugs** to the QA kanban board (notify Lucas or the QA lead)
5. **Test again** after fixes are made

### For New QA Testers
If you're new to game testing, don't worry! This guide explains everything step by step. Start with the **Quick Checklist** below - it covers the most important features. Once you're comfortable, move to the detailed sections.

**Remember**: Your job is to find problems, not to fix them. Report everything you find, even if you're not sure it's a bug. It's better to report too much than to miss something important.

---

## ⚡ Quick Checklist (Start Here!)

Use this checklist for a fast test of critical features. Perfect for new testers or quick verification.

### Essential Functions (5-10 minutes)
- [ ] **App launches** without crashing
- [ ] **Main menu** appears and buttons work
- [ ] **Start a battle** - Level 1 loads correctly
- [ ] **Build a building** - Can place and construct
- [ ] **Produce units** - Units spawn and move
- [ ] **Win a battle** - Victory screen appears
- [ ] **Rewarded ad button** appears after level 7+ (if free edition)
- [ ] **Watch rewarded ad** - Rewards granted correctly
- [ ] **Progress saves** - Close app, reopen, progress still there

### Ad System (Critical for Revenue)
- [ ] **Rewarded ad button** shows after completing level 7+
- [ ] **First ad** grants big reward (5000 coins + 25000 credits)
- [ ] **Second ad** grants smaller reward (15 coins + 2000 credits)
- [ ] **Interstitial ads** appear every 3 battles (free edition only)
- [ ] **Premium users** see no ads

### Progression
- [ ] **Level unlocks** work (complete level 1, level 2 unlocks)
- [ ] **New buildings unlock** as you progress
- [ ] **Currency** (coins/credits) increases correctly

**If all these pass, the game is in good shape!** For detailed testing, continue to the sections below.

---

## 🔧 Admin Panel

The Admin Panel is a special testing tool for QA testers and developers. It lets you quickly test features without playing through the entire game.

### How to Access
1. **Open the Battle Hub** (main menu screen)
2. **Look for the big spanner/wrench icon button** in the top-right corner
3. **Tap the spanner icon** - Admin Panel opens
4. **Note**: Admin Panel only appears when `ENABLE_CHEATS` is enabled in the build

### Admin Panel Buttons

#### Player Management
- **Unlock Everything** - Unlocks all levels, buildings, units, hulls
- **Reset to State** - Resets to a specific level (default: level 31)
- **Reset** - Deletes all save data (full reset)
- **Add Money** - Adds 16 coins and 32 credits
- **Remove Money** - Removes 15 coins and 30 credits
- **Show Player Status** - Shows current player info (level, coins, credits, edition)

#### Ad Testing
- **Toggle Premium Edition** - Switch between free and premium (premium = no ads)
- **Reset Ad Counters** - Clears ad frequency and cooldown timers
- **Toggle Ad Watcher Status** - Switch between first-time ad viewer and returning viewer
- **Show Interstitial If Ready** - Shows interstitial ad immediately (if loaded)
- **Show Rewarded And Grant** - Shows rewarded ad and grants rewards
- **Rewarded Ad Watched** - Simulates successful ad completion (grants rewards)
- **Rewarded Ad Offline** - Simulates failed/offline ad (shows joke ad, no rewards)
- **Show Ad Status** - Shows current ad configuration and readiness
- **Show Mediation Debugger** - Opens AppLovin MAX debugger UI

#### Content Unlocks
- **Unlock Exos** - Unlocks all captain exos (0-50)
- **Reset Exos** - Resets to only Charlie (captain 0)
- **Unlock Heckles** - Unlocks all heckles (0-278)
- **Reset Heckles** - Resets to random 3 heckles
- **Unlock Bodykits** - Unlocks all bodykits (respects premium/free edition)
- **Reset Bodykits** - Removes all bodykits (keeps Trident Prototype for premium)
- **Unlock Variants** - Unlocks all building/unit variants
- **Reset Variants** - Removes all variants

#### Battle Simulation
- **Simulate PvE Win** - Simulates a campaign victory (goes to destruction screen)
- **Simulate PvP Win** - Simulates a PvP victory
- **Simulate PvP Loss** - Simulates a PvP defeat

#### Testing & Debugging
- **Test Firebase Analytics** - Sends a test analytics event
- **Refresh Ad Config** - Reloads ad configuration from Unity Remote Config
- **Show Remote Config Details** - Displays all remote config values
- **Show Economy Status** - Shows current coins and credits
- **Show Relevant Logs** - Exports recent error/warning logs
- **Clear Battle Log** - Saves AppLovin logs and clears battle log file
- **Show Battle Log Path** - Shows where battle logs are saved
- **Force Show Rewarded Ad Offer** - Forces rewarded ad button to appear
- **Force Show Rewarded Ad Button** - Shows button directly (bypasses conditions)
- **Check AppLovin Status** - Shows AppLovin SDK initialization status
- **Test Kill Switch UI** - Tests the ad kill switch overlay visibility

#### Cloud Save Testing
- **Test Cloud Save** - Tests saving game data to cloud
- **Test Cloud Load** - Tests loading game data from cloud

### Common Testing Workflows

**Test First-Time Ad Reward:**
1. Reset → Clear all data
2. Unlock Everything → Get to level 7+
3. Simulate PvE Win → Go to destruction screen
4. Watch rewarded ad → Should get 5000 coins + 25000 credits

**Test Subsequent Ad Reward:**
1. Toggle Ad Watcher Status → Set to "ADWATCHER"
2. Simulate PvE Win → Go to destruction screen
3. Watch rewarded ad → Should get 15 coins + 2000 credits

**Test Premium Edition (No Ads):**
1. Toggle Premium Edition → Set to PREMIUM
2. Complete multiple battles → No ads should appear
3. Check Show Ad Status → Should show ads disabled

**Test Ad Frequency:**
1. Reset Ad Counters → Clear cooldown
2. Complete 3 battles → Interstitial should appear after 3rd
3. Check Show Ad Status → Verify counter and cooldown

---

## 📋 Pre-Testing Setup

### 1. Device Requirements
- **OS**: Android API 23+ (Android 6.0+)
- **Recommended**: Android API 33+ (Android 13+) for optimal performance
- **RAM**: Minimum 2GB, Recommended 4GB+
- **Storage**: 500MB free space
- **Network**: Stable internet connection required for ads and cloud saves

### 2. Test Device Preparation
1. **Install the APK** from the internal test link above
2. **Enable USB Debugging** (if testing via ADB):
   ```
   Settings → Developer Options → USB Debugging → Enable
   ```
3. **Grant Permissions**:
   - Storage access (for logs and cloud saves)
   - Network access
   - Vibration (optional)

### 3. Test Account Setup
1. **Clear App Data** before each major test session
2. **Test Different User Types**:
   - New player (0 levels completed)
   - Mid-game player (5-10 levels completed)
   - Veteran player (15+ levels completed)
   - Premium user (purchased edition)

---

## 🧪 Core Testing Areas

### 1. Installation & First Launch

#### ✅ Installation Testing
- [ ] Download completes successfully
- [ ] Installation completes without errors
- [ ] App icon appears correctly
- [ ] App launches without crashes
- [ ] Permissions requested appropriately

#### ✅ First Launch Experience
- [ ] Loading screen displays properly
- [ ] No black screens or hangs >30 seconds
- [ ] Audio plays (if enabled)
- [ ] UI elements load correctly
- [ ] Tutorial prompt appears (if applicable)

**Expected Behavior**: Clean launch, loading screen, main menu access

---

### 2. Basic Gameplay Testing

#### ✅ Core RTS Mechanics
- [ ] **Cruiser Management**: Player cruiser spawns and is controllable
- [ ] **Building Construction**: Buildings can be queued and constructed
- [ ] **Unit Production**: Units spawn from factories and move appropriately
- [ ] **Resource Management**: Credits and coins accumulate and spend correctly
- [ ] **Combat System**: Units engage enemy targets and deal/take damage

#### ✅ Camera & Controls
- [ ] **Camera Movement**: Pan, zoom, and rotate smoothly
- [ ] **Touch Controls**: Tap to select, drag to move, pinch to zoom
- [ ] **UI Responsiveness**: Buttons respond to taps
- [ ] **Gesture Recognition**: Multi-touch gestures work correctly

#### ✅ Audio & Visual Feedback
- [ ] **Sound Effects**: Building/unit actions have audio feedback
- [ ] **Visual Effects**: Explosions, construction animations play
- [ ] **UI Animations**: Menus transition smoothly
- [ ] **Particle Effects**: Combat and destruction effects display

---

### 3. Game Mode Testing

#### ✅ Campaign Mode (Primary Mode)
**Objective**: Complete levels by destroying enemy cruiser
```
Test Steps:
1. Select Campaign → Choose Level 1
2. Verify level loads with correct enemy
3. Build defensive/offensive structures
4. Produce units and engage enemy
5. Destroy enemy cruiser to win
```
- [ ] Level selection works
- [ ] Enemy AI behaves appropriately
- [ ] Victory/defeat conditions trigger correctly
- [ ] Post-battle screen shows correct rewards

#### ✅ Tutorial Mode
**Objective**: Learn game mechanics
```
Test Steps:
1. Start Tutorial mode
2. Follow tutorial prompts
3. Complete basic building/unit production
4. Win tutorial battle
```
- [ ] Tutorial hints appear at correct times
- [ ] Game pauses for tutorial steps
- [ ] Skip tutorial option works
- [ ] Progress saves after tutorial completion

---

### 4. Progression & Unlocks System

#### ✅ Level Progression
**Test Requirements**: Complete multiple campaign levels
```
Verification Points:
1. Level completion saves progress
2. New levels unlock sequentially
3. Difficulty increases appropriately
4. Rewards granted correctly
```
- [ ] Progress persists across sessions
- [ ] Level unlocks work correctly
- [ ] Difficulty scaling functions

#### ✅ Building & Unit Unlocks
**Test Requirements**: Reach unlock thresholds
```
Test Steps:
1. Check available buildings at level 1
2. Progress to level 5, 10, 15
3. Verify new buildings unlock
4. Test unlocked buildings function
```
- [ ] Building categories unlock correctly:
  - [ ] Factory (production)
  - [ ] Defense (turrets/shields)
  - [ ] Offense (weapons)
  - [ ] Tactical (support)
  - [ ] Ultra (special weapons)

---

### 5. Monetization Testing (Critical)

#### ✅ Ad System Overview
Battlecruisers uses **AppLovin MAX SDK** with:
- **Rewarded Video Ads**: Optional ads for bonus rewards (coins/credits)
- **Interstitial Ads**: Automatic ads between battles
- **Ad Frequency**: Every 3 battles (configurable)
- **Level Requirement**: Ads start at level 7
- **Premium Bypass**: Paid users skip ads

#### ✅ Rewarded Ad Testing
**Test Requirements**: Reach level 7+, Free edition

**Method 1: Normal Gameplay**
```
Test Steps:
1. Complete level 7 or higher
2. Look for "Watch Ad" button after battle
3. Click button and watch ad completely
4. Verify rewards granted (first-time: 5000 coins + 25000 credits)
5. Watch second ad (subsequent: 15 coins + 2000 credits)
```

**Method 2: Using Admin Panel (Faster)**
```
Test Steps:
1. Open Admin Panel → "Unlock Everything" (gets you to level 7+)
2. "Simulate PvE Win" → Goes to destruction screen
3. "Show Rewarded And Grant" → Shows ad and grants rewards
4. Check "Show Player Status" → Verify rewards were added
5. "Toggle Ad Watcher Status" → Switch to returning viewer
6. "Simulate PvE Win" again → "Show Rewarded And Grant"
7. Verify smaller rewards (15 coins + 2000 credits)
```

**Expected Behaviors**:
- [ ] Button appears only after level 7
- [ ] First ad shows big reward message (5000 coins + 25000 credits)
- [ ] Ad plays successfully
- [ ] Rewards granted immediately after ad completion
- [ ] Subsequent ads show smaller rewards (15 coins + 2000 credits)
- [ ] Button disappears if ad unavailable
- [ ] "Show Ad Status" shows correct ad readiness

#### ✅ Interstitial Ad Testing
**Test Requirements**: Free edition, multiple battles
```
Test Steps:
1. Complete 3 battles in sequence
2. Verify interstitial appears after 3rd battle
3. Complete ad viewing
4. Continue to next battle
```

**Expected Behaviors**:
- [ ] Ads appear every 3 battles
- [ ] No ads for premium users
- [ ] Ad frequency respects cooldown (5+ minutes between ads)
- [ ] Veteran players (>15 levels) see ads more frequently

#### ✅ Premium Edition Testing
**Test Requirements**: Toggle to premium mode
```
Test Steps:
1. Open Admin Panel (spanner icon in Battle Hub)
2. Tap "Toggle Premium Edition" button
3. Complete multiple battles
4. Verify NO ads appear
5. Check "Show Ad Status" - should show "PREMIUM" and ads disabled
```

**Expected Behaviors**:
- [ ] Premium users never see ads
- [ ] All ad buttons hidden
- [ ] Show Ad Status shows "Edition: PREMIUM"
- [ ] Interstitials and rewarded ads both disabled

---

### 6. User Interface Testing

#### ✅ Main Menu UI
- [ ] **Navigation**: All menu buttons functional
- [ ] **Visual Polish**: Clean layout, readable text
- [ ] **Settings**: Audio, graphics options work
- [ ] **Store Access**: Shop opens correctly

#### ✅ Battle UI
- [ ] **Build Menu**: Building categories accessible
- [ ] **Drone Counter**: Updates correctly
- [ ] **Resource Display**: Credits/coins show accurate values
- [ ] **Minimap**: Shows battle overview
- [ ] **Speed Controls**: 1x/2x/3x speed work

#### ✅ Post-Battle UI
- [ ] **Victory/Defeat Screens**: Display appropriate results
- [ ] **Rewards Display**: Shows coins/credits earned
- [ ] **Continue Button**: Advances to next level
- [ ] **Rewarded Ad Button**: Appears when eligible

---

### 7. Performance Testing

#### ✅ Frame Rate Testing
**Test Method**: Use Android performance monitors
```
Expected Performance:
- 30+ FPS during battles (minimum)
- 60 FPS in menus (target)
- No frame drops during combat
- Smooth camera movement
```

#### ✅ Memory Usage Testing
**Test Method**: Android Developer Options → Memory
```
Expected Behavior:
- < 500MB RAM usage during gameplay
- No memory leaks across sessions
- Proper cleanup when switching scenes
```

#### ✅ Load Time Testing
**Test Method**: Stopwatch app/scene transitions
```
Expected Load Times:
- App launch: < 30 seconds
- Scene transitions: < 5 seconds
- Battle start: < 10 seconds
- Ad loading: < 15 seconds
```

---

### 8. Compatibility Testing

#### ✅ Android Version Testing
**Test Devices**: Different Android API levels
```
API Levels to Test:
- API 23 (Android 6.0) - Minimum supported
- API 29 (Android 10) - Mid-range
- API 33 (Android 13) - Current
- API 35 (Android 15) - Latest
```

#### ✅ Device Size Testing
**Test Categories**: Phone sizes and aspect ratios
```
Screen Sizes:
- Small phones (4.5" - 5.0")
- Standard phones (5.5" - 6.2")
- Large phones (6.5"+)
- Tablets (7" - 10")
```

---

## 🐛 Bug Reporting Format

### Required Information for Each Bug Report

#### 1. Bug Classification
```
Priority: [Critical/High/Medium/Low]
Category: [Gameplay/UI/Ads/Performance/Crash/Graphics/Audio]
Platform: [Android API level + Device Model]
```

#### 2. Reproduction Steps
```
1. Detailed step-by-step reproduction
2. Expected behavior vs actual behavior
3. Frequency (Always/Sometimes/Rare)
4. Test environment details
```

#### 3. Evidence Collection
```
- Screenshots/videos of the issue
- Device logs (if accessible)
- Console output from Admin Panel
- Ad status information
- Performance metrics (FPS, memory usage)
```

### Critical Bug Categories

#### 🚨 Show-Stopper Bugs
- App crashes or force closes
- Unable to progress in campaign
- Ads break core gameplay
- Data corruption/loss

#### ⚠️ High Priority Bugs
- Ad system failures
- Major UI blocking issues
- Performance issues affecting gameplay
- Progression blockers

---

## ✅ Test Completion Checklist

### Pre-Release Verification
- [ ] **Core Gameplay**: All game modes functional
- [ ] **Progression**: Level completion and unlocks work
- [ ] **Ads**: Rewarded and interstitial ads function correctly
- [ ] **UI/UX**: All screens and interactions work smoothly
- [ ] **Performance**: Acceptable frame rates and load times
- [ ] **Compatibility**: Works on target Android versions
- [ ] **Edge Cases**: Handles network issues and low resources
- [ ] **Data Persistence**: Progress saves and syncs correctly

### Final QA Report Summary
```
Test Coverage: [Percentage]
Critical Issues: [Count]
High Priority Issues: [Count]
Medium/Low Priority Issues: [Count]
Release Readiness: [Ready/Hold]
```

---

## 📞 Support Resources

### Testing Tools
- **Admin Panel**: In-game testing tools
- **ADB Logcat**: System and app logging
- **Android Studio Profiler**: Performance analysis
- **Firebase Console**: Analytics and crash reporting

### Key Contacts
- **Development Team**: For technical clarifications
- **AppLovin Support**: For ad-related issues
- **Firebase Support**: For analytics/cloud issues

### Documentation References
- [PROJECT_DOCUMENTATION.md](PROJECT_DOCUMENTATION.md) - Technical implementation details
- [AD_TESTING_GUIDE.md](Docs/AD_TESTING_GUIDE.md) - Ad system testing procedures
- [BATTLECRUISERS_ARCHITECTURE_GUIDE.md](Docs/BATTLECRUISERS_ARCHITECTURE_GUIDE.md) - System architecture overview

---

**Remember**: Thorough testing ensures player satisfaction and revenue optimization. Focus on the ad system as it directly impacts monetization, and verify all progression paths work correctly for retention.