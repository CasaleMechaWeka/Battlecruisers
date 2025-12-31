# Ad Kill Switch UI - Setup Guide

## What It Does

The kill switch UI appears **above stuck ads** after 30 seconds, allowing users to force-close ads that won't dismiss normally.

## Features

- **Countdown Timer**: Shows at 15.5s (halfway to timeout), counting down to kill switch activation
- **Force Close Button**: Big red button appears at 31s that users can tap to force-close the ad
- **Auto-trigger**: If user doesn't press the button, auto-closes at 36s as a backup
- **Android Back Press**: Also tries sending Android back button to dismiss the native ad UI
- **Renders Above Everything**: Uses maximum sort order (32767) to appear above ad overlays

## Quick Setup (Auto-Create UI)

1. **In Unity Editor:**
   - Open `LandingScene.unity`
   - Find the `AppLovinManager` GameObject (child of SceneGod)
   - Add Component → `AdKillSwitchUI` script
   - Leave `Auto Create UI` checked ✓
   - Done! The UI will create itself at runtime.

2. **Build and Test:**
   - Build to Android
   - Show an ad using AdminPanel
   - Wait 15.5 seconds → countdown appears
   - Wait 31 seconds → "FORCE CLOSE AD" button appears
   - Tap it or wait 5 more seconds for auto-close

## Manual Setup (Optional)

If you want to customize the UI in the editor:

1. Create the UI hierarchy manually in LandingScene:
   ```
   AppLovinManager (GameObject)
   ├─ AdKillSwitchUI (Component)
   └─ AdKillSwitchCanvas (Canvas)
      ├─ Background (Image - semi-transparent black)
      └─ ButtonContainer (RectTransform)
         ├─ Panel (Image - dark gray)
         ├─ TimerText (Text - countdown display)
         └─ KillSwitchButton (Button - red)
            └─ Text (Text - "FORCE CLOSE AD")
   ```

2. Set Canvas properties:
   - Render Mode: Screen Space - Overlay
   - Sort Order: 32767 (maximum)
   - Canvas Scaler: Scale With Screen Size

3. Assign references in `AdKillSwitchUI`:
   - Uncheck `Auto Create UI`
   - Drag the Canvas, Button, and Timer Text to their fields

## Inspector Settings

### AppLovinManager
- **Ad Watchdog Timeout**: Time before kill switch appears (default 31s)
- **Send Android Back On Watchdog**: Try Android back press when triggered (default ON)

### AdKillSwitchUI
- **Auto Create UI**: Create the UI programmatically (default ON)
- **Kill Switch Canvas**: Reference to the Canvas (auto-assigned)
- **Kill Switch Button**: Reference to the Button (auto-assigned)
- **Kill Switch Timer Text**: Reference to the Text (auto-assigned)

## How It Works

1. User sees an ad
2. Ad starts playing normally
3. At **15.5 seconds** (50% of timeout):
   - Semi-transparent overlay appears
   - Timer shows: "Kill switch in 15s" (counting down)
4. At **31 seconds** (timeout reached):
   - Timer changes to: "AD STUCK - TAP TO CLOSE"
   - Big red button becomes active
5. User can tap the button to force-close
6. At **36 seconds** (timeout + 5s):
   - Auto-triggers force-close as backup
   - Also sends Android back button press

## Testing

Use AdminPanel buttons:
- "Show Interstitial Ad" or "Show Rewarded Ad"
- Watch the timer appear at ~15s
- See the kill switch activate at 31s
- Verify tapping closes the ad and returns to game

## Troubleshooting

**UI doesn't appear:**
- Check that `AdKillSwitchUI` component is on the same GameObject as `AppLovinManager`
- Check Unity console for "[AdKillSwitchUI] Kill switch UI initialized" message
- Verify `Auto Create UI` is checked

**UI appears but button doesn't work:**
- Check that Canvas has a `GraphicRaycaster` component
- Verify sort order is 32767
- Check that button OnClick is wired (auto-wired in code)

**Ad still won't close:**
- The button fires callbacks so your game logic continues
- The native ad UI might still be visible (OS-level issue)
- Check logcat for "Kill switch pressed by user" or "WATCHDOG: Ad has been showing"
- Report the network/creativeId to AppLovin support

## Next Steps

After implementing the kill switch:
1. Test on multiple devices
2. Use Mediation Debugger to identify problematic networks/creatives
3. In MAX Dashboard, disable those networks or report bad creatives
4. Monitor Firebase/Analytics for "ad_watchdog_triggered" events (if you add that logging)

## Files Modified

- `Assets/Scripts/Ads/AppLovinManager.cs` - Added kill switch logic
- `Assets/Scripts/Ads/AdKillSwitchUI.cs` - NEW - UI creation script
- `AD_KILL_SWITCH_SETUP.md` - This file

