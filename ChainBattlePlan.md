# ChainCruiser System - Multi-Hull Boss Implementation

## Status: ✅ IMPLEMENTATION COMPLETE (January 2026)

ChainCruiser is a multi-hull boss entity system where each hull section has independent health, targeting, and destruction behavior. Primary hull destruction triggers game victory.

---

## Core Philosophy

### 🎯 **Multi-Hull Architecture**
- **Independent Hulls**: Each hull section can be independently targeted and damaged
- **Primary/Secondary Logic**: Primary hull destruction = victory, secondary hulls = continued battle
- **Shared Cruiser Identity**: One cruiser with multiple targetable sections
- **Independent Death Sequences**: Each hull has its own explosion effects

### 🎨 **Hybrid Inheritance Design**
- Inherits from `Cruiser` for full ICruiser compatibility
- Overrides health properties to route to primary hull
- Maintains all existing cruiser functionality (slots, drones, boosts)
- Adds hull section management and destruction handling

---

## File Structure

### Core Implementation
```
Assets/Scripts/Cruisers/
├── ChainCruiser.cs                       # Multi-hull cruiser coordinator
│   ├── Inherits: Cruiser                  # Full ICruiser compatibility
│   ├── HullSections[]                     # Array of HullSection components
│   ├── _primaryHull                       # Cached primary hull reference
│   ├── IsAlive override                   # Routes to primary hull
│   ├── Health/MaxHealth override          # Routes to primary hull
│   ├── OnHullSectionDestroyed()           # Handles primary/secondary death
│   └── SecondaryHullDestroyed event       # For external listeners
├── HullSection.cs                         # Individual targetable hull
│   ├── Inherits: MonoBehaviour, ITarget   # Independent targeting
│   ├── maxHealth                          # Independent health pool
│   ├── _healthTracker                     # Own HealthTracker instance
│   ├── IsPrimary                          # Victory condition flag
│   └── DeathPrefab                        # Explosion on destruction
└── HullSectionDestroyedEventArgs.cs       # Event args for hull destruction
```

### Targeting Integration
```
Assets/Scripts/Targets/TargetFinders/
└── GlobalTargetFinder.cs                  # Multi-hull targeting support
    ├── ChainCruiser support               # Emits hull sections as targets
    ├── SecondaryHullDestroyed event       # Removes destroyed hulls
    └── DisposeManagedState()              # Cleanup event subscriptions
```

### Modified Files
```
Assets/Scripts/Cruisers/
└── Cruiser.cs                             # Enhanced for ChainCruiser
    ├── protected _selectedSound           # Hull click handlers access
    ├── protected _helper                  # Hull click handlers access
    ├── protected _cruiserDoubleClickHandler # Hull double-click access
    └── virtual IsAlive                    # ChainCruiser can override
```

---

## GameObject Hierarchy

### 1️⃣ **ChainCruiser Root Structure**
```
ChainCruiser (Root GameObject)
├── ChainCruiser.cs                    # Main coordinator component
├── Animator                           # Boss animations and effects
├── Cruiser components...              # Inherited from Cruiser base class
│   ├── CruiserDeathExplosion          # Primary death explosion prefab
│   ├── FogOfWar                       # Vision/obscurement system
│   ├── SlotWrapperController          # Building slots management
│   ├── Cruiser components...          # All standard cruiser systems
│   └── Boosts[]                       # Stat boost configuration
├── HullSection_A (Child)              # Primary hull (IsPrimary = true)
│   ├── HullSection.cs                 # Independent hull component
│   ├── SpriteRenderer                 # Visual representation
│   ├── PolygonCollider2D              # Targeting/collision
│   ├── ClickHandlerWrapper            # Click handling
│   ├── SlotWrapperController?         # Optional building slots
│   ├── maxHealth = 2000               # Independent health pool
│   └── DeathPrefab                    # Hull-specific explosion
├── HullSection_B (Child)              # Secondary hull (IsPrimary = false)
│   ├── HullSection.cs                 # Independent hull component
│   ├── AudioSource                    # Sound effects for this hull
│   └── [Same components as above]     # Independent health/destruction
└── HullSection_C (Child)              # Secondary hull (IsPrimary = false)
    └── [Same components as above]     # Independent health/destruction
```

### 2️⃣ **Instantiation Flow**
```csharp
// ChainCruiser prefab loaded in battle scene
ChainCruiser cruiser = Instantiate(chainCruiserPrefab);

// 1. StaticInitialise() - Early setup
cruiser.StaticInitialise();
├── base.StaticInitialise()           // Cruiser base initialization
├── Find primary hull                 // Cache _primaryHull reference
└── Set maxHealth from primary        // UI compatibility

// 2. Initialise() - Full setup
await cruiser.Initialise(args);
├── await base.Initialise(args)       // Full Cruiser initialization
├── Initialize hull sections          // Call Initialize() on each hull
├── Subscribe to destruction events   // Track hull deaths
└── Ready for battle
```

### 3️⃣ **Combat Flow**
```csharp
// Player targets HullSection_A
HullSection_A.TakeDamage(damage, source);
├── HullSection_A._healthTracker.RemoveHealth(damage)
├── Trigger Damaged event
└── If health ≤ 0: Destroy sequence

// HullSection_A dies (IsPrimary = true)
HullSection_A.OnHealthGone()
├── Spawn DeathPrefab explosion
├── Hide sprite/collider
└── ParentCruiser.OnHullSectionDestroyed(this)

// ChainCruiser handles primary death
OnHullSectionDestroyed(HullSection_A)
├── cruiser.Destroy()                 // Trigger inherited death flow
└── Game victory for player

// HullSection_B dies (IsPrimary = false)
OnHullSectionDestroyed(HullSection_B)
├── SecondaryHullDestroyed?.Invoke() // Notify listeners
├── Add destruction score
└── Battle continues (no victory)
```

### 4️⃣ **Targeting Integration**
```csharp
// GlobalTargetFinder emits targets
EmitCruiserAsGlobalTarget()
├── InvokeTargetFoundEvent(cruiser)   // Primary cruiser target
└── foreach hull in HullSections:     // Individual hull targets
    InvokeTargetFoundEvent(hull)

// When hull dies
OnSecondaryHullDestroyed()
└── InvokeTargetLostEvent(destroyedHull) // Remove from targeting
```

---

## Designer Workflow

### 🚀 **Quick Start: Create ChainCruiser Multi-Hull Boss**

**Goal**: Create a boss with multiple independently targetable hull sections.

#### Step 1: Set Up Base Cruiser
1. **Create GameObject**: Name it `ChainCruiser_Boss`
2. **Add ChainCruiser Component**: Component → ChainCruiser
3. **Add Animator Component**: Component → Animator (for boss animations/effects)
4. **Configure Basic Settings**:
   - `stringKeyBase`: "ChainBoss"
   - `numOfDrones`: 6
   - `hullType`: Cruiser
   - `startsWithFogOfWar`: true

#### Step 2: Create Hull Sections
1. **Create Child GameObject**: Name it `PrimaryHull`
2. **Add HullSection Component**: Component → HullSection
3. **Configure Primary Hull**:
   - `HullId`: "Primary"
   - `IsPrimary`: true (⚠️ Only ONE hull can be primary)
   - `maxHealth`: 3000 (boss health pool)
   - `healthGainPerDroneS`: 1.0
   - Assign `SpriteRenderer` (boss main sprite)
   - Assign `PrimaryCollider` (targeting area)

4. **Create Secondary Hulls**:
   - **Hull 2**: Name `LeftWing`, `IsPrimary`: false, `maxHealth`: 1500
   - **Hull 3**: Name `RightWing`, `IsPrimary`: false, `maxHealth`: 1500
   - **Hull 4**: Name `EngineSection`, `IsPrimary`: false, `maxHealth`: 1000
     - **Add Audio Source**: Component → Audio → Audio Source (for sound effects)

#### Step 3: Configure Death Effects
**For Each Hull Section**:
1. **Death Explosion**: Assign unique explosion prefab

#### Step 4: Set Up Building Slots (Optional)
**Primary Hull** gets main slots:
- Add `SlotWrapperController` to PrimaryHull
- Configure slots for main weapons/defenses

**Secondary Hulls** can have their own slots:
- Add `SlotWrapperController` to wing hulls
- Configure secondary weapons/defenses

#### Step 5: Assign Hull Array
1. **Select Root** `ChainCruiser_Boss`
2. **In ChainCruiser component**:
   - Drag all hull GameObjects into `HullSections` array
   - Order: Primary first, then secondaries

#### Step 6: Configure Boosts (Optional)
**In ChainCruiser component**:
- `Boosts` array for stat modifications
- Example: Damage +25%, Health +50%

#### Step 7: Save as Prefab
1. **Drag** `ChainCruiser_Boss` to `Assets/Resources/Cruisers/`
2. **Name**: `ChainCruiser_Boss.prefab`

#### Step 8: Test Multi-Hull Combat
1. **Load Test Scene** with ChainCruiser
2. **Verify Each Hull**:
   - Can be independently targeted
   - Shows separate health bars
   - Has unique death effects
   - Secondary destruction doesn't end battle
   - Primary destruction triggers victory

---

## Hull Section Configuration

### 🎯 **HullSection Component Properties**

**Identity Settings**:
- `HullId`: Unique string identifier ("Primary", "LeftWing", "Engine")
- `IsPrimary`: ⚠️ Only ONE hull can be true - triggers victory when destroyed
- `ParentCruiser`: Auto-assigned reference to ChainCruiser

**Health Configuration**:
- `maxHealth`: Independent health pool for this hull
- `healthGainPerDroneS`: Repair rate when drones are assigned

**Visual Components**:
- `SpriteRenderer`: Visual representation of this hull section
- `PrimaryCollider`: PolygonCollider2D for targeting and collision

**Death Effects**:
- `DeathPrefab`: Explosion GameObject spawned on destruction

**Optional Features**:
- `SlotController`: SlotWrapperController for building slots on this hull

### 🏗️ **Building Slots on Hull Sections**

**Primary Hull Slots** (Recommended):
- Main battle stations, heavy weapons
- Anti-ship turrets, missile launchers
- Shield generators, repair facilities

**Secondary Hull Slots** (Optional):
- Point defense, flak turrets
- Light weapons, sensor arrays
- Auxiliary systems

**Slot Configuration**:
1. Add `SlotWrapperController` to hull GameObject
2. Configure slot positions relative to hull
3. Slots inherit ChainCruiser's faction and targeting

### 💪 **Stat Boosts**

**ChainCruiser-Level Boosts**:
- Apply to entire cruiser (all hulls)
- Configured in root ChainCruiser component
- `Boosts[]` array with BoostType and BoostAmount

**Available Boost Types**:
- `MaxHealth`: Increases health capacity
- `Damage`: Multiplies weapon damage
- `BuildSpeed`: Construction speed
- `Armor`: Damage reduction
- `Shield`: Shield effectiveness

### 🚀 **Death Sequence Mechanics**

**Individual Hull Death**:
1. Health reaches 0
2. Spawn `DeathPrefab` explosion
3. Hide sprite and disable collider
4. Notify `ParentCruiser.OnHullSectionDestroyed()`

**Primary Hull Death** → **Victory**:
- Triggers standard cruiser destruction
- GameEndMonitor detects victory
- Standard victory screen and rewards

**Secondary Hull Death** → **Continued Battle**:
- `SecondaryHullDestroyed` event fired
- Partial score awarded
- Remaining hulls continue fighting

---

## Hull Section Setup Guide

### 🎨 **Creating Multi-Hull Visual Design**

#### Basic Hull Section Setup
1. **Import Sprites**: Prepare individual hull sprites
2. **Create Hull GameObjects**: Child objects under ChainCruiser root
3. **Add SpriteRenderer**: Assign unique sprite to each hull
4. **Position Hulls**: Arrange for multi-section appearance
   - Primary hull: Center/most important
   - Secondary hulls: Wings, engines, auxiliary sections

#### Collider Configuration
**For Each Hull Section**:
1. **Add PolygonCollider2D**: Component → Polygon Collider 2D
2. **Edit Shape**: Click "Edit Collider" and trace hull outline
3. **Assign to HullSection**: Drag collider to `PrimaryCollider` field
4. **Test Targeting**: Ensure each hull is independently targetable

#### Visual Hierarchy Example
```
ChainCruiser_Boss
├── PrimaryHull (center, largest)
│   ├── SpriteRenderer: main_hull_sprite
│   └── PolygonCollider2D: main targeting area
├── LeftWing (left side, medium)
│   ├── SpriteRenderer: wing_sprite
│   └── PolygonCollider2D: wing targeting area
├── RightWing (right side, medium)
│   ├── SpriteRenderer: wing_sprite (flipped)
│   └── PolygonCollider2D: wing targeting area
└── Engine (rear, small)
    ├── SpriteRenderer: engine_sprite
    └── PolygonCollider2D: engine targeting area
```

#### Death Effect Configuration
**Explosion Effects**:
- Primary hull: Large explosion, screen shake
- Secondary hulls: Medium explosions, hull-specific


---

## Best Practices

### 🎯 **Design Philosophy**
- **Multi-Hull Identity**: One cruiser with multiple targetable sections
- **Independent Destruction**: Each hull dies separately with unique effects
- **Primary/Secondary Logic**: Clear victory condition (primary death)
- **Inherited Reliability**: Full Cruiser compatibility and features

### 🏗️ **Prefab Organization**
- **Naming**: `ChainCruiser_[BossName].prefab`
- **Location**: `Assets/Resources/Cruisers/` or `Assets/Prefabs/Cruisers/`
- **Structure**: Root ChainCruiser → Multiple HullSection children
- **Hierarchy**: Primary hull first in array, secondaries follow

### 🎨 **Hull Design Guidelines**
- **Visual Distinction**: Each hull should look unique and targetable
- **Health Balance**: Primary hull strongest, secondaries progressively weaker
- **Death Spectacle**: Unique explosions per hull
- **Slot Distribution**: Primary gets heavy weapons, secondaries get support

### 🧪 **Testing Workflow**
1. **Individual Hull Testing**:
   - Target each hull separately
   - Verify independent health bars
   - Test unique death sequences
   - Confirm death effects trigger
2. **Full Combat Testing**:
   - Destroy secondary hulls first
   - Verify battle continues
   - Destroy primary hull
   - Confirm victory triggers
3. **Edge Cases**:
   - Rapid-fire damage to multiple hulls
   - Targeting after hull destruction
   - Building destruction on hull sections
   - Simultaneous hull deaths

### 📝 **Debugging Tips**
- **Console Logs**: ChainCruiser logs hull destruction and primary death
- **Inspector Debug**: Check each hull's IsDestroyed and current health
- **Targeting Debug**: Verify GlobalTargetFinder emits/removes hull targets
- **Hierarchy View**: Watch hull GameObjects hide/show during destruction

---

## Example ChainCruiser Configurations

### Example 1: Classic Winged Battleship
```
ChainCruiser_Battleship.prefab
├── ChainCruiser (root)
│   ├── HullSections[3]: PrimaryHull, LeftWing, RightWing
│   ├── Boosts: Damage +25%, Health +50%
│   └── numOfDrones: 6
├── PrimaryHull (IsPrimary=true, maxHealth=3000)
│   ├── Heavy weapons, main shields
│   ├── DeathPrefab: massive explosion
│   └── SlotWrapperController: 6 slots
├── LeftWing (IsPrimary=false, maxHealth=1200)
│   ├── Light weapons, point defense
│   └── SlotWrapperController: 2 slots
└── RightWing (IsPrimary=false, maxHealth=1200)
    ├── Light weapons, point defense
    └── SlotWrapperController: 2 slots
```

### Example 2: Multi-Engine Destroyer
```
ChainCruiser_Destroyer.prefab
├── ChainCruiser (root)
│   ├── HullSections[4]: MainHull, Engine1, Engine2, Bridge
│   ├── Boosts: BuildSpeed +100%
│   └── numOfDrones: 4
├── MainHull (IsPrimary=true, maxHealth=2500)
│   ├── Main battery, command center
│   ├── DeathPrefab: bridge explosion
│   └── SlotWrapperController: 4 slots
├── Engine1 (IsPrimary=false, maxHealth=800)
│   ├── Propulsion system, vulnerable
│   └── No building slots
├── Engine2 (IsPrimary=false, maxHealth=800)
│   ├── Propulsion system, vulnerable
│   └── No building slots
└── Bridge (IsPrimary=false, maxHealth=600)
    ├── Command and control
    └── SlotWrapperController: 1 slot
```

### Example 3: Carrier Battle Group
```
ChainCruiser_Carrier.prefab (Final Boss)
├── ChainCruiser (root)
│   ├── HullSections[5]: FlightDeck, Island, EngineBlock, PortHull, StarboardHull
│   ├── Boosts: MaxHealth +200%, Armor +50%
│   └── numOfDrones: 8
├── FlightDeck (IsPrimary=true, maxHealth=4000)
│   ├── Aircraft operations center
│   ├── DeathPrefab: catastrophic explosion
│   └── SlotWrapperController: AirFactory + defenses
├── Island (IsPrimary=false, maxHealth=1000)
│   ├── Command superstructure
│   └── SlotWrapperController: Radar, CIWS
├── EngineBlock (IsPrimary=false, maxHealth=1500)
│   ├── Propulsion and power
│   └── No building slots
├── PortHull (IsPrimary=false, maxHealth=1200)
│   ├── Port side armor and weapons
│   └── SlotWrapperController: 3 slots
└── StarboardHull (IsPrimary=false, maxHealth=1200)
    ├── Starboard side armor and weapons
    └── SlotWrapperController: 3 slots
```

---

## Technical Reference

### 🔧 **ChainCruiser.cs Key Methods**

```csharp
// Health routing to primary hull
public new float Health => _primaryHull?.Health ?? 0;
public new float MaxHealth => _primaryHull?.MaxHealth ?? maxHealth;
public new bool IsDestroyed => _primaryHull?.IsDestroyed ?? true;
public override bool IsAlive => _primaryHull != null && !_primaryHull.IsDestroyed;

// Hull destruction handling
public void OnHullSectionDestroyed(HullSection hull)
{
    if (hull.IsPrimary)
    {
        // Primary death = victory
        Destroy(); // Triggers inherited death flow
    }
    else
    {
        // Secondary death = continue battle
        SecondaryHullDestroyed?.Invoke(this, new HullSectionDestroyedEventArgs(hull));
    }
}

// Initialization sequence (bypasses SpriteRenderer requirement)
public override void StaticInitialise()
{
    // Find primary hull BEFORE component init (sets maxHealth)
    _primaryHull = HullSections.FirstOrDefault(h => h.IsPrimary);
    if (_primaryHull != null)
        maxHealth = _primaryHull.maxHealth;

    // Manual initialization - skips base.SpriteRenderer assertion
    // ChainCruiser gets sprite from hull sections, not root object
    // ... initialize SlotWrapperController, Fog, ClickHandler, etc. ...
}
```

### 🎯 **HullSection.cs Key Methods**

```csharp
// Independent health management
private HealthTracker _healthTracker;
public float MaxHealth => maxHealth;
public float Health => _healthTracker?.Health ?? maxHealth;
public bool IsDestroyed => _isDestroyed;

// Independent damage handling
public void TakeDamage(float damage, ITarget source, bool ignoreImmune = false)
{
    if (_isDestroyed) return;
    _lastDamagedSource = source;
    if (_healthTracker.RemoveHealth(damage))
        Damaged?.Invoke(this, new DamagedEventArgs(source));
}

// Death sequence
private void OnHealthGone(object sender, EventArgs e)
{
    _isDestroyed = true;
    // Spawn explosion
    Instantiate(DeathPrefab, transform.position, transform.rotation);

    // Hide this hull section
    HideHullSection();

    // Notify parent cruiser
    ParentCruiser?.OnHullSectionDestroyed(this);
}
```

### 📊 **GlobalTargetFinder.cs Integration**

```csharp
// Emits hull sections as targets
public void EmitCruiserAsGlobalTarget()
{
    InvokeTargetFoundEvent(_enemyCruiser);
    if (_enemyCruiser is ChainCruiser chainCruiser)
    {
        foreach (var hull in chainCruiser.HullSections)
        {
            if (hull != null && hull.PrimaryCollider != null && !hull.IsDestroyed)
                InvokeTargetFoundEvent(hull);
        }
    }
}

// Removes destroyed hulls from targeting
private void OnSecondaryHullDestroyed(object sender, HullSectionDestroyedEventArgs e)
{
    if (e.DestroyedHull != null)
        InvokeTargetLostEvent(e.DestroyedHull);
}
```

---

## Troubleshooting

### ❌ **Hull Not Targetable**
**Symptoms**: Can't click on hull section, no targeting reticle
**Fixes**:
- Verify `PolygonCollider2D` is attached and enabled
- Check collider shape matches sprite outline
- Ensure `ClickHandlerWrapper` component is present
- Confirm hull GameObject is active in hierarchy

### ❌ **Health Not Independent**
**Symptoms**: Damaging one hull affects all hulls' health
**Fixes**:
- Verify each hull has its own `HullSection` component
- Check `maxHealth` is set individually per hull
- Ensure hulls don't share `HealthTracker` instances
- Confirm `TakeDamage()` calls local `_healthTracker.RemoveHealth()`

### ❌ **Death Effects Don't Trigger**
**Symptoms**: Hull reaches 0 health but no explosion
**Fixes**:
- Verify `DeathPrefab` is assigned
- Check prefab exists in project and is not null
- Ensure `OnHealthGone` event handler is subscribed
- Confirm hull has `HealthTracker.HealthGone += OnHealthGone`

### ❌ **Primary Death Doesn't End Battle**
**Symptoms**: Primary hull destroyed but battle continues
**Fixes**:
- Verify exactly ONE hull has `IsPrimary = true`
- Check `OnHullSectionDestroyed` calls `Destroy()` for primary
- Ensure inherited `Cruiser.Destroy()` triggers victory
- Confirm `GameEndMonitor` detects cruiser destruction

### ❌ **Secondary Hulls Don't Award Points**
**Symptoms**: Secondary destruction doesn't give score
**Fixes**:
- Verify `SecondaryHullDestroyed` event is raised
- Check score calculation: `(int)(hull.maxHealth * 0.3f)`
- Ensure `Faction == Faction.Reds` for enemy scoring
- Confirm `BattleSceneGod.AddDeadBuildable()` is called


---

## Comparison: Multi-Hull vs Single Entity

### ✅ **Multi-Hull ChainCruiser (Current)**
**Pros**:
- Independent targeting: Each hull can be attacked separately
- Spectacular destruction: Unique death effects per hull section
- Strategic depth: Players choose which parts to destroy first
- Visual impact: Large boss with multiple vulnerable areas
- Inherited reliability: Full Cruiser compatibility and features

**Cons**:
- Complex setup: Multiple GameObjects and components
- Performance cost: More colliders and targeting calculations

### ❌ **Single Hull Cruiser (Traditional)**
**Pros**:
- Simple: One GameObject, straightforward setup
- Efficient: Single collider, single health system

**Cons**:
- Less engaging: Single target area, predictable combat
- Less spectacular: Single death explosion
- Limited strategy: All-or-nothing damage approach

**Decision**: Multi-hull approach provides superior gameplay depth and visual spectacle for boss encounters.

---

## Summary

**ChainCruiser System** provides a **multi-hull boss framework** with independent targeting and destruction:

✅ **Independent Hulls** - Each section can be targeted and destroyed separately
✅ **Primary/Secondary Logic** - Primary death triggers victory, secondaries continue battle
✅ **Spectacular Effects** - Unique explosions per hull
✅ **Full Inheritance** - Complete Cruiser compatibility and all existing features
✅ **Flexible Configuration** - Buildings, boosts, and effects per hull section
✅ **Production Ready** - Hybrid inheritance design, fully functional

**Current Status**: Implementation complete, ready for prefab creation and testing

**Recommended Workflow**:
1. Create ChainCruiser root with multiple HullSection children
2. Configure one primary hull, multiple secondary hulls
3. Set up unique death effects
4. Assign building slots and stat boosts
5. Test multi-hull combat and victory conditions

**Design Philosophy**: Combine the reliability of inheritance with the spectacle of multi-part destruction. Give players strategic choices while maintaining system simplicity.

---

**Last Updated**: 2026-01-04
**System Version**: 1.0 (Multi-Hull Hybrid Inheritance)
**Status**: Implementation Complete
**Compatibility**: Full Cruiser inheritance + hull extensions
