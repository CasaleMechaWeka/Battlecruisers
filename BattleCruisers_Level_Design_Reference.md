# BattleCruisers Level Design Reference Sheet

## Overview
This document provides a comprehensive reference for designing levels, side quests, and related systems in BattleCruisers. All level data is stored in `Assets/Scripts/Data/Static/StaticData.cs`.

## 1. Level Structure

### Main Campaign Levels
- **Levels 1-31**: Standard campaign levels
- **Levels 32-40**: ChainBattle levels (special gameplay mechanics)
- **Total**: 40 main campaign levels

### Side Quests
- **31 side quests** (IDs 0-30)
- Unlocked via level progression or completing prerequisite side quests
- Provide additional content and rewards

---

## 2. Level Data Configuration

### Level Definition (in StaticData.cs `Levels` collection)
```csharp
new Level(
    levelNumber,           // int: 1-40
    Hulls.hullKey,         // IPrefabKey: Enemy cruiser type
    BackgroundMusic.track, // SoundKeyPair: Background music
    SkyMaterials.material, // string: Sky material name
    Exos.GetCaptainExoKey(captainId), // IPrefabKey: Enemy captain
    GetDefaultHeckleConfig(), // HeckleConfig: Trash talk settings
    hasSequencer           // bool: Optional - enables battle sequencer
)
```

#### Level Properties:
- **Hull**: Enemy cruiser type (determines AI behavior and appearance)
- **Background Music**: Audio track for the battle
- **Sky Material**: Visual environment theme
- **Captain/Exoskeleton**: Enemy commander appearance (IDs 0-50)
- **Heckle Config**: Random taunt system settings
- **Sequencer**: Optional cinematic battle events

---

## 3. Background Image System

### BackgroundImageStats Structure
```csharp
new BackgroundImageStats(
    cameraSize,        // float: Camera zoom level
    position,          // Vector2: Background offset position
    width, height,     // int, int: Background dimensions
    imageName,         // string: Background texture name
    overlayColor,      // Color: Tint/color overlay
    useParallax,       // bool: Parallax scrolling effect
    sortOrder          // int: Rendering layer order
)
```

### Background Assignment:
- **Levels**: Indexed by level number (0-based array index)
- **Side Quests**: Indexed by side quest ID (0-based array index)
- **Example**: Level 32 uses `LevelBackgrounds[31]` (index 31 for level 32)

---

## 4. Progression & Unlocking System

### Level Unlocking:
- **Linear progression**: Beat level N to unlock level N+1
- **Side quests**: Require specific level completion OR prerequisite side quest
- **Example**: SideQuest 23 requires level 32 completion AND side quest 22 completion

### Side Quest Definition:
```csharp
new SideQuestData(
    playerTalksFirst,           // bool: Dialogue initiative
    Exos.GetCaptainExoKey(id),  // IPrefabKey: Enemy captain
    unlockLevel,               // int: Required main level
    requiredSideQuestId,       // int: Required side quest (-1 = none)
    Hulls.hullKey,             // PrefabKey: Enemy hull
    BackgroundMusic.track,     // SoundKeyPair: Music
    SkyMaterials.material,     // string: Sky theme
    isCompleted,               // bool: Initial completion state
    sideQuestNum,              // int: Unique ID (0-30)
    heckleConfig,              // HeckleConfig: Optional taunts
    hasSequencer               // bool: Cinematic events
)
```

---

## 5. Loot & Reward System

### First-Time Completion Rewards:
- **Hulls**: New cruiser types
- **Units**: Aircraft and naval units
- **Buildings**: Structures and defenses
- **Unlocked via**: `_hullToUnlockedLevel`, `_unitToUnlockedLevel`, `_buildingToUnlockedLevel`

### Loot Assignment:
- **Main Levels**: `GetLevelLoot(levelNum)` - rewards unlocked in that level
- **Side Quests**: `GetSideQuestLoot(sideQuestId)` - rewards unlocked by completing side quest

### Unlock Level Mapping:
```csharp
_buildingToUnlockedLevel = new ReadOnlyDictionary<BuildingKey, int>()
{
    { Buildings.AntiShipTurret, 1 },  // Unlocked in level 1
    { Buildings.DroneStation4, 27 },  // Unlocked in level 27
    { Buildings.Railgun, 6 },         // Unlocked in level 6
    // ... etc
}
```

---

## 6. AI Strategy System

### Two-Part Strategy: Build Order + Offensive Requests

#### Build Orders (StaticBuildOrders.cs):
- **100-slot arrays** (0-99) of `BuildingKey` or `null`
- `null` slots = offensive request slots
- **Predefined strategies**: `Balanced`, `Boom`, `FortressPrime`, `Rush`, `LV032`

#### Offensive Requests:
- **Types**: `Buildings`, `Units`, `Air`, `Naval`, `Ultras`
- **Focus**: `Low`, `High` (intensity levels)
- **Cycles through available types** when AI hits `null` build slots

### Strategy Assignment (LevelStrategies.cs):
```csharp
// In CreateAdaptiveBaseStrategies()
StaticBuildOrders.LV032,  // Level 32

// In CreateOffensiveRequests()
new OffensiveRequest[] {
    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
    // ... etc
}
```

### Custom Strategy Creation:
```csharp
// StaticBuildOrders.cs
public static ReadOnlyCollection<BuildingKey> MyStrategy = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>() {
    StaticPrefabKeys.Buildings.AntiShipTurret,  // Opener
    StaticPrefabKeys.Buildings.DroneStation,     // Economy
    null,  // Offensive slot
    null,  // More offense
});

// LevelStrategies.cs
StaticBuildOrders.MyStrategy,  // Assign to level
new OffensiveRequest[] {  // Offensive requests for null slots
    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
}
```

---

## 7. Captain/Exoskeleton System

### Captain Data:
- **51 captains** (IDs 0-50)
- **Sprite naming**: `Exoskeleton{ID:00}{Name}.png`
- **Example**: ID 1 = "Exoskeleton01Fei.png"

### Captain Names Dictionary:
```csharp
private static readonly ReadOnlyDictionary<int, string> CaptainSpriteNames = new Dictionary<int, string> {
    { 0, "Charlie" }, { 1, "Fei" }, { 2, "Jimmo" },
    // ... up to ID 50
}
```

### Usage in Levels:
```csharp
Exos.GetCaptainExoKey(1),  // Fei (default for many levels)
Exos.GetCaptainExoKey(32), // TridentsOfPeter (uncommon)
```

---

## 8. Heckle/Trash Talk System

### HeckleConfig Structure:
```csharp
new HeckleConfig {
    enableHeckles = true,              // Master on/off
    maxHeckles = 3,                    // Total taunts per battle
    minTimeBeforeFirstHeckle = 1f,     // Earliest first taunt (seconds)
    maxTimeBeforeFirstHeckle = 60f,    // Latest first taunt (seconds)
    minTimeBetweenHeckles = 180f,      // Cooldown between taunts
    heckleOnFirstDamage = false,       // Trigger on AI first damage
    enableHealthThresholdHeckle = true,// Health-based triggers
    heckleOnHealthThreshold = 0.1f,    // Health % trigger (10%)
    heckleOnPlayerDamaged = false,     // Heavy damage triggers
    specificHeckleIndices = null       // Specific taunt IDs (empty = random)
}
```

### Default Configuration:
- **3 taunts** maximum per battle
- **1-60 second** delay before first taunt
- **3 minute** cooldown between taunts
- **Triggers at 10% health**

### Assignment:
- **Levels**: Use `GetDefaultHeckleConfig()`
- **Side Quests**: Can override with custom `HeckleConfig` or `null` for disabled

---

## 9. Bodykit/Cruiser Skin System

### BodykitData Structure:
```csharp
new BodykitData(
    nameBase,          // string: Localization key base ("Bodykit000")
    descriptionBase,   // string: Description key base
    cost,              // int: Coin cost
    id                 // int: Unique identifier
)
```

### Properties:
- **Cost**: Coin purchase price
- **Name**: Localized display name
- **Description**: Localized flavor text
- **ID**: Unique identifier for save/load

### Usage:
- **Purchasable cosmetics** for cruiser customization
- **Separate from functional upgrades** (variants)
- **Shop integration** via `BodykitItemController`

---

## 10. IAP (In-App Purchase) System

### IAPManager Products:
```csharp
public const string premium_version_product = "premium_version";    // Non-consumable
public const string small_coin_pack = "coins100_pack";              // Consumable
public const string medium_coin_pack = "coins500_pack";             // Consumable
public const string large_coin_pack = "coins1000_pack";             // Consumable
public const string extralarge_coin_pack = "coins5000_pack";        // Consumable
```

### Product Types:
- **Premium Version**: One-time purchase, unlocks premium features
- **Coin Packs**: Consumable purchases that add currency
- **Validation**: Firebase Analytics tracking
- **Processing**: `ProcessPurchase()` handles fulfillment

---

## 11. Battle Sequencer System

### Sequencer Prefabs:
- **Campaign**: `SequencerLV{Level:000}.prefab` (e.g., `SequencerLV032.prefab`)
- **Side Quest**: `SequencerSQ{SideQuestID:000}.prefab` (e.g., `SequencerSQ023.prefab`)
- **Location**: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/`

### Level Configuration:
```csharp
new Level(32, Hulls.LV032Raptor, ..., true)  // hasSequencer = true

new SideQuestData(..., 23, null, true)       // hasSequencer = true
```

### Sequencer Components:
- **SequencePoint[]**: Array of timed events
- **DelayMS**: Millisecond delay before execution
- **Faction**: `Player` or `Enemy` target
- **Actions**: Building, unit, boost, or script events

### Loading Logic (BattleSceneGod.cs):
```csharp
if (ApplicationModel.Mode == GameMode.Campaign && StaticData.Levels[ApplicationModel.SelectedLevel].HasSequencer) {
    string path = SEQUENCER_PATH + "SequencerLV" + ApplicationModel.SelectedLevel.ToString("000") + ".prefab";
    // Load and instantiate via Addressables
}
```

---

## 12. Constants & Limits

### Game Limits:
```csharp
public const int NUM_OF_LEVELS = 40;
public const int NUM_OF_CAMPAIGN_LEVELS = 40;
public const int NUM_OF_PvPLEVELS = 9;
public const int NUM_OF_STANDARD_LEVELS = 40;
public const int NUM_OF_LEVELS_IN_DEMO = 7;
public const int NUM_OF_SIDEQUESTS = 31;
```

### Build Order Limits:
- **100 slots** per build order (0-99)
- **60 offensive requests** maximum per level
- **280 heckles** available (IDs 0-279)

---

## 13. Design Workflow

### Creating a New Level:
1. **Add to Levels collection** in StaticData.cs
2. **Assign background** in LevelBackgrounds array
3. **Configure loot** in unlock dictionaries
4. **Set AI strategy** in LevelStrategies.cs
5. **Create sequencer prefab** (optional)
6. **Test Addressables** setup

### Creating a Side Quest:
1. **Add to SideQuests collection** in StaticData.cs
2. **Assign background** in SideQuestBackgrounds array
3. **Configure loot** in side quest unlock dictionaries
4. **Set unlock requirements** (level + prerequisite side quest)
5. **Create sequencer prefab** (optional)

### Custom AI Strategy:
1. **Define build order** in StaticBuildOrders.cs
2. **Create offensive requests** array
3. **Assign to level** in LevelStrategies.cs
4. **Test AI behavior** in battles

---

## 14. File Organization

### Core Files:
- `Assets/Scripts/Data/Static/StaticData.cs` - Main level data
- `Assets/Scripts/Data/Static/StaticBuildOrders.cs` - AI build orders
- `Assets/Scripts/Data/Static/Strategies/Helper/LevelStrategies.cs` - Strategy assignment

### Asset Locations:
- **Sequencers**: `Assets/Resources_moved/Prefabs/BattleScene/Sequencer/`
- **Hulls**: `Assets/Resources_moved/Prefabs/BattleScene/Hulls/`
- **Addressables**: `Assets/AddressableAssetsData/AssetGroups/`

### Configuration Files:
- **IAP**: `Assets/Scripts/UI/IAPManager.cs`
- **Bodykits**: `Assets/Scripts/UI/ScreensScene/ShopScreens/BodykitData.cs`
- **Heckles**: Integrated into level/SideQuestData definitions

---

## 15. Best Practices

### Level Design:
- **Balance difficulty** through hull selection and AI strategy
- **Variety backgrounds** enhance visual interest
- **Reward progression** should feel meaningful
- **Test sequencers** thoroughly (Addressables setup critical)

### AI Strategy:
- **Economic buildings** first for sustainable growth
- **Offensive requests** should match level difficulty
- **Null slots** allow dynamic adaptation
- **Test extensively** against different player strategies

### Content Creation:
- **Consistent naming** for assets and localization
- **Proper indexing** (0-based arrays)
- **Addressables setup** for all prefab references
- **Save/load compatibility** when adding new fields

This reference covers the core systems needed for level design in BattleCruisers. Always test changes thoroughly and ensure Addressables are rebuilt after adding new assets.
