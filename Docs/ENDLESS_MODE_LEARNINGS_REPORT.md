# Endless Mode Implementation - Learnings Report

**Date:** October 10, 2025  
**Project:** BattleCruisers - Endless Mode Game Mode Development  
**Status:** Implementation saved to separate branch for reference  
**Outcome:** Functional but with visual glitches (buildings appeared in mid-air due to cruiser movement)

---

## Introduction to BattleCruisers

BattleCruisers is a real-time strategy (RTS) game developed in Unity where players construct and manage a battlecruiser, equipping it with various buildings and units (ships and aircraft) to engage in tactical combat against an enemy cruiser. The core gameplay revolves around resource management (drones for building), strategic placement of offensive and defensive structures, and deploying units to attack the opponent. Each cruiser has a set number of slots where buildings can be placed, and a "shipblocker" that defines the forward boundary for player units.

---

## Executive Summary

This report documents the complete process of implementing a new "Endless Mode" game mode for BattleCruisers, including all mistakes, misconceptions, and successful discoveries. While the final implementation had visual issues (buildings spawned at original cruiser position while cruiser moved), the learning process revealed critical insights into the game's architecture that will be invaluable for future game mode development.

**Key Achievement:** Successfully created a functional game mode where enemy units spawn continuously and an enemy cruiser slowly approaches while building defenses.

**Critical Issue:** Buildings are anchored to their spawn position, not the moving cruiser, causing them to appear in mid-air as the cruiser moved away.

---

## Table of Contents

1. [Initial Requirements & Vision](#initial-requirements--vision)
2. [Architecture Overview](#architecture-overview)
3. [Critical Discoveries](#critical-discoveries)
4. [Common Misconceptions & Mistakes](#common-misconceptions--mistakes)
5. [Technical Implementation Details](#technical-implementation-details)
6. [The Building Positioning Problem](#the-building-positioning-problem)
7. [Unit Spawning System](#unit-spawning-system)
8. [Successful Patterns](#successful-patterns)
9. [Failed Approaches](#failed-approaches)
10. [Recommendations for Future Agents](#recommendations-for-future-agents)

---

## Initial Requirements & Vision

### Original Concept
A "Plants vs. Zombies" style endless wave defense mode where:
- Random enemy units continuously spawn from the far right
- Enemy cruiser starts very far away (x=200) and slowly approaches home position (x=35)
- Enemy cruiser builds defenses while approaching
- Defensive buildings can fire immediately
- Offensive/Ultra buildings can be built but don't fire until cruiser reaches home
- Enemy cruiser cannot build units (ships/aircraft) until reaching home
- Shipblocker stays forward to prevent player units from advancing too far
- Player wins by destroying the enemy cruiser
- Player loses if their cruiser is destroyed

### Evolution of Design
The design went through several iterations:
1. **Initial:** Multiple cruisers spawning in sequence (like waves)
2. **Intermediate:** Pre-built structures on cruisers
3. **Final:** Single enemy cruiser moving while building, with separate unit spawning system

---

## Architecture Overview

### Core Components Created

#### 1. **EndlessModeManager.cs**
- Main coordinator for Endless mode
- Manages both the cruiser controller and unit spawner
- Initializes enemy cruiser at far position (x=200)
- Creates AI for enemy cruiser
- Handles mode lifecycle

#### 2. **EndlessUnitSpawner.cs**
- Spawns random enemy units from off-screen (x=100)
- Configurable spawn interval (5 seconds default)
- Handles unit initialization sequence
- Manages unit lifecycle

#### 3. **EndlessCruiserController.cs**
- Controls enemy cruiser movement from x=200 to x=35
- Movement duration: 5 minutes (300 seconds)
- Tracks offensive/ultra buildings
- Disables their firing until cruiser reaches home

#### 4. **EndlessHelper.cs**
- Implements `IBattleSceneHelper` interface
- Provides mode-specific configurations
- Handles AI difficulty and strategy

#### 5. **EndlessGameEndMonitor.cs**
- Custom game end detection
- Only ends game when player cruiser is destroyed
- Prevents game end when AI cruiser is destroyed

### Integration Points

```
BattleSceneGod
    ├── EndlessModeManager (when GameMode.Endless)
    │   ├── EndlessUnitSpawner
    │   └── EndlessCruiserController
    ├── EndlessGameEndMonitor (replaces GameEndMonitor)
    └── EndlessHelper (replaces standard helper)
```

---

## Critical Discoveries

### 1. **Cruiser Positioning & Building Anchors**

**Discovery:** Buildings are NOT children of the cruiser GameObject hierarchy. They are anchored to the cruiser's **spawn position**, not its current position.

**Evidence:**
- When the cruiser moved from x=200 to x=35, buildings stayed at x=200
- Buildings appeared to float in mid-air as cruiser moved away
- Building positions are set relative to cruiser position **at creation time**

**Implication:** To have buildings move with a cruiser, you need:
- Either: Keep cruiser stationary
- Or: Implement a system to update building positions continuously
- Or: Pre-build everything before cruiser reaches battle position

### 2. **Unit Initialization Sequence**

**Critical Discovery:** Units must be initialized in a specific order:

```csharp
// CORRECT SEQUENCE:
1. Instantiate(prefab)
2. SetActive(false)                    // MUST be inactive!
3. StaticInitialise()                  // Unity components
4. Initialise(UIManager)               // Game logic (OFTEN FORGOTTEN!)
5. Set Position/Rotation               // Before activation
6. Activate(activationArgs)            // Sets active internally
7. StartConstruction()                 // Begin building
```

**Common Mistake:** Calling `SetActive(true)` before `Activate()` causes assertion failure:
```csharp
Assert.IsFalse(_parent.activeSelf);  // Line 307 in Buildable.cs
```

**Why It Matters:** `Activate()` expects the GameObject to be inactive so it can control the activation process.

### 3. **Building Event System**

**Discovery:** Building construction events are named differently than expected.

```csharp
// WRONG (doesn't exist):
_cruiser.BuildingMonitor.BuildingAddedToMonitor

// CORRECT:
_cruiser.BuildingMonitor.BuildingStarted  // EventHandler<BuildingStartedEventArgs>
_cruiser.BuildingMonitor.BuildingCompleted
_cruiser.BuildingMonitor.BuildingDestroyed
```

**Event Args Structure:**
```csharp
BuildingStartedEventArgs.StartedBuilding   // NOT .Building
BuildingCompletedEventArgs.CompletedBuilding
BuildingDestroyedEventArgs.DestroyedBuilding
```

### 4. **Protected vs Public Methods**

**Discovery:** Many completion methods are `protected` and cannot be called externally.

```csharp
// PROTECTED (cannot call):
Unit.OnBuildableCompleted()
Buildable.BuildableState setter

// PUBLIC (can call):
unit.StartConstruction()
unit.Activate(args)
unit.CompletedBuildable event (can subscribe, not invoke)
```

**Lesson:** Don't try to force-complete units externally. Let the normal construction flow handle it, or use reflection sparingly.

### 5. **Cruiser AI and Enemy References**

**Critical Discovery:** When spawning new cruisers or units, you must update ALL references:

```csharp
// Required updates when spawning new enemy cruiser:
1. Dispose old AI
2. Create new AI for new cruiser
3. Update BattleSceneGod.enemyCruiserSprite
4. Update BattleSceneGod.enemyCruiserName
5. **CRITICAL:** _playerCruiser.UpdateEnemyCruiserReference(newCruiser)
```

**The MissingReferenceException Problem:**
- Player's units and buildings hold references to the enemy cruiser
- When enemy is destroyed and new one spawns, old references remain
- Player units try to target destroyed cruiser → crash
- **Solution:** Added `UpdateEnemyCruiserReference()` method to `Cruiser.cs`

### 6. **StaticPrefabKeys - What Actually Exists**

**Common Mistake:** Assuming unit names from other games or imagination.

**Reality Check - Actual Units from StaticData.cs:**
```csharp
// AIRCRAFT (all exist):
Bomber, Gunship, Fighter, SteamCopter, Broadsword, StratBomber, 
MissileFighter, SpyPlane

// SHIPS (all exist):
AttackBoat, Frigate, Destroyer, SiegeDestroyer, GunBoat, 
ArchonBattleship, AttackRIB, GlassCannoneer, RocketTurtle, FlakTurtle

// DOES NOT EXIST:
Havok, Hammer, Fury, Reaper, Firestarter, Warthog, Avalanche, 
Battleship, AegisShip, WarBarge
```

**Lesson:** Always check `StaticData.cs` for actual available prefabs before using them.

### 7. **Namespace Ambiguities**

**Discovery:** Some common names have conflicts:

```csharp
// AMBIGUOUS (need full qualification):
Random          // UnityEngine.Random vs System.Random
Object          // UnityEngine.Object vs System.Object

// SOLUTION:
UnityEngine.Random.Range(0, count)
UnityEngine.Object.Instantiate(prefab)
```

### 8. **BuildableActivationArgs Location**

**Discovery:** This type is NOT where you'd expect.

```csharp
// WRONG:
using BattleCruisers.Buildables;

// CORRECT:
using BattleCruisers.Buildables.Pools;  // BuildableActivationArgs lives here
```

---

## Common Misconceptions & Mistakes

### Mistake #1: Imagining Non-Existent API Methods

**What Happened:**
```csharp
// I assumed this existed:
unit.BuildProgress.Set(1.0f);

// Reality:
BuildProgress is a float property (getter only)
No public setter or Set() method exists
```

**Lesson:** Don't assume APIs. Always grep or search for actual method signatures.

### Mistake #2: Assuming Cruisers Spawn Multiple Times

**Initial Approach:** Designed system to spawn new cruisers each wave.

**Reality:** 
- Cruisers are complex objects with many dependencies
- Each cruiser needs: AI, UI references, drone managers, fog systems, etc.
- Spawning new cruisers repeatedly is fragile
- Better: One cruiser that moves, or reset positions

**Final Design:** One enemy cruiser that moves slowly toward player.

### Mistake #3: Setting GameObject Active Too Early

**The Bug:**
```csharp
unitWrapper.gameObject.SetActive(true);    // ❌ Too early!
unitWrapper.StaticInitialise();
unit.Activate(activationArgs);             // ❌ Fails assertion
```

**Why It Fails:**
```csharp
// From Buildable.cs line 307:
Assert.IsFalse(_parent.activeSelf);  // Expects inactive!
```

**Correct Pattern:**
```csharp
unitWrapper.gameObject.SetActive(false);   // ✅ Keep inactive
unitWrapper.StaticInitialise();
unit.Initialise(_uiManager);               // ✅ Initialize first
unit.Activate(activationArgs);             // ✅ Now it activates
```

### Mistake #4: Forgetting Initialise(UIManager)

**The Bug:**
```csharp
unit.StaticInitialise();
unit.Activate(activationArgs);   // ❌ NullReferenceException later
```

**Why It Fails:**
- `Initialise(UIManager)` sets up critical internal state
- Without it, many properties are null
- Causes crashes when unit tries to use those properties

**Correct Pattern:**
```csharp
unit.StaticInitialise();          // Unity setup
unit.Initialise(uiManager);       // ✅ Game logic setup
unit.Activate(activationArgs);    // Activation
```

### Mistake #5: Trying to Access Protected Members

**The Bug:**
```csharp
concreteUnit.BuildableState = BuildableState.Completed;  // ❌ Protected setter
concreteUnit.OnBuildableCompleted();                     // ❌ Protected method
```

**Lesson:** Work with public interfaces. Don't fight the access modifiers.

**Better Approach:**
```csharp
unit.StartConstruction();  // Let normal flow handle completion
// Or accept that units need build time in your game mode
```

### Mistake #6: Not Cleaning Up Death Explosions

**The Bug:**
- Cruiser death spawns `CruiserDeathExplosion` prefab
- Prefab plays particle effects
- Tried to `Destroy()` it → Unity warning about destroying assets

**Solution:**
```csharp
// Don't destroy, just deactivate:
deathExplosion.gameObject.SetActive(false);
```

**Lesson:** Unity distinguishes between:
- Scene instances (can be destroyed)
- Prefab assets (cannot be destroyed at runtime)
- Use `SetActive(false)` when unsure

### Mistake #7: Wrong Event Property Names

**The Bug:**
```csharp
IBuilding building = e.Building;  // ❌ Property doesn't exist
```

**Reality:**
```csharp
IBuilding building = e.StartedBuilding;   // ✅ Actual property name
```

**Pattern:**
```csharp
BuildingStartedEventArgs    → StartedBuilding
BuildingCompletedEventArgs  → CompletedBuilding
BuildingDestroyedEventArgs  → DestroyedBuilding
```

### Mistake #8: Assuming Buildings Are Children of Cruiser

**Critical Misconception:**
- Buildings are NOT in cruiser's GameObject hierarchy
- Buildings have their own independent GameObjects
- Buildings track cruiser reference but don't move with it
- Building positions are set once at creation time

**Why This Matters:**
- When cruiser moves, buildings stay put
- Buildings appeared in mid-air as cruiser moved away
- To have buildings move with cruiser, you'd need continuous position updates

---

## Technical Implementation Details

### GameMode Enum Addition

**File:** `Assets/Scripts/Data/ApplicationModel.cs`

```csharp
public enum GameMode
{
    Campaign = 1,
    Tutorial = 2,
    Skirmish = 3,
    PvP_1VS1 = 4,
    CoinBattle = 5,
    SideQuest = 6,
    Endless = 7      // ✅ Added
}
```

### BattleSceneGod Integration

**File:** `Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`

```csharp
// Helper creation:
protected virtual IBattleSceneHelper CreateHelper(IDeferrer deferrer)
{
    switch (ApplicationModel.Mode)
    {
        case GameMode.Endless:
            return new EndlessHelper(deferrer);  // ✅ Custom helper
        // ... other cases
    }
}

// Game end monitor creation:
if (ApplicationModel.Mode == GameMode.Endless)
{
    _endlessGameEndMonitor = new EndlessGameEndMonitor(
        playerCruiser, battleCompletionHandler, gameEndHandler);
}
else
{
    _gameEndMonitor = new GameEndMonitor(/* ... */);
}

// Mode manager initialization:
if (ApplicationModel.Mode == GameMode.Endless)
{
    _endlessModeManager = gameObject.AddComponent<EndlessModeManager>();
    _endlessModeManager.Initialise(
        playerCruiser, aiCruiser, cruiserFactory, uiManager,
        cameraFocuser, userChosenTargetHelper, helper);
}
```

### Unit Spawning - Complete Flow

**File:** `Assets/Scripts/Cruisers/EndlessUnitSpawner.cs`

```csharp
private void SpawnRandomUnit()
{
    // 1. Select random unit
    UnitKey randomUnit = _availableUnits[UnityEngine.Random.Range(0, _availableUnits.Count)];

    // 2. Get prefab and instantiate
    IBuildableWrapper<IUnit> unitWrapperPrefab = PrefabFactory.GetUnitWrapperPrefab(randomUnit);
    BuildableWrapper<IUnit> unitWrapper = UnityEngine.Object.Instantiate(unitWrapperPrefab.UnityObject);
    
    // 3. Keep inactive (CRITICAL!)
    unitWrapper.gameObject.SetActive(false);
    
    // 4. Static initialization (Unity components)
    unitWrapper.StaticInitialise();
    IUnit unit = unitWrapper.Buildable;

    // 5. Game logic initialization (OFTEN FORGOTTEN!)
    unit.Initialise(_uiManager);

    // 6. Set position and rotation BEFORE activation
    float spawnY = UnityEngine.Random.Range(-SPAWN_Y_VARIATION, SPAWN_Y_VARIATION);
    unit.Position = new Vector3(SPAWN_X_POSITION, spawnY, 0);
    
    Quaternion rotation = unit.Rotation;
    rotation.eulerAngles = new Vector3(0, 180, 0);  // Face left
    unit.Rotation = rotation;

    // 7. Activate (sets GameObject active internally)
    BuildableActivationArgs activationArgs = new BuildableActivationArgs(
        _enemyCruiser, _playerCruiser, _factories);
    unit.Activate(activationArgs);

    // 8. Start construction
    unit.StartConstruction();
    
    // Note: Units will complete construction over time
    // We did NOT force immediate completion in final version
}
```

### Cruiser Movement System

**File:** `Assets/Scripts/Cruisers/EndlessCruiserController.cs`

```csharp
private IEnumerator MoveCruiserCoroutine()
{
    float startTime = Time.time;
    float endTime = startTime + _moveDuration;

    while (Time.time < endTime)
    {
        if (_cruiser == null || _cruiser.IsDestroyed)
        {
            _isMoving = false;
            yield break;
        }

        // Smooth lerp from start to home position
        float t = (Time.time - startTime) / _moveDuration;
        _cruiser.Position = Vector3.Lerp(_startPosition, _homePosition, t);
        
        yield return null;  // Wait one frame
    }

    // Ensure exact final position
    _cruiser.Position = _homePosition;
    _isAtHome = true;
    
    // Enable restricted buildings
    EnableRestrictedBuildings();
    
    ReachedHome?.Invoke(this, EventArgs.Empty);
}
```

### Building Restriction System

**How It Works:**

```csharp
// Subscribe to building starts
_cruiser.BuildingMonitor.BuildingStarted += OnBuildingStarted;

private void OnBuildingStarted(object sender, BuildingStartedEventArgs e)
{
    IBuilding building = e.StartedBuilding;
    
    if (building.Category == BuildingCategory.Offence)
    {
        _offensiveBuildings.Add(building);
        if (!_isAtHome)
            DisableBuilding(building);  // Disable firing
    }
    else if (building.Category == BuildingCategory.Ultra)
    {
        _ultraBuildings.Add(building);
        if (!_isAtHome)
            DisableBuilding(building);  // Disable firing
    }
    // Defense buildings work immediately (no restriction)
}

private void DisableBuilding(IBuilding building)
{
    // Disable barrel components (turrets)
    var barrelWrappers = building.GameObject
        .GetComponentsInChildren<MonoBehaviour>()
        .Where(mb => mb.GetType().Name.Contains("Barrel"));
    
    foreach (var barrel in barrelWrappers)
        barrel.enabled = false;
}
```

---

## The Building Positioning Problem

### The Issue

**Symptom:** Buildings appeared to float in mid-air as enemy cruiser moved.

**Root Cause Analysis:**

1. **Building Creation:**
   ```csharp
   // When AI builds a building:
   building.Position = cruiser.Position + slotOffset;  // Set once at creation
   ```

2. **Cruiser Movement:**
   ```csharp
   // Cruiser moves over time:
   cruiser.Position = Vector3.Lerp(startPos, homePos, t);  // Updates every frame
   ```

3. **Building Position:**
   ```csharp
   // Building position never updates!
   // It stays at the original cruiser.Position + slotOffset
   // Buildings do NOT follow cruiser
   ```

### Why Buildings Don't Follow Cruiser

**Investigation Results:**

1. Buildings are not children of cruiser in Unity hierarchy
2. Buildings have independent GameObject.transform
3. Building position is set once at spawn time
4. No system exists to continuously update building positions

**Evidence from Code:**
```csharp
// Buildings are instantiated independently:
Building building = Instantiate(buildingPrefab);
building.transform.position = worldPosition;  // Set once
// NOT: building.transform.parent = cruiser.transform;
```

### Potential Solutions (Not Implemented)

**Option 1: Make Buildings Children**
```csharp
building.transform.SetParent(cruiser.transform, worldPositionStays: true);
```
**Risk:** May break other systems that expect buildings to be independent.

**Option 2: Continuous Position Updates**
```csharp
void Update()
{
    foreach (var building in _buildings)
    {
        building.Position = _cruiser.Position + building.InitialOffset;
    }
}
```
**Risk:** Performance impact, may conflict with building systems.

**Option 3: Pre-Build Before Movement**
```csharp
// Give AI time to build everything BEFORE moving cruiser
_aiBuildsForDuration(60f);  // 1 minute
await Task.Delay(60000);
_cruiserController.StartMoving();  // Now move with completed buildings
```
**Risk:** Reduces gameplay tension, AI might not build optimally.

**Option 4: Don't Move Cruiser**
```csharp
// Spawn cruiser at home position immediately
// Only spawn units from far away
// Buildings stay properly positioned
```
**Impact:** Changes gameplay feel significantly.

---

## Unit Spawning System

### Complete Unit Initialization Checklist

✅ **Required Steps (in order):**

1. Get unit prefab from `PrefabFactory`
2. Instantiate wrapper
3. **SetActive(false)** ← CRITICAL
4. Call `StaticInitialise()`
5. Call `Initialise(UIManager)` ← OFTEN FORGOTTEN
6. Set position and rotation
7. Call `Activate(activationArgs)` ← Sets active internally
8. Call `StartConstruction()`

### Common Errors and Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| `AssertionException: Value was True` | GameObject active before Activate() | Keep inactive until Activate() |
| `NullReferenceException` in OnDestroyed | Missing Initialise(UIManager) | Always call before Activate() |
| Unit doesn't move | Didn't call StartConstruction() | Call after Activate() |
| Unit faces wrong way | Rotation set after Activate() | Set rotation before Activate() |

### Unit Categories

**All Available Units (from StaticData.cs):**

```csharp
// AIRCRAFT (8 types):
StaticPrefabKeys.Units.Bomber
StaticPrefabKeys.Units.Gunship
StaticPrefabKeys.Units.Fighter
StaticPrefabKeys.Units.SteamCopter
StaticPrefabKeys.Units.Broadsword
StaticPrefabKeys.Units.StratBomber
StaticPrefabKeys.Units.SpyPlane
StaticPrefabKeys.Units.MissileFighter

// SHIPS (10 types):
StaticPrefabKeys.Units.AttackBoat
StaticPrefabKeys.Units.Frigate
StaticPrefabKeys.Units.Destroyer
StaticPrefabKeys.Units.SiegeDestroyer
StaticPrefabKeys.Units.GunBoat
StaticPrefabKeys.Units.ArchonBattleship
StaticPrefabKeys.Units.AttackRIB
StaticPrefabKeys.Units.GlassCannoneer
StaticPrefabKeys.Units.RocketTurtle
StaticPrefabKeys.Units.FlakTurtle
```

---

## Successful Patterns

### Pattern 1: Mode-Specific Helper

```csharp
// Create custom IBattleSceneHelper for game mode
public class EndlessHelper : BattleSceneHelper
{
    public override IManagedDisposable CreateAI(Cruiser aiCruiser, Cruiser playerCruiser, int level)
    {
        // Custom AI creation logic
    }
    
    public override bool ShowInGameHints => false;  // Disable tutorials
    
    public override Loadout GetPlayerLoadout()
    {
        return DataProvider.GameModel.PlayerLoadout;
    }
}
```

**Why It Works:** Leverages existing infrastructure while customizing behavior.

### Pattern 2: Component-Based Mode Manager

```csharp
public class EndlessModeManager : MonoBehaviour
{
    private EndlessUnitSpawner _unitSpawner;
    private EndlessCruiserController _cruiserController;
    
    public void Initialise(/* parameters */)
    {
        // Create sub-components
        _unitSpawner = gameObject.AddComponent<EndlessUnitSpawner>();
        _cruiserController = gameObject.AddComponent<EndlessCruiserController>();
        
        // Initialize them
        _unitSpawner.Initialise(/* ... */);
        _cruiserController.Initialise(/* ... */);
        
        // Start the mode
        _unitSpawner.StartSpawning();
        _cruiserController.StartMoving();
    }
}
```

**Why It Works:** Separation of concerns, each component has one job.

### Pattern 3: Custom Game End Monitor

```csharp
public class EndlessGameEndMonitor
{
    public EndlessGameEndMonitor(Cruiser playerCruiser, /* ... */)
    {
        // Only watch player cruiser
        _playerCruiser.Destroyed += OnPlayerCruiserDestroyed;
        // Don't watch AI cruiser - that's the goal!
    }
    
    private void OnPlayerCruiserDestroyed(object sender, DestroyedEventArgs e)
    {
        _gameEndHandler.HandleCruiserDestroyed(playerWon: false, score);
    }
}
```

**Why It Works:** Gives full control over win/loss conditions.

### Pattern 4: Event-Driven Building Tracking

```csharp
// Subscribe to events instead of polling
_cruiser.BuildingMonitor.BuildingStarted += OnBuildingStarted;

private void OnBuildingStarted(object sender, BuildingStartedEventArgs e)
{
    // React to building creation immediately
    if (ShouldRestrict(e.StartedBuilding))
        RestrictBuilding(e.StartedBuilding);
}
```

**Why It Works:** Event-driven is more efficient than Update() polling.

---

## Failed Approaches

### Failed Approach #1: Force-Completing Units

**What We Tried:**
```csharp
unit.BuildProgress.Set(1.0f);  // Doesn't exist
concreteUnit.BuildableState = BuildableState.Completed;  // Protected
concreteUnit.OnBuildableCompleted();  // Protected
```

**Why It Failed:**
- These members are protected/non-existent for good reason
- Normal construction flow handles important setup
- Forcing completion skips critical initialization

**Lesson:** Don't fight the architecture. Let units build normally.

### Failed Approach #2: Multiple Cruiser Spawning

**What We Tried:**
- Destroy enemy cruiser when it dies
- Spawn new random cruiser
- Initialize new cruiser with AI
- Repeat for endless waves

**Why It Failed:**
- Player cruiser still referenced old destroyed cruiser
- Buildings/units tried to target destroyed cruiser → crashes
- New cruiser spawn was complex and fragile
- Death explosions caused cleanup issues

**What Worked Instead:**
- Single enemy cruiser that moves in slowly
- Separate unit spawning system
- Much simpler and more stable

### Failed Approach #3: Destroying Death Explosions

**What We Tried:**
```csharp
Destroy(deathExplosion.gameObject);
// Warning: "Destroying assets is not permitted"
```

**Why It Failed:**
- Unity distinguishes between scene instances and prefab assets
- The death explosion reference was sometimes to the asset
- `Destroy()` only works on scene instances

**What Worked:**
```csharp
deathExplosion.gameObject.SetActive(false);  // Just hide it
```

### Failed Approach #4: Reflection for Protected Methods

**What We Tried:**
```csharp
typeof(Unit).GetMethod("OnBuildableCompleted",
    BindingFlags.NonPublic | BindingFlags.Instance)
    ?.Invoke(concreteUnit, null);
```

**Why It Failed:**
- Brittle and breaks encapsulation
- Method signature could change
- Doesn't handle all the internal state properly

**Lesson:** If you need reflection, your approach is probably wrong.

---

## Recommendations for Future Agents

### Starting a New Game Mode

**Step 1: Create the Enum Value**
```csharp
// ApplicationModel.cs
public enum GameMode
{
    // ... existing modes
    YourNewMode = 8  // Pick next available number
}
```

**Step 2: Create Helper Implementation**
```csharp
// YourModeHelper.cs : BattleSceneHelper
public class YourModeHelper : BattleSceneHelper
{
    // Override virtual methods as needed
    public override IManagedDisposable CreateAI(/* ... */) { }
    public override Loadout GetPlayerLoadout() { }
    // etc.
}
```

**Step 3: Integrate in BattleSceneGod**
```csharp
// BattleSceneGod.cs CreateHelper()
case GameMode.YourNewMode:
    return new YourModeHelper(deferrer);
```

**Step 4: Create Mode Manager**
```csharp
// YourModeManager.cs : MonoBehaviour
public class YourModeManager : MonoBehaviour
{
    public void Initialise(/* dependencies */)
    {
        // Your mode logic here
    }
}
```

**Step 5: Wire Up in BattleSceneGod.Start()**
```csharp
if (ApplicationModel.Mode == GameMode.YourNewMode)
{
    _yourModeManager = gameObject.AddComponent<YourModeManager>();
    _yourModeManager.Initialise(/* ... */);
}
```

### Working with Units

**Always Follow This Sequence:**

```csharp
// 1. Instantiate inactive
var wrapper = UnityEngine.Object.Instantiate(prefab);
wrapper.gameObject.SetActive(false);  // ← CRITICAL!

// 2. Static setup
wrapper.StaticInitialise();
IUnit unit = wrapper.Buildable;

// 3. Game logic setup
unit.Initialise(uiManager);  // ← DON'T FORGET!

// 4. Position/rotation before activation
unit.Position = spawnPos;
unit.Rotation = spawnRot;

// 5. Activate (makes it active)
unit.Activate(new BuildableActivationArgs(parent, enemy, factories));

// 6. Start construction
unit.StartConstruction();
```

### Working with Cruisers

**If You Need to Move a Cruiser:**

⚠️ **WARNING:** Buildings won't move with the cruiser!

**Options:**
1. **Don't move cruiser** - Keep it stationary
2. **Pre-build everything** - Build first, then move
3. **Implement position updates** - Update building positions every frame (performance cost)
4. **Make buildings children** - Parent them to cruiser transform (may break systems)

**Recommended:** Keep cruisers stationary in battle positions.

### Working with Buildings

**Building Categories:**
```csharp
BuildingCategory.Factory    // Factories (Air/Naval/Drone)
BuildingCategory.Defence    // Defensive turrets
BuildingCategory.Offence    // Offensive turrets
BuildingCategory.Tactical   // Shields, boosters, stealth
BuildingCategory.Ultra      // Nukes, kamikaze, broadsides
```

**Event Monitoring:**
```csharp
cruiser.BuildingMonitor.BuildingStarted    // When construction starts
cruiser.BuildingMonitor.BuildingCompleted  // When construction finishes
cruiser.BuildingMonitor.BuildingDestroyed  // When destroyed

// Event args:
BuildingStartedEventArgs.StartedBuilding
BuildingCompletedEventArgs.CompletedBuilding
BuildingDestroyedEventArgs.DestroyedBuilding
```

### Common Pitfalls to Avoid

❌ **DON'T:**
- Set GameObject active before calling Activate()
- Forget to call Initialise(UIManager)
- Try to access protected members
- Assume API methods exist without checking
- Use System.Random or System.Object without qualification
- Destroy Unity prefab assets at runtime
- Move cruisers and expect buildings to follow

✅ **DO:**
- Follow initialization sequences exactly
- Check StaticData.cs for available prefabs
- Use events instead of Update() polling
- Dispose of AI instances properly
- Update all enemy references when changing enemies
- Test with actual game to catch visual issues

### Debug Checklist

When something isn't working:

1. **Check initialization order**
   - StaticInitialise called?
   - Initialise(UIManager) called?
   - Activate called last?

2. **Check GameObject active state**
   - Should be inactive before Activate()?
   - Gets activated by Activate()?

3. **Check references**
   - All parameters non-null?
   - Enemy cruiser reference updated?
   - UI Manager passed through?

4. **Check event subscriptions**
   - Using correct event name?
   - Using correct property on EventArgs?
   - Unsubscribing in OnDestroy()?

5. **Check namespace ambiguities**
   - Qualified UnityEngine.Random?
   - Qualified UnityEngine.Object?

### Performance Considerations

**Good:**
- Event-driven systems (subscribe to BuildingStarted)
- Coroutines for timed actions
- Component-based architecture

**Bad:**
- Update() loops checking every frame
- Frequent Instantiate/Destroy cycles
- Large numbers of active GameObjects

**Optimization Tips:**
- Object pool units if spawning many
- Use coroutines with delays instead of Update()
- Disable distant/off-screen objects
- Limit particle effects

---

## Conclusion

### What We Learned

1. **Unit initialization requires precise ordering** - StaticInitialise → Initialise(UIManager) → Activate
2. **Buildings don't follow moving cruisers** - They're anchored to spawn position
3. **Event names matter** - BuildingStarted, not BuildingAdded
4. **Protected means protected** - Don't fight access modifiers
5. **Check actual prefab names** - StaticData.cs is the source of truth
6. **Namespace qualifications required** - UnityEngine.Random vs System.Random

### What Worked

- ✅ Component-based mode manager architecture
- ✅ Event-driven building tracking
- ✅ Custom game end monitor
- ✅ Mode-specific helper implementation
- ✅ Random unit spawning system
- ✅ Cruiser movement with Vector3.Lerp
- ✅ Building category filtering

### What Didn't Work

- ❌ Moving cruiser while buildings build (visual glitch)
- ❌ Force-completing units externally
- ❌ Multiple cruiser spawning approach
- ❌ Destroying death explosion GameObjects
- ❌ Reflection for protected methods

### Best Path Forward

For a similar game mode in the future:

1. **Keep cruisers stationary** or pre-build everything before moving
2. **Use separate spawning systems** for units vs buildings
3. **Leverage existing infrastructure** (helpers, monitors, factories)
4. **Follow strict initialization sequences** for units
5. **Test early and often** to catch visual issues

### Final Thoughts

While the implementation had visual issues, the exploration revealed deep insights into BattleCruisers' architecture. The building positioning problem is fundamental to how the game works - buildings are independent entities, not cruiser children. Any future mode involving moving cruisers must account for this from the start.

The successful patterns (component architecture, event systems, proper initialization) provide a strong foundation for future game mode development. The mistakes documented here will prevent future agents from repeating them.

---

## Appendix: File Structure

### Files Created (All Deleted - Saved to Branch)

```
Assets/Scripts/
├── Data/
│   └── ApplicationModel.cs (modified)
├── Cruisers/
│   ├── Cruiser.cs (modified - added UpdateEnemyCruiserReference)
│   ├── EndlessModeManager.cs (created)
│   ├── EndlessUnitSpawner.cs (created)
│   └── EndlessCruiserController.cs (created)
├── Scenes/BattleScene/
│   ├── BattleSceneGod.cs (modified)
│   └── EndlessHelper.cs (created)
├── Utils/BattleScene/
│   └── EndlessGameEndMonitor.cs (created)
├── UI/ScreensScene/BattleHubScreen/
│   └── BattleHubScreensController.cs (modified)
└── Scenes/
    └── DestructionSceneGod.cs (modified - reward handling)
```

### Integration Points

1. **ApplicationModel.cs** - GameMode enum
2. **BattleSceneGod.cs** - Mode initialization
3. **DestructionSceneGod.cs** - Reward calculation
4. **BattleHubScreensController.cs** - UI button
5. **Cruiser.cs** - Enemy reference update method

---

## Quick Reference Card

### Unit Spawning Checklist
- [ ] Get prefab from PrefabFactory
- [ ] Instantiate with SetActive(false)
- [ ] StaticInitialise()
- [ ] Initialise(UIManager)
- [ ] Set Position/Rotation
- [ ] Activate(args)
- [ ] StartConstruction()

### Common Error Solutions
| Error Message | Fix |
|--------------|-----|
| "Assertion failure. Value was True" | Keep GameObject inactive before Activate() |
| "Object reference not set" in OnDestroyed | Call Initialise(UIManager) before Activate() |
| "BuildingAddedToMonitor not found" | Use BuildingStarted instead |
| "Building property not found" | Use StartedBuilding property |
| "Destroying assets not permitted" | Use SetActive(false) instead |
| "Random is ambiguous" | Use UnityEngine.Random |
| "Object is ambiguous" | Use UnityEngine.Object |

### Key Contacts/References
- **StaticData.cs** - All available units/buildings
- **BattleSceneGod.cs** - Mode integration point
- **IBattleSceneHelper.cs** - Helper interface to implement
- **ICruiserBuildingMonitor.cs** - Building event definitions

---

**Report End**

Generated: October 10, 2025  
Author: AI Assistant  
Status: Comprehensive learnings documented for future development

