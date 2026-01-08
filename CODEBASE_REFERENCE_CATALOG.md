# BATTLECRUISERS - COMPREHENSIVE CODEBASE REFERENCE CATALOG
**Generated**: 2026-01-08
**Purpose**: AI Agent Quick Reference Guide
**Total Scripts Audited**: 2,506 C# files
**Project Type**: Unity Game (PvE Campaign + PvP Multiplayer)

---

## TABLE OF CONTENTS

1. [Project Architecture Overview](#project-architecture-overview)
2. [Quick Reference: Find Code By Feature](#quick-reference-find-code-by-feature)
3. [System Architecture Maps](#system-architecture-maps)
4. [Critical Files Index](#critical-files-index)
5. [Cross-Reference Guide](#cross-reference-guide)
6. [Common Patterns & Conventions](#common-patterns--conventions)
7. [Debugging Entry Points](#debugging-entry-points)

---

## PROJECT ARCHITECTURE OVERVIEW

### Top-Level Structure
```
Assets/Scripts/
├── Buildables/          # Units, buildings, turrets (182 files)
├── Targets/             # Targeting, detection, tracking (70 files)
├── Projectiles/         # Missiles, shells, beams (44 files)
├── Movement/            # Unit movement, rotation (32 files)
├── Cruisers/            # Player/enemy capital ships (75 files)
├── AI/                  # Enemy AI behavior (48 files)
├── UI/                  # User interface (426 files)
├── Scenes/              # Scene managers (221 files)
├── Data/                # Save/load, models (varied)
├── Tutorial/            # Tutorial system (90 files)
├── PvP/                 # Multiplayer (461 files)
├── Utils/               # Helper systems (127 files)
└── Analytics/           # Firebase analytics
```

### Technology Stack
- **Unity Version**: Not specified in code
- **Networking**: Unity Netcode for GameObjects
- **Backend**: Unity Gaming Services (Lobbies, Relay, Matchmaker)
- **Serialization**: BinaryFormatter (deprecated - needs migration)
- **Testing**: NUnit + NSubstitute mocking
- **Localization**: Custom LocTable system (8 translation tables)

---

## QUICK REFERENCE: FIND CODE BY FEATURE

### "I need to fix weapons/turrets..."
**Start Here**: `/Assets/Scripts/Buildables/Buildings/Turrets/`
- **Main Controller**: `TurretController.cs` - Base turret logic
- **Barrel Controllers**: `BarrelControllers/` folder (10 types)
  - `LightningBarrelController.cs` → **ION CANNON** (mentioned in bug report)
  - `LaserBarrelController.cs` → Laser turrets
  - `MissileBarrelController.cs` → Missile launchers
  - `RocketBarrelController.cs` → Rocket artillery
- **Stats**: `Stats/` folder - Damage, fire rate, range
- **Angle Calculation**: `AngleCalculators/` folder
- **Key Bug**: ION CANNON not firing - see `BeamEmitter.cs:58-62` (silent failure on collision detection)

### "I need to fix PvP connection issues..."
**Start Here**: `/Assets/Scripts/PvP/`
- **Connection Manager**: `ConnectionManagement/ConnectionManager.cs`
- **Network States**: `ConnectionManagement/ConnectionState/` (7 state classes)
- **Relay Setup**: `ConnectionManagement/ConnectionMethod.cs`
- **Auth**: `UnityServices/Auth/AuthenticationServiceFacade.cs`
- **Lobbies**: `UnityServices/Lobbies/LobbyServiceFacade.cs`
- **Key Bug**: Android/Editor mismatch - see `ConnectionManager.cs:129-139` (latency limit asymmetry)

### "I need to fix missing translation strings..."
**Start Here**: `/Assets/Scripts/Utils/Localisation/`
- **Main Table**: `LocTable.cs` - String lookup system
- **Cache**: `LocTableCache.cs` - 8 translation tables (COMMON, BATTLE_SCENE, SCREENS_SCENE, etc.)
- **Dynamic Keys**: Check `ItemDetailsManager.cs:60,83,126,149` - Buildable names
- **Missing Keys Pattern**: Keys constructed as `"Buildables/Buildings/" + building.keyName + "Name"`
- **Error Detection**: `LocalisationFontChanges.cs:206-222` - Returns "Not localised" on missing keys

### "I need to fix pooling/spawning..."
**Start Here**: `/Assets/Scripts/Buildables/Pools/` and `/Assets/Scripts/Utils/BattleScene/Pools/`
- **Unit Pools**: `UnitPoolProvider.cs` - 20 unit pools
- **Core Pool**: `Utils/BattleScene/Pools/Pool.cs` - Generic pool implementation
- **Factory Spawning**: `Buildables/Buildings/Factories/Spawning/` folder
- **Key Bug**: Memory leak from BoostChanged event not unsubscribed - see `Buildable.cs:332`

### "I need to fix unit tests..."
**Start Here**: `/Assets/Editor/Tests/`
- **Critical Issue**: 78 test files have `SetuUp()` typo instead of `SetUp()`
- **Test Base Classes**: Look for `*TestsBase.cs` files (9 found)
- **Mocking**: Uses NSubstitute library
- **Coverage**: Only 12.5% of codebase tested; PvP has 0 tests

### "I need to fix AI behavior..."
**Start Here**: `/Assets/Scripts/AI/`
- **Task System**: `Tasks/` folder - State machine for AI actions
- **Threat Monitor**: `ThreatMonitor/` folder - Enemy detection
- **Drone Management**: `Drones/` folder - Resource allocation
- **Factory Managers**: `FactoryManagers/` folder - Unit production AI
- **Key Bug**: SlotNumCalculator reuse bug - see `AIManager.cs:85-96`

### "I need to fix projectile damage..."
**Start Here**: `/Assets/Scripts/Projectiles/`
- **Stats**: `Stats/ProjectileStats.cs` - Damage, speed, radius
- **Damage Appliers**: `DamageAppliers/` folder
  - `SingleDamageApplier.cs` - Direct hit
  - `AreaOfEffectDamageApplier.cs` - Splash damage
- **Spawners**: `Spawners/` folder (8 types)
- **Key Bug**: ShellSpawner uses MaxVelocity instead of InitialVelocity - see `ShellSpawner.cs:27`

### "I need to fix targeting/movement..."
**Start Here**: `/Assets/Scripts/Targets/` and `/Assets/Scripts/Movement/`
- **Target Detection**: `Targets/TargetDetectors/` folder
- **Target Filtering**: `Targets/TargetFinders/Filters/` folder
- **Target Ranking**: `Targets/TargetTrackers/Ranking/` folder
- **Movement**: `Movement/Velocity/` and `Movement/Rotation/` folders
- **Key Bug**: LinearTargetPositionPredictor missing discriminant check - see `LinearTargetPositionPredictor.cs:32-33`

### "I need to fix scene loading/transitions..."
**Start Here**: `/Assets/Scripts/Scenes/`
- **Battle Scene**: `BattleScene/BattleSceneGod.cs` (702 lines) - Main battle controller
- **UI Scenes**: `ScreensSceneGod.cs` (1005 lines) - Menu/hub controller
- **Scene Navigation**: `SceneNavigator.cs` - Async scene loading
- **Key Bug**: EventSystem duplication - see `SceneNavigator.cs:68-69`

### "I need to fix save/load data..."
**Start Here**: `/Assets/Scripts/Data/`
- **Serialization**: `Serializer.cs` (993 lines) - Save/load logic
- **Models**: `Models/GameModel.cs` (662 lines) - Player data
- **Data Provider**: `DataProvider.cs` (1137 lines) - Data access layer
- **Key Bug**: Missing null checks on Reflection - see `Serializer.cs:220-225`

### "I need to fix tutorial..."
**Start Here**: `/Assets/Scripts/Tutorial/`
- **Manager**: `TutorialManager.cs` - Main controller
- **Steps**: `Steps/` folder (30+ step types)
- **Factories**: `Steps/Factories/` folder (24 factories)
- **Key Bug**: CameraAdjustmentWaitStep can hang forever - see `CameraAdjustmentWaitStep.cs:21-31`

---

## SYSTEM ARCHITECTURE MAPS

### WEAPON FIRING SYSTEM FLOW
```
TurretController
  ↓ (has)
BarrelWrapper (determines firing pattern)
  ↓ (uses)
BarrelController.Fire()
  ↓ (checks)
BarrelFiringHelper.TryFire()
  ↓ (validates)
BarrelAdjustmentHelper.GetAdjustment()
  ↓ (if on target)
BarrelFirer.Fire() / DeferredBarrelFirer.Fire()
  ↓ (spawns)
ProjectileSpawner.Spawn()
  ↓ (creates)
ProjectileController
  ↓ (applies)
DamageApplier.ApplyDamage()
```

**Ion Cannon Specific Path**:
```
LightningBarrelController.Fire()
  ↓
BeamEmitter.FireBeam()
  ↓
BeamCollisionDetector.FindCollision() [CRITICAL: Returns null if no collision]
  ↓
[If null] → Return silently (BUG: No firing indication)
[If collision] → LightningEmitter.HandleCollision()
```

### NETWORKING SYSTEM FLOW (PvP)
```
User Clicks "Find Match"
  ↓
MatchmakingScreenController
  ↓
ConnectionManager.StartConnection()
  ↓
OfflineState → StartingHostState / ClientConnectingState
  ↓
ConnectionMethod.SetupHostConnectionAsync() / SetupClientConnectionAsync()
  ↓
[Host] RelayService.CreateAllocationAsync()
[Client] RelayService.JoinAllocationAsync()
  ↓
UnityTransport.SetConnectionData() [Sets relay server data]
  ↓
NetworkManager.StartHost() / StartClient()
  ↓
HostingState / ClientConnectedState
  ↓
SceneManager.LoadScene("PvPBattleScene")
  ↓
PvPBattleSceneGod initialization
  ↓
Match begins
```

**Critical Points**:
- Line 126 of ConnectionMethod.cs: `await Task.Delay(1000)` - hardcoded relay propagation wait
- ConnectionManager.cs:129-139: Different latency limits (Editor: 2000ms, Mobile: 300ms)

### TARGET ACQUISITION FLOW
```
TargetDetectorController (2D trigger detection)
  ↓
TargetColliderHandler.OnTriggerEnter2D()
  ↓
ManualProximityTargetDetector.AddTarget()
  ↓
TargetFilter.IsMatch() [Faction + Type filtering]
  ↓
RangedTargetFinder / GlobalTargetFinder
  ↓
RankedTargetTracker.AddTarget() [Ranks by threat]
  ↓
CompositeTargetTracker.FindBestTarget()
  ↓
TurretController.Target [Property set]
  ↓
Fires when in range
```

### UNIT CONSTRUCTION FLOW
```
User clicks Building Button
  ↓
BuildingButtonController.OnClicked()
  ↓
SlotHighlighter highlights free slots
  ↓
User clicks Slot
  ↓
Slot.StartConstruction()
  ↓
Factory.StartBuildingUnit()
  ↓
UnitPoolProvider.GetItem() [Get from pool]
  ↓
Unit.Activate() [Initialize unit]
  ↓
UnitBuildProgress.StartConstruction()
  ↓
BuildProgressCalculator updates progress each frame
  ↓
Factory.UnitCompleted event fires
  ↓
Unit.CompletedBuildable() → Spawned at factory position
```

### AI DECISION MAKING FLOW
```
AIManager.Update()
  ↓
ThreatMonitor.Evaluate() [Detect threats]
  ↓
TaskProducer.ProduceTasks() [Create response tasks]
  ↓
TaskList.Add() [Prioritized queue]
  ↓
TaskConsumer.ConsumeTask()
  ↓
Task transitions: Initial → InProgress → Completed
  ↓
ConstructBuildingTask example:
  - Find free slot
  - Select building type
  - Start construction
  - Wait for completion
```

---

## CRITICAL FILES INDEX

### Must-Read Files (Core Systems)

#### Scene Management
1. **BattleSceneGod.cs** - `/Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs` (702 lines)
   - **Purpose**: Main battle scene initialization and lifecycle
   - **Key Methods**: `Start()`, `SetupPlayerCruiser()`, `SetupAICruiser()`
   - **Critical Bug**: Static `GameOver` flag never reset (line 92)
   - **Used By**: All battle scene components

2. **ScreensSceneGod.cs** - `/Assets/Scripts/Scenes/ScreensSceneGod.cs` (1005 lines)
   - **Purpose**: Menu/UI scene controller
   - **Key Methods**: `Awake()`, screen transition methods
   - **Screen Controllers**: 12 managed screens (homeScreen, levelsScreen, etc.)

3. **SceneNavigator.cs** - `/Assets/Scripts/Scenes/SceneNavigator.cs` (123 lines)
   - **Purpose**: Async scene loading orchestrator
   - **Critical Bug**: EventSystem duplication (line 68-69)
   - **Pattern**: LoadSceneMode.Single then Additive

#### Core Gameplay
4. **Buildable.cs** - `/Assets/Scripts/Buildables/Buildable.cs` (699 lines)
   - **Purpose**: Base class for all units and buildings
   - **Lifecycle**: Activate() → OnStartedConstruction() → OnCompletedBuildable() → OnDestroyed()
   - **Critical Bug**: BoostChanged event not unsubscribed (line 332)
   - **Inherits From**: IBuildable, IActivatable, IDeactivatable

5. **Cruiser.cs** - `/Assets/Scripts/Cruisers/Cruiser.cs` (489 lines)
   - **Purpose**: Player/enemy capital ship
   - **Components**: DroneManager, HealthTracker, SlotInitialiser
   - **Key Feature**: Slot-based building placement

#### Networking
6. **ConnectionManager.cs** - `/Assets/Scripts/PvP/ConnectionManagement/ConnectionManager.cs` (varies)
   - **Purpose**: PvP connection state machine
   - **States**: Offline, StartingHost, Hosting, ClientConnecting, ClientConnected
   - **Critical Bug**: Latency limit asymmetry (Android vs Editor)

7. **PvPBattleSceneGodTunnel.cs** - `/Assets/Scripts/PvP/GamePlay/BattleScene/Managers/PvPBattleSceneGodTunnel.cs` (100+ lines)
   - **Purpose**: Pass PvP battle data between scenes
   - **Critical Bug**: Static variables never cleared between matches

#### Data Management
8. **Serializer.cs** - `/Assets/Scripts/Data/Serializer.cs` (993 lines)
   - **Purpose**: Save/load game state
   - **Critical Bugs**: Missing null checks on Reflection (lines 220-225), empty catch blocks (337, 361)
   - **Uses**: BinaryFormatter (deprecated)

9. **GameModel.cs** - `/Assets/Scripts/Data/Models/GameModel.cs` (662 lines)
   - **Purpose**: Player progression data model
   - **Contains**: Credits, unlocked items, completed levels, loadout
   - **Critical Bug**: Dual state tracking (_completedSideQuests + _completedSideQuestIDs)

---

## CROSS-REFERENCE GUIDE

### "If you're working on X, you'll need to understand Y"

#### Working on Turrets? Also check:
- **Projectiles** → `Projectiles/Spawners/` (what gets fired)
- **Target Tracking** → `Targets/TargetTrackers/` (what turret aims at)
- **Barrel Wrappers** → Determines firing pattern (burst, continuous, etc.)
- **Turret Stats** → Affects damage, range, fire rate
- **Boost System** → Can modify turret stats at runtime

#### Working on PvP? Also check:
- **PvE versions** → Many have parallel implementations
- **Network Variables** → All synchronized state uses NetworkVariable<T>
- **RPC calls** → Search for `ClientRpc` and `ServerRpc` attributes
- **Tunnel classes** → Pass data between scenes (static variables)

#### Working on UI? Also check:
- **Localization** → All visible text should use LocTable
- **Data Models** → UI reflects GameModel state
- **Screen Controllers** → Managed by ScreensSceneGod
- **Tutorial** → May need highlighting/explanation systems

#### Working on Pooling? Also check:
- **Factory systems** → Spawn units from pools
- **Activation Args** → Data passed when activating pooled objects
- **IPoolable interface** → All pooled objects implement this
- **Event subscriptions** → Must be cleaned up on Deactivate()

#### Working on AI? Also check:
- **Task System** → State machine pattern
- **Threat Monitors** → Detect what to respond to
- **Drone Management** → Resource allocation
- **Build Orders** → What AI constructs

---

## COMMON PATTERNS & CONVENTIONS

### Naming Conventions
```csharp
// Private fields
private ITarget _target;
private int _healthPoints;

// Properties (PascalCase)
public float BuildProgress { get; private set; }

// Constants
public const float MAX_RANGE_IN_M = 50f;

// Events
public event EventHandler<UnitStartedEventArgs> UnitStarted;

// Interfaces
public interface ITargetProvider { }
```

### Lifecycle Pattern (Pooled Objects)
```csharp
// Implementation in Buildable.cs and similar classes
public void Activate(BuildableActivationArgs args)
{
    // 1. Store activation args
    // 2. Subscribe to events
    // 3. Initialize components
    // 4. Fire Activated event
}

public void Deactivate()
{
    // 1. Unsubscribe from ALL events
    // 2. Clean up references
    // 3. Reset state
    // 4. Fire Deactivated event
    // 5. Return to pool
}
```

### Event Subscription Pattern
```csharp
// CORRECT: Subscribe in Activate, unsubscribe in Deactivate
public void Activate(...)
{
    _factory.UnitCompleted += OnUnitCompleted;
}

public void OnDestroyed()
{
    _factory.UnitCompleted -= OnUnitCompleted;  // MUST unsubscribe
    Deactivate();
}
```

### Network Synchronization Pattern (PvP)
```csharp
// Server authoritative
public NetworkVariable<float> pvp_Health = new NetworkVariable<float>();

void Update()
{
    if (IsServer)
    {
        // Update authoritative state
        pvp_Health.Value = _healthTracker.Health;
    }
    else
    {
        // Read synchronized state
        _localHealth = pvp_Health.Value;
    }
}
```

### Factory Pattern (Common)
```csharp
// Used for: Tasks, Turrets, Spawners, etc.
public class TurretStatsFactory
{
    public ITurretStats Create(TurretType type, TurretData data)
    {
        return type switch
        {
            TurretType.Basic => new BasicTurretStats(data),
            TurretType.BurstFire => new BurstFireTurretStats(data),
            _ => throw new ArgumentException($"Unknown type: {type}")
        };
    }
}
```

---

## DEBUGGING ENTRY POINTS

### "Game crashes on startup"
**Check**:
1. `LandingSceneGod.Awake()` - Entry point
2. `DataProvider.Initialise()` - Data loading
3. Missing prefab references in scenes

### "Turret not firing"
**Check**:
1. `BarrelFiringHelper.TryFire()` - Add debug logs
2. `BarrelAdjustmentHelper.GetAdjustment().IsOnTarget` - Is it aimed correctly?
3. `TargetTracker.CurrentTarget` - Does turret have a target?
4. `TurretStats.IsInBurst` - Check fire interval state

**For Ion Cannon specifically**:
1. `BeamEmitter.FireBeam()` line 58 - Does FindCollision() return null?
2. `BeamCollisionDetector.FindCollision()` - Add raycast debug visualization
3. Check layer masks (unitsLayerMask, shieldsLayerMask)

### "PvP won't connect"
**Check**:
1. `ConnectionManager.cs:129-139` - Latency limits (Android vs Editor)
2. `ConnectionMethod.cs:126` - 1-second relay wait
3. `AuthenticationServiceFacade.cs:26-34` - ParrelSync profile switching (Editor only)
4. `ConnectionState/HostingState.cs:67-72` - NetworkManager.Singleton null check

### "Save file corrupted"
**Check**:
1. `Serializer.ValidateCurrentSave()` - Validation logic
2. `SaveGameModel.GetSaveVersion()` - Version compatibility
3. `GameModel.OnDeserializedMethod()` - Null initialization fixes
4. Logs for Reflection GetProperty/GetField exceptions

### "Tutorial stuck"
**Check**:
1. `TutorialStepConsumer.Update()` - Which step is active?
2. Wait steps: Do events fire? (BuildableCompleted, TargetDestroyed, etc.)
3. `CameraAdjustmentWaitStep` - Is camera adjustment already complete?
4. `ExplanationDismissableStep` - Is button visible and enabled?

### "Memory leak / performance degradation"
**Check**:
1. Event subscriptions - Search for `+=` without matching `-=`
2. `Buildable.cs:332` - BoostChanged leak
3. `BattleSceneGod.cs` - Addressables handle not released (lines 523, 560)
4. `Pool.cs` MaxLimit reached - Units stop spawning
5. Static collections not cleared between battles

---

## APPENDIX: FILE COUNTS BY SYSTEM

| System | File Count | Tested | Coverage |
|--------|-----------|---------|----------|
| PvP | 461 | 0 | 0% |
| UI | 426 | 82 | 19% |
| Scenes | 221 | 0 | 0% |
| Buildables | 182 | 36 | 20% |
| Utils | 127 | varies | partial |
| Tutorial | 90 | 31 | 34% |
| Cruisers | 75 | varies | partial |
| Targets | 70 | varies | partial |
| AI | 48 | 30 | 63% |
| Projectiles | 44 | 0 | 0% |
| Movement | 32 | varies | partial |
| Data | varies | varies | partial |
| **TOTAL** | **~2,506** | **279** | **12.5%** |

---

## QUICK SEARCH PATTERNS

### Find all event subscriptions:
```bash
grep -r " += " Assets/Scripts/ --include="*.cs"
```

### Find all NetworkRpc calls:
```bash
grep -r "Rpc\|ClientRpc\|ServerRpc" Assets/Scripts/PvP/ --include="*.cs"
```

### Find all TODOs:
```bash
grep -r "TODO\|FIXME\|HACK" Assets/Scripts/ --include="*.cs"
```

### Find all Debug.Log statements:
```bash
grep -r "Debug.Log" Assets/Scripts/ --include="*.cs"
```

### Find test files with SetUp typo:
```bash
grep -r "SetuUp" Assets/Editor/Tests/ --include="*.cs"
```

---

**END OF REFERENCE CATALOG**
*For bug details and issues, see: BUG_REPORT_AND_ISSUES.md*
