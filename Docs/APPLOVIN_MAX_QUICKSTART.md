# AppLovin MAX Quick Start Guide

## 5-Minute Setup

### 1. Download & Import AppLovin MAX SDK

**Option A: Unity Asset Store** (Easiest)
```
1. Open Unity Asset Store
2. Search "AppLovin MAX"
3. Download & Import
```

**Option B: Direct Download**
```
1. Go to: https://dash.applovin.com/documentation/mediation/unity/getting-started
2. Download .unitypackage
3. In Unity: Assets → Import Package → Custom Package
```

### 2. Create AppLovin Account & Get Keys

```
1. Go to: https://dash.applovin.com
2. Create account (free)
3. Create new app:
   - Platform: Android
   - Package Name: com.Bluebottle.Battlecruisers
4. Copy your SDK Key (looks like: abc123def...)
5. Create Ad Units:
   a) Interstitial Ad Unit → Copy Ad Unit ID
   b) Rewarded Video Ad Unit → Copy Ad Unit ID
```

### 3. Create MonetizationSettings in Unity

```
1. In Unity Project window:
   Assets → Create → BattleCruisers → Monetization Settings

2. Name it: MonetizationSettings

3. Move to: Assets/Resources/ folder (IMPORTANT!)

4. Select it in Inspector and fill in:
   ┌─────────────────────────────────────────────┐
   │ AppLovin MAX Configuration                  │
   ├─────────────────────────────────────────────┤
   │ SDK Key: [Paste from dashboard]             │
   │ Enable AppLovin Ads: ✓                      │
   │ Verbose Logging: ✓                          │
   │ Test Mode: ✓                                │
   ├─────────────────────────────────────────────┤
   │ Android Ad Unit IDs                         │
   ├─────────────────────────────────────────────┤
   │ Interstitial: [Paste interstitial ID]      │
   │ Rewarded: [Paste rewarded ID]               │
   └─────────────────────────────────────────────┘
```

### 4. Link AdminPanel (for testing)

```
1. Open scene: Assets/Scenes/ScreensScene.unity (or any scene with AdminPanel)

2. Select "AdminPanel" GameObject in Hierarchy

3. Find "FullScreenAdverts" GameObject

4. Drag FullScreenAdverts into AdminPanel's inspector:
   ┌──────────────────────────────┐
   │ AdminPanel (Script)          │
   ├──────────────────────────────┤
   │ Ad Testing                   │
   │ Full Screen Adverts: [Drag]  │
   └──────────────────────────────┘
```

### 5. Test in Editor

```
1. Press Play in Unity Editor
2. Open Console (Ctrl+Shift+C)
3. Watch for logs:
   [AppLovin MAX] [Editor] Initializing...
   [AppLovin MAX] [Editor] Initialization complete
   [AppLovin MAX] [Editor] Interstitial ad ready
```

### 6. Test on Android Device

```
1. Build Settings → Android → Build and Run

2. Play the game:
   - Complete tutorial
   - Reach level 7
   - Complete 3 battles
   
3. Ad should show! (with "TEST MODE" watermark)

4. Check via adb:
   adb logcat -s Unity AppLovin
```

---

## Quick Test Checklist

✅ **SDK Imported**
- [ ] AppLovin MAX Unity plugin installed
- [ ] No import errors in Console

✅ **Configuration**
- [ ] `MonetizationSettings` created
- [ ] SDK Key filled in
- [ ] Ad Unit IDs filled in
- [ ] File in `Assets/Resources/` folder

✅ **Scene Setup**
- [ ] AdminPanel → FullScreenAdverts linked
- [ ] No missing references

✅ **Editor Test**
- [ ] Console shows initialization logs
- [ ] No red errors
- [ ] Ads simulate correctly

✅ **Device Test**
- [ ] Test Mode enabled
- [ ] Build completes without errors
- [ ] Ads show on device
- [ ] Rewarded ads grant 2x/3x rewards

---

## Common Issues

### "MonetizationSettings not found"
**Solution:** Move MonetizationSettings to `Assets/Resources/` folder

### "SDK Key invalid"
**Solution:** 
1. Go to AppLovin dashboard
2. Copy SDK key exactly (no spaces)
3. Paste into MonetizationSettings

### "No ads showing"
**Solution:**
1. Enable Test Mode in MonetizationSettings
2. Build to device (not editor)
3. Check internet connection
4. Check logcat: `adb logcat -s AppLovin`

### "AdminPanel can't find FullScreenAdverts"
**Solution:** Link it manually in Inspector (step 4 above)

---

## Next Steps

Once basic setup works:

1. **Disable Test Mode** (for production)
   - Uncheck "Test Mode" in MonetizationSettings
   
2. **Set Up Firebase Remote Config** (for A/B testing)
   - See: `FIREBASE_REMOTE_CONFIG_SETUP.md`
   
3. **Monitor Analytics** (Firebase Console)
   - Events: `ad_impression`, `ad_closed`, `rewarded_ad_completed`
   
4. **Optimize Ad Frequency** (via Remote Config)
   - Test different frequencies
   - Monitor retention and revenue

---

## Help & Documentation

- **Full Guide:** `APPLOVIN_MAX_INTEGRATION.md`
- **Architecture:** `BATTLECRUISERS_ARCHITECTURE_GUIDE.md`
- **Android Setup:** `ANDROID_ADS_SETUP.md`
- **Remote Config:** `FIREBASE_REMOTE_CONFIG_SETUP.md`

---

**Setup Time:** ~5 minutes  
**First Ad:** ~10 minutes (after reaching level 7)  
**Production Ready:** ~1 hour (with testing)

