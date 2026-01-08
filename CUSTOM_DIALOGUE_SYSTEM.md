# Custom Dialogue System for Battle Scenes

## Overview

This system allows you to trigger custom, level-specific dialogue for both the player and enemy during battle. It extends the existing heckle system to support scripted conversations at specific moments in the battle.

## System Components

### 1. HeckleMessage Component (Enhanced)
**File:** `Assets/Scripts/UI/BattleScene/Heckles/HeckleMessage.cs`

Now includes a `ShowCustom(string localizationKey)` method that displays dialogue from the StoryTable localization.

### 2. BattleSceneGod (Enhanced)
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`

- **New Variable:** `public HeckleMessage playerHeckleMessage;` (line 107)
- This is initialized alongside `enemyHeckleMessage` in the Start method
- Both are accessible via `BattleSceneGod.Instance` for use in sequencer scripts

### 3. BattleSequencer (Enhanced)
**File:** `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`

**New Methods:**
- `ShowPlayerDialogue(string localizationKey)` - Displays player speech bubble
- `ShowEnemyDialogue(string localizationKey)` - Displays enemy speech bubble

**New SequencePoint Action:**
- `DialogueAction` - A new serializable action type for dialogue
  - `SpeakerType` enum: Player or Enemy
  - `LocalizationKey` string: The StoryTable key

## Usage Guide

### Method 1: Using DialogueActions (Recommended)

In the Unity Inspector for your Battle Sequencer prefab:

1. Create a new Sequence Point
2. Set the `DelayMS` to when you want the dialogue to appear
3. Add a new `DialogueAction` to the list:
   - Set `Speaker` to either `Player` or `Enemy`
   - Set `LocalizationKey` to your story table key (e.g., "level32/PlayerChat1")

**Example Setup:**
```
SequencePoint 0:
  DelayMS: 5000  (5 seconds into battle)
  DialogueActions:
    - Speaker: Player
      LocalizationKey: "level32/PlayerChat1"

SequencePoint 1:
  DelayMS: 8000  (8 seconds into battle)
  DialogueActions:
    - Speaker: Enemy
      LocalizationKey: "level32/EnemyChat1"
```

### Method 2: Using ScriptCallActions (Alternative)

You can also call the dialogue methods directly from Unity Events in the ScriptCallActions:

1. Add a new Sequence Point
2. In the `ScriptCallActions` section, add a method call:
   - Target: Your BattleSequencer component (auto-binds)
   - Function: `BattleSequencer.ShowPlayerDialogue` or `BattleSequencer.ShowEnemyDialogue`
   - Parameter: The localization key string

## Localization Setup

### Creating Localization Keys

Your localization keys should follow this format in the **StoryTable**:

- `level32/PlayerChat1` - Player's first line
- `level32/EnemyChat1` - Enemy's first line
- `level32/PlayerChat2` - Player's second line
- `level32/EnemyChat2` - Enemy's second line

### Adding to StoryTable

1. Open the Unity Localization Tables window
2. Select the **StoryTable**
3. Add your keys with the format shown above
4. Provide translations for all supported languages

**Example Entries:**
```
Key: level32/PlayerChat1
English: "I've been looking for you, Fei!"

Key: level32/EnemyChat1
English: "Then you've found your doom, pilot!"
```

## Level Configuration

### Disabling Automatic Heckles

For levels with custom dialogue, you should disable the automatic random heckles:

**File:** `Assets/Scripts/Data/Static/StaticData.cs`

```csharp
new Level(32, Hulls.LV032Raptor, BackgroundMusic.Fortress, SkyMaterials.Purple, 
    Exos.GetCaptainExoKey(1), 
    new HeckleConfig() { enableHeckles = false }, // Disable random heckles
    hasSequencer: true)  // Enable sequencer for custom dialogue
```

### Keeping Default Heckles

For most levels, keep the default behavior:

```csharp
new Level(33, Hulls.Bullshark, BackgroundMusic.Bobby, SkyMaterials.Midnight, 
    Exos.GetCaptainExoKey(3), 
    GetDefaultHeckleConfig())  // Normal random heckles enabled
```

## Creating a Battle Sequencer Prefab

### For Level 32 (Campaign Mode)

1. Create a new GameObject in the scene
2. Add the `BattleSequencer` component
3. Configure your SequencePoints as described above
4. Create a prefab at: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/SequencerLV032.prefab`
5. The system will automatically load it when level 32 starts

**Note:** There's currently a bug in BattleSceneGod.cs line 517 where it uses `ApplicationModel.SelectedSideQuestID` instead of `ApplicationModel.SelectedLevel`. This should be fixed:

```csharp
// CURRENT (BUG):
string path = SEQUENCER_PATH + "SequencerLV" + ApplicationModel.SelectedSideQuestID.ToString("000") + ".prefab";

// SHOULD BE:
string path = SEQUENCER_PATH + "SequencerLV" + ApplicationModel.SelectedLevel.ToString("000") + ".prefab";
```

### For Side Quests

Side quest sequencers use a different path:
- Format: `SequencerSQ###.prefab`
- Uses `ApplicationModel.SelectedSideQuestID` (this one is correct)

## Example: Level 32 Setup

### 1. Unity Inspector Setup

**Sequencer SequencePoints:**

```
Sequence Point 0:
  DelayMS: 3000
  Faction: Player
  DialogueActions:
    [0]
      Speaker: Player
      LocalizationKey: "level32/PlayerChat1"

Sequence Point 1:
  DelayMS: 6000
  Faction: Enemy
  DialogueActions:
    [0]
      Speaker: Enemy
      LocalizationKey: "level32/EnemyChat1"

Sequence Point 2:
  DelayMS: 45000  (45 seconds - mid battle)
  Faction: Player
  DialogueActions:
    [0]
      Speaker: Player
      LocalizationKey: "level32/PlayerChat2"

Sequence Point 3:
  DelayMS: 48000
  Faction: Enemy
  DialogueActions:
    [0]
      Speaker: Enemy
      LocalizationKey: "level32/EnemyChat2"
```

### 2. Localization Entries

In **StoryTable**:

```
level32/PlayerChat1: "Fei, your reign of terror ends today!"
level32/EnemyChat1: "You dare challenge me? Prepare to be destroyed!"
level32/PlayerChat2: "Your tactics won't work on me!"
level32/EnemyChat2: "We'll see about that, pilot!"
```

### 3. Unity Scene Setup

In the BattleScene prefab or scene:

1. Locate the `BattleSceneGod` GameObject
2. In the Inspector, you should see:
   - `Enemy Heckle Message` - Already assigned
   - `Player Heckle Message` - **Assign this to the player's speech bubble UI element**

**UI Hierarchy:**
```
Canvas
└── HeckleMessages
    ├── EnemyHeckleMessage (already exists)
    └── PlayerHeckleMessage (you may need to duplicate and position this)
```

## Testing Your Dialogue

1. Ensure your localization keys are added to StoryTable
2. Create your sequencer prefab with DialogueActions
3. Save the prefab in the correct location
4. Set level 32 to have `hasSequencer: true` in StaticData
5. Play level 32 and watch for the dialogue to appear at the specified times

## Troubleshooting

### Dialogue Not Appearing

**Check:**
1. Is `playerHeckleMessage` assigned in BattleSceneGod Inspector?
2. Is the sequencer prefab in the correct path?
3. Are your localization keys correct (check spelling)?
4. Check the Console for errors about missing keys or null references

### Wrong Timing

- Adjust `DelayMS` in your SequencePoints
- Remember: DelayMS is cumulative delay from the start of battle

### Random Heckles Still Appearing

- Ensure level has `enableHeckles = false` in HeckleConfig
- Check that StaticData has been recompiled

## Advanced: Multiple Dialogue in One Sequence Point

You can have both player and enemy speak in the same SequencePoint:

```
Sequence Point 0:
  DelayMS: 5000
  DialogueActions:
    [0]
      Speaker: Player
      LocalizationKey: "level32/PlayerChat1"
    [1]
      Speaker: Enemy  
      LocalizationKey: "level32/EnemyChat1"
```

They will appear simultaneously. For alternating dialogue, use separate SequencePoints with different delays.

## System Architecture Notes

### Default Behavior (Most Levels)

- Enemy NPC shows random heckles from the 280 heckle pool
- Triggered by time intervals and health thresholds
- Managed by `NPCHeckleManager`
- Player doesn't speak

### Custom Dialogue (Special Levels like 32)

- Disable automatic heckles with `enableHeckles = false`
- Use BattleSequencer to trigger specific dialogue
- Both player and enemy can speak
- Full control over timing and content

### Both Systems Can Coexist

You can have:
- Automatic random enemy heckles enabled
- PLUS custom scripted dialogue at specific moments
- Just don't set `enableHeckles = false`

## API Reference

### HeckleMessage.ShowCustom(string localizationKey)
- **Parameter:** `localizationKey` - Key from StoryTable (e.g., "level32/PlayerChat1")
- **Behavior:** Shows the speech bubble with the localized text for 5 seconds

### BattleSequencer.ShowPlayerDialogue(string localizationKey)
- **Parameter:** `localizationKey` - Key from StoryTable
- **Behavior:** Displays player's speech bubble using BattleSceneGod.Instance.playerHeckleMessage

### BattleSequencer.ShowEnemyDialogue(string localizationKey)
- **Parameter:** `localizationKey` - Key from StoryTable
- **Behavior:** Displays enemy's speech bubble using BattleSceneGod.Instance.enemyHeckleMessage

## Files Modified

1. `Assets/Scripts/UI/BattleScene/Heckles/HeckleMessage.cs`
   - Added `ShowCustom()` method

2. `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`
   - Added `playerHeckleMessage` field
   - Added initialization for player heckle message

3. `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`
   - Added `ShowPlayerDialogue()` and `ShowEnemyDialogue()` methods
   - Added `DialogueAction` class
   - Added `DialogueActions` list to `SequencePoint`
   - Added dialogue processing in `ProcessSequencePoint()`

4. `Assets/Scripts/Data/Static/StaticData.cs`
   - Updated Level 32 to disable heckles and enable sequencer

## Future Enhancements

Potential improvements:
- Add delay between player/enemy responses in same SequencePoint
- Support for different speech bubble styles per character
- Animation triggers when dialogue appears
- Sound effects for dialogue
- Multiple speech bubbles visible at once

