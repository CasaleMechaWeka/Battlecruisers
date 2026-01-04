# Multi-Section Cruiser System - Implementation Guide

---

## 📖 Context for LLMs and Future Developers

**System Overview**: This system enables a single `Cruiser` class to handle 1-N independent, targetable sections. Previously, multi-section cruisers required a specialized `ChainCruiser` subclass. This refactor inverted that architecture so the base `Cruiser` is flexible enough to handle any configuration.

**Key Files to Know**:
- **Cruiser.cs** (Assets/Scripts/Cruisers/) - Base class, handles both single and multi-section, includes DisplayMessage() for battle messaging
- **CruiserSection.cs** (Assets/Scripts/Cruisers/) - Individual targetable section component
- **CruiserFactory.cs** (Assets/Scripts/Cruisers/) - Handles automatic section detection and initialization
- **GlobalTargetFinder.cs** (Assets/Scripts/Targets/TargetFinders/) - Emits sections as targets
- **BattleSceneMessageDisplay.cs** (Assets/Scripts/Utils/Debugging/) - Singleton message display system for battle scene

**Core Design Pattern**:
1. **Single-section**: Cruiser root has SpriteRenderer + Collider2D, no CruiserSection children, `_hulls = null`
2. **Multi-section**: Cruiser root has NO renderer/collider, has 2+ CruiserSection children, `_hulls = array of sections`
3. **Automatic Setup**: CruiserFactory detects which pattern and initializes accordingly
4. **No Specialization**: Same code path handles both - determined entirely by prefab structure

**Critical Implementation Detail**: First CruiserSection child in hierarchy is automatically marked as primary. This is intentional and matters.

**Health & UI**:
- Cruiser.Health property always returns primary section's health (for UI display)
- But each section tracks its own health independently
- Primary section destruction = cruiser destroyed = game over
- Secondary section destruction = continues battle + score awarded

---

## 📋 Version History & Changes

<<<<<<< HEAD
### Version 3.2 - Message Display System (January 4, 2026)
**Files Modified**: BattleSceneMessageDisplay.cs, Cruiser.cs, ChainBattlePlan.md
- **Added**: Singleton instance to BattleSceneMessageDisplay for automatic discovery
- **Added**: DisplayMessage() method to Cruiser class for easy message display during battle
- **Feature**: Automatic battle scene message display without manual variable assignment
- **What we learned**: Messages can be displayed from any Cruiser during battle by simply calling DisplayMessage(). The system automatically finds the message display singleton. Graceful fallback to Debug.Log if display not available.
=======
### Version 3.4 - Debugging and Diagnostics (January 4, 2026)
**Commits: 1c422ac, fe10a01, 41697dd, 92e7ade, 82036d4**
- **Added**: Comprehensive debug logging to building positioning and selection
- **Fixed**: BuildingPlacementPoint is now dynamic (returns current world position, not fixed)
- **Fixed**: IsVisible null check prevents NullReferenceException
- **Fixed**: LandingSceneGod null check prevents scene initialization crash
- **Simplified**: Removed message display system from BattleSequencer (was untested feature)
- **What we learned**: SlotWrapper configuration is critical - single parent works best for multi-section. BuildingPlacementPoint must be dynamic to track moving slots. Extensive debug logging helps diagnose positioning offsets.

### Version 3.3 - Building Selection Debugging (January 4, 2026)
- **Added**: Debug logs for building positioning calculations
- **Added**: Debug logs for click detection chain (slot vs building)
- **Added**: Debug logs for UIManager selection calls
- **Issue**: Buildings appear at wrong world positions and cannot be selected

### Version 3.2 - Message Display (January 4, 2026 - REVERTED)
- **Added**: Message display system (reverted - was untested and complex)
- **Lesson**: Focus on core functionality first, avoid experimental features when core is unstable
>>>>>>> 2a35a3ef08 (Update ChainBattlePlan.md with troubleshooting guide and debug logs)

### Version 3.1 - Code Cleanup (January 3, 2026)
**Commit: 2f5a9b1**
- **Removed**: Dead code `CruiserSection.SlotController` field and assignment
- **Clarified**: Cruiser.deathPrefab IS USED (instantiated by CruiserDeathManager, not dead code)
- **What we learned**: Only CruiserSection.SlotController was actually unused. Cruiser.deathPrefab is essential for single-section cruisers and for the final cruiser death explosion in multi-section cruisers.

### Version 3.0 - Final Fixes (January 3, 2026)
**Commit: 7a8c191**
- **Fixed**: Removed unused `deathPrefab` assertion from StaticInitialise
- **Fixed**: Made Collider2D assertion optional (multi-section cruisers have colliders on children)
- **What we learned**: SpriteRenderer and Collider2D are only required on root for single-section; multi-section has them on CruiserSection children

### Version 2.0 - Event Args Renaming (January 2, 2026)
**Commits: c403a65, 702efa2**
- **Changed**: Renamed HullSectionDestroyedEventArgs → CruiserSectionDestroyedEventArgs
- **Changed**: Renamed HullSectionTargetedEventArgs → CruiserSectionTargetedEventArgs
- **Changed**: Properties renamed (DestroyedHull → DestroyedSection, Hull → Section)
- **Why**: These are NEW features (not legacy), so should follow the CruiserSection naming convention
- **Updated**: All references in Cruiser.cs and GlobalTargetFinder.cs

### Version 1.0 - Architecture Inversion (January 1-2, 2026)
**Commits: 859dbc9, cb26e2d, d51fa54, 49e41f7, ad0d26a, e88e9f7, f1edab5**
- **Deleted**: ChainCruiser class entirely
- **Inverted**: Architecture from inheritance-based to composition-based (Cruiser + CruiserSection array)
- **Added**: Automatic section detection in CruiserFactory
- **Renamed**: Hull → CruiserSection throughout codebase
- **Updated**: Documentation with new implementation guide
- **Made flexible**: SpriteRenderer optional on root (only required for single-section)

### Known Issues / Resolved
- ✅ Collider2D assertion was breaking multi-section (FIXED - made optional)
- ✅ DeathPrefab confusion - Cruiser's IS USED (by CruiserDeathManager, not dead code)
- ✅ CruiserSection.SlotController was dead code (REMOVED)
- ✅ AdditionalColliders only apply single-section (SAFE - early return in multi-section)
- ✅ SlotWrapperController found via GetComponentsInChildren (SAFE - uses root's)
- ✅ MaxHealth syncing via SetupHulls (SAFE - synchronized at initialization)

---

## 🎯 Executive Summary

**Status**: ✅ Complete and Production Ready
**Architecture**: Flexible Single-Class System (Cruiser natively supports 1-N sections)
**Date**: January 2026

### The Simple Idea
**Cruiser = Section(s)**

One `Cruiser` class handles both single-section and multi-section configurations. Prefabs determine complexity:
- **Single-section cruiser**: No CruiserSection children (renderer on Cruiser object)
- **Multi-section cruiser**: 2+ CruiserSection children (one renderer per section)
- **Same code path** - no specialization, **automatic initialization**

### What Was Done
Inverted the architecture from "ChainCruiser extends Cruiser with special logic" to "Cruiser natively supports any section count."

**Result**:
- ❌ Deleted ChainCruiser entirely (no longer needed)
- ✅ Merged all multi-section logic into base Cruiser
- ✅ CruiserSection[] array in base class
- ✅ Property routing works for 1-N sections
- ✅ Secondary section destruction scoring built-in
- ✅ **Automatic section detection and initialization in CruiserFactory**

---

## 🏗️ Architecture Overview

### One Class, Multiple Configurations

```
┌─────────────────────────────────────┐
│          Cruiser                    │
├─────────────────────────────────────┤
│                                     │
│  CruiserSection[] _hulls            │
│  ├─ Properties route through [0]    │
│  ├─ Events for section destruction  │
│  └─ Secondary section scoring logic │
│                                     │
│  Virtual section callbacks:         │
│  ├─ OnHullClicked(section)          │
│  ├─ OnHullDestroyed(section)        │
│  └─ SetupHulls(section[])           │
│                                     │
└─────────────────────────────────────┘
         ▲
         │
    Configuration via Prefab:

    Single-Section:     Multi-Section:
    ┌──────────┐        ┌──────────┐
    │ Cruiser  │        │ Cruiser  │
    ├──────────┤        ├──────────┤
    │Renderer* │        │Section[3]│
    │Collider* │        │- Primary │
    │          │        │- Wing L  │
    │          │        │- Wing R  │
    └──────────┘        └──────────┘
    * No children       * Has children
```

### Key Classes

**Cruiser.cs** (1 class, handles everything):
```csharp
public class Cruiser : Target, ICruiser
{
    // Section array for 1-N sections
    protected CruiserSection[] _hulls;
    public CruiserSection[] Hulls => _hulls;  // Public accessor

    // Properties automatically route through primary section
    public float Health => _hulls?[0]?.Health ?? base.Health;
    public float MaxHealth => _hulls?[0]?.MaxHealth ?? maxHealth;
    public bool IsDestroyed => _hulls?[0]?.IsDestroyed ?? false;

    // Virtual methods for section events
    public virtual void OnHullDestroyed(CruiserSection section)
    {
        if (section.IsPrimary)
            Destroy();  // Primary = game over
        else if (_hulls?.Length > 1)
        {
            SecondaryHullDestroyed?.Invoke(this, new CruiserSectionDestroyedEventArgs(section));
            BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(section.maxHealth * 0.3f));
        }
    }

    public virtual void SetupHulls(CruiserSection[] sections)
    {
        _hulls = sections;
        if (_hulls?[0] != null)
            maxHealth = _hulls[0].maxHealth;
    }
}
```

**CruiserSection.cs** (individual targetable section component):
```csharp
public class CruiserSection : MonoBehaviour, ITarget
{
    public Cruiser ParentCruiser;      // Reference to parent cruiser
    public string HullId;               // Unique identifier
    public bool IsPrimary;              // Only one should be true
    public float maxHealth = 1000f;
    public SpriteRenderer SpriteRenderer;   // For rendering
    public PolygonCollider2D PrimaryCollider; // For targeting

    // Notifies parent when destroyed
    private void OnHealthGone(object sender, EventArgs e)
    {
        ParentCruiser?.OnHullDestroyed(this);
    }
}
```

---

## 🎨 Comprehensive Unity Hierarchy & Component Setup Guide

### SINGLE-SECTION CRUISER HIERARCHY

```
PlayerCruiser (GameObject)
├── Components:
│   ├── Cruiser (script)
│   ├── SpriteRenderer
│   ├── PolygonCollider2D
│   ├── ClickHandlerWrapper
│   ├── FogOfWar
│   └── (Other components)
├── Child: SlotWrapperController
│   ├── Slot[0]
│   ├── Slot[1]
│   └── ...
└── Child: PersistentObjects (if any)
```

**Single-Section Setup**:
- Cruiser component on root
- SpriteRenderer on root (renderer for display)
- PolygonCollider2D on root (hitbox for damage)
- NO CruiserSection children
- _hulls = null (system detects single-section mode)

---

### MULTI-SECTION CRUISER HIERARCHY

```
EnemyBoss (GameObject)
├── Components:
│   ├── Cruiser (script) - NO SpriteRenderer on root!
│   ├── ClickHandlerWrapper
│   ├── FogOfWar
│   ├── (Other shared components)
│   └── [NO PolygonCollider2D on root!]
│
├── Child[0]: PrimarySection (MUST be first child!)
│   ├── Components:
│   │   ├── CruiserSection (script)
│   │   ├── SpriteRenderer (assign sprite)
│   │   └── PolygonCollider2D (trace outline)
│   └── CruiserSection Inspector:
│       ├── HullId: "Primary"
│       ├── IsPrimary: TRUE ⚠️
│       ├── maxHealth: 3000
│       ├── healthGainPerDroneS: 1.0
│       ├── SpriteRenderer: [drag the SpriteRenderer]
│       ├── PrimaryCollider: [drag the PolygonCollider2D]
│       └── DeathPrefab: [explosion effect prefab]
│
├── Child[1]: LeftWing
│   ├── Components:
│   │   ├── CruiserSection (script)
│   │   ├── SpriteRenderer (assign sprite)
│   │   └── PolygonCollider2D (trace outline)
│   └── CruiserSection Inspector:
│       ├── HullId: "LeftWing"
│       ├── IsPrimary: FALSE
│       ├── maxHealth: 1500
│       ├── SpriteRenderer: [drag the SpriteRenderer]
│       ├── PrimaryCollider: [drag the PolygonCollider2D]
│       └── DeathPrefab: [explosion effect prefab]
│
├── Child[2]: RightWing
│   ├── Components:
│   │   ├── CruiserSection (script)
│   │   ├── SpriteRenderer (assign sprite)
│   │   └── PolygonCollider2D (trace outline)
│   └── CruiserSection Inspector:
│       ├── HullId: "RightWing"
│       ├── IsPrimary: FALSE
│       ├── maxHealth: 1500
│       ├── SpriteRenderer: [drag the SpriteRenderer]
│       ├── PrimaryCollider: [drag the PolygonCollider2D]
│       └── DeathPrefab: [explosion effect prefab]
│
└── Child: SlotWrapperController (found by GetComponentsInChildren)
    ├── Slot[0]
    └── ...
```

**Multi-Section Setup**:
- Cruiser component on root (NO renderer/collider!)
- CruiserSection children (one per section)
- First child automatically becomes primary
- Each section has own SpriteRenderer + PolygonCollider2D
- Each section has own CruiserSection component with maxHealth
- _hulls = populated automatically by CruiserFactory
- System detects multi-section mode and handles initialization

---

### COMPONENT CONFIGURATION CHECKLIST

#### Root Cruiser Component
```
✅ stringKeyBase: [unique name, e.g., "Boss", "Trident"]
✅ numOfDrones: [4-8, depends on design]
✅ hullType: HullType.Cruiser (or appropriate type)
✅ yAdjustmentInM: [vertical offset for UI, e.g., 5]
✅ trashTalkScreenPosition: [screen coords for dialog]
✅ startsWithFogOfWar: [true/false]
✅ persistentObjects: [any objects to keep after destruction]
✅ deathPrefab: [REQUIRED - used by CruiserDeathManager for final death explosion]
❌ useAdditionalColliders: [only for single-section complex shapes, multi-section ignores]

For Single-Section Only:
✅ SpriteRenderer: [renderer on same object]
✅ PolygonCollider2D: [collider on same object]
```

#### Each CruiserSection Component
```
✅ HullId: [unique identifier, e.g., "Primary", "LeftWing"]
✅ IsPrimary: [TRUE only for first child, FALSE for others]
✅ maxHealth: [health pool for this section, e.g., 3000]
✅ healthGainPerDroneS: [repair rate, e.g., 1.0]
✅ SpriteRenderer: [DRAG the SpriteRenderer component on same object]
✅ PrimaryCollider: [DRAG the PolygonCollider2D component]
✅ DeathPrefab: [explosion effect that spawns when destroyed]
```

---

### INITIALIZATION FLOW (WHAT HAPPENS AUTOMATICALLY)

```
1. BattleSceneGod.Start()
   → CruiserFactory.CreateAICruiser(key)

2. PrefabFactory.CreateCruiser()
   → Instantiate(cruiserPrefab)
   → cruiser.StaticInitialise()
      ├─ Finds SpriteRenderer (optional, only if exists)
      ├─ Finds Collider2D (optional for multi-section)
      └─ Validates additional colliders if enabled

3. BattleSceneGod sets up camera, UI

4. CruiserFactory.InitialisePlayerCruiser()
   → cruiser.Initialise(args)
      ├─ Sets faction, drones, repair, UI, etc.
      └─ Returns

5. CruiserFactory.InitialiseCruiser() [NEW AUTOMATIC PART]
   → CruiserSection[] sections = GetComponentsInChildren<CruiserSection>()
   ├─ if (sections.Length > 0):
   │  ├─ cruiser.SetupHulls(sections)
   │  │  └─ _hulls = sections array
   │  │  └─ maxHealth = sections[0].maxHealth
   │  └─ for each section:
   │     ├─ section.ParentCruiser = cruiser
   │     ├─ if (i == 0) section.IsPrimary = true
   │     └─ section.Initialize()
   │        ├─ Creates health tracker
   │        ├─ Subscribes to health events
   │        └─ Sets up click handlers
   └─ if (no sections):
      └─ Single-section mode, _hulls stays null

6. GlobalTargetFinder detects multi-section
   → Subscribes to SecondaryHullDestroyed event
   → Will emit all sections as targets

7. Battle ready!
```

---

## 🎮 How to Build a Multi-Section Level


### Key Concept: Automatic Detection
When a Cruiser is instantiated, **CruiserFactory automatically detects and initializes any CruiserSection children**. No manual array assignment needed.

### Step 1: Create Base Cruiser Prefab

```
In Unity:
1. Create empty GameObject: "EnemyBoss"
2. Add Component: Cruiser
3. Configure Cruiser component:
   ├─ stringKeyBase: "Boss"
   ├─ numOfDrones: 6
   ├─ hullType: Cruiser
   └─ [other settings]
```

### Step 2: Create CruiserSection Children

For each section you want (primary + secondaries):

```
EnemyBoss/
├─ PrimarySection (Child GameObject)
│  ├─ Add Component: CruiserSection
│  ├─ Add Component: SpriteRenderer (assign sprite)
│  ├─ Add Component: PolygonCollider2D (trace outline)
│  └─ Configure CruiserSection component:
│     ├─ HullId: "Primary"
│     ├─ IsPrimary: TRUE ⚠️ (MUST be first child!)
│     ├─ maxHealth: 3000
│     ├─ SpriteRenderer: [drag the SpriteRenderer]
│     ├─ PrimaryCollider: [drag the PolygonCollider2D]
│     └─ DeathPrefab: [explosion effect]
│
├─ LeftWing (Child GameObject)
│  ├─ Add Component: CruiserSection
│  ├─ Add Component: SpriteRenderer
│  ├─ Add Component: PolygonCollider2D
│  └─ Configure CruiserSection component:
│     ├─ HullId: "LeftWing"
│     ├─ IsPrimary: FALSE
│     ├─ maxHealth: 1500
│     └─ [assign components as above]
│
└─ RightWing (Child GameObject)
   ├─ Add Component: CruiserSection
   ├─ [same configuration as LeftWing]
```

### Step 3: Hierarchy Order Matters ⚠️

**The first CruiserSection child MUST be the primary section**. When CruiserFactory initializes:
```csharp
// First child found → marked IsPrimary = true
// Remaining children → marked IsPrimary = false
```

So arrange your hierarchy carefully:
```
Good:    Bad:
├─ Primary      ├─ LeftWing ← Will be marked Primary!
├─ LeftWing     ├─ RightWing
└─ RightWing    └─ Primary

✅ Good         ❌ Wrong
```

### Step 4: Save as Prefab

```
1. Drag "EnemyBoss" to: Assets/Resources/Cruisers/
2. Name: "Boss_MultiSection.prefab" (or your boss name)
```

### Step 5: Use in Level - It Just Works!

```csharp
// In BattleSceneGod or level config:
IPrefabKey bossKey = new HullKey("Boss_MultiSection");
Cruiser boss = cruiserFactory.CreateAICruiser(bossKey);

// At this point, CruiserFactory has ALREADY:
// ✅ Found all CruiserSection children
// ✅ Called boss.SetupHulls(sections)
// ✅ Initialized each section with ParentCruiser reference
// ✅ Marked first as primary
// ✅ Called section.Initialize() on each

// Battle is ready to go!
```

---

## 🧪 Testing Multi-Section Combat

```
1. Load level with your multi-section boss
2. Target each section individually:
   - Click Primary: Should select boss, show primary health
   - Click LeftWing: Should select boss, still show primary health
   - Click RightWing: Should select boss, still show primary health
3. Damage secondary section:
   - Attack LeftWing for ~1500 damage
   - LeftWing should hide/explode (DeathPrefab spawned)
   - Battle continues
   - Score awarded (~450 points from maxHealth * 0.3f)
4. Damage primary section:
   - Attack Primary for ~3000 damage
   - Primary hides/explodes
   - GAME OVER/VICTORY (primary destroyed = cruiser destroyed)
```

---

## 🎨 Example Boss Configurations

### Simple 3-Section Boss
```
Primary (HP: 3000, IsPrimary: true)   ← First child in hierarchy
LeftWing (HP: 1500, IsPrimary: false)
RightWing (HP: 1500, IsPrimary: false)

Total perceived health: 3000 (primary shown in UI)
Secondary destruction: Continues battle + awards points (450+450)
```

### Complex 5-Section Boss
```
Main (HP: 4000, IsPrimary: true)      ← First child
Engine1 (HP: 1200, IsPrimary: false)
Engine2 (HP: 1200, IsPrimary: false)
CannonLeft (HP: 800, IsPrimary: false)
CannonRight (HP: 800, IsPrimary: false)

Total: 8000 possible damage, but game ends at 4000 (primary destroyed)
Secondary destruction awards: 360 + 360 + 240 + 240 = 1200 points
```

---

## 🔧 Technical Details

### How It All Works

#### Initialization Flow (CruiserFactory - Automatic)
```
1. BattleSceneGod calls: cruiserFactory.CreateAICruiser(aiCruiserKey)
2. PrefabFactory instantiates prefab: new Cruiser()
3. PrefabFactory calls: cruiser.StaticInitialise()
   ├─ Finds SpriteRenderer (optional - only if on same object)
   └─ Finds Collider2D

4. BattleSceneGod calls: cruiserFactory.InitialisePlayerCruiser()
5. CruiserFactory calls: cruiser.Initialise(args)
   ├─ Sets faction, UI manager, drones, repair, etc.
   └─ No hull setup yet

6. CruiserFactory (NEW!) Automatically Detects Sections:
   ├─ GetComponentsInChildren<CruiserSection>()
   ├─ if (sections.Length > 0):
   │  ├─ cruiser.SetupHulls(sections)  ← Populates _hulls array
   │  └─ for each section:
   │     ├─ section.ParentCruiser = cruiser
   │     ├─ if (first) section.IsPrimary = true
   │     └─ section.Initialize()  ← Sets up health tracking, click handlers
   └─ if (no sections) → Single-section mode, _hulls stays null

7. Battle ready!
```

#### When a Section Takes Damage
```
1. Player clicks a section (e.g., LeftWing)
2. GlobalTargetFinder emits it as a target (via Hulls array)
3. Weapon fires on LeftWing
4. LeftWing.TakeDamage(100) called
5. LeftWing._healthTracker.RemoveHealth(100)
6. LeftWing health: 1500 → 1400
7. UI displays cruiser.Health (routes to primary: still 3000)
```

#### When Secondary Section Dies
```
1. LeftWing.TakeDamage(1500) [remaining health]
2. LeftWing._healthTracker reaches 0
3. CruiserSection.OnHealthGone() triggered
4. Spawn LeftWing.DeathPrefab (explosion)
5. Call ParentCruiser.OnHullDestroyed(leftWing)
6. Cruiser.OnHullDestroyed(leftWing) checks:
   ├─ if (leftWing.IsPrimary) → Destroy() → GAME OVER
   └─ else →
      ├─ SecondaryHullDestroyed event fires
      ├─ BattleSceneGod.AddDeadBuildable() → Score += 450
      └─ Battle continues with remaining sections
```

#### When Primary Section Dies
```
1. Primary.TakeDamage(3000) [remaining health]
2. Primary._healthTracker reaches 0
3. CruiserSection.OnHealthGone() triggered
4. Spawn Primary.DeathPrefab (explosion)
5. Call ParentCruiser.OnHullDestroyed(primary)
6. Cruiser.OnHullDestroyed(primary) checks:
   └─ if (primary.IsPrimary) → Destroy() → VICTORY!
```

### Properties Always Route Through Primary Section

```csharp
// These all return primary section values (for UI display)
boss.Health           // ← Primary section health (shown in UI)
boss.MaxHealth        // ← Primary section max health
boss.IsDestroyed      // ← Primary section destroyed? (→ game over)
boss.IsAlive          // ← Primary section alive?
boss.Size             // ← Primary section collider bounds

// But each section tracks its own health independently!
boss.Hulls[0].Health  // ← Primary section's actual health
boss.Hulls[1].Health  // ← LeftWing section's actual health
boss.Hulls[2].Health  // ← RightWing section's actual health

// These apply to ALL sections
boss.Color = red         // ← All sections turn red
boss.MakeInvincible()    // ← All sections become invincible
boss.MakeDamagable()     // ← All sections become damageable
```

---

## 📋 Checklist for Creating Multi-Section Boss

- [ ] Create Cruiser GameObject (root)
- [ ] Add Cruiser component to root
- [ ] Configure Cruiser: stringKeyBase, numOfDrones, etc.
- [ ] Create CruiserSection children (at least 1 primary, 1+ secondary)
  - [ ] **First child MUST be Primary** (CruiserFactory marks it)
- [ ] For each CruiserSection child:
  - [ ] Add CruiserSection component
  - [ ] Set HullId (unique name)
  - [ ] Set maxHealth
  - [ ] Add SpriteRenderer (assign sprite)
  - [ ] Add PolygonCollider2D (trace outline)
  - [ ] In CruiserSection inspector, assign:
    - [ ] SpriteRenderer field → drag SpriteRenderer component
    - [ ] PrimaryCollider field → drag PolygonCollider2D component
    - [ ] DeathPrefab field → explosion effect prefab
  - [ ] Set IsPrimary (only first child!)
- [ ] **NO manual Hulls array assignment needed** (automatic detection)
- [ ] Save as prefab to Resources/Cruisers/
- [ ] Test in battle scene

---

## ❓ FAQ

**Q: Do I need to manually assign the Hulls array?**
A: **No!** CruiserFactory automatically detects CruiserSection children and calls SetupHulls. Just add children to the prefab.

**Q: Can I have more than 3 sections?**
A: Yes, unlimited. Add as many CruiserSection children as you want. Only the first becomes primary.

**Q: What if I have no CruiserSection children?**
A: That's fine! The cruiser works in single-section mode - renderer on the root object, no _hulls array.

**Q: Why is the FIRST child important?**
A: CruiserFactory marks the first child as IsPrimary. If ordered wrong, wrong section will be primary. This is intentional - hierarchy order matters.

**Q: Do secondary sections need SlotWrapperController?**
A: No. Only the root Cruiser needs SlotWrapperController. Sections just need SpriteRenderer and PolygonCollider2D.

**Q: Can I change which section is primary at runtime?**
A: Not recommended. IsPrimary is set during initialization based on hierarchy order. Changing it won't affect gameplay.

**Q: How does click targeting work?**
A: GlobalTargetFinder detects multi-section cruisers and emits each section as a separate target via the Hulls array. Clicking a section targets that specific section, but the UI still shows primary health.

**Q: What's the destruction score formula?**
A: `(int)(section.maxHealth * 0.3f)` - 30% of the section's max health as points. Only for secondary sections (primary = game over, no points).

**Q: Can I have sections with different health independently tracked?**
A: Yes! Each CruiserSection has its own maxHealth and health tracker. The Cruiser's Health property just displays the primary's value for UI purposes.

**Q: How do I display messages from a Cruiser during battle?**
A: Use the `DisplayMessage()` method. It automatically finds the battle scene message display:
```csharp
// From any Cruiser instance
cruiser.DisplayMessage("Taking damage!");
cruiser.DisplayMessage("Section destroyed!", BattleSceneMessageDisplay.MessageType.Warning);
cruiser.DisplayMessage("Critical hit!", BattleSceneMessageDisplay.MessageType.Error);
```
**Note**: Messages appear in the admin panel debug display (on-screen in battle). The system automatically finds the `BattleSceneMessageDisplay` singleton - no manual setup needed.

---

## Message Display System

### How It Works

The `DisplayMessage()` method on Cruiser provides a simple interface to the battle scene message display:

```csharp
public void DisplayMessage(string message,
    BattleSceneMessageDisplay.MessageType messageType = BattleSceneMessageDisplay.MessageType.Info)
```

**Features**:
- ✅ Automatic singleton discovery (no manual assignment)
- ✅ Color-coded message types (Error=red, Warning=yellow, Success=green, Info=white, etc.)
- ✅ Queue-based display with auto-fade
- ✅ Messages persist to console even if display is inactive
- ✅ Graceful fallback to Debug.Log if no display available

### Message Types

```csharp
BattleSceneMessageDisplay.MessageType.Info       // [INFO] White
BattleSceneMessageDisplay.MessageType.Success    // [OK] Green
BattleSceneMessageDisplay.MessageType.Warning    // [WARN] Yellow
BattleSceneMessageDisplay.MessageType.Error      // [ERROR] Red
BattleSceneMessageDisplay.MessageType.Building   // [BUILD] Magenta
BattleSceneMessageDisplay.MessageType.Boost      // [BOOST] Cyan
```

### Example Usage in CruiserSection

```csharp
// When a section takes damage
private void OnTakingDamage(float damage)
{
    ParentCruiser?.DisplayMessage($"{HullId} taking {damage:F0} damage!");
}

// When a section is destroyed
private void OnHealthGone(object sender, EventArgs e)
{
    ParentCruiser?.DisplayMessage(
        $"{HullId} destroyed!",
        BattleSceneMessageDisplay.MessageType.Error);

    ParentCruiser?.OnHullDestroyed(this);
}
```

### Implementation Details

- **Location**: `Assets/Scripts/Utils/Debugging/BattleSceneMessageDisplay.cs`
- **Singleton Access**: `BattleSceneMessageDisplay.Instance`
- **Message Lifetime**: 8 seconds (configurable in inspector)
- **Max Visible**: 5 messages (configurable in inspector)
- **Fallback**: If no display found, message logs to console only

---

## Summary

### Before This Refactor (Deleted)
- `ChainCruiser` class extended `Cruiser`
- Specialized `HullSection` components (separate from Cruiser)
- Complex initialization, property overrides, duplication
- Fragile inheritance hierarchy

### After This Refactor (Current) ✅
- **One `Cruiser` class** handles all configurations
- **`CruiserSection[]` array** in base class
- **Prefab structure determines complexity** (not code)
- **Zero inheritance specialization** - same code path for all
- **Automatic initialization** in CruiserFactory - no manual array setup

### The Process
1. **Design**: Create Cruiser prefab with CruiserSection children
2. **Structure**: First child = primary, rest = secondary
3. **Configuration**: Assign sprites, colliders, health values
4. **Instantiation**: CruiserFactory auto-detects sections
5. **Battle**: Sections targeted independently, primary controls game state

**That's it.** Design a Cruiser prefab with multiple CruiserSection children in the hierarchy, and the system handles everything automatically. Same code path, no specialization.

---

## 🔧 Troubleshooting Guide

### Building Position Offset Issue

**Symptom**: Buildings spawn at wrong world position (e.g., -30, 6 instead of 0, 0 on slot)

**Debug Steps**:
1. Check console for `[Slot]` debug logs when building is placed
2. Look for these key messages:
   - `[Slot] Setting building {name} on slot {name}`
   - `[Slot] BuildingPlacementPoint: {position}` - Should be on the slot
   - `[Slot] Target world position calculated: {position}` - Desired position
   - `[Slot] After positioning - World: {position}, Local: {position}`

3. **Diagnosis**:
   - If `BuildingPlacementPoint` is wrong: Check if BuildingPlacementPoint child exists and has correct position on slot
   - If offset calculation is wrong: Verify PuzzleRootPoint on building prefab
   - If final position is wrong: Check Transform hierarchy and parent-child relationships

**Common Fixes**:
- Verify BuildingPlacementPoint is positioned at (0, 0) relative to slot
- Ensure building is properly parented to slot
- Check that slot is properly positioned on cruiser/section

### Building Selection Not Working

**Symptom**: Buildings can be targeted by weapons but clicking doesn't select them (no red highlight, no info panel)

**Debug Steps**:
1. Click a building and check console for:
   - `[Slot.OnPointerClick]` - Indicates which object was clicked
   - `[Building.OnSingleClick]` - Indicates if building's click handler fired

2. **If only Slot logs appear**:
   - Building's click detection isn't firing
   - Check: Is building's ClickHandlerWrapper component present?
   - Check: Is building's collider properly positioned?
   - Check: Is building in correct layer for raycasting?

3. **If Building logs appear but no UI update**:
   - UIManager problem
   - Check: `[Building.OnSingleClick] UIManager: {Valid/NULL}`
   - If NULL: Building wasn't properly initialized with UIManager reference

**Common Fixes**:
- Ensure building has ClickHandlerWrapper component
- Ensure building collider is in correct position (moves with building)
- Verify building is activated properly (Activate method called after creation)
- Check UIManager is assigned during Buildable.Initialise()

### SlotWrapper Configuration for Multi-Section Cruisers

**Best Practice** (Confirmed Working):
- **Single parent SlotWrapper** on root Cruiser (not per-section)
- SlotWrapper finds all Slot children via GetComponentsInChildren
- Works for both single-section and multi-section cruisers
- Ensures all slots are accessible regardless of section structure

**Why Not Per-Section SlotWrappers**:
- Creates duplicate slot collections
- Adds unnecessary complexity
- Can cause initialization order issues
- Single parent approach is simpler and more robust

### Battle Initialization Crash

**Symptom**: "Cannot process sequence point: Invalid faction Reds or cruiser is null"

**Debug Steps**:
1. Check for earlier console errors **before** this message:
   - Look for `[Slot.Initialise]` failures (missing SlotImage)
   - Look for null reference exceptions
   - Look for assertion failures

2. **Likely causes**:
   - LandingSceneGod.Instance is null (fixed: added null check)
   - Cruiser initialization failed silently
   - Slots weren't properly initialized with SlotImage renderer

**Fix Applied**:
- Added null check for LandingSceneGod.Instance
- Added null check in Slot.IsVisible property
- Added comprehensive debug logging throughout initialization

---

## 📝 Debug Log Reference

When troubleshooting, look for these patterns in console:

```
[Slot.Initialise] Initializing slot {name} on cruiser {cruiser}
[Slot.Initialise] Found SlotImage renderer on {name}     ← Good
[Slot.Initialise] Failed to find SlotImage SpriteRenderer ← Problem!

[Slot] Setting building {name} on slot {name}
[Slot] BuildingPlacementPoint: {position}
[Slot] Target world position calculated: {position}
[Slot] After positioning - World: {pos}, Local: {pos}

[Slot.OnPointerClick] Slot {name} clicked. IsVisible: {T/F}, IsFree: {T/F}
[Building.OnSingleClick] Building {name} clicked. UIManager: {Valid/NULL}
```

Use these logs to trace the exact point where something goes wrong.
