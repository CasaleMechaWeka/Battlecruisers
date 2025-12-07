# NPC Heckle System Integration Guide

## Overview
This guide shows you how to integrate the NPC heckle system into `BattleSceneGod.cs`.

## What Was Created

### 1. **HeckleMessage.cs** (`Assets/Scripts/UI/BattleScene/Heckles/`)
- Non-networked UI component that displays heckle speech bubbles
- Based on `PvPHeckleMessage.cs` but simplified for single-player
- Uses DOTween animations to scale in/out

### 2. **NPCHeckleManager.cs** (`Assets/Scripts/Scenes/BattleScene/`)
- Manages timing and triggers for NPC heckles
- Handles time-based and event-based triggers
- Tracks state to limit total heckles shown

### 3. **HeckleConfig.cs** (`Assets/Scripts/Data/`)
- Configuration data structure
- Can be customized per-level or per-sidequest
- Includes settings for timing, triggers, and specific heckles

### 4. **Updated Level.cs and SideQuestData.cs**
- Added `HeckleConfig` property to both
- Defaults to disabled if not provided

---

## Integration Steps

### Step 1: Create UI Prefab (Unity Editor)

1. **Duplicate the PvP HeckleMessage prefab:**
   - Find: `Assets/Resources_moved/Prefabs/PvP/BattleScene/UI/HeckleMessage.prefab`
   - Duplicate it to: `Assets/Resources_moved/Prefabs/BattleScene/UI/EnemyHeckleMessage.prefab`

2. **Update the prefab:**
   - Remove the `PvPHeckleMessage` component
   - Add the new `HeckleMessage` component
   - Assign the Text component to the `message` field
   - Set `hideTime` to `5` (or your preferred duration)

3. **Position it in the scene:**
   - Should be positioned near the enemy captain/cruiser area
   - Make it a child of the main Canvas in the BattleScene

---

### Step 2: Add Components to BattleSceneGod

Add these fields to `BattleSceneGod.cs` (around line 90-96 where other UI refs are):

```csharp
// Heckle system
public HeckleMessage enemyHeckleMessage;
private NPCHeckleManager _npcHeckleManager;
```

---

### Step 3: Initialize in BattleSceneGod.Start()

Add this code in the `Start()` method, **after** the cruisers are created and initialized (around line 440-460):

```csharp
// Initialize NPC Heckle System
Logging.Log(Tags.BATTLE_SCENE, "Heckle system setup");
if (enemyHeckleMessage != null)
{
    enemyHeckleMessage.Initialise();
    
    // Get heckle config from current level or side quest
    HeckleConfig heckleConfig = null;
    if (ApplicationModel.Mode == GameMode.SideQuest)
    {
        heckleConfig = currentSideQuest?.HeckleConfig;
    }
    else if (currentLevel != null)
    {
        heckleConfig = currentLevel.HeckleConfig;
    }

    // Create and initialize the heckle manager
    if (heckleConfig != null && heckleConfig.enableHeckles)
    {
        _npcHeckleManager = gameObject.AddComponent<NPCHeckleManager>();
        _npcHeckleManager.Initialise(enemyHeckleMessage, heckleConfig, aiCruiser, playerCruiser);
    }
}
```

---

### Step 4: Configure Heckles in StaticData.cs

Now you can add heckle configurations when defining levels. Here's an example:

#### Example 1: Simple time-based heckles on level 15 (boss)
```csharp
new Level(15, 
    Hulls.ManOfWarBoss, 
    BackgroundMusic.Juggernaut, 
    SkyMaterials.Midnight, 
    Exos.GetCaptainExoKey(15),
    new HeckleConfig 
    {
        enableHeckles = true,
        maxHeckles = 3,
        minTimeBeforeFirstHeckle = 15f,
        maxTimeBeforeFirstHeckle = 30f,
        minTimeBetweenHeckles = 20f
    }),
```

#### Example 2: Event-based heckles with specific messages
```csharp
new Level(20, 
    Hulls.Megalodon, 
    BackgroundMusic.Confusion, 
    SkyMaterials.Dusk, 
    Exos.GetCaptainExoKey(20),
    new HeckleConfig 
    {
        enableHeckles = true,
        maxHeckles = 4,
        minTimeBeforeFirstHeckle = 10f,
        maxTimeBeforeFirstHeckle = 40f,
        heckleOnFirstDamage = true,
        enableHealthThresholdHeckle = true,
        heckleOnHealthThreshold = 0.25f,
        specificHeckleIndices = new List<int> { 42, 108, 156, 234 }
    }),
```

#### Example 3: Sidequest with aggressive heckles
```csharp
new SideQuestData(
    playerTalksFirst: false,
    enemyCaptainExo: Exos.GetCaptainExoKey(15),
    unlockRequirementLevel: 10,
    requiredSideQuestID: -1,
    hull: Hulls.Yeti,
    musicBackgroundKey: BackgroundMusic.Juggernaut,
    skyMaterial: SkyMaterials.Midnight,
    isCompleted: false,
    sideLevelNum: 1,
    heckleConfig: new HeckleConfig
    {
        enableHeckles = true,
        maxHeckles = 5,
        minTimeBeforeFirstHeckle = 5f,
        maxTimeBeforeFirstHeckle = 15f,
        minTimeBetweenHeckles = 10f,
        heckleOnFirstDamage = true,
        heckleOnPlayerDamaged = true,
        enableHealthThresholdHeckle = true,
        heckleOnHealthThreshold = 0.5f
    })
```

---

## Configuration Options Explained

### General Settings
- **enableHeckles**: Master on/off switch
- **maxHeckles**: Total number of heckles that can be shown during battle

### Time-Based Triggers
- **minTimeBeforeFirstHeckle**: Earliest time for first heckle (e.g., 10s)
- **maxTimeBeforeFirstHeckle**: Latest time for first heckle (e.g., 50s)
- **minTimeBetweenHeckles**: Cooldown between time-based heckles

### Event-Based Triggers
- **heckleOnFirstDamage**: Trigger when AI takes first damage
- **enableHealthThresholdHeckle**: Enable health-based trigger
- **heckleOnHealthThreshold**: Health % to trigger (e.g., 0.25 = 25%)
- **heckleOnPlayerDamaged**: Trigger when player takes heavy damage
- **heckleOnBuildingDestroyed**: Trigger when AI destroys player building (not yet implemented)

### Specific Heckles
- **specificHeckleIndices**: List of exact heckle indices to use
  - If empty, uses random heckles (0-279)
  - If provided, cycles through the list in order
  - Useful for bosses with personality-specific taunts

---

## Testing Recommendations

### Start Small
1. Enable heckles on just 1-2 levels first (like level 15 boss)
2. Test with simple time-based triggers
3. Gradually add event-based triggers

### Suggested Level Distribution
- **Levels 1-5**: No heckles (let players learn)
- **Levels 6-10**: Occasional heckles (1-2 per battle)
- **Levels 11-20**: More frequent (2-3 per battle)
- **Boss levels (15, 25, 31)**: Aggressive heckles (3-5 per battle)
- **Side quests**: Personality-based heckles with specific messages

### Finding Good Heckle Messages
The heckle strings are in the localization table. You can browse them to find good ones for specific bosses:
- Index 0-50: Generic taunts
- Index 51-100: Combat-focused
- Index 101-150: Confident/boasting
- Index 151-200: Aggressive
- Index 201-279: Various personalities

---

## Future Enhancements (Optional)

### 1. Building Destroyed Trigger
Add to `NPCHeckleManager.cs`:
```csharp
// Subscribe to building destroyed events
// This would require adding an event system for building destruction
```

### 2. Player Unit Destroyed Trigger
Trigger heckle when player loses expensive units (ships, ultras)

### 3. Combo Triggers
Multiple events within a short time = extra mocking heckle

### 4. Dynamic Heckles Based on Game State
- Different heckles if player is winning vs losing
- Special heckles for certain strategies (rush, turtle, etc.)

---

## Troubleshooting

**Heckles not showing?**
- Check that `enableHeckles = true` in config
- Verify `enemyHeckleMessage` is assigned in inspector
- Check console for initialization logs
- Ensure the UI prefab is visible in the scene

**Heckles showing at wrong time?**
- Adjust `minTimeBeforeFirstHeckle` and `maxTimeBeforeFirstHeckle`
- Check `minTimeBetweenHeckles` cooldown

**Invalid heckle index errors?**
- Ensure `specificHeckleIndices` only contains values 0-279
- Check that the localization table has all entries

---

## Quick Start Checklist

- [ ] Create `EnemyHeckleMessage.prefab` from PvP version
- [ ] Add `HeckleMessage` component to prefab
- [ ] Add prefab to BattleScene Canvas
- [ ] Add `enemyHeckleMessage` field to `BattleSceneGod.cs`
- [ ] Assign prefab in Unity Inspector
- [ ] Add initialization code to `BattleSceneGod.Start()`
- [ ] Configure heckles for 1-2 test levels in `StaticData.cs`
- [ ] Test in-game!

---

## Example: Full Boss Level Configuration

Here's a complete example for a boss that taunts you throughout the battle:

```csharp
new Level(31, // Final boss
    Hulls.Megalodon, 
    BackgroundMusic.Juggernaut, 
    SkyMaterials.Midnight, 
    Exos.GetCaptainExoKey(31),
    new HeckleConfig 
    {
        enableHeckles = true,
        maxHeckles = 5,
        minTimeBeforeFirstHeckle = 15f,
        maxTimeBeforeFirstHeckle = 30f,
        minTimeBetweenHeckles = 25f,
        heckleOnFirstDamage = true,
        enableHealthThresholdHeckle = true,
        heckleOnHealthThreshold = 0.33f,
        heckleOnPlayerDamaged = true,
        // Specific boss personality heckles
        specificHeckleIndices = new List<int> 
        { 
            150, // "Is that all you've got?"
            200, // "Pathetic!"
            125, // "You call that strategy?"
            180, // "I expected more..."
            240  // "Time to end this!"
        }
    }),
```

Good luck! This should add a lot of personality to your battles! 🎮

