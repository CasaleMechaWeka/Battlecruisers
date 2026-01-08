# Unity Inspector Setup Guide - Visual Reference

## Setting Up Dialogue in the Battle Sequencer

### Step 1: Create the Sequencer Prefab

1. **Create GameObject:**
   - Right-click in Hierarchy
   - Create Empty GameObject
   - Name it: `SequencerLV032`

2. **Add Component:**
   - Select the GameObject
   - Add Component → `BattleSequencer`

### Step 2: Configure Sequence Points

In the Inspector, you'll see:

```
BattleSequencer (Script)
├── Cruisers (Array) - Leave empty (auto-assigned at runtime)
└── Sequence Points (Array)
    Size: 4  ← Set this to number of dialogue moments
```

### Step 3: Configure Each Sequence Point

Click the arrow to expand Sequence Point 0:

```
Sequence Points
└── Element 0
    ├── Delay MS: 5000                    ← 5 seconds into battle
    ├── Faction: Player                   ← Select from dropdown
    ├── Building Actions (List)           ← Ignore (leave empty)
    ├── Boost Actions (List)              ← Ignore (leave empty)  
    ├── Unit Actions (List)               ← Ignore (leave empty)
    ├── Dialogue Actions (List)           ← EXPAND THIS ▼
    │   Size: 1                           ← Set to 1
    │   └── Element 0
    │       ├── Speaker: Player           ← Dropdown: Player or Enemy
    │       └── Localization Key: level32/PlayerChat1  ← Type the key
    └── Script Call Actions               ← Ignore (leave empty)
```

### Step 4: Add More Sequence Points

For each additional dialogue line:

```
Sequence Point 1 (Enemy Response):
├── Delay MS: 8000         ← 3 seconds after player spoke
├── Faction: Enemy
└── Dialogue Actions
    Size: 1
    └── Element 0
        ├── Speaker: Enemy
        └── Localization Key: level32/EnemyChat1

Sequence Point 2 (Mid-Battle - Player):
├── Delay MS: 45000        ← 45 seconds into battle
├── Faction: Player
└── Dialogue Actions
    Size: 1
    └── Element 0
        ├── Speaker: Player
        └── Localization Key: level32/PlayerChat2

Sequence Point 3 (Enemy Response):
├── Delay MS: 48000
├── Faction: Enemy
└── Dialogue Actions
    Size: 1
    └── Element 0
        ├── Speaker: Enemy
        └── Localization Key: level32/EnemyChat2
```

### Step 5: Save as Prefab

1. Drag the GameObject from Hierarchy to Project window
2. Navigate to: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/`
3. Name it exactly: `SequencerLV032`
4. Delete the GameObject from Hierarchy (it will load automatically)

## Setting Up BattleSceneGod

### In the BattleScene:

1. **Find BattleSceneGod:**
   - Open BattleScene
   - In Hierarchy, find `BattleSceneGod` GameObject
   - Select it

2. **In the Inspector:**

```
BattleSceneGod (Script)
├── ... (existing fields)
├── Enemy Heckle Message: ▢ [Already assigned]  ← Should already have a component
├── Player Heckle Message: ▢ [Needs assignment]  ← ASSIGN THIS! ⚠️
└── ... (more fields)
```

3. **Assign Player Heckle Message:**
   - Click the circle next to "Player Heckle Message"
   - OR drag the UI element from Hierarchy

### Creating the Player Heckle Message UI

If `PlayerHeckleMessage` doesn't exist in your scene:

1. **Find the Enemy's UI:**
   - In Hierarchy, search for: `EnemyHeckleMessage`
   - It should be under Canvas or UI container

2. **Duplicate It:**
   - Right-click `EnemyHeckleMessage`
   - Select Duplicate
   - Rename the duplicate to: `PlayerHeckleMessage`

3. **Reposition It:**
   - Select `PlayerHeckleMessage`
   - In Rect Transform, adjust position
   - Move it to appear above/near the player's cruiser
   - Typical position: Mirror the enemy's, but on the left side

4. **Verify Components:**
   - Should have `HeckleMessage` script attached
   - Should have Text component
   - Should have RectTransform for positioning

5. **Assign to BattleSceneGod:**
   - Select `BattleSceneGod`
   - Drag `PlayerHeckleMessage` into the "Player Heckle Message" field

## Alternative: Using Script Call Actions

Instead of Dialogue Actions, you can use Script Call Actions:

```
Sequence Point 0
├── Delay MS: 5000
├── Faction: Player
└── Script Call Actions
    ├── + Add Event
    └── ▼ Expand
        ├── Runtime Only (dropdown) - leave as is
        ├── Target: ▢ [Drag BattleSequencer here]
        ├── Function: BattleSequencer ▼
        │   └── ShowPlayerDialogue(string)  ← Select this
        └── Parameter: level32/PlayerChat1  ← Type the key
```

**Note:** The Script Call Actions approach requires manually setting the target, while Dialogue Actions are cleaner and self-contained.

## Typical Inspector Layout Summary

### Minimal Configuration (2 lines of dialogue):

```
BattleSequencer
└── Sequence Points
    Size: 2
    
    Element 0 (Player speaks):
    ├── Delay MS: 5000
    ├── Dialogue Actions → Size: 1
        └── Speaker: Player, Key: level32/PlayerChat1
    
    Element 1 (Enemy responds):
    ├── Delay MS: 8000
    ├── Dialogue Actions → Size: 1
        └── Speaker: Enemy, Key: level32/EnemyChat1
```

### Extended Configuration (4 lines):

Just add 2 more Sequence Points following the same pattern at different times.

## Common Mistakes to Avoid

❌ **Wrong:** Misspelling localization keys
✅ **Right:** Copy-paste keys from your localization table

❌ **Wrong:** Forgetting to set Dialogue Actions Size to 1 or more
✅ **Right:** Always set Size before filling in elements

❌ **Wrong:** Using same DelayMS for multiple sequence points
✅ **Right:** Increase DelayMS for each subsequent point

❌ **Wrong:** Not assigning Player Heckle Message in BattleSceneGod
✅ **Right:** Always assign the UI element in the Inspector

❌ **Wrong:** Naming prefab `Sequencer_LV032` or `Level32Sequencer`
✅ **Right:** Must be exactly `SequencerLV032.prefab`

## Testing Quick Checklist

Before testing:
1. ✅ Localization keys added to StoryTable
2. ✅ SequencerLV032.prefab in correct folder
3. ✅ Sequence Points configured
4. ✅ Player Heckle Message assigned in BattleSceneGod
5. ✅ Scene saved

To test:
1. Enter Play mode
2. Start Campaign
3. Play Level 32
4. Wait for dialogue at specified times

## Timing Tips

**Good Timing Examples:**

```
Start of battle (3-10 seconds):
- DelayMS: 3000 to 10000
- Good for introductions, threats, taunts

Early battle (10-30 seconds):
- DelayMS: 10000 to 30000
- Good for reactions to first attacks

Mid battle (30-60 seconds):
- DelayMS: 30000 to 60000
- Good for strategy changes, frustration

Late battle (60+ seconds):
- DelayMS: 60000+
- Good for desperation, final words
```

**Spacing Between Lines:**
- 2-3 seconds: Fast-paced banter
- 3-5 seconds: Normal conversation
- 5+ seconds: Dramatic pauses

Remember: Speech bubbles auto-hide after 5 seconds, so space accordingly!

## Visual Hierarchy

```
GameObject: SequencerLV032
└── Component: BattleSequencer
    └── Field: Sequence Points (Array)
        ├── [0] Sequence Point
        │   └── Field: Dialogue Actions (List)
        │       └── [0] Dialogue Action
        │           ├── Speaker: Player/Enemy
        │           └── LocalizationKey: "level32/..."
        ├── [1] Sequence Point
        │   └── Field: Dialogue Actions (List)
        │       └── [0] Dialogue Action
        └── [2] Sequence Point
            └── Field: Dialogue Actions (List)
                └── [0] Dialogue Action
```

This nested structure lets you have multiple conversations throughout the battle!

