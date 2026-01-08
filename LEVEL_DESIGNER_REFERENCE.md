# Battlecruisers Level Designer Reference Sheet

> **Quick Reference**: Complete technical guide to all level design systems and static data

---

## Table of Contents
1. [Game Architecture Fundamentals](#game-architecture-fundamentals)
2. [Level Types Overview](#level-types-overview)
2. [Campaign Level Configuration](#campaign-level-configuration)
3. [SideQuest Level Configuration](#sidequest-level-configuration)
4. [Background Image System](#background-image-system)
5. [Sky Materials](#sky-materials)
6. [Background Music](#background-music)
7. [Heckle System](#heckle-system)
8. [Trash Talk (Pre-Battle Dialogue)](#trash-talk-pre-battle-dialogue)
9. [Loot & Unlock System](#loot--unlock-system)
10. [AI Strategy System](#ai-strategy-system)
11. [Battle Sequencer System](#battle-sequencer-system)
12. [Captains (Exoskeletons)](#captains-exoskeletons)
13. [Hulls (Cruisers)](#hulls-cruisers)
14. [Bodykits & Variants](#bodykits--variants)
15. [Quick Reference Tables](#quick-reference-tables)
16. [Constants & Limits](#constants--limits)
17. [IAP System](#iap-in-app-purchase-system)
18. [File Organization](#file-organization)
19. [Design Workflow](#design-workflow)
20. [Best Practices](#best-practices)

---

## Game Architecture Fundamentals

### Core Concept: What is a Cruiser?

A **Cruiser** = **Hull** (model/chassis) + **Bodykit** (cosmetic skin)

- **Hull**: The core ship type (Raptor, Trident, Salvage, etc.) that determines appearance, slot count, and abilities
- **Bodykit**: Optional cosmetic skin that changes the hull's visual appearance without affecting gameplay
- **Example**: `Hulls.Raptor` is a specific hull; `Bodykits.Bodykit001` is a purchasable skin players can apply

### Buildables System

**Buildables** = anything that can be constructed, split into two categories:

1. **Buildings** (go in slots)
   - Placed on specific cruiser slots
   - Examples: DroneStation, MissilePod, ShieldGenerator, NavalFactory
   - Can be destroyed/replaced via sequencer

2. **Units** (produced by factories)
   - Spawned by Factory buildings (AirFactory, NavalFactory)
   - NOT placed in slots; they spawn automatically when factory produces them
   - Examples: Frigate, Destroyer, Bomber, Fighter
   - Can be manually spawned via sequencer `UnitAction`

### Slot System

Every cruiser has multiple **slots** (typically 5-50, usually around 20 total).

**Critical Clarification**: Build order array index ≠ Slot ID

- **Build Order Array**: `[0] [1] [2]... [9] [10]... [99]` = sequential list of WHAT buildings to build
- **Cruiser Slots**: Each slot has a specific ID and TYPE (bow, stern, deck, mast, etc.)
- **Placement Logic**: When AI builds item [9] from build order, the system finds the next available compatible slot for that building type and places it there

**Example: Level 33 Salvage Hull**
- Salvage has exactly 12 usable slots with specific types
- Build order item [0] = DroneStation → placed in first available economy slot
- Build order item [9] = NavalFactory → placed in the bow slot (Slot ID 0, the only slot that accepts factories on Salvage)
- Build order items are processed sequentially, but slot placement depends on hull's physical slot layout and building requirements

**Slot Type System**:
- Each slot has a specific type (bow, stern, deck, mast, hangar, etc.)
- Each building has slot type requirements (e.g., NavalFactory only fits in bow slots)
- Slot assignment system finds compatible slots automatically

### AI Strategy & Unit Production

**Build Orders** (Fixed):
- Define BUILDING placement only (which buildings to place in which slots)
- Cannot directly specify units in build order
- `null` slots trigger offensive requests for additional buildings

**Offensive Requests**:
- When AI hits a `null` slot, offensive request determines what building to place there
- Types: Buildings, Air (units), Naval (units), Ultras (buildings)
- BUT: When requesting "Air" or "Naval" type, AI doesn't directly build units
- Instead: AI builds the appropriate Factory → Factory produces those unit types automatically

**Example Flow**:
```
Build Order [0-11] specifies: DroneStation, DroneStation, ShieldGenerator, DroneStation4, etc.
AI processes sequentially:
  → Build item [0] (DroneStation) → Slot System finds compatible slot → Places in economy slot
  → Build item [1] (DroneStation) → Finds next economy slot → Places there
  → ... continues through build order ...
  → Build item [9] (NavalFactory) → Finds bow slot → Places NavalFactory in Slot ID 0
NavalFactory in Slot ID 0 → Automatically produces: Frigate, Destroyer, ArchonBattleship, etc.
```

---

## Level Types Overview

| Type | Count | File | Index Pattern |
|------|-------|------|---------------|
| Campaign Levels | 40 | `StaticData.Levels[]` | 1-based (Level 1 = index 0) |
| SideQuests | 31 | `StaticData.SideQuests[]` | 0-based (SQ 0 = index 0) |
| PvP Levels | 9 | `StaticData.PvPLevels{}` | Map enum key |

---

## Campaign Level Configuration

**File**: `Assets/Scripts/Data/Static/StaticData.cs` → `Levels` collection  
**Class**: `Assets/Scripts/Data/Level.cs`

### Constructor Signature

```csharp
new Level(
    int num,                    // Level number (1-40)
    IPrefabKey hull,            // Enemy cruiser hull
    SoundKeyPair musicKeys,     // Background music
    string skyMaterialName,     // Sky/atmosphere
    IPrefabKey captain,         // Enemy exoskeleton
    HeckleConfig heckleConfig,  // Mid-battle taunts (optional)
    bool hasSequencer = false   // Enable battle sequencer
)
```

### Example

```csharp
// Level 32: ChainBattle with sequencer
new Level(32, Hulls.LV032Raptor, BackgroundMusic.Juggernaut, SkyMaterials.Sunrise, 
    Exos.GetCaptainExoKey(1), GetDefaultHeckleConfig(), true)
```

### Level Properties

| Property | Type | Description |
|----------|------|-------------|
| `Num` | int | Level number (1-based) |
| `Hull` | IPrefabKey | Enemy cruiser prefab |
| `MusicKeys` | SoundKeyPair | Background + combat music |
| `SkyMaterialName` | string | Skybox material name |
| `Captains` | IPrefabKey | Enemy captain exoskeleton |
| `HeckleConfig` | HeckleConfig | Random taunt configuration |
| `HasSequencer` | bool | Load custom battle sequencer |

---

## SideQuest Level Configuration

**File**: `Assets/Scripts/Data/Static/StaticData.cs` → `SideQuests` collection  
**Class**: `Assets/Scripts/UI/ScreensScene/LevelsScreen/SideQuestData.cs`

### Constructor Signature

```csharp
new SideQuestData(
    bool playerTalksFirst,         // Pre-battle dialogue order
    IPrefabKey enemyCaptainExo,    // Enemy exoskeleton
    int unlockRequirementLevel,    // Main campaign level required (0 = always)
    int requiredSideQuestID,       // SideQuest chain (-1 = none)
    PrefabKey hull,                // Enemy cruiser hull
    SoundKeyPair musicBackgroundKey,
    string skyMaterial,
    bool isCompleted,              // Always false in static data
    int sideLevelNum,              // SideQuest index (0-30)
    HeckleConfig heckleConfig,     // Optional
    bool hasSequencer = false      // Enable battle sequencer
)
```

### Example

```csharp
// SideQuest 23: Requires Level 32, follows SQ 22, has sequencer
new SideQuestData(false, Exos.GetCaptainExoKey(49), 32, 22, 
    Hulls.FortressPrime, BackgroundMusic.Fortress, SkyMaterials.Midnight, 
    false, 23, null, true)
```

### Unlock Chain Example

```
SideQuest 0:  unlockRequirementLevel=32, requiredSideQuestID=-1  (unlocks at level 32)
SideQuest 1:  unlockRequirementLevel=32, requiredSideQuestID=0   (unlocks after SQ 0)
SideQuest 2:  unlockRequirementLevel=32, requiredSideQuestID=1   (unlocks after SQ 1)
```

---

## Background Image System

**File**: `Assets/Scripts/Data/Static/StaticData.cs` → `LevelBackgrounds[]` and `SideQuestBackgrounds[]`  
**Class**: `Assets/Scripts/UI/BattleScene/Clouds/Stats/BackgroundImageStats.cs`

### Constructor

```csharp
new BackgroundImageStats(
    float scale,              // Sprite scale (typically 50-160)
    Vector2 positionAt4to3,   // Position for 4:3 aspect ratio
    float yPositionAt16to9,   // Y position for 16:9
    float yPositionAt24to10,  // Y position for 24:10 (ultrawide)
    string spriteName,        // Sprite asset name (null = no background)
    Color colour,             // Tint/overlay color (RGBA 0-1)
    bool flipX,               // Mirror horizontally
    int orderInLayer          // Rendering order (typically 10, -50 for Paris)
)
```

### Position Guidelines

| Y Position Range | Effect |
|------------------|--------|
| 400-700 | Background visible in upper portion |
| 200-400 | Background centered |
| -300 to 0 | Background low (fight club style) |

### Available Backgrounds

```
EvenNewerZealand, Sydney, KualaLimpur, Himalayas, Egypt, Paris, SeaWall,
Rio, Andes, Sanfran, China, Dubai, London, NYC, TwinCityFlotilla,
BlimpCity, Russia, FightClub, Junkyard, Antarctica, CapeTown,
TableMountain, RicketyCity, UACBattleNight, NuclearDome, UACArena,
Rio2, UACUltimate, MercenaryOne, Wreckyards, Oz, ImperiusTower,
TowerCity, BlockCity, UACHQ
```

---

## Sky Materials

**File**: `Assets/Scripts/Data/Static/SkyMaterials.cs`

| Constant | Material Name | Description |
|----------|---------------|-------------|
| `SkyMaterials.Morning` | Skybox1-Morning | Warm sunrise tones |
| `SkyMaterials.Sunrise` | Skybox2-Sunrise | Golden hour |
| `SkyMaterials.Purple` | Skybox3-Purple | Dramatic purple hues |
| `SkyMaterials.Cold` | Skybox4-Cold | Blue/grey atmosphere |
| `SkyMaterials.Midday` | Skybox5-Midday | Bright clear sky |
| `SkyMaterials.Dusk` | Skybox6-Dusk | Evening oranges |
| `SkyMaterials.Midnight` | Skybox7-Midnight | Night sky |

---

## Background Music

**File**: `Assets/Scripts/Data/Static/SoundKeys.cs` → `Music.Background`

| Key | Usage |
|-----|-------|
| `BackgroundMusic.Bobby` | Upbeat action |
| `BackgroundMusic.Juggernaut` | Heavy combat |
| `BackgroundMusic.Experimental` | Unusual encounters |
| `BackgroundMusic.Nothing` | Ambient/minimal |
| `BackgroundMusic.Confusion` | Chaotic battles |
| `BackgroundMusic.Sleeper` | Subtle tension |
| `BackgroundMusic.Againagain` | Repetitive rhythm |
| `BackgroundMusic.Fortress` | Boss/epic |

---

## Heckle System

**File**: `Assets/Scripts/Data/HeckleConfig.cs`

### Configuration

```csharp
new HeckleConfig
{
    // Master switch
    enableHeckles = true,              // Enable/disable all heckles
    
    // Time-based triggers
    maxHeckles = 3,                    // Total heckles per battle (0-10)
    minTimeBeforeFirstHeckle = 1f,     // Seconds until first possible heckle (5-60)
    maxTimeBeforeFirstHeckle = 60f,    // Max delay for first heckle (10-120)
    minTimeBetweenHeckles = 180f,      // Cooldown between heckles (5-60)
    
    // Event-based triggers
    heckleOnFirstDamage = false,       // Trigger on first damage (not implemented)
    enableHealthThresholdHeckle = true,// Enable health-based trigger
    heckleOnHealthThreshold = 0.1f,    // Health % trigger (0.0-1.0)
    heckleOnPlayerDamaged = false,     // Trigger on player damage (not implemented)
    
    // Specific heckles (optional)
    specificHeckleIndices = null       // List<int> of specific heckle IDs (0-279)
}
```

### Default Configuration

```csharp
private static HeckleConfig GetDefaultHeckleConfig()
{
    return new HeckleConfig
    {
        enableHeckles = true,
        maxHeckles = 3,
        minTimeBeforeFirstHeckle = 1f,
        maxTimeBeforeFirstHeckle = 60f,
        minTimeBetweenHeckles = 180f,
        heckleOnFirstDamage = false,
        enableHealthThresholdHeckle = true,
        heckleOnHealthThreshold = 0.1f,
        heckleOnPlayerDamaged = false
    };
}
```

### Heckle Pool

- 280 heckle entries available (indices 0-279)
- Each entry: `new HeckleData(15, index)` (15 = display duration)
- Random selection unless `specificHeckleIndices` is provided

---

## Trash Talk (Pre-Battle Dialogue)

**File**: `Assets/Scripts/Data/Static/StaticData.cs` → `LevelTrashTalk[]` and `SideQuestTrashTalk[]`  
**Class**: `Assets/Scripts/UI/ScreensScene/TrashScreen/TrashTalkData.cs`

### Constructor

```csharp
new TrashTalkData(
    int levelNumber,        // Level/SideQuest number for localization key
    int exoId,              // Captain exoskeleton ID (0-50)
    bool playerTalksFirst,  // Dialogue order
    string stringKeyBasePrefix  // "level" or "sideQuest"
)
```

### Localization Keys Generated

```
{prefix}{levelNumber}/EnemyName     → Enemy display name
{prefix}{levelNumber}/EnemyText     → Enemy dialogue
{prefix}{levelNumber}/PlayerText    → Player dialogue
{prefix}{levelNumber}/AppraisalDroneText → Drone analysis
```

### Example

```csharp
new TrashTalkData(32, 1, false, "level")
// Generates keys: level32/EnemyName, level32/EnemyText, etc.
// Uses Captain ID 1 (Fei)
// Enemy speaks first
```

---

## Loot & Unlock System

### Unlock Sources

| Content Type | Main Campaign | SideQuest |
|--------------|---------------|-----------|
| Buildings | `_buildingToUnlockedLevel` | `_buildingToCompletedSideQuest` |
| Units | `_unitToUnlockedLevel` | `_unitToCompletedSideQuest` |
| Hulls | `_hullToUnlockedLevel` | `_hullToCompletedSideQuest` |

### Unlock Logic

```csharp
// Campaign: Unlocks AFTER completing level (availabilityLevel = completedLevel + 1)
{ Buildings.ShieldGenerator, 2 }  // Available from Level 2 (unlocked by completing Level 1)

// SideQuest: Unlocks WHEN completing specific SideQuest
{ Buildings.IonCannon, 5 }        // Unlocked by completing SideQuest 5
```

### Special Values

| Value | Meaning |
|-------|---------|
| 1 | Available from start |
| 95 | Not unlocked via main campaign (SideQuest only) |

### Loot Retrieval

```csharp
// Campaign loot
Loot loot = StaticData.GetLevelLoot(levelCompleted);

// SideQuest loot  
Loot loot = StaticData.GetSideQuestLoot(sideQuestID);
```

---

## AI Strategy System

**Files**:
- `Assets/Scripts/Data/Static/StaticBuildOrders.cs` - Build order templates
- `Assets/Scripts/Data/Static/Strategies/Helper/LevelStrategies.cs` - Campaign level assignments
- `Assets/Scripts/Data/Static/Strategies/Helper/SideQuestStrategies.cs` - SideQuest assignments

### Strategy = Build Order + Offensive Requests

The AI uses a two-part strategy:
1. **Build Order**: Fixed sequence of buildings to construct
2. **Offensive Requests**: What to build when hitting `null` slots

### Build Order Templates

```csharp
// StaticBuildOrders.cs
public static ReadOnlyCollection<BuildingKey> Balanced = new ReadOnlyCollection<BuildingKey>(
    new List<BuildingKey>()
    {
        StaticPrefabKeys.Buildings.DroneStation,     // Slot 0
        StaticPrefabKeys.Buildings.DroneStation,     // Slot 1
        StaticPrefabKeys.Buildings.DroneStation,     // Slot 2
        StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 3
        null,  // ← Offensive slot (uses OffensiveRequest)
        StaticPrefabKeys.Buildings.DroneStation,     // Slot 5
        // ...
    });
```

### Available Templates

| Template | Style | Description |
|----------|-------|-------------|
| `Balanced` | Mixed | Economy + defense + offense |
| `Boom` | Economic | Heavy drone stations first |
| `Rush` | Aggressive | Early offensive slots |
| `FortressPrime` | Late-game | Massive economy scaling |
| `LV032` | Custom | 100 slots with 70+ offensive opportunities |

### Offensive Request Types

**Important**: These types determine what the AI will BUILD when filling a `null` slot:

```csharp
public enum OffensiveType
{
    Air,        // Request → AI builds AirFactory
    Naval,      // Request → AI builds NavalFactory
    Buildings,  // Request → AI builds offensive buildings (Artillery, Railgun, MissilePod, etc.)
    Ultras      // Request → AI builds ultra weapons (NukeLauncher, Deathstar, etc.)
}

public enum OffensiveFocus
{
    Low,   // Basic/cheaper factories or buildings
    High   // Advanced/expensive factories or buildings
}
```

**Critical Clarification**:
- `OffensiveType.Naval` does NOT mean "build naval units"
- It means "build a NavalFactory" → which then produces naval units automatically
- Same for `OffensiveType.Air` → builds AirFactory → produces aircraft
- Only `OffensiveType.Buildings` and `OffensiveType.Ultras` are actual building types placed in slots

### Offensive Request Arrays

```csharp
// Each null in build order consumes one offensive request
new OffensiveRequest[]
{
    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),  // First null → build cheap offensive building
    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),    // Second null → build expensive ultra
    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),      // Third null → build NavalFactory (produces ships)
    // Array cycles when exhausted
}
```

**Unit Production Flow**:
- When OffensiveType is `Air` or `Naval`, AI builds the respective factory
- Factory then automatically produces units as drones become available
- **Normal battles**: which units are produced depends on `GameModel.GetUnlockedUnits(UnitCategory)`
- **Sequencer / ChainBattle (`HasSequencer=true`)**: enemy naval factories bypass unlock gating and cycle through **all naval units**, starting at **Frigate** (see `AIManager.GetSequencerShipCycle`)

### Creating Custom Strategy

**Step 1**: Define build order in `StaticBuildOrders.cs`

```csharp
public static ReadOnlyCollection<BuildingKey> MyCustomStrategy = 
    new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
{
    StaticPrefabKeys.Buildings.AntiShipTurret,  // Opener
    StaticPrefabKeys.Buildings.DroneStation,
    StaticPrefabKeys.Buildings.DroneStation,
    null, null, null,  // 3 offensive slots
    StaticPrefabKeys.Buildings.ShieldGenerator,
    null, null,        // 2 more offensive slots
});
```

**Step 2**: Assign in `LevelStrategies.cs` or `SideQuestStrategies.cs`

```csharp
// CAMPAIGN: In LevelStrategies.CreateAdaptiveBaseStrategies()
// Index = levelNum - 1 (Level 32 = index 31)
StaticBuildOrders.MyCustomStrategy,

// SIDEQUEST: In SideQuestStrategies.CreateAdaptiveBaseStrategies()
// Index = sideQuestID (SideQuest 23 = index 23)
StaticBuildOrders.MyCustomStrategy,

// In CreateOffensiveRequests() at same index
new OffensiveRequest[]
{
    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
}
```

> ⚠️ **Index Difference**: Campaign uses `levelNum - 1`, SideQuest uses `sideQuestID` directly

### AI Banned Buildings

```csharp
// AI will never build these as ultras:
StaticData.AIBannedUltrakeys = {
    Buildings.KamikazeSignal,  // Only effective with many planes
    Buildings.Ultralisk        // Only effective when followed by more builds
}
```

---

## Battle Sequencer System

**Class**: `Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`

### Enabling Sequencer

1. Set `hasSequencer = true` in Level or SideQuestData
2. Create prefab at correct path:
   - Campaign: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/SequencerLV{XXX}.prefab`
   - SideQuest: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/SequencerSQ{XXX}.prefab`
3. Add as Addressable asset

### SequencePoint Structure

```csharp
[Serializable]
public class SequencePoint
{
    public int DelayMS;                      // Milliseconds from battle start
    public Faction Faction;                  // Player or Enemy (Enemy = AI Cruiser)
    public List<BuildingAction> BuildingActions;
    public List<BoostAction> BoostActions;
    public List<UnitAction> UnitActions;
    public ScriptCallAction ScriptCallActions;  // Custom UnityEvents
}
```

### BuildingAction

```csharp
public class BuildingAction
{
    public BuildingOp Operation;      // Add or Destroy
    public PrefabKeyName PrefabKeyName;
    public byte SlotID;
    public bool IgnoreDroneReq;       // Build without drones
    public bool IgnoreBuildTime;      // Instant construction
}
```

### UnitAction

```csharp
public class UnitAction
{
    public PrefabKeyName PrefabKeyName;
    public Vector2 Postion;           // Spawn location
    public Vector2 SpawnArea;         // Random spread area
    public byte Amount;               // Number to spawn
}
```

### BoostAction

```csharp
public class BoostAction
{
    public BoostOp Operation;         // Add, Remove, or Replace
    public BoostType BoostType;
    public float BoostAmount;
}
```

### Custom Dialogue (Current Implementation)

**Status**: Currently implemented via `ScriptCallActions` UnityEvents. Full `DialogueAction` system is planned (see `CUSTOM_DIALOGUE_SYSTEM.md`).

**Current Method - Using ScriptCallActions:**

1. Create public dialogue methods on BattleSequencer:
```csharp
public void ShowPlayerDialogue(string localizationKey)
{
    // Call player speech bubble with key
}

public void ShowEnemyDialogue(string localizationKey)
{
    // Call enemy speech bubble with key
}
```

2. In sequencer prefab Inspector, add to `ScriptCallActions`:
   - Target: BattleSequencer component
   - Method: `ShowPlayerDialogue` or `ShowEnemyDialogue`
   - Parameter: Localization key (e.g., "level32/PlayerChat1")

3. Localization keys follow format:
   - `level{N}/PlayerChat1`, `level{N}/EnemyChat1`, etc.
   - `sideQuest{N}/PlayerChat1`, `sideQuest{N}/EnemyChat1`, etc.

**Future Enhancement - DialogueAction System:**

The `CUSTOM_DIALOGUE_SYSTEM.md` documents a planned enhancement adding native `DialogueAction` support:
- `DialogueAction` class with `SpeakerType` enum (Player/Enemy)
- Native dialogue processing in `ProcessSequencePoint()`
- Direct localization key assignment in Inspector

When implemented, will replace need for ScriptCallActions workaround.

---

## Captains (Exoskeletons)

**File**: `StaticData.Captains[]` (IDs 0-50)

### CaptainData Structure

```csharp
new CaptainData(int cost, int id)
```

### Captain IDs and Names

| ID | Name | Cost | ID | Name | Cost |
|----|------|------|----|------|------|
| 0 | Charlie | 0 | 26 | Presidentron | 2732 |
| 1 | Fei | 50 | 27 | KingKupa | 180 |
| 2 | Jimmo | 40 | 28 | Chimothy | 150 |
| 3 | Hexapod | 60 | 29 | Kentron | 200 |
| 4 | Destruktor | 120 | 30 | LittleWillie | 30 |
| 5 | Huntress | 340 | 31 | HuntressPrime | 2500 |
| 6 | Craner | 60 | 32-39 | (SQ bosses) | 110-640 |
| 7 | Bort | 80 | 40 | HansGrubot | 640 |
| 8 | DogSquared | 750 | 41-42 | LangR/UrbA | 2000 |
| 9 | Clived | 200 | 43 | SpikyJoe | 200 |
| 10 | Artnuncle | 100 | 44 | Xilen | 960 |
| 11 | Androwett | 80 | 45 | Garvitron | 1400 |
| 12 | HuntressV2 | 320 | 46-49 | Peter series | 777 |
| 13 | Li | 900 | 50 | Barbester | 560 |

### Getting Captain Keys

```csharp
Exos.GetCaptainExoKey(captainId)  // Returns CaptainExoKey for prefab

StaticData.GetCaptainSpriteFilename(captainId)  // Returns "Exoskeleton{ID:00}{Name}.png"
```

---

## Hulls (Cruisers)

**File**: `StaticPrefabKeys.Hulls`

### Player-Unlockable Hulls

| Hull | Unlock Level | Unlock SideQuest |
|------|--------------|------------------|
| Trident | 1 (start) | - |
| Raptor | 4 | - |
| Bullshark | 8 | - |
| Rockjaw | 11 | - |
| Eagle | 15 | - |
| Hammerhead | 19 | - |
| Longbow | 23 | - |
| Megalodon | 26 | - |
| Rickshaw | 34 | SQ 1 |
| TasDevil | 35 | SQ 2 |
| BlackRig | 37 | SQ 4 |
| Yeti | 40 | SQ 7 |
| Megalith | 45 | SQ 25 |
| Microlodon | - | SQ 12 |
| Flea | - | SQ 13 |
| Shepherd | - | SQ 15 |
| Pistol | - | SQ 26 |
| Goatherd | - | SQ 27 |

### Enemy-Only Hulls

```
ManOfWarBoss, HuntressBoss, LV032Raptor, FortressPrime,
BasicRig, Cricket, FortNova, Zumwalt, Yucalux, TekGnosis,
Salvage, Orac, Middlodon, Essex, Axiom, October, EndlessWall,
AlphaSpace, Arkdeso
```

---

## Bodykits & Variants

### Bodykits (Hull Skins/Cosmetics)

A **Bodykit** is a cosmetic skin that changes a hull's visual appearance without affecting gameplay.

**Relationship**:
- Hull = Base ship model (Raptor, Trident, Salvage, etc.)
- Bodykit = Visual customization for that hull
- Players can apply a Bodykit to any hull they own for cosmetic variety

**Structure**:
```csharp
new BodykitData(nameBase, descriptionBase, cost, id)
// 59 bodykits (IDs 0-58), costs 300-1650 coins
```

### Variants (Buildable Upgrades)

A **Variant** is a purchasable upgrade applied to a buildable (building or unit) that modifies its characteristics.

```csharp
new VariantData(variantNameBase, coins, credits, id)
// 131 variants (IDs 0-130), 662 credits each
```

### Variant Types

| Type | Effect |
|------|--------|
| DoubleShot | Fires 2 projectiles |
| TripleShot | Fires 3 projectiles |
| QuickBuild | Reduced build time |
| RapidFire | Faster fire rate |
| Robust | More health |
| Refined | Better accuracy |
| Damaging | More damage |
| LongRange | Extended range |
| Sniper | Maximum range |

---

## Quick Reference Tables

### New Campaign Level Checklist

```
□ Add to StaticData.Levels[]
  → Level number, Hull, Music, Sky, Captain, HeckleConfig, Sequencer
  
□ Add to StaticData.LevelBackgrounds[]
  → BackgroundImageStats at matching index
  
□ Add to StaticData.LevelTrashTalk[]
  → TrashTalkData at matching index
  
□ Add strategy in LevelStrategies.cs
  → CreateAdaptiveBaseStrategies() at index (level-1)
  → CreateOffensiveRequests() at same index
  
□ Add localization keys in StoryTable
  → level{N}/EnemyName, EnemyText, PlayerText, AppraisalDroneText

□ (Optional) Add loot unlocks
  → _buildingToUnlockedLevel, _unitToUnlockedLevel, _hullToUnlockedLevel
  
□ (Optional) Create SequencerLV{XXX}.prefab if hasSequencer=true
```

### New SideQuest Checklist

```
□ Add to StaticData.SideQuests[]
  → All SideQuestData fields

□ Add to StaticData.SideQuestBackgrounds[]
  → At matching index (reuse if under 14 entries)

□ Add to StaticData.SideQuestTrashTalk[]
  → At matching index

□ Add strategy in SideQuestStrategies.cs

□ Add localization keys
  → sideQuest{N}/EnemyName, EnemyText, PlayerText, AppraisalDroneText

□ (Optional) Add loot unlocks
  → _buildingToCompletedSideQuest, _unitToCompletedSideQuest, _hullToCompletedSideQuest

□ (Optional) Create SequencerSQ{XXX}.prefab if hasSequencer=true
```

### Index Alignment Warning

> ⚠️ **CRITICAL**: The following collections must have matching indices:
> - `Levels[]` (index = levelNum - 1)
> - `LevelBackgrounds[]` (same index)
> - `LevelTrashTalk[]` (same index)
> - `LevelStrategies` arrays (same index)

---

## Constants & Limits

```csharp
public const int NUM_OF_LEVELS = 40;
public const int NUM_OF_CAMPAIGN_LEVELS = 40;
public const int NUM_OF_SIDEQUESTS = 31;
public const int NUM_OF_PvPLEVELS = 9;
public const int NUM_OF_LEVELS_IN_DEMO = 7;
public static int LastLevelWithLoot = 40;
```

### Build Order Limits
- **100 slots** per build order (0-99)
- **60 offensive requests** maximum per level
- **280 heckles** available (IDs 0-279)
- **51 captains** available (IDs 0-50)

---

## IAP (In-App Purchase) System

**File**: `Assets/Scripts/UI/IAPManager.cs`

### Product IDs

```csharp
public const string premium_version_product = "premium_version";  // Non-consumable
public const string small_coin_pack = "coins100_pack";            // Consumable
public const string medium_coin_pack = "coins500_pack";           // Consumable
public const string large_coin_pack = "coins1000_pack";           // Consumable
public const string extralarge_coin_pack = "coins5000_pack";      // Consumable
```

### IAPData Structure

```csharp
new IAPData(0, 0.99f, 100),   // (id, price, coins)
new IAPData(0, 1.99f, 500),
new IAPData(0, 2.99f, 1000),
new IAPData(0, 3.99f, 5000)
```

### Product Types
| Type | Description |
|------|-------------|
| Premium Version | One-time purchase, unlocks premium features |
| Coin Packs | Consumable purchases that add currency |

---

## File Organization

### Core Data Files
| File | Purpose |
|------|---------|
| `Assets/Scripts/Data/Static/StaticData.cs` | Main level data, unlocks, loot |
| `Assets/Scripts/Data/Static/StaticBuildOrders.cs` | AI build order templates |
| `Assets/Scripts/Data/Static/Strategies/Helper/LevelStrategies.cs` | Campaign strategy assignment |
| `Assets/Scripts/Data/Static/Strategies/Helper/SideQuestStrategies.cs` | SideQuest strategy assignment |
| `Assets/Scripts/Data/Level.cs` | Level class definition |
| `Assets/Scripts/UI/ScreensScene/LevelsScreen/SideQuestData.cs` | SideQuest class definition |

### Asset Locations
| Asset Type | Path |
|------------|------|
| Sequencers | `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/` |
| Hulls | `Assets/Resources_moved/Prefabs/BattleScene/Hulls/` |
| Captain Sprites | `Assets/Resources_moved/Sprites/Exoskeletons/` |
| Addressables Config | `Assets/AddressableAssetsData/AssetGroups/` |
| Background Images | `Assets/Resources_moved/Sprites/Backgrounds/` |

### Configuration Files
| System | File |
|--------|------|
| IAP | `Assets/Scripts/UI/IAPManager.cs` |
| Bodykits | `Assets/Scripts/UI/ScreensScene/ShopScreens/BodykitData.cs` |
| Heckles | `Assets/Scripts/Data/HeckleConfig.cs` |
| Sky Materials | `Assets/Scripts/Data/Static/SkyMaterials.cs` |
| Sound Keys | `Assets/Scripts/Data/Static/SoundKeys.cs` |

---

## Design Workflow

### Creating a New Campaign Level

```
Step 1: StaticData.cs
   └── Add Level to Levels[] collection
   └── Add BackgroundImageStats to LevelBackgrounds[]
   └── Add TrashTalkData to LevelTrashTalk[]
   └── (Optional) Add unlock entries to _buildingToUnlockedLevel, etc.

Step 2: LevelStrategies.cs
   └── Add build order to CreateAdaptiveBaseStrategies() at index (levelNum-1)
   └── Add offensive requests to CreateOffensiveRequests() at same index

Step 3: Localization
   └── Add keys: level{N}/EnemyName, EnemyText, PlayerText, AppraisalDroneText

Step 4: (Optional) Sequencer
   └── Create SequencerLV{XXX}.prefab
   └── Add to Addressables group
   └── Set hasSequencer=true in Level constructor
```

### Creating a New SideQuest

```
Step 1: StaticData.cs
   └── Add SideQuestData to SideQuests[] collection
   └── Add BackgroundImageStats to SideQuestBackgrounds[] (or reuse existing)
   └── Add TrashTalkData to SideQuestTrashTalk[]
   └── (Optional) Add unlock entries to _buildingToCompletedSideQuest, etc.

Step 2: SideQuestStrategies.cs
   └── Add build order to CreateAdaptiveBaseStrategies() at index (sideQuestID)
   └── Add offensive requests to CreateOffensiveRequests() at same index

Step 3: Localization
   └── Add keys: sideQuest{N}/EnemyName, EnemyText, PlayerText, AppraisalDroneText

Step 4: (Optional) Sequencer
   └── Create SequencerSQ{XXX}.prefab
   └── Add to Addressables group
   └── Set hasSequencer=true in SideQuestData constructor
```

### Creating a Custom AI Strategy

```
Step 1: StaticBuildOrders.cs
   └── Define new ReadOnlyCollection<BuildingKey>
   └── Use null for offensive slots, specific buildings for fixed builds

Step 2: LevelStrategies.cs or SideQuestStrategies.cs
   └── Reference your build order in CreateAdaptiveBaseStrategies()
   └── Create matching OffensiveRequest[] in CreateOffensiveRequests()

Step 3: Test
   └── Play level multiple times
   └── Verify AI builds correctly
   └── Adjust offensive focus (Low/High) for difficulty
```

---

## Best Practices

### Level Design
- **Balance difficulty** through hull selection and AI strategy combination
- **Vary backgrounds** to enhance visual interest across level sets
- **Reward progression** should feel meaningful (new units/buildings unlock gameplay)
- **Test sequencers thoroughly** - Addressables setup is critical for loading

### AI Strategy
- **Economic buildings first** for sustainable AI growth (DroneStation early)
- **Offensive requests** should match intended level difficulty
- **Null slots** allow dynamic adaptation to player behavior
- **Avoid banned ultras** (KamikazeSignal, Ultralisk) in early offensive requests
- **Test extensively** against different player strategies and loadouts

### Content Creation
- **Consistent naming** for assets and localization keys
- **Proper indexing** - Campaign uses 1-based levels but 0-based arrays
- **Rebuild Addressables** after adding new prefab references
- **Save/load compatibility** - be cautious when adding new fields to data classes

### Common Pitfalls
| Issue | Solution |
|-------|----------|
| Sequencer not loading | Verify Addressables group, rebuild catalog |
| Wrong background | Check array index alignment (level-1) |
| AI not building | Verify strategy array has enough entries |
| Loot not unlocking | Check unlock dictionary value matches level |
| Heckles not appearing | Verify `enableHeckles = true` in config |

---

*Document generated from Battlecruisers codebase analysis*

### Designer Quick Notes

**Slot System Clarification**:
- Cruisers have 5-50 slots (typically ~20), each with a specific type (bow, stern, deck, mast, etc.)
- Build order array indices `[0] [1] [2]... [99]` are NOT slot IDs
- Build order specifies WHAT buildings to construct in sequence
- Slot system automatically places each building in the next compatible slot type
- Example: Build order item [9] might be NavalFactory → automatically placed in bow slot (Slot ID 0) on Salvage hull

**Unit Production**:
- Once a factory (Naval or Air) is placed in a slot via build order, it automatically produces units
- **Normal battles**: which units are produced depends on `GameModel.GetUnlockedUnits(UnitCategory)` (e.g., GunBoat appears after SideQuest 20 unlocks it)
- **Sequencer / ChainBattle (`HasSequencer=true`)**: enemy naval factories bypass unlock gating and cycle through all naval units starting at Frigate

**Buildable vs Factory**:
- "Buildings" in build orders = structures placed in slots (includes factories)
- Factories are a TYPE of building that produce units as a secondary effect
- Offensive requests can trigger factory builds, which then auto-produce units based on available unlocks

