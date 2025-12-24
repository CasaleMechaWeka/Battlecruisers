# ChainBattle Level Editor Plan


## Core Components

### 1. ChainBattleConfiguration ScriptableObject
**Location**: `Assets/Scripts/Data/ChainBattleConfiguration.cs`
- **Purpose**: Store all ChainBattle configuration data
- **Key Properties**:
  - `int levelNumber`: Assigned level number for integration with `LevelButtonController.cs`
  - `string levelName`: Display name for the level
  - `TrashTalkData trashTalkData`: Enemy dialog configuration from `StaticData.cs`
  - `LevelBackgroundImageStatsKey backgroundImageKey`: Background configuration from `BackgroundImageStats.cs`
  - `string skyMaterialName`: Sky material from `SkyStatsController.cs`
  - `SoundKeyPair musicKeys`: Music tracks from `LevelMusicPlayer.cs`
  - `ChainBattleHeckleConfig heckleConfig`: Extended heckling configuration with custom dialog
  - `List<CruiserPhase> cruiserPhases`: Sequential enemy cruiser configurations
  - `SequencePoint[] sequencePoints`: Battle sequencer events from `BattleSequencer.cs`
  - `Loot levelLoot`: Unlock rewards from `StaticData.cs`

### 2. ChainBattleHeckleConfig (Extended HeckleConfig)
**Purpose**: Enhanced heckle configuration supporting custom dialog sequences
- **Properties**:
  - All standard `HeckleConfig` properties
  - `List<ChainBattleDialog> customDialogs`: Custom dialog entries with string keys
  - `List<DialogSequencePoint> dialogSequencePoints`: Timed dialog triggers

### 3. CruiserPhase Data Structure
**Purpose**: Define each sequential enemy cruiser in the chain
- **Properties**:
  - `IPrefabKey hullKey`: Enemy hull type from `StaticData.cs`
  - `List<BodykitData> bodykits`: Applied bodykits from `StaticData.cs`
  - `IPrefabKey captainExoKey`: Captain exoskeleton from `StaticData.cs`
  - `int transitionHealthThreshold`: HP at which to trigger transition (default: 1)
  - `bool spawnDeathPrefab`: Whether to spawn cruiser death effect
  - `Vector2 teleportOffset`: Offscreen position for teleportation (right side)
  - `List<BuildingAction> initialBuildings`: Pre-battle building setup
  - `List<UnitAction> initialUnits`: Pre-battle unit spawning

### 4. ChainBattleDialog System
**Purpose**: Custom dialog entries for story-driven conversations during battles
- **ChainBattleDialog Structure**:
  - `string dialogKey`: Unique string key (e.g., "ChainBattle001_Intro")
  - `string englishText`: English dialog text injected into localization table at runtime
  - `int speakerIndex`: 0=Enemy Captain, 1=Player Captain, 2=Narrative/Story
  - `float displayDuration`: How long to show dialog (seconds)

- **DialogSequencePoint Structure**:
  - `float triggerTime`: Time from battle start to show dialog
  - `string dialogKey`: Reference to dialog entry
  - `SequenceTriggerType triggerType`: TimeBased, HealthBased, BuildingDestroyed, PhaseTransition

### 5. ChainBattle Editor Window
**Location**: `Assets/Editor/ChainBattleEditorWindow.cs`
- **Purpose**: Unity editor window for creating and editing ChainBattle configurations
- **Key Features**:
  - Level number assignment with validation against existing levels
  - Dropdown selections populated from `StaticData` for hulls, captains, bodykits
  - Drag-and-drop sequence point management with timeline visualization
  - Cruiser phase configuration with add/remove/reorder capabilities
  - Dialog editor with string key injection into localization tables
  - Real-time validation of configuration integrity
  - Save/Load ChainBattle ScriptableObject assets

### 6. ChainBattleManager Component
**Location**: `Assets/Scripts/Scenes/BattleScene/ChainBattleManager.cs`
- **Purpose**: Runtime management of ChainBattle transitions, dialog, scripted sequences, and reactive behaviors
- **Key Responsibilities**:
  - **No AI System**: ChainBattles are entirely scripted - NO regular AI decision making
  - Monitor enemy cruiser health for phase transitions
  - Handle teleportation mechanics when transition threshold reached
  - **Automatic Rebuilding**: Monitor building destruction and trigger BattleSequencer rebuilds
  - **Reactive Monitoring**: Track player building construction and trigger conditional sequences
  - **Factory Management**: Coordinate factory building → unit production sequences
  - **Dynamic Building Control**: Destroy and rebuild buildings in response to player actions
  - Spawn death prefabs at correct positions using existing cruiser destruction system
  - Replace enemy cruiser with next phase configuration using `CruiserFactory`
  - Maintain `ShipBlockingFriendlyProvider.cs` positioning during transitions
  - Coordinate with `BattleSequencer.cs` for ALL timed events and scripted actions
  - Inject custom dialog strings into `LocTableCache.HecklesTable` at runtime
  - Manage dialog display through extended heckle system
  - **Scripted Unit Production**: Trigger specific aircraft/ship spawns from built factories

### 5. ChainBattle Integration Points

#### BattleSequencer.cs Integration (Critical for Scripted Battles)
- **Automatic Rebuilding**: When buildings are destroyed, BattleSequencer automatically rebuilds them at same slots
- **Conditional Logic**: Support for conditional sequences based on player actions ("If player builds X, then destroy Y and build Z")
- **Factory Production Chains**: Scripted sequences where factories are built, then produce specific units in order
- **Reactive Sequences**: Pre-defined response sequences triggered by player building construction
- **Extend SequencePoint**: Add ChainBattleTransitionAction and ConditionalAction for programmatic control
- **Event Monitoring**: ChainBattleManager monitors both enemy destruction and player construction events
- **Unit Production Scripting**: Pre-determined sequences of aircraft/ship production from built factories
- **Slot Management**: Handle reserved slots and dynamic slot assignment for reactive behaviors
- **Maintain Compatibility**: Existing `BuildingAction`, `BoostAction`, `UnitAction` remain unchanged

#### LevelButtonController.cs Integration
- Add detection for ChainBattle levels vs regular levels
- Load ChainBattle configuration when level selected
- Pass ChainBattle data to battle scene initialization

#### StaticData.cs Integration
- Add `ChainBattles` collection parallel to existing `Levels`
- Maintain backward compatibility with existing level system
- Provide lookup methods for ChainBattle levels

## Technical Implementation Details

### Cruiser Transition Mechanics
1. **Health Monitoring**: Track enemy cruiser HP during battle via `HealthChanged` event
2. **Transition Trigger**: When HP ≤ `transitionHealthThreshold` (default: 1)
3. **Cruiser Replacement Sequence** (Updated - 11 phases, ~6-8 seconds total):

   - **Phase 1: Transition Start** - Damage prevention enabled
   - **Phase 2: Building Cleanup** - Destroy all enemy buildings (units lose factories but remain active)
   - **Phase 3: Death VFX** - Spawn CruiserDeathExplosion at cruiser position
   - **Phase 4: Game Pause** - Time.timeScale = 0
   - **Phase 5: Bonus Selection** - Display 3 random bonus cards, await player choice
   - **Phase 6: Bonus Confirmation** - Show "Bonus Engaged!" message (1-2 seconds)
   - **Phase 7: Game Resume** - Time.timeScale = 1
   - **Phase 8: Cruiser Swap** - Deactivate old phase cruiser, activate next phase cruiser
   - **Phase 9: Entry Animation** - Slide new cruiser from offscreen right to battle position
   - **Phase 10: Phase Setup** - Spawn initial buildings at slot positions
   - **Phase 11: Combat Resumes** - Damage prevention disabled, battle continues

### Automatic Rebuilding System

**Core Mechanism**: When any enemy building is destroyed, BattleSequencer automatically rebuilds it at the same slot position.

**Implementation**:
- ChainBattleManager monitors `Destroyed` events on all enemy buildings
- When building destroyed, triggers BattleSequencer to rebuild identical building at same slot
- Rebuilding happens immediately or at scripted timing (configurable)
- Maintains factory production chains even after destruction/rebuilding

**Factory Production Chains**:
- Scripted sequences: Build AirFactory → wait X seconds → spawn Gunship → wait Y seconds → spawn Bomber
- If factory destroyed during sequence, rebuilding preserves production progress
- Unit production continues seamlessly after rebuild

### Reactive Enemy Behavior System

**Core Concept**: Enemy can react to specific player actions by destroying and rebuilding buildings in response.

**Modular Reactive Rules**:
- **Individual ScriptableObjects**: Each reactive rule is a separate `ReactiveRule` ScriptableObject for easy copy/paste between ChainBattles
- **Rule Components**: Rules contain trigger conditions, response actions, and priority settings
- **Rule Library**: Shared rule assets can be reused across multiple ChainBattles
- **Rule Editor**: Visual editor for configuring triggers, conditions, and responses

**Player Action Monitoring**:
- ChainBattleManager monitors player building construction events (`CompletedBuildable` events)
- Tracks which buildings player has constructed and when
- Can trigger conditional sequences based on player building types

**Conditional Sequences**:
- BattleSequencer supports conditional branches: "If player builds X, then do Y"
- Example: If player builds Control Tower, destroy it and build Kamikaze Signal in same slot
- Conditions can be based on building type, timing, or combinations of player actions

**Slot Reservation and Management**:
- **Reserved Slots**: Certain cruiser slots can be marked as "reactive" and held empty for specific responses
- **Dynamic Slot Assignment**: When condition met, destroy existing building and build response building in that slot
- **Slot Priority**: Reserved slots take precedence over automatic rebuilding of other buildings

**Example Reactive Behaviors**:
- **Anti-Air Response**: If player builds Strike Bomber factory, destroy Control Tower and build Kamikaze Signal
- **Shield Counter**: If player builds artillery, destroy existing shield and build stronger shield
- **Factory Protection**: If player builds destroyer, destroy naval factory and rebuild in protected position
- **Economic Pressure**: If player builds drone stations, destroy credit-generating buildings

**Implementation Requirements**:
- Extend BattleSequencer with conditional logic and player action monitoring
- Add slot reservation system to cruiser configuration
- ChainBattleManager tracks player building construction
- Reactive sequences defined in editor with clear condition-action relationships

### Phase-Specific Stat Bonuses

**Bonus Application Strategy**: **STACK with existing bonuses** (recommended approach)

**Technical Implementation**:
- Uses existing `Cruiser.AddBoost()` and `Cruiser.RemoveBoost()` methods
- Bonuses applied via `BoostStats` objects with `BoostType` and `boostAmount`
- Phase bonuses stack with cruiser's base bonuses and any existing boosts
- When transitioning phases, new phase bonuses are added to existing bonuses
- Bonus selection UI applies additional bonuses that persist for the ChainBattle

**Available Boost Types** (from `BoostType` enum):
- **Build Rate Bonuses**: Aircraft, AirFactory, AllBuilding, Defense, Drone, Mast, Offense, Rocket, SeaFactory, Shield, Ship, Tactical, Ultra
- **Combat Bonuses**: AccuracyAllBuilding, FireRateDefense, FireRateOffense, FireRateRocket, HealthAllBuilding
- **Special Bonuses**: RechargeRateShield (shield recharge speed)

**Example Phase Bonuses**:
- Phase 1: BuildRateAircraft +25% (helps player build air units faster)
- Phase 2: FireRateOffense +30% (player offensive buildings fire faster)
- Phase 3: HealthAllBuilding +50% (all buildings have more health)

**Bonus Persistence**:
- Phase bonuses remain active until ChainBattle ends
- Bonus selection UI adds additional bonuses that also persist
- All bonuses are automatically cleared when ChainBattle ends

### Critical Technical Details

#### Scripted Battle Architecture (Key Distinction)
- **No AI System**: ChainBattles use NO regular AI - all enemy actions are pre-scripted through BattleSequencer
- **Event-Driven**: All building construction, unit production, and rebuilding is triggered by timed events or conditions
- **Automatic Rebuilding**: When buildings are destroyed, BattleSequencer automatically rebuilds them at the same slot positions
- **Factory Chains**: Scripted sequences where factories are built, then used to produce specific unit types in predetermined order

#### Cruiser Replacement Mechanics (Gap Fill - Critical)
- **Phase Management**: Deactivate old phase cruiser GameObject, activate next phase cruiser GameObject
- **Positioning**: Move active cruiser sprite to battle position
- **Building Control**: ChainBattleManager coordinates with BattleSequencer for building spawning at slot positions

#### Reference Updates After Replacement (Gap Fill - Critical)
- **BattleSceneGod**: Update `aiCruiser` reference and `enemyCruiserSprite`/`enemyCruiserName` variables
- **TargetTracker**: Update AI cruiser target references in `UserTargetTracker`
- **UIManager**: Update health bars, minimap, build menus via `UIManager` interface
- **AI System**: Reinitialize AI with new cruiser reference
- **ShipBlockingFriendlyProvider**: Update collision detection for new cruiser (maintains existing behavior)

#### Slide-in Animation Implementation (Gap Fill - High)
- **Animation Method**: Coroutine-based `Vector3.Lerp()` or DOTween `DOMove()`
- **Start Position**: `Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 0.5f, camera.nearClipPlane))` (offscreen RIGHT)
- **End Position**: Standard enemy battle position on RIGHT side of screen
- **Movement Direction**: Slides LEFT from offscreen right to visible battle position
- **Duration**: `entryAnimationDuration` seconds (2-4 seconds typical)
- **Easing**: Smooth ease-out curve using `Mathf.SmoothStep()` or DOTween ease functions

#### Building Spawn Timing (Gap Fill - High)
- **Pre-Battle**: Spawn Phase 1 buildings before battle starts via `BattleSequencer.BuildingAction`
- **During Transition**: No building spawning (all buildings destroyed)
- **Post-Transition**: Immediate spawn of next phase buildings after cruiser arrives
- **Sequence Integration**: Use `BattleSequencer` for timed building spawns during active phases

#### Captain UI Update Sequence (Gap Fill - High)
1. **During Transition**: Fade out old captain UI elements
2. **Post-Replacement**: Instantiate new `CaptainExo` prefab with `Object.Instantiate()`
3. **Positioning**: Set captain container position and apply color tinting (red for enemies)
4. **Animation**: Fade in new captain with scale animation from `Vector3.zero` to `Vector3.one`
5. **Name Update**: Update `EnemyName` Text component with new captain name from `AICaptain.captainName`
6. **UI Refresh**: Update captain portrait and any related UI elements

#### Enemy Units During Transition
- **Units persist**: Enemy units in the air or sea remain alive and continue fighting during transition
- **No cleanup needed**: Units are NOT destroyed - they simply lose their factories but continue operating
- **Natural behavior**: Existing units will continue attacking player targets as normal
- **Factory loss impact**: Units can no longer be produced from destroyed factories, but existing units remain fully functional

#### Projectiles During Transition
- Projectiles continue functioning normally - no special handling needed
- If a projectile was targeting the old cruiser, it will miss (splashes in ocean)
- This is existing behavior and works fine
- No interception or disabling logic required

### Dialog Display System

Simple approach - display the correct string with the correct speaker:

1. **Dialog Data**: Each `ChainBattleDialog` contains:
   - `string dialogKey`: Identifier for this dialog
   - `string englishText`: The text to display
   - `int speakerIndex`: 0=Enemy Captain, 1=Player Captain, 2=Narrative

2. **Display Method**: Create simple display function:
```csharp
   public void ShowChainBattleDialog(ChainBattleDialog dialog)
   {
       // Display the string with appropriate speaker portrait
       if (dialog.speakerIndex == 0) // Enemy
           ShowEnemyCaptainDialog(dialog.englishText, dialog.displayDuration);
       else if (dialog.speakerIndex == 1) // Player
           ShowPlayerCaptainDialog(dialog.englishText, dialog.displayDuration);
       else // Narrative
           ShowNarrativeDialog(dialog.englishText, dialog.displayDuration);
   }
```

3. **Integration**: Use existing heckle UI components but with direct string display
4. **No index management**: Don't inject into localization tables, just display the string directly

Investigation needed: How does the existing heckle UI accept text for display?
Can we call it directly with a string, or must we go through localization?

### Between-Phase Bonus Selection System

When a phase transition occurs, the player is offered a choice of temporary bonuses:

#### Bonus Selection Flow
1. **Trigger**: After enemy cruiser "death" VFX, before new cruiser appears
2. **Pause**: Game pauses (Time.timeScale = 0) - this is the ONE pause point in ChainBattle
3. **UI Display**: Bonus selection panel appears with 3 cards
4. **Selection**: Player taps one card to select their bonus
5. **Confirmation**: Brief "Bonus Engaged!" message with bonus name (e.g., "Offensive Fire Rate +50%!")
6. **Resume**: Panel disappears, Time.timeScale = 1, new cruiser slides in

#### Bonus Card System
- **Bonus Bank**: Predefined list of possible bonuses (ScriptableObject or static data)
- **Random Selection**: 3 bonuses randomly chosen from bank (no duplicates)
- **Card Display**: Each card shows bonus name and effect description
- **Visual Design**: Rogue-like card selection aesthetic

#### Example Bonus Bank
```csharp
public class ChainBattleBonus
{
    public string bonusName;           // "Offensive Fire Rate Up"
    public string description;         // "+50% fire rate for all weapons"
    public string engagedMessage;      // "Offensive Fire Rate +50%!"
    public BonusType type;             // Enum: FireRate, Damage, Health, BuildSpeed, etc.
    public float value;                // 0.5f for 50%
}

// Example bonuses:
- Offensive Fire Rate +50%
- Damage Output +30%
- Build Speed +40%
- Shield Strength +25%
- Drone Capacity +2
- Health Regeneration (slow)
- Critical Hit Chance +15%
- Projectile Speed +25%
```

#### Bonus Application
- Bonuses apply to PLAYER cruiser only
- Bonuses last for remainder of the ChainBattle (all subsequent phases)
- Bonuses stack if player earns multiple (one per transition)
- Implementation: Apply via existing boost system (`cruiser.AddBoost()`) or stat modifiers

#### UI Components Needed
- `BonusSelectionPanel`: Main container, appears/disappears
- `BonusCard`: Individual card prefab (x3 instantiated)
- `BonusEngagedMessage`: Confirmation text that fades out

### Design Principle: Scripted Battles with Reactive Elements

ChainBattles are fundamentally different from regular levels:
- **Primarily Scripted**: Core enemy actions are pre-scripted through BattleSequencer events
- **Reactive Elements**: Enemy responds to specific player actions with conditional sequences
- **Zero AI**: No AI decision-making - all responses are pre-defined conditional branches
- **Event-Driven**: Building construction, unit production, rebuilding, and reactive responses triggered by events
- **Automatic Rebuilding**: Destroyed buildings automatically rebuild, maintaining scripted production chains
- **Factory Chains**: Build factory → produce specific units in predetermined sequences
- **Predictable with Adaptation**: Players learn scripted patterns, but enemy adapts via pre-defined reactions

The only truly NEW systems are:
1. ChainBattleManager (orchestrates transitions, rebuilding, and reactive monitoring)
2. Bonus Selection UI (rogue-like card choice between phases)
3. Reactive Behavior System (conditional responses to player actions)
4. Pre-instantiated cruiser management (phase switching)

Everything else reuses existing BattleSequencer infrastructure.

### Cruiser Pre-Instantiation Strategy

All cruiser phases are instantiated at battle start, but only Phase 1 is active:

1. **Battle Start**: Instantiate all CruiserPhase cruisers as inactive GameObjects
2. **Cruiser Script Disabled**: Pre-instantiated cruisers do NOT use their Cruiser.cs script
   - Only the slot positions (child GameObjects) and hull Sprite are used
   - The ChainBattleManager controls building placement directly via slot transforms
3. **Phase Activation**: When a phase begins, ChainBattleManager:
   - Positions the phase's hull sprite at battle position
   - Spawns buildings at the slot positions using existing building instantiation
   - Buildings handle their own targeting and combat automatically
4. **No Cruiser Script Logic**: ChainBattleManager tells each slot what to build and when
   - Slot positions come from the hull prefab's child transforms
   - Buildings are independent once spawned (use existing Buildable behavior)
5. **Cleanup**: When battle ends, all pre-instantiated cruisers are destroyed

This approach requires investigation into:
- How to instantiate a hull prefab without activating Cruiser.cs
- How to access slot transforms from the inactive prefab
- Whether buildings can be parented to slot transforms without Cruiser.cs

### Battle Scene Initialization
- **GameMode.ChainBattle**: New game mode in `ApplicationModel.Mode` for ChainBattle detection
- **ChainBattleHelper**: New helper class extending `IBattleSceneHelper` for ChainBattle-specific setup
- **ScriptableObject Detection**: `BattleSceneGod` loads ChainBattle ScriptableObject by level number
- **Sequence Integration**: Merge ChainBattle sequence points with standard battle events
- **State Preservation**: Maintain player cruiser state across transitions

### Configuration Clarifications

#### Phase 1 Entry Animation = 0 Meaning (Gap Fill - Medium)
- **EntryAnimationDuration = 0**: First phase cruiser starts in battle position (no slide-in animation)
- **EntryAnimationDuration > 0**: Subsequent phases slide in from offscreen over specified duration
- **Use Case**: Phase 1 appears to "already be there" as the initial enemy presence

#### First Phase Initialization (Gap Fill - Medium)
- **Pre-Battle Setup**: Phase 1 buildings/units spawn before battle begins via `BattleSequencer`
- **Captain Setup**: Phase 1 captain instantiated and configured in `BattleSceneGod.Start()`
- **UI Initialization**: Phase 1 enemy name and sprite set before battle UI appears
- **Sequence Start**: `BattleSequencer.StartF()` called with Phase 1 sequence points

#### TransitionHealthThreshold = 0 Meaning (Gap Fill - Medium)
- **Threshold = 0**: Phase never transitions (used for final phase of ChainBattle)
- **Threshold = 1**: Phase transitions when cruiser reaches 1 HP (near-death)
- **Threshold > 1**: Phase transitions earlier (softer difficulty or faster pacing)
- **Use Case**: Final phase threshold = 0 ensures battle ends on cruiser destruction

#### Pause/Fast-Forward Behavior
- **During Combat**: Normal pause behavior (existing implementation)
- **During Bonus Selection**: Game is paused (Time.timeScale = 0), UI is interactive
- **During Entry Animation**: Pause disabled (brief window, ~3 seconds)
- **Fast-Forward**: Respects Time.timeScale for entry animation speed

#### Loot Distribution Timing (Gap Fill - Medium)
- **Battle Completion**: Loot awarded when final phase cruiser is destroyed (normal victory condition)
- **DestructionSceneGod**: Receives `ApplicationModel.Mode = GameMode.ChainBattle` for special handling
- **Progress Tracking**: Only final victory counts toward level completion and unlocks
- **Partial Rewards**: No rewards for intermediate phase completions (maintains illusion)

#### Damage Prevention Window Details (Gap Fill - Medium)
- **Duration**: Damage prevention active during entire transition sequence (~3-5 seconds)
- **Method**: Override `OnTakeDamage()` in ChainBattle cruiser to ignore damage calls
- **Visual Feedback**: Screen flash or UI indicator showing "transition in progress"
- **Projectile Handling**: Existing projectiles become non-functional during transition
- **Player Communication**: Clear visual indicator that enemy is transitioning

#### Timeline Component Clarification (Gap Fill - Low)
- **Not Unity Timeline**: Refers to battle sequence timeline managed by `ChainBattleManager`
- **Sequence Management**: Tracks dialog triggers, building spawns, unit waves by battle time
- **Visual Editor**: In Unity scene view, shows timeline markers for events (not complex like Unity Timeline)
- **Designer Tool**: Simple timeline for sequencing ChainBattle events, not full animation system

#### ShipBlockingFriendlyProvider Handling (Gap Fill - High)
- **Purpose**: Individual ship units use `ShipBlockingFriendlyProvider` to detect friendly ships directly ahead and stop moving to avoid collision
- **Significance**: Unlike `EnemyShipBlockerInitialiser` (used by cruisers for AI targeting), this affects unit navigation and pathfinding
- **Transition Impact**: During cruiser replacement, existing friendly units may still reference the "destroyed" cruiser position
- **Solution**: No special handling needed - units will naturally continue normal behavior since cruiser position changes don't affect unit-to-unit collision detection
- **Key Difference**: `ShipBlockingFriendlyProvider` = unit navigation; `EnemyShipBlockerInitialiser` = cruiser AI targeting

#### Error Recovery (Gap Fill - Medium)
- **Configuration Errors**: Log error and fall back to standard Campaign mode battle
- **Transition Failures**: If transition fails, destroy enemy cruiser and trigger victory
- **Missing Assets**: Validate all required prefabs exist before battle starts
- **Memory Issues**: If out of memory during transition, skip to victory with error logging
- **UI Failures**: Continue battle with degraded UI rather than crashing

## What the Designer Actually Creates

**Final Output**: A **ScriptableObject asset** (not a prefab) containing all ChainBattle configuration data

**Why ScriptableObject, Not Prefab**:
- **Data-Only**: Contains configuration data, not instantiated GameObjects
- **Version Control Friendly**: ScriptableObjects are easier to merge and version than scene prefabs
- **Runtime Instantiation**: System programmatically creates cruisers, buildings, and units from StaticData based on the configuration
- **No Scene Bloat**: Doesn't require maintaining separate scene hierarchies or prefab variants
- **Asset References**: Uses existing StaticData assets (hulls, buildings, units) via dropdown selections

**Created Asset Structure**:
```
Assets/Resources/ChainBattles/
├── PirateArmada.asset (ChainBattleConfiguration)
├── DesertConvoy.asset (ChainBattleConfiguration)
└── MountainSiege.asset (ChainBattleConfiguration)
```

Each `.asset` file contains the complete ChainBattle definition that the runtime system uses to orchestrate the battle.

## Designer Workflow

### Creating a ChainBattle
1. Open ChainBattle Editor Window (Window → ChainBattle Editor)
2. Assign unique level number (validated against existing levels)
3. Configure level metadata (name, music, background, sky, trash talk)
4. Add cruiser phases using the phase management UI
5. For each phase: select hull, captain, bodykits from StaticData dropdowns
6. Configure transition settings and initial buildings/units
7. Use sequence timeline to arrange battle events and dialog triggers
8. Set completion loot rewards
9. Save ChainBattle ScriptableObject asset

### Adding Custom Dialog
1. In ChainBattle Editor, expand "Dialog Configuration" section
2. Add dialog entries with unique keys and English text
3. Select speaker type from dropdown (Enemy Captain/Player Captain/Narrative)
4. Configure display duration and trigger conditions
5. Use timeline editor to position dialog events
6. Test dialog flow with preview buttons

### Detailed Editor Tool Usage

**Main Editor Interface:**
- **Menu Access**: `Window → ChainBattle Editor`
- **Tabbed Layout**: Separate tabs for Level Config, Phases, Dialog, Bonuses, Testing
- **Asset Validation**: Real-time checking against `StaticData` for asset existence
- **Auto-save**: Drafts saved to EditorPrefs, final save creates ScriptableObject

**Level Configuration Tab:**
- **Level Number Field**: Input with immediate validation (red if conflicts)
- **Metadata Dropdowns**: Auto-populated from `StaticData.SoundKeys`, `StaticData.Backgrounds`, etc.
- **Preview Panel**: Shows selected assets (cruiser sprite, background image, etc.)

**Phase Configuration Tab:**
- **Phase List**: Reorderable list with add/remove buttons
- **Per-Phase Inspector** (Fully Scripted Configuration):
  - **Hull Dropdown**: Shows all `StaticData.Hulls` with sprite previews
  - **Captain Dropdown**: `StaticData.Captains` with name and image
  - **Bodykit Multi-Select**: Available bodykits with visual indicators
  - **Transition Slider**: 0-100% with special handling for 0 (final phase)
  - **Initial Buildings**: Scripted building construction sequence (build order and timing)
  - **Automatic Rebuilding**: Enable/disable automatic rebuilds of destroyed buildings
  - **Reactive Behaviors**: Define conditional responses to player building construction
  - **Reserved Slots**: Mark cruiser slots as reserved for specific reactive building placements
  - **Factory Production**: Define factory → unit production chains (e.g., build AirFactory, then spawn 3 Gunships, 2 Bombers)
  - **Timeline Editor**: Comprehensive sequencing for all phase events, rebuilds, unit spawns, and reactive triggers
**BattleSequencer Integration**: Visual editor for creating complex factory → unit production chains
**Automatic Rebuilding Configuration**: Set rebuild timing and conditions per building type
**Script Validation**: Ensure factory production sequences are logically consistent

**Dialog Configuration Tab:**
- **Dialog Table**: Spreadsheet-like interface for dialog entries
- **Bulk Import**: CSV import for large dialog trees
- **Preview Playback**: Test dialog sequences in editor
- **Localization Export**: Generate localization keys for translation

**Reactive Behavior Configuration Tab:**
- **Rule Library Browser**: Browse and select existing `ReactiveRule` ScriptableObjects
- **Rule Editor**: Create new reactive rules with drag-and-drop building selection
- **Rule Import/Export**: Copy rules between ChainBattles or save rule sets for reuse
- **Conditional Rules Editor**: Define "If player builds X, then enemy does Y" rules
- **Trigger Conditions**: Set building type triggers, timing windows, and combination conditions
- **Response Actions**: Configure destruction targets, rebuild buildings, and slot assignments
- **Slot Reservation Manager**: Mark cruiser slots as reserved for specific reactive responses
- **Rule Validation**: Ensure conditional logic doesn't create conflicts or impossible states
- **Priority System**: Set precedence when multiple reactive conditions are met simultaneously

**Phase Bonus Configuration Tab:**
- **Phase Bonus Editor**: Configure stat bonuses for each cruiser phase using BoostType dropdowns
- **Bonus Stacking Preview**: Show how phase bonuses combine with base cruiser bonuses
- **Bonus Balance Testing**: Simulate bonus effects and validate balance
- **Bonus Persistence Settings**: Configure which bonuses persist across phase transitions

**Bonus Configuration Tab:**
- **Bonus Bank Editor**: Define ChainBattle-specific bonuses
- **Balance Testing**: Simulate bonus combinations and effects
- **Randomization Rules**: Configure selection probability weights

**Testing Tab:**
- **Validation Suite**: Automated checks for configuration integrity, especially BattleSequencer timing
- **Sequence Validation**: Ensures factory production chains are logically consistent
- **Rebuild Logic Testing**: Validates automatic rebuilding configurations
- **Play Mode Preview**: Limited battle simulation in editor
- **Error Reporting**: Detailed logs for troubleshooting BattleSequencer integration issues

**Editor Design Considerations:**
- **BattleSequencer Sensitivity**: The editor must be very careful with timing and sequencing to avoid breaking the delicate BattleSequencer integration
- **Factory Chain Validation**: Extensive validation to ensure production sequences make logical sense
- **Rebuild Timing**: Careful consideration of when and how buildings should automatically rebuild
- **Performance Impact**: Editor operations should not significantly impact Unity editor performance

### Testing a ChainBattle
1. Set `ApplicationModel.Mode = GameMode.ChainBattle` in test setup
2. Load ChainBattle via level selection (integrated with LevelButtonController)
3. Verify proper loading of first phase and dialog injection
4. Confirm transition mechanics work correctly
5. Validate dialog appears at correct times through heckle system
6. Ensure loot is awarded upon completion in `DestructionSceneGod.cs`

## Validation and Safety Checks

### Editor Validation
- Prevent duplicate level numbers across ChainBattles
- Validate all referenced assets exist in `StaticData.cs` with dropdown population
- Ensure at least one CruiserPhase is configured
- Check sequence point timing logic and dialog key uniqueness
- Validate dialog keys don't conflict with existing localization keys
- Real-time validation feedback in editor window

### Runtime Validation
- Verify cruiser transitions don't break `ShipBlockingFriendlyProvider.cs` targeting
- Confirm UI updates correctly during transitions (captain names, sprites)
- Ensure battle completion detection works with multiple phases
- Validate memory usage stays within bounds during transitions
- Confirm dialog injection doesn't corrupt existing localization tables
- Verify dialog display works through existing heckle system

## Future Extensions

### Skirmish Mode Integration
- Adapt ChainBattle editor for player-vs-player scenarios
- Add AI difficulty scaling across phases
- Implement phase-specific victory conditions

### Advanced Sequencing
- Conditional sequence points based on player actions
- Dynamic phase generation based on player performance
- Multi-path battle narratives

### Performance Optimizations
- Background loading of next phase assets
- Predictive cruiser state caching
- Memory pool management for frequent transitions

## Implementation Details

### Dialog System Architecture
**Runtime String Injection**:
```csharp
// In ChainBattleManager.Start()
foreach (var dialog in chainBattleConfig.CustomDialogs)
{
    LocTableCache.HecklesTable.AddRuntimeString(dialog.DialogKey, dialog.EnglishText);
}
```

**Extended NPCHeckleManager**:
- Inherit from or extend existing `NPCHeckleManager`
- Add support for custom dialog sequences alongside random heckles
- Use different visual styling for story dialog vs combat heckles

**Dialog Display Logic**:
- Use existing `HeckleMessage.Show()` but with custom keys
- Add speaker indicators (enemy captain icon, player captain icon, narrative style)
- Override default 5-second display time with custom durations

### ChainBattle GameMode Integration
**BattleSceneGod.cs Modifications**:
- Add `GameMode.ChainBattle` case to `CreateHelper()` method
- Create `ChainBattleHelper` class extending `IBattleSceneHelper`
- Detect ChainBattle prefab presence in scene

**ApplicationModel Extensions**:
- Add `SelectedChainBattlePrefab` property
- Maintain backward compatibility with existing campaign/side quest flow

## Risk Assessment

### Technical Risks
- **Localization Conflicts**: Runtime string injection may conflict with existing keys
- **Targeting System Breaks**: Cruiser teleportation may confuse `ShipBlockingFriendlyProvider.cs`
- **Memory Leaks**: Improper cleanup between phases or string table corruption
- **Performance Issues**: Asset loading during battle transitions

### Mitigation Strategies
- **Comprehensive Testing**: Unit tests for transition mechanics and dialog system
- **Safe String Injection**: Validate keys don't exist before injection, cleanup after battle
- **Profiling**: Monitor memory and performance during transitions
- **Fallback Mechanisms**: Graceful degradation if ChainBattle features fail
- **Incremental Rollout**: Start with simple 2-phase ChainBattles with basic dialog

## Success Metrics

### Developer Experience
- Time to create a ChainBattle level (target: <30 minutes for simple battles)
- Ease of dialog authoring and sequencing
- Visual feedback in prefab editor for transitions and timing
- Integration friction with existing battle systems

### Player Experience
- Seamless transition between cruiser phases without jarring interruptions
- Immersive story-driven dialog during battles
- Appropriate challenge scaling and variety across phases
- Clear visual feedback for phase changes and captain switches

### Technical Performance
- Memory usage stability during transitions (<10% increase)
- Frame rate consistency across phases (maintain 60fps)
- Asset loading times between transitions (<500ms)
- No localization table corruption or key conflicts

---

## Implementation Order

### Phase 1: Core Infrastructure (Do First)
1. Create `ChainBattleConfiguration` ScriptableObject
2. Create `CruiserPhase` data structure
3. Add `GameMode.ChainBattle` to `ApplicationModel`
4. Add `ChainBattles` collection to `StaticData`
5. Basic `ChainBattleManager` with health monitoring (no transition yet)

### Phase 2: Transition Mechanics (Core Feature)
1. Implement building cleanup using `IDamagable.Destroy()`
2. Implement death VFX spawning using `CruiserDeathExplosion` system
3. Implement cruiser teleportation to offscreen positions
4. Implement simple slide-in entry animation (2-3 seconds)
5. Implement damage prevention during transition via `OnTakeDamage()` override
6. Implement hull/bodykit/captain swapping via cruiser replacement

### Phase 3: Phase Setup
1. Implement initial building spawning per phase using `BattleSequencer.BuildingAction`
2. Implement initial unit spawning per phase using `BattleSequencer.UnitAction`
3. Implement phase-specific sequencing (timed spawns using pre-configured `SequencePoint`s)

### Phase 4: Dialogue System
1. Create dialogue injection system for `LocTableCache.HecklesTable`
2. Implement dialogue trigger points (time-based, transition-based)
3. Extend `NPCHeckleManager` with custom dialog display methods
4. Add speaker differentiation (Enemy Captain/Player Captain/Narrative)
5. Test dialogue cleanup after battle

### Phase 5: Integration & Polish
1. Integrate with `LevelButtonController` for level selection
2. Integrate with `BattleSceneGod` via new `ChainBattleHelper`
3. Add comprehensive editor validation and dropdown population
4. Create example ChainBattle ScriptableObject
5. Test full flow from level select to battle completion

### Phase 6: Victory & Rewards
1. Ensure final phase death triggers normal victory condition
2. Integrate with `DestructionSceneGod` for loot distribution
3. Test reward distribution and progression

---

## Testing Checklist

### Unit Tests
- [ ] Health monitoring correctly identifies threshold breach (`HealthChanged` event)
- [ ] Building cleanup removes all enemy buildings without VFX
- [ ] Death VFX spawning replicates existing `CruiserDeathExplosion` system
- [ ] Dialogue strings inject without corrupting existing `HecklesTable`
- [ ] Dialogue strings clean up after battle (prevent memory leaks)
- [ ] Phase configuration validation catches invalid slots/buildings
- [ ] Phase configuration validation catches missing assets from `StaticData`
- [ ] Damage interception prevents damage during transitions (`OnTakeDamage` override)

### Integration Tests
- [ ] Full 2-phase ChainBattle from start to victory
- [ ] Full 3-phase ChainBattle from start to victory
- [ ] Player projectiles don't cause issues during transition (damage interception)
- [ ] Player cruiser continues operating normally during transition
- [ ] Dialogue appears at correct moments through extended heckle system
- [ ] Victory triggers on final phase death only (not intermediate phases)
- [ ] Loot is awarded correctly in `DestructionSceneGod`

### Edge Cases
- [ ] Player defeats phase faster than entry animation duration
- [ ] Player pauses during transition (time scaling)
- [ ] Player fast-forwards during transition
- [ ] Phase transition threshold set higher than cruiser max health (should never trigger)
- [ ] Empty `InitialBuildings` list (should not crash)
- [ ] Only one phase configured (effectively a normal battle)
- [ ] Camera bounds calculation for different aspect ratios
- [ ] Dialogue system handles missing speaker types gracefully

---

## Example ChainBattle Configuration

For reference, here's what a complete configuration might look like:
```
ChainBattle: "Pirate Armada"
├── LevelNumber: 32
├── LevelDisplayName: "Pirate Armada"
├── BackgroundKey: OceanSunset
├── SkyMaterialName: "Sky_Sunset"
├── MusicKeys: (BattleTheme_Intense, VictoryTheme_Epic)
│
├── CruiserPhases:
│   ├── Phase 1: "Scout Ship"
│   │   ├── HullKey: Hull_Light
│   │   ├── Bodykits: [Pirate_Flag]
│   │   ├── CaptainExoKey: Captain_Pirate_Scout
│   │   ├── TransitionHealthThreshold: 1 (transitions at 1 HP)
│   │   ├── EntryAnimationDuration: 0 (starts in position, no slide-in)
│   │   ├── InitialBuildings: [Turret@Slot0, Turret@Slot1]
│   │   └── PhaseSequencePoints: [Gunship@30s, Gunship@60s]
│   │
│   ├── Phase 2: "Frigate"
│   │   ├── HullKey: Hull_Medium
│   │   ├── Bodykits: [Pirate_Flag, Armored_Hull]
│   │   ├── CaptainExoKey: Captain_Pirate_Lieutenant
│   │   ├── TransitionHealthThreshold: 1 (transitions at 1 HP)
│   │   ├── EntryAnimationDuration: 3 (slides in over 3 seconds)
│   │   ├── InitialBuildings: [Turret@Slot0, Turret@Slot1, Shield@Slot2, DestroyerFactory@Slot3]
│   │   └── PhaseSequencePoints: [Destroyer@20s, Bomber@40s, Destroyer@60s]
│   │
│   └── Phase 3: "Flagship" (Final)
│       ├── HullKey: Hull_Heavy
│       │   ├── Bodykits: [Pirate_Flag, Armored_Hull, Gold_Trim]
│       │   ├── CaptainExoKey: Captain_Pirate_Admiral
│       │   ├── TransitionHealthThreshold: 0 (never transitions - final phase)
│       │   ├── EntryAnimationDuration: 4 (slides in over 4 seconds)
│       │   ├── InitialBuildings: [UltraTurret@Slot0, Shield@Slot1, Shield@Slot2, NukeSilo@Slot3]
│       │   └── PhaseSequencePoints: [Battleship@30s, Bomber@45s, Battleship@60s, Bomber@75s]
│
├── DialogueSequence:
│   ├── {Trigger: PhaseStart, Phase: 0, Key: "CHAIN32_INTRO", Speaker: Enemy}
│   │   └── "You dare enter Pirate waters? You'll regret this!"
│   ├── {Trigger: TransitionBegin, Phase: 0, Key: "CHAIN32_P1_DEATH", Speaker: Player}
│   │   └── "One down!"
│   ├── {Trigger: TransitionMidpoint, Phase: 1, Key: "CHAIN32_P2_ARRIVE", Speaker: Enemy}
│   │   └── "You've made a powerful enemy today!"
│   ├── {Trigger: TransitionBegin, Phase: 1, Key: "CHAIN32_P2_DEATH", Speaker: Player}
│   │   └── "Is that all you've got?"
│   ├── {Trigger: TransitionMidpoint, Phase: 2, Key: "CHAIN32_P3_ARRIVE", Speaker: Enemy}
│   │   └── "NOW YOU FACE THE ADMIRAL!"
│   └── {Trigger: BattleVictory, Key: "CHAIN32_VICTORY", Speaker: Player}
│       └── "The seas are safe once more."
│
├── Reactive Behaviors:
│   ├── Rule 1: If Player builds Control Tower → Destroy Control Tower, build Kamikaze Signal in slot 7
│   ├── Rule 2: If Player builds Strike Bomber Factory → Destroy Naval Factory, rebuild in reserved slot 8
│   └── Rule 3: If Player builds Artillery → Destroy Shield Generator, build Ultra Shield in reserved slot 9
│
├── Phase Bonuses:
│   ├── Phase 1: FireRateOffense +25% (enemy offensive buildings fire faster)
│   ├── Phase 2: BuildRateAircraft +40% (enemy builds aircraft faster)
│   └── Phase 3: HealthAllBuilding +50% (enemy buildings have more health)
│
└── CompletionLoot: [500 Credits, Pirate_Hull_Unlock]



## Answers to Technical Questions

### Must-Answer Questions

1. **Cruiser Health Event Signature**: `event EventHandler HealthChanged;` - Standard EventHandler with no custom args. Located in `IDamagable` interface.

2. **Building Destruction API**: `IDamagable.Destroy()` method silently destroys buildings without VFX. Located in `IDamagable` interface (inherited by `IBuilding` → `IBuildable` → `ITarget` → `IDamagable`).

3. **Cruiser Entry Animation**: No dedicated entry animation system exists. Cruisers appear instantly at their starting positions (defined by `CRUISER_OFFSET_IN_M = 35` in `CruiserFactory`). For ChainBattles, we'll implement a simple slide-in animation.

4. **Death VFX Spawning**: Use `CruiserDeathExplosion` prefab system:
   ```csharp
   CruiserDeathExplosion deathPrefab = Object.Instantiate(cruiser.DeathPrefab);
   deathPrefab.ApplyBodykitWreck(bodykit);
   IPoolable<Vector3> explosion = deathPrefab.Initialise(settings);
   explosion.Activate(cruiser.Transform.Position);
   ```

5. **Damage Interception**: `Target.TakeDamage()` is not virtual, but `Target.OnTakeDamage()` is protected virtual. Override `OnTakeDamage()` in a custom cruiser class to intercept damage during transitions.

6. **Hull Swapping**: Runtime hull swapping is not supported. `CruiserFactory` creates cruisers from prefabs, so hull changes require destroying and reinstantiating the cruiser. Use cruiser replacement approach.

7. **NPCHeckleManager API**: `HeckleMessage.Show(int heckleIndex)` - takes integer index only, no custom key support. For ChainBattle dialog, extend `NPCHeckleManager` with custom dialog display methods that inject strings into `LocTableCache.HecklesTable` and use temporary heckle indices.

8. **BattleSequencer Runtime Addition**: `SequencePoint` objects are only processed at battle start from the pre-assigned array. Cannot add at runtime. ChainBattle sequences must be pre-configured.

### Nice-to-Know Questions

9. **Camera Bounds**: No explicit camera bounds defined. Use `Camera.main.ViewportToWorldPoint()` to calculate offscreen positions. For "offscreen right", use `viewportPoint = new Vector3(1.5f, 0.5f, camera.nearClipPlane)`.

10. **Unit Spawning Logic**: `BattleSequencer` already implements `UnitAction` processing with `SpawnUnit()` method. Supports both single unit placement and area spawning with random distribution.

11. **BoostAction**: Works by calling `cruiser.AddBoost()`/`cruiser.RemoveBoost()` with `Cruiser.BoostStats` objects containing `boostType` and `boostAmount`.

## Overview
Create an editor-based system for designing **fully scripted sequenced battles (ChainBattles)** that leverage the existing `BattleSequencer.cs` for all enemy actions. Unlike regular levels that use AI decision-making, ChainBattles are primarily event-driven with pre-scripted building construction, unit production, and automatic rebuilding, but include **reactive elements** that respond to player actions. Uses ScriptableObjects for data storage and Unity editor windows for configuration, with dropdowns selecting from existing StaticData assets.

Final Notes

Keep the illusion simple: Don't overcomplicate the transition. Fast, clean substitution is better than elaborate choreography.
Test on low-end devices: The transition must not cause frame drops.
Designer-friendly first: Dropdown selections from existing StaticData make configuration intuitive and error-resistant.
Fail gracefully: If any phase configuration is broken, log an error and skip to victory rather than crashing.
Asset reuse: Leverage existing prefabs through programmatic instantiation rather than creating new prefab variants.

---

## Complete File Implementation List

### NEW FILES TO CREATE (14 total):

#### Data Structures (6 files):
1. **`Assets/Scripts/Data/ChainBattleConfiguration.cs`** - ScriptableObject containing all ChainBattle data (phases, dialog, sequence points, loot, reactive behaviors, phase bonuses)
2. **`Assets/Scripts/Data/CruiserPhase.cs`** - Data structure defining each cruiser phase (hull, bodykits, captain, transition settings, phase bonuses)
3. **`Assets/Scripts/Data/ChainBattleDialog.cs`** - Dialog entry with key, text, speaker type, and display duration
4. **`Assets/Scripts/Data/DialogSequencePoint.cs`** - Dialog timing and trigger configuration
5. **`Assets/Scripts/Data/ReactiveRule.cs`** - Individual ScriptableObject for each conditional response rule (reusable across ChainBattles)
6. **`Assets/Scripts/Data/PhaseBonus.cs`** - Defines stat bonuses that can be applied to cruiser phases

#### Editor Interface (1 file):
5. **`Assets/Editor/ChainBattleEditorWindow.cs`** - Unity editor window with dropdowns, timeline, and validation for creating ChainBattles

#### Runtime Systems (3 files):
6. **`Assets/Scripts/Scenes/BattleScene/ChainBattleManager.cs`** - Runtime manager handling transitions, health monitoring, and dialog injection
7. **`Assets/Scripts/Scenes/BattleScene/ChainBattleHelper.cs`** - Helper class extending IBattleSceneHelper for ChainBattle-specific BattleSceneGod integration
8. **`Assets/Scripts/Scenes/BattleScene/ExtendedNPCHeckleManager.cs`** - Extended heckle manager supporting custom dialog sequences with speaker differentiation

#### UI Components (4 files):
9. **`Assets/Scripts/UI/BattleScene/Heckles/ChainBattleHeckleMessage.cs`** - Extended HeckleMessage component supporting speaker-based styling for ChainBattle dialog
10. **`Assets/Scripts/UI/BattleScene/BonusSelectionPanel.cs`** - Main container for bonus selection UI
11. **`Assets/Scripts/UI/BattleScene/BonusCard.cs`** - Individual card component for bonus selection
12. **`Assets/Scripts/UI/BattleScene/BonusEngagedMessage.cs`** - Confirmation message component

### EXISTING FILES TO MODIFY (10 files):

#### Core Data Systems:
10. **`Assets/Scripts/Data/Static/StaticData.cs`** - Add ChainBattles ReadOnlyCollection and lookup methods
11. **`Assets/Scripts/Data/ApplicationModel.cs`** - Add GameMode.ChainBattle enum value

#### UI Integration:
12. **`Assets/Scripts/UI/ScreensScene/LevelsScreen/LevelButtonController.cs`** - Detect ChainBattle levels and pass configuration to BattleSceneGod

#### Battle Scene Systems:
13. **`Assets/Scripts/Scenes/BattleScene/BattleSceneGod.cs`** - Add ChainBattle mode support in CreateHelper() and initialization logic
14. **`Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs`** - Extend SequencePoint with ChainBattleTransitionAction for programmatic sequence control

#### Victory & Rewards:
15. **`Assets/Scripts/Scenes/DestructionSceneGod.cs`** - Handle ChainBattle victory conditions and loot distribution

#### Cruiser Systems:
16. **`Assets/Scripts/Cruisers/Cruiser.cs`** - Override OnTakeDamage() for transition damage prevention
17. **`Assets/Scripts/Cruisers/CruiserFactory.cs`** - Ensure CreateAICruiser() supports bodykit application
18. **`Assets/Scripts/Cruisers/CruiserDeathManager.cs`** - Support bodykit-aware death VFX spawning for transitions

#### Localization:
19. **`Assets/Scripts/Utils/Localization/LocTableCache.cs`** - Ensure HecklesTable supports runtime string injection

---

## Detailed File Purpose Breakdown

### NEW FILES - Data Structures:
1. **`ChainBattleConfiguration.cs`** - Central ScriptableObject storing complete ChainBattle definition including phases, dialog, sequence points, and victory conditions
2. **`CruiserPhase.cs`** - Defines individual cruiser phases with hull, bodykits, captain, transition thresholds, and initial loadouts
3. **`ChainBattleDialog.cs`** - Contains dialog text, speaker type, display duration, and localization keys for story-driven conversations
4. **`DialogSequencePoint.cs`** - Specifies when and how dialog should be triggered during battle (time-based, event-based, phase-based)

### NEW FILES - Editor Interface:
5. **`ChainBattleEditorWindow.cs`** - Provides Unity editor interface for designers to create ChainBattles with dropdown selections from StaticData, timeline editing, and real-time validation

### NEW FILES - Runtime Systems:
6. **`ChainBattleManager.cs`** - Core runtime component managing health monitoring, phase transitions, cruiser replacement, and battle flow coordination
7. **`ChainBattleHelper.cs`** - Extends IBattleSceneHelper to provide ChainBattle-specific initialization logic for BattleSceneGod integration
8. **`ExtendedNPCHeckleManager.cs`** - Enhanced heckle manager that supports custom dialog sequences alongside random heckles with proper speaker differentiation

### NEW FILES - UI Components:
9. **`ChainBattleHeckleMessage.cs`** - Extended HeckleMessage component that supports different visual styling for enemy captain, player captain, and narrative dialog

### MODIFIED FILES - Core Data:
10. **`StaticData.cs`** - Adds ChainBattles collection and lookup methods to integrate with existing level system
11. **`ApplicationModel.cs`** - Adds GameMode.ChainBattle enum value for mode detection throughout the application

### MODIFIED FILES - UI Integration:
12. **`LevelButtonController.cs`** - Modified to detect ChainBattle levels and load appropriate configuration instead of standard level data

### MODIFIED FILES - Battle Scene:
13. **`BattleSceneGod.cs`** - Extended CreateHelper() method and initialization logic to support ChainBattle mode with proper helper selection
14. **`BattleSequencer.cs`** - Extended with ChainBattleTransitionAction to allow programmatic sequence point addition during runtime transitions

### MODIFIED FILES - Victory & Rewards:
15. **`DestructionSceneGod.cs`** - Modified to handle ChainBattle victory conditions and award loot only after final phase completion

### MODIFIED FILES - Cruiser Systems:
16. **`Cruiser.cs`** - Override OnTakeDamage() virtual method to prevent damage during transition sequences
17. **`CruiserFactory.cs`** - Ensure CreateAICruiser() properly applies bodykits when creating cruisers for new phases
18. **`CruiserDeathManager.cs`** - Extended to support spawning death VFX with correct bodykits during fake deaths

### MODIFIED FILES - Localization:
19. **`LocTableCache.cs`** - Ensure HecklesTable.AddRuntimeString() method exists for dialog injection during battle initialization

---

## REVISED USER STORY

**As a game designer**, I want to create custom sequenced battles (ChainBattles) that are primarily scripted with zero AI involvement, where enemy buildings automatically rebuild when destroyed, factories produce specific units in predetermined sequences, each cruiser phase can have unique stat bonuses, and the enemy can react to player building construction with modular reusable conditional rules, so that players experience epic boss-style encounters with rich story-driven dialog, escalating difficulty, and adaptive enemy behavior.

**Acceptance Criteria:**
- ✅ Designers can use a Unity editor window to configure ChainBattles by selecting from existing StaticData assets via dropdowns
- ✅ Each ChainBattle can have 2-5 sequential cruiser phases with different hulls, bodykits, captains, and stat bonuses
- ✅ Enemy cruisers transition to the next phase when reaching 1 HP, creating the illusion of sequential battles
- ✅ Transitions include death VFX, offscreen teleportation, slide-in animation, and reference updates
- ✅ Enemy can react to player building construction with modular reusable conditional rules (ScriptableObjects)
- ✅ Reserved cruiser slots can be used for specific reactive building placements
- ✅ Phase bonuses stack with existing cruiser bonuses using the established Boost system
- ✅ Custom dialog can be injected into the existing heckle system with speaker differentiation (enemy captain/player captain/narrative)
- ✅ Dialog is triggered by time-based, health-based, phase transition, or reactive events
- ✅ ChainBattles integrate seamlessly with existing level selection, battle scene initialization, and victory rewards
- ✅ Only the final phase completion awards loot and counts as level completion
- ✅ Comprehensive error handling ensures graceful fallback if configuration is invalid
- ✅ All transitions maintain existing gameplay systems (targeting, UI, reactive behaviors, bonuses)

**Technical Implementation:**
- Create 14 new files for data structures, editor interface, runtime systems, and UI components
- Modify 10 existing files for integration with core systems
- Use ScriptableObjects for data storage with dropdown population from StaticData
- Leverage existing BattleSequencer, HeckleMessage, and CruiserFactory systems
- Implement 11-phase transition sequence with bonus selection and building-only cleanup
- Support conditional reactive behaviors based on player building construction
- Support runtime dialog injection into localization tables
- Maintain backward compatibility with existing Campaign/SideQuest modes

---

## Macro View Assessment: Is This a Sensible Method?

### ✅ Strengths of This Approach

**1. Maximum Reuse of Existing Systems**
- Leverages proven `BattleSequencer` for event timing and building/unit spawning
- Uses existing `HeckleMessage` system for dialog display with speaker differentiation
- Integrates with established `CruiserFactory` and `StaticData` systems
- Maintains compatibility with existing level selection and victory systems

**2. Minimal Technical Risk**
- Pre-instantiated cruiser approach avoids runtime instantiation complexity
- Simple teleportation + VFX illusion is visually convincing without complex animation systems
- Existing projectile/building/unit behavior works unchanged during transitions
- Error recovery is straightforward (skip to victory on failures)

**3. Designer-Friendly Workflow**
- Editor window with dropdowns from existing assets eliminates asset creation overhead
- Visual timeline for sequencing events is intuitive
- Real-time validation prevents configuration errors
- Bonus selection system adds strategic depth without complexity

**4. Scalable Architecture**
- ScriptableObject storage allows easy expansion to hundreds of ChainBattles
- Phase-based structure supports varied difficulty curves
- Dialog system extensible for complex story arcs
- Bonus system provides replayability incentives

### ⚠️ Potential Concerns

**1. Pre-Instantiation Complexity**
- Investigating hull prefab instantiation without Cruiser.cs activation
- Managing multiple inactive cruiser GameObjects in scene
- Ensuring proper cleanup of pre-instantiated assets

**2. Dialog System Limitations**
- Direct string display bypasses existing localization workflow
- Speaker differentiation may require UI modifications
- No built-in voice acting integration

**3. Bonus System Balance**
- Need careful tuning to prevent overpowering effects
- Random selection might create unbalanced experiences
- Additional UI complexity for card selection

### 🎯 Overall Assessment: HIGHLY RECOMMENDED

**This approach is exceptionally sensible because:**

1. **Zero AI, 100% Scripted**: Unlike regular levels, ChainBattles are entirely event-driven through BattleSequencer
2. **Automatic Rebuilding**: Destroyed buildings automatically rebuild, maintaining scripted factory production chains
3. **Factory Production Sequences**: Build factory → spawn specific units in predetermined order (no AI decision-making)
4. **Illusion vs Reality**: Clever use of existing VFX and positioning creates convincing sequential battles
5. **Iterative Development**: Can start simple (2-phase battles) and expand to complex multi-phase encounters
6. **Player Experience**: Bonus selection adds strategic choice, dialog adds narrative depth
7. **Maintenance**: Changes to core battle systems automatically benefit ChainBattles

**Compared to Alternatives:**
- **Prefab-based**: Too complex, version control issues, asset bloat
- **Runtime Generation**: Higher technical risk, performance concerns
- **Separate Battle System**: Massive duplication, maintenance nightmare
- **AI-based**: Unpredictable behavior, harder to balance and test

**This method achieves the "epic boss-style encounters" goal with minimal risk and maximum leverage of existing, proven systems. It's essentially extending the current battle system rather than creating a parallel one.**

**Key Technical Insight**: ChainBattles are primarily scripted with zero AI involvement, but include reactive elements that respond to player actions. All enemy building construction, unit production, rebuilding, and conditional responses are handled by BattleSequencer events. When buildings are destroyed, they automatically rebuild at the same positions, maintaining factory production chains. The enemy can react to specific player buildings by destroying and rebuilding in reserved slots (e.g., Control Tower → Kamikaze Signal). Enemy units persist through transitions, creating tactical depth as surviving units from earlier phases continue fighting alongside new cruisers.

---

## Specific Scripts/Methods for Building Destruction

**Building Destruction API**: `IDamagable.Destroy()` method (inherited by `IBuilding`)
- **Location**: `Assets/Scripts/Buildables/IDamagable.cs` interface
- **Silent Destruction**: No VFX, just removes the building
- **Usage**: `enemyBuilding.Destroy()` - building disappears, units lose factory but remain active
- **Investigation**: Confirm buildings can be enumerated via cruiser's slot system or scene query

**ShipBlockingFriendlyProvider Significance**:
- **Purpose**: Unit navigation system - ships detect FRIENDLY ships ahead and stop to avoid collision
- **Location**: `Assets/Scripts/Targets/TargetProviders/ShipBlockingFriendlyProvider.cs`
- **Key Difference**: Unlike `EnemyShipBlockerInitialiser` (cruiser AI targeting), this affects individual unit movement
- **Transition Impact**: Minimal - units continue normal collision avoidance with other units
- **No Special Handling Needed**: Existing behavior works correctly during cruiser transitions
