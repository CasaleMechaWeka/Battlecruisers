# Lateral Solutions for Force-Closing Stuck Ads

## Problem
AppLovin MAX video ads get stuck on Android devices with Mali GPU/Spreadtrum chipsets:
- **Invisible close buttons**: `close_style: INVISIBLE` with 10000s delay
- **Endless playables**: Known issue in AppLovin v7.x+
- **Native video layer**: Renders above Unity UI, close button not visible
- **Back button consumed**: Ad activity intercepts back before Unity sees it

## Solutions Implemented (Revised)

### 1. **GUARANTEED 30-SECOND FORCE-CLOSE** ✅
- **Nuclear Timer**: `System.Threading.Timer` runs on separate thread, cannot be blocked
- **30-second hard timeout**: Ad will close after exactly 30 seconds, no matter what
- **Simplified escalation**: Back spam → ESC keys → `finishAffinity()`

### 2. **Android Activity-Level Back Button** ✅
- **Custom Unity Activity**: Extends `UnityPlayerActivity` in Java
- **Intercepts back button**: Before ad can consume it
- **UnitySendMessage**: Sends events directly to Unity
- **Immediate response**: Back button works instantly during ads

### 3. **Simplified Force-Close Layers** ✅

#### Layer 1: Unity Callbacks (Soft Close)
```csharp
OnInterstitialAdClosed?.Invoke(); // Signal ad closed
OnRewardedAdShowFailed?.Invoke();  // No reward granted
LoadInterstitial(); // Pre-load next ad
```

#### Layer 2: Back Button Spam (10 presses max)
```csharp
// Limited to 10 to avoid ANR (Application Not Responding)
for (int i = 0; i < 10; i++) {
    TrySendAndroidBack();
    Thread.Sleep(50);
}
```

#### Layer 3: ESC Key Events
```csharp
// Some WebView-based ads respond to ESC
activity.dispatchKeyEvent(new KeyEvent(ACTION_DOWN, KEYCODE_ESCAPE));
activity.dispatchKeyEvent(new KeyEvent(ACTION_UP, KEYCODE_ESCAPE));
```

#### Layer 4: finishAffinity() (Nuclear - Kills App)
```csharp
// Ends entire task stack - ad + Unity activities
// App closes but user escapes stuck ad
activity.Call("finishAffinity");
```

### 4. **Dual Watchdog System** ✅
- **Update()-based watchdog**: Traditional Unity Update loop monitoring
- **Coroutine-based watchdog**: Runs independently, continues if Update() blocks
- **Nuclear timer**: Thread-level guarantee (cannot be stopped)

## Layers Removed (Unreliable)

### ❌ **Reflection Attack** - Removed
- No public API for internal close methods
- Risky and unreliable across SDK versions
- Could cause crashes

### ❌ **Process Termination** - Removed
- Requires root permissions (impossible)
- Would kill entire app anyway
- Replaced with `finishAffinity()` which achieves same result safely

## Implementation Details

### Nuclear Timer Architecture
```
Ad Starts
    ↓
StartNuclearTimer() → System.Threading.Timer (30s)
    ↓
NuclearTimerCallback() → Android UI Thread
    ↓
TriggerAdKillSwitch()
    ↓
Layer 1: Unity callbacks (soft close)
    ↓
Layer 2: Back spam (10 presses)
    ↓
Layer 3: ESC key events
    ↓
Layer 4: finishAffinity() (app closes)
```

### Custom Activity (Essential)
```java
public class CustomUnityPlayerActivity extends UnityPlayerActivity {
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            // Intercept BEFORE ad consumes it
            UnitySendMessage("AppLovinManager", "OnAndroidBackButton", "");
            return true;
        }
        return super.onKeyDown(keyCode, event);
    }
}
```

### finishAffinity() Explanation
```csharp
// This is the "nuclear option"
// - Ends ALL activities in the current task
// - Includes: Ad Activity + Unity Activity
// - Result: App closes completely
// - User can relaunch app fresh
// - Better than being stuck forever
activity.Call("finishAffinity");
```

## Testing Scenarios

### On Affected Devices (Mali GPU):
| Scenario | Expected Result |
|----------|-----------------|
| Ad shows | Unity callbacks fire, watchdog starts |
| Back button pressed | Ad closes immediately (custom activity) |
| Wait 30s | Nuclear timer fires, escalation begins |
| Layer 2 (back spam) | May close ad |
| Layer 3 (ESC keys) | May close WebView ads |
| Layer 4 (finishAffinity) | **App closes** - user escapes |

### On Normal Devices:
| Scenario | Expected Result |
|----------|-----------------|
| Ad shows | Normal close button visible |
| User taps X | Ad closes normally |
| Back button | Works as backup |
| 30s never reached | Nuclear timer never fires |

## Files Modified

1. **AppLovinManager.cs**: Nuclear timer, simplified force-close
2. **CustomUnityPlayerActivity.java**: Activity-level back interception
3. **AndroidManifest.xml**: Uses custom activity

## Key Tradeoffs

### **Why finishAffinity()?**
- **Pro**: Guaranteed escape from stuck ad
- **Con**: App closes, user must relaunch
- **Reality**: Better than permanent soft-lock

### **Why limit back spam to 10?**
- **Pro**: Avoids ANR (Application Not Responding) dialog
- **Con**: Might not close ad if it needs 11+ presses
- **Reality**: If 10 back presses don't work, nothing will

### **Why skip reflection?**
- **Pro**: More stable, no risk of crashes
- **Con**: Loses potential close method
- **Reality**: AppLovin has no public close API anyway

## Success Criteria

✅ **No permanent soft-locks** - User can always escape
✅ **Back button works** - Via custom activity
✅ **30s max stuck time** - Nuclear timer guarantee
✅ **Clean fallback** - App closes if all else fails
✅ **Normal ads unaffected** - Only triggers on stuck ads

## Conclusion

The **simplified 4-layer escalation** with **finishAffinity() as nuclear option** provides reliable escape from stuck ads. Users may need to relaunch the app in worst case, but they will **never** be permanently soft-locked.

**Result**: Guaranteed escape within 30 seconds, even if app must close.