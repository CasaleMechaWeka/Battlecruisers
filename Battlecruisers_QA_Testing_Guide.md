# Battlecruisers QA Testing Guide

## 🎯 Overview
This guide provides comprehensive testing instructions for Battlecruisers, a real-time strategy (RTS) mobile game where players build and manage battlecruisers equipped with various buildings and units. The game features multiple game modes, extensive progression systems, and AppLovin MAX monetization.

**Game Link**: [Internal Test Release](https://play.google.com/apps/internaltest/4701718057790780450)

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

#### ✅ Skirmish Mode
**Objective**: Practice against AI
```
Test Steps:
1. Select Skirmish → Choose difficulty
2. Test different AI difficulty levels
3. Verify AI aggression scales with difficulty
```
- [ ] All difficulty levels work
- [ ] AI behavior differs between difficulties
- [ ] Score tracking functions

#### ✅ Endless Mode
**Objective**: Survival gameplay
```
Test Steps:
1. Start Endless mode
2. Survive multiple waves
3. Test increasing difficulty
```
- [ ] Wave progression works
- [ ] Difficulty scaling functions
- [ ] Score tracking persists

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
```
Test Steps:
1. Complete level 7 or higher
2. Look for "Watch Ad" button after battle
3. Click button and watch ad completely
4. Verify rewards granted (first-time: 5000 coins + 25000 credits)
5. Watch second ad (subsequent: 15 coins + 2000 credits)
```

**Expected Behaviors**:
- [ ] Button appears only after level 7
- [ ] First ad shows big reward message
- [ ] Ad plays successfully
- [ ] Rewards granted immediately after ad completion
- [ ] Subsequent ads show smaller rewards
- [ ] Button disappears if ad unavailable

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
1. Use Admin Panel → Toggle Premium Edition
2. Complete multiple battles
3. Verify NO ads appear
```

**Expected Behaviors**:
- [ ] Premium users never see ads
- [ ] All ad buttons hidden
- [ ] Console shows: "Skipped - Premium user"

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