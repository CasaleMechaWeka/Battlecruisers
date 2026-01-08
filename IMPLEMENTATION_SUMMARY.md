# Custom Battle Dialogue System - Implementation Summary

## ✅ Implementation Complete

The custom dialogue system has been fully implemented and is ready to use.

## What Was Implemented

### 1. Extended HeckleMessage Component
**File:** `Assets/Scripts/UI/BattleScene/Heckles/HeckleMessage.cs`

Added `ShowCustom(string localizationKey)` method:
- Displays custom dialogue from StoryTable localization keys
- Uses same speech bubble animation as regular heckles
- Automatically hides after 5 seconds

### 2. Added Player Speech Bubble Support
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`

- Added `public HeckleMessage playerHeckleMessage` field (line 107)
- Initialized in Start() method (lines 426-429)
- Both player and enemy heckle messages are now available

### 3. Enhanced BattleSequencer with Dialogue Methods
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`

Added public methods callable from Unity Inspector:
- `ShowPlayerDialogue(string localizationKey)` - Shows player speech
- `ShowEnemyDialogue(string localizationKey)` - Shows enemy speech

### 4. Created DialogueAction System
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`

Added new `DialogueAction` class to `SequencePoint`:
- `SpeakerType` enum: Player or Enemy
- `LocalizationKey` string: StoryTable key
- Automatically processes in sequence points
- Integrated with existing sequencer flow

### 5. Configured Level 32
**File:** `Assets/Scripts/Data/Static/StaticData.cs`

Updated Level 32 configuration:
- Set `enableHeckles = false` to disable random trash talk
- Set `hasSequencer = true` to enable custom sequencer
- Ready for custom dialogue implementation

### 6. Fixed Critical Bug
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs` (line 524)

Fixed incorrect variable usage:
- **Before:** Used `ApplicationModel.SelectedSideQuestID` for campaign levels
- **After:** Correctly uses `ApplicationModel.SelectedLevel`
- This bug would have prevented level sequencers from loading

## How It Works

### Default Behavior (Most Levels)
1. Enemy NPC shows random heckles from the 280-heckle pool
2. Triggered by time intervals and health thresholds
3. Managed by `NPCHeckleManager`
4. Player remains silent
5. Configurable via `HeckleConfig` in level definition

### Custom Dialogue (Level 32 and others you configure)
1. Disable automatic heckles with `enableHeckles = false`
2. Create a BattleSequencer prefab for the level
3. Add DialogueActions to SequencePoints
4. Specify exact timing and speaker for each line
5. Both player and enemy can speak

## What You Need to Do

### 1. Unity Scene Setup (REQUIRED)

In the BattleScene, you need to:

1. **Assign Player Heckle Message UI:**
   - Open BattleScene
   - Find `BattleSceneGod` GameObject
   - In Inspector, assign the `Player Heckle Message` field
   
2. **Create Player Speech Bubble UI (if it doesn't exist):**
   - Duplicate the existing `EnemyHeckleMessage` UI element
   - Rename to `PlayerHeckleMessage`
   - Reposition to appear near the player's cruiser
   - Assign to BattleSceneGod's field

### 2. Create Localization Strings

Add these keys to the **StoryTable** in Unity Localization:

```
level32/PlayerChat1
level32/EnemyChat1
level32/PlayerChat2
level32/EnemyChat2
```

Example values:
- `level32/PlayerChat1`: "I've been looking for you, Fei!"
- `level32/EnemyChat1`: "Then you've found your doom, pilot!"

### 3. Create Battle Sequencer Prefab

**Path:** `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/SequencerLV032.prefab`

1. Create new GameObject
2. Add `BattleSequencer` component
3. Configure Sequence Points:

**Example Configuration:**

```
Sequence Point 0:
  Delay MS: 5000
  Faction: Player
  Dialogue Actions:
    Element 0:
      Speaker: Player
      Localization Key: level32/PlayerChat1

Sequence Point 1:
  Delay MS: 8000
  Faction: Enemy
  Dialogue Actions:
    Element 0:
      Speaker: Enemy
      Localization Key: level32/EnemyChat1
```

4. Save as prefab in the correct location

## Two Ways to Trigger Dialogue

### Method 1: DialogueActions (Recommended)
Configure directly in SequencePoint's DialogueActions list in Unity Inspector.

**Pros:**
- Clean, organized
- Easy to see all dialogue in one place
- No code needed

### Method 2: ScriptCallActions
Use Unity Events to call `ShowPlayerDialogue()` or `ShowEnemyDialogue()`.

**Pros:**
- Can combine with other logic
- Useful for complex sequencing

## Files Modified

1. ✅ `Assets/Scripts/UI/BattleScene/Heckles/HeckleMessage.cs`
2. ✅ `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`
3. ✅ `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`
4. ✅ `Assets/Scripts/Data/Static/StaticData.cs`

## Documentation Created

1. **CUSTOM_DIALOGUE_SYSTEM.md** - Complete system documentation
2. **LEVEL32_DIALOGUE_QUICKSTART.md** - Quick start guide for level 32
3. **IMPLEMENTATION_SUMMARY.md** - This file

## Testing Checklist

Before playing level 32:

- [ ] Player Heckle Message UI assigned in BattleSceneGod
- [ ] Localization keys added to StoryTable
- [ ] SequencerLV032.prefab created at correct path
- [ ] Sequence Points configured with dialogue
- [ ] Prefab saved

**To Test:**
1. Start game in Campaign mode
2. Select and play Level 32
3. Observe dialogue appearing at specified times

## Backward Compatibility

✅ **All existing levels will work exactly as before**
- Levels 1-31: Continue using random heckles
- Levels 33-40: Continue using random heckles
- Only Level 32 has custom dialogue enabled
- No changes needed to existing content

## Architecture Summary

```
BattleSceneGod
├── enemyHeckleMessage (HeckleMessage)
│   ├── Show(int heckleIndex) - Random heckles
│   └── ShowCustom(string key) - Custom dialogue [NEW]
│
├── playerHeckleMessage (HeckleMessage) [NEW]
│   ├── Show(int heckleIndex) - Not used for player
│   └── ShowCustom(string key) - Custom dialogue [NEW]
│
└── _npcHeckleManager (NPCHeckleManager)
    └── Manages automatic random heckles

BattleSequencer
├── ShowPlayerDialogue(string key) [NEW]
├── ShowEnemyDialogue(string key) [NEW]
└── ProcessSequencePoint()
    └── DialogueActions processing [NEW]

SequencePoint
├── BuildingActions (existing)
├── BoostActions (existing)
├── UnitActions (existing)
├── DialogueActions [NEW]
│   ├── SpeakerType: Player/Enemy
│   └── LocalizationKey: string
└── ScriptCallActions (existing)
```

## Future Enhancements (Not Implemented)

Potential additions you could make:
- Timed delays between multiple dialogue in one SequencePoint
- Different speech bubble styles per character
- Animation triggers when dialogue appears
- Dialogue sound effects
- Conditional dialogue based on battle state

## Known Limitations

1. Speech bubbles use fixed 5-second display time
2. Multiple simultaneous dialogues will overlap
3. No built-in conversation pacing (use separate SequencePoints)
4. Requires manual UI setup in Unity scene

## Support

If dialogue isn't appearing:

1. **Check Console** for error messages
2. **Verify** localization keys are spelled correctly
3. **Confirm** prefab path is correct: `SequencerLV032.prefab`
4. **Ensure** Player Heckle Message is assigned in Inspector
5. **Test** in Campaign mode (not Skirmish/Tutorial)

## Success!

The system is ready to use. You now have:
- ✅ Player and enemy can both speak custom dialogue
- ✅ Full control over timing via SequencePoints
- ✅ Localization support via StoryTable
- ✅ Easy to configure in Unity Inspector
- ✅ Backward compatible with existing levels
- ✅ Clean, maintainable architecture

Create your localization strings and battle sequencer prefab, and level 32 will come alive with custom dialogue!

