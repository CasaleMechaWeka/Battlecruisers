# Level 32 Custom Dialogue - Quick Start Guide

## What You Need to Do

### 1. Create Your Localization Strings (Do This Now)

Open Unity Localization Tables and add these keys to **StoryTable**:

```
level32/PlayerChat1
level32/EnemyChat1
level32/PlayerChat2  (optional - for additional dialogue)
level32/EnemyChat2  (optional - for additional dialogue)
```

Add your text for each language you support.

### 2. Create the Battle Sequencer Prefab

**Path:** `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/SequencerLV032.prefab`

1. Create a new GameObject in your scene
2. Add the `BattleSequencer` component
3. Configure the Sequence Points in the Inspector

### 3. Configure Sequence Points in Inspector

**For each dialogue moment:**

#### Example: Player speaks first

```
Sequence Point 0:
  Delay MS: 5000         (5 seconds after battle start)
  Faction: Player        (not critical for dialogue, but set it)
  Dialogue Actions:
    Size: 1
    Element 0:
      Speaker: Player
      Localization Key: level32/PlayerChat1
```

#### Example: Enemy responds

```
Sequence Point 1:
  Delay MS: 8000         (3 seconds after player spoke)
  Faction: Enemy
  Dialogue Actions:
    Size: 1
    Element 0:
      Speaker: Enemy
      Localization Key: level32/EnemyChat1
```

### 4. Assign Player Speech Bubble in BattleScene

**In Unity:**

1. Open the Battle Scene
2. Find the `BattleSceneGod` GameObject
3. In the Inspector, find the new field: `Player Heckle Message`
4. You need to assign a HeckleMessage UI component here

**If the UI doesn't exist yet:**

You might need to duplicate the enemy heckle message UI element:
- Find `EnemyHeckleMessage` in the scene hierarchy
- Duplicate it
- Rename to `PlayerHeckleMessage`
- Reposition it to appear above/near the player's cruiser
- Assign it to BattleSceneGod's `Player Heckle Message` field

## Code Already Done ✅

The following has been implemented for you:

- ✅ `HeckleMessage.ShowCustom()` method to display custom dialogue
- ✅ `BattleSceneGod.playerHeckleMessage` field added
- ✅ `BattleSequencer.ShowPlayerDialogue()` and `ShowEnemyDialogue()` methods
- ✅ `DialogueAction` system in BattleSequencer
- ✅ Level 32 configured in StaticData with `hasSequencer: true` and `enableHeckles: false`

## Using the Animator to Trigger Dialogue

### Method 1: Time-Based (Simplest)

Just set `DelayMS` to when you want the dialogue:

```
DelayMS: 30000 = 30 seconds into battle
DelayMS: 60000 = 1 minute into battle
```

### Method 2: Unity Events from Animation (Advanced)

If you need to trigger dialogue at a specific animation frame:

1. In your Animator, add an Animation Event
2. Call a method in your BattleSequencer script
3. That method calls `ShowPlayerDialogue()` or `ShowEnemyDialogue()`

**Example:**

Create a public method in your SequencerLV032 script:

```csharp
public void OnAnimationState_PlayerAttacks()
{
    ShowPlayerDialogue("level32/PlayerChat1");
}
```

Then in your Animation timeline, add an event that calls this method.

### Method 3: ScriptCallActions (Alternative)

Instead of DialogueActions, you can use ScriptCallActions:

```
Sequence Point:
  Script Call Actions:
    - Add Method: BattleSequencer.ShowPlayerDialogue
      Parameter: "level32/PlayerChat1"
```

## Timing Recommendations

- **Opening Dialogue:** 3-5 seconds after battle start
- **Between Lines:** 3-5 seconds apart (allows time to read)
- **Mid-Battle:** 30-60 seconds in
- **Late Battle:** 90+ seconds or tie to health thresholds

## Testing Checklist

Before testing level 32:

- [ ] Localization keys added to StoryTable
- [ ] SequencerLV032.prefab created in correct path
- [ ] Sequence Points configured with DialogueActions
- [ ] Player Heckle Message assigned in BattleSceneGod Inspector
- [ ] Prefab saved

**To Test:**
1. Start Campaign mode
2. Play level 32
3. Watch for dialogue at the specified times

## Troubleshooting

**"BattleSceneGod.Instance is null"**
- The sequencer is loading before BattleSceneGod initializes
- This shouldn't happen, but if it does, add a small delay

**"playerHeckleMessage is not assigned"**
- Check BattleSceneGod Inspector
- Make sure the Player Heckle Message field has a UI component assigned

**Dialogue not appearing**
- Check Console for errors
- Verify localization key spelling exactly matches
- Make sure the prefab is named correctly: `SequencerLV032.prefab`

**⚠️ KNOWN BUG**
There's a bug in BattleSceneGod.cs line 517. It should use `ApplicationModel.SelectedLevel` instead of `ApplicationModel.SelectedSideQuestID` for campaign levels. You may need to fix this:

```csharp
// Line 517 - BEFORE (BUG):
string path = SEQUENCER_PATH + "SequencerLV" + ApplicationModel.SelectedSideQuestID.ToString("000") + ".prefab";

// AFTER (FIXED):
string path = SEQUENCER_PATH + "SequencerLV" + ApplicationModel.SelectedLevel.ToString("000") + ".prefab";
```

## Default Behavior for Other Levels

All other levels will continue to work as before:
- Enemy NPCs will trash talk randomly from the heckle pool
- Player stays silent
- No custom dialogue

Level 32 is special because we disabled automatic heckles and enabled the sequencer.

## Example Timeline for Level 32

```
Time    | Speaker | Key
--------|---------|----------------------
0:03    | Player  | level32/PlayerChat1
0:06    | Enemy   | level32/EnemyChat1
0:45    | Player  | level32/PlayerChat2
0:48    | Enemy   | level32/EnemyChat2
```

Adjust timing based on your battle pacing and story needs.

Good luck! 🚀

