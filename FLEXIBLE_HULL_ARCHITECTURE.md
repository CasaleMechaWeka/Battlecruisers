# Flexible Multi-Hull Cruiser Architecture

## Overview

This document describes the **inverted architecture** that makes `Cruiser` flexible enough to support **1-N hulls natively** without specialization.

### Key Principle

**Instead of**: "Make ChainCruiser a special Cruiser that has multiple hulls"

**We do**: "Make Cruiser flexible enough to have 1-N hulls natively"

---

## Architecture

### Component Structure

```
Cruiser (base class)
├── Hull[] _hulls               ← All cruisers have a hull array
│   ├── Hull[0] (primary)      ← Always used for health/UI
│   └── Hull[N] (secondary)    ← Optional additional hulls
├── Properties route through hulls
└── Virtual methods for hull events

ChainCruiser (configuration)
├── Inherits: Hull[] and all hull routing
├── Adds: Secondary hull scoring logic
└── That's it! No property overrides needed

Regular Cruiser
├── Inherits: Hull[] and all hull routing
├── SetupHulls(1 hull)
└── Works exactly the same way
```

### Key Classes

#### `Hull` (formerly HullSection)
```csharp
public class Hull : MonoBehaviour, ITarget
{
    public Cruiser ParentCruiser;      // Generic, not ChainCruiser-specific
    public string HullId;
    public bool IsPrimary;             // Primary = destruction = game over

    public float Health { get; }
    public float MaxHealth { get; }
    public bool IsDestroyed { get; }

    // Events
    public event EventHandler<DestroyedEventArgs> Destroyed;

    // When destroyed, calls:
    public void OnHealthGone()
    {
        ParentCruiser?.OnHullDestroyed(this);  // ← Generic callback
    }
}
```

#### `Cruiser` (base class - now flexible)
```csharp
public class Cruiser : Target, ICruiser
{
    protected Hull[] _hulls;  // ← THE KEY: All cruisers have this

    // Properties now route through _hulls[0]
    public virtual float Health
    {
        get => _hulls?[0]?.Health ?? maxHealth;
    }

    public virtual float MaxHealth
    {
        get => _hulls?[0]?.MaxHealth ?? maxHealth;
    }

    public virtual bool IsDestroyed
    {
        get => _hulls?[0]?.IsDestroyed ?? false;
    }

    public override Color Color
    {
        set
        {
            // Apply to ALL hulls (why ChainCruiser doesn't need to override)
            if (_hulls != null)
            {
                foreach (var hull in _hulls)
                    hull?.Color = value;
            }
        }
    }

    // Hull event callbacks
    public virtual void OnHullClicked(Hull hull) { /* ... */ }
    public virtual void OnHullDoubleClicked(Hull hull) { /* ... */ }
    public virtual void OnHullTripleClicked(Hull hull) { /* ... */ }
    public virtual void OnHullDestroyed(Hull hull)
    {
        if (hull.IsPrimary)
            Destroy();  // Primary = cruiser death
    }

    // Setup hulls
    public virtual void SetupHulls(Hull[] hulls)
    {
        _hulls = hulls;
        if (_hulls?[0] != null)
            maxHealth = _hulls[0].maxHealth;
    }
}
```

#### `ChainCruiser` (minimal configuration)
```csharp
public class ChainCruiser : Cruiser
{
    public Hull[] Hulls;  // Serialized in prefab

    private Hull _primaryHull;

    public override void StaticInitialise()
    {
        _primaryHull = Hulls.FirstOrDefault(h => h.IsPrimary);
        SetupHulls(Hulls);  // ← Use base class method
        // ... other standard initialization ...
    }

    public override async void Initialise(CruiserArgs args)
    {
        base.Initialise(args);

        // Initialize each hull
        foreach (var hull in Hulls)
            hull?.Initialize();
    }

    // ONLY override if you need custom behavior
    public override void OnHullDestroyed(Hull hull)
    {
        if (hull.IsPrimary)
        {
            base.OnHullDestroyed(hull);  // Destroy the cruiser
        }
        else
        {
            // Award points for secondary hull destruction
            SecondaryHullDestroyed?.Invoke(this, new HullSectionDestroyedEventArgs(hull));
            BattleSceneGod.AddDeadBuildable(...);
        }
    }
}
```

---

## How It Works

### Single-Hull Cruiser Example

```
BattleSequencer spawns "Regular Cruiser"
        ↓
Cruiser.StaticInitialise()
        ↓
Load Hull[] from prefab (1 element)
        ↓
SetupHulls(hull[])
        ↓
maxHealth = hull[0].maxHealth
        ↓
Properties route through hull[0]
```

**Result**: Works perfectly. No specialization needed.

### Multi-Hull Cruiser Example

```
BattleSequencer spawns "ChainBattle Cruiser"
        ↓
ChainCruiser.StaticInitialise()
        ↓
Load Hull[] from prefab (3+ elements)
        ↓
SetupHulls(hull[])
        ↓
maxHealth = hull[0].maxHealth (primary)
        ↓
Properties route through hull[0] (always primary hull displayed)
        ↓
OnHullDestroyed() handles primary vs secondary logic
```

**Result**: Same code path as single-hull. ChainCruiser only adds secondary hull handling.

---

## Property Routing Example

When UI asks for cruiser health:

```csharp
// Old way (ChainCruiser had to override):
public new float Health => _primaryHull?.Health ?? 0;

// New way (works for all cruisers, no override needed):
public virtual float Health
{
    get
    {
        if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
            return _hulls[0].Health;
        return base.Health;  // Fallback for legacy single-hull
    }
}
```

---

## Hull Destruction Flow

### Single Hull Destroyed
```
Hull.OnHealthGone()
    ↓
ParentCruiser.OnHullDestroyed(this)
    ↓
Cruiser.OnHullDestroyed(hull)
    ↓
if (hull.IsPrimary)
    Cruiser.Destroy()  // Game over
```

### Multiple Hulls - Secondary Destroyed
```
Secondary Hull.OnHealthGone()
    ↓
ParentCruiser.OnHullDestroyed(this)
    ↓
ChainCruiser.OnHullDestroyed(hull)
    ↓
if (!hull.IsPrimary)
    SecondaryHullDestroyed?.Invoke()  // Award points
    BattleSceneGod.AddDeadBuildable()  // Score
```

### Multiple Hulls - Primary Destroyed
```
Primary Hull.OnHealthGone()
    ↓
ParentCruiser.OnHullDestroyed(this)
    ↓
ChainCruiser.OnHullDestroyed(hull)
    ↓
if (hull.IsPrimary)
    base.OnHullDestroyed(hull)
    ↓
    Cruiser.Destroy()  // Game over
```

---

## Design Benefits

### ✅ No Code Duplication
- Single property routing code works for all cruisers
- No need to override Color, Sprite, Health, Size, etc.
- New properties automatically work for 1-N hulls

### ✅ Extensible
- Adding 3+ hull cruisers: Just create prefab with 3 Hull children
- No code changes needed
- OnHullDestroyed still works correctly

### ✅ Maintainable
- Hull lifecycle is centralized in Hull class
- Cruiser doesn't care about number of hulls
- BattleSequencer doesn't care about hull structure

### ✅ Backwards Compatible
- All existing regular cruisers still work
- All existing ChainCruisers still work
- No API breakage

### ✅ Testable
- Hull can be tested independently (ITarget interface)
- Cruiser can be tested with 1 or N hulls
- Single code path = easier to verify

---

## Adding a New Multi-Hull Cruiser

### Old Way (Inheritance Hell)
1. Create `TripleCruiser extends Cruiser`
2. Override: Color, Sprite, Health, MaxHealth, IsDestroyed, IsAlive, Size, MakeInvincible, MakeDamagable, StaticInitialise, Initialise, FixedUpdate
3. Add hull-specific methods: OnHullSectionClicked, OnHullSectionDestroyed
4. Duplicate all the logic

### New Way (Configuration)
1. Create `TripleCruiser : Cruiser` or just use `ChainCruiser`
2. In prefab: Add 3 Hull child GameObjects
3. Mark one Hull with IsPrimary = true
4. That's it! Done.

If you need custom secondary hull logic:
```csharp
public override void OnHullDestroyed(Hull hull)
{
    base.OnHullDestroyed(hull);

    if (!hull.IsPrimary)
    {
        // Your custom logic here
    }
}
```

---

## Execution Flow

### Initialization Sequence

```
BattleSceneGod.Initialize()
├── cruiserFactory.CreatePlayerCruiser()
├── cruiserFactory.CreateAICruiser()
├── cruiserFactory.InitialisePlayerCruiser(player, ai)
│   ├── Cruiser.StaticInitialise()
│   │   ├── Load components (SpriteRenderer, Collider, SlotWrapperController)
│   │   ├── Load Hull[] from scene or create from root renderer
│   │   └── SetupHulls(_hulls)
│   │
│   └── Cruiser.Initialise(args)  [async]
│       ├── Set _enemyCruiser (NOW synchronously before enable!)
│       ├── Load sounds (async, won't block FixedUpdate)
│       └── Hull.Initialize() for each hull
│
├── GameObject.SetActive(true)
│   └── Now FixedUpdate can run safely
│       └── _enemyCruiser is guaranteed set
│
└── BattleSequencer.StartF()  [async]
    └── Spawn units/buildings as timed events
```

---

## Migration from Old ChainCruiser

If you have code that checks `cruiser.HullSections`:

### Old
```csharp
if (cruiser is ChainCruiser chainCruiser)
{
    foreach (var section in chainCruiser.HullSections)
    {
        // Do something with hull
    }
}
```

### New
```csharp
// Works for ALL cruisers now
if (cruiser is Cruiser c && c._hulls != null)
{
    foreach (var hull in c._hulls)
    {
        // Do something with hull
    }
}
```

Or just access the primary hull:
```csharp
if (cruiser._hulls?[0] != null)
{
    var primaryHull = cruiser._hulls[0];
}
```

---

## Summary: The Inversion

### Old Thinking
> "Cruiser is single-hull. ChainCruiser adds multi-hull support via specialization."

**Problem**: Every property must be overridden. Complex inheritance. Hard to add 3+ hull variants.

### New Thinking
> "Cruiser supports 1-N hulls natively. ChainCruiser is just a configuration variant."

**Solution**: Zero property overrides. Simple inheritance. Easy to add any number of hulls.

---

## Example: Regular vs Chain Cruiser

### Regular Cruiser Prefab
```
CruiserGameObject
├── SpriteRenderer (root)
├── Collider
├── SlotWrapperController
└── ClickHandler
```

At runtime:
```csharp
// Load root SpriteRenderer as primary hull
// _hulls = [hull_from_root_renderer]
// Properties route through _hulls[0]
```

### Chain Cruiser Prefab
```
CruiserGameObject
├── SlotWrapperController
├── ClickHandler
├── Hull1
│   ├── SpriteRenderer
│   ├── Collider
│   └── SlotWrapperController (for this hull's slots)
├── Hull2
│   ├── SpriteRenderer
│   ├── Collider
│   └── SlotWrapperController
└── Hull3
    ├── SpriteRenderer
    ├── Collider
    └── SlotWrapperController
```

At runtime:
```csharp
// Load Hull children
// _hulls = [Hull1, Hull2, Hull3]
// Properties route through _hulls[0] (primary)
```

**Same Cruiser code handles both.**

---

## Architecture Diagram

```
┌─────────────────────────────────────────┐
│           Target (base)                  │
│   - Health, MaxHealth, IsDestroyed      │
│   - TakeDamage, Destroy                 │
│   - ITarget interface                   │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│      Cruiser (flexible)                  │
│                                          │
│  NEW:                                    │
│  - Hull[] _hulls                         │
│  - SetupHulls(hulls)                     │
│  - OnHullClicked(hull)                   │
│  - OnHullDoubleClicked(hull)             │
│  - OnHullTripleClicked(hull)             │
│  - OnHullDestroyed(hull)                 │
│                                          │
│  UPDATED:                                │
│  - Color property (apply to all hulls)   │
│  - Size (use primary hull)               │
│  - Health/MaxHealth/IsDestroyed (route   │
│    through _hulls[0])                    │
│  - Sprite (use primary hull)             │
│  - MakeInvincible/Damagable (all hulls)  │
└──────┬──────────────────────┬────────────┘
       │                      │
   ┌───▼───────────┐      ┌──▼──────────────────┐
   │ Regular Cruiser│      │  ChainCruiser       │
   │                │      │                     │
   │ Uses base      │      │ - Hulls[] (config)  │
   │ implementation │      │ - OnHullDestroyed() │
   │ No overrides   │      │   (secondary logic) │
   │ 1 Hull in _    │      │ - 2+ Hulls in _     │
   │ hulls[]        │      │   hulls[]           │
   └────────────────┘      └─────────────────────┘
```

---

## Conclusion

The inverted architecture achieved:

1. **Flexibility**: Support 1-N hulls without specialization
2. **Simplicity**: Zero property overrides in ChainCruiser
3. **Maintainability**: Single code path for all hull counts
4. **Extensibility**: Add new multi-hull variants instantly
5. **Compatibility**: All existing code still works

This is the "fresh perspective" approach: instead of making the complex special case in a subclass, make the base class flexible enough to handle all cases uniformly.
