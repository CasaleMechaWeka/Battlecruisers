# Multi-Hull Cruiser System - Implementation Guide

## 🎯 Executive Summary

**Status**: ✅ Complete and Production Ready
**Architecture**: Flexible Single-Class System (Cruiser natively supports 1-N hulls)
**Date**: January 2026

### The Simple Idea
**Cruiser = Hull(s)**

One `Cruiser` class handles both single-hull and multi-hull configurations. Prefabs determine complexity:
- **Single-hull cruiser**: 1 Hull child
- **Multi-hull cruiser**: 3+ Hull children
- **Same code path** - no specialization

### What Was Done
Inverted the architecture from "ChainCruiser extends Cruiser with special logic" to "Cruiser natively supports any hull count."

**Result**:
- ❌ Deleted ChainCruiser entirely (no longer needed)
- ✅ Merged all multi-hull logic into base Cruiser
- ✅ Hull[] array in base class
- ✅ Property routing works for 1-N hulls
- ✅ Secondary hull destruction scoring built-in

---

## 🏗️ Architecture Overview

### One Class, Multiple Configurations

```
┌─────────────────────────────────────┐
│          Cruiser                    │
├─────────────────────────────────────┤
│                                     │
│  Hull[] _hulls                      │
│  ├─ Properties route through [0]    │
│  ├─ Events for hull destruction     │
│  └─ Secondary hull scoring logic    │
│                                     │
│  Virtual hull callbacks:            │
│  ├─ OnHullClicked(hull)             │
│  ├─ OnHullDestroyed(hull)           │
│  └─ SetupHulls(hull[])              │
│                                     │
└─────────────────────────────────────┘
         ▲
         │
    Configuration via Prefab:

    Single-Hull:        Multi-Hull:
    ┌──────────┐        ┌──────────┐
    │ Cruiser  │        │ Cruiser  │
    ├──────────┤        ├──────────┤
    │ Hull[1]  │        │ Hull[3]  │
    │ - Primary│        │ - Primary│
    │          │        │ - Wing L │
    │          │        │ - Wing R │
    └──────────┘        └──────────┘
```

### Key Classes

**Cruiser.cs** (1 class, handles everything):
```csharp
public class Cruiser : Target, ICruiser
{
    // Hull array for 1-N hulls
    protected Hull[] _hulls;
    public Hull[] Hulls => _hulls;  // Public accessor

    // Properties automatically route through primary hull
    public float Health => _hulls?[0]?.Health ?? maxHealth;
    public float MaxHealth => _hulls?[0]?.MaxHealth ?? maxHealth;
    public bool IsDestroyed => _hulls?[0]?.IsDestroyed ?? false;

    // Virtual methods for hull events
    public virtual void OnHullDestroyed(Hull hull)
    {
        if (hull.IsPrimary)
            Destroy();  // Primary = game over
        else if (_hulls.Length > 1)
        {
            SecondaryHullDestroyed?.Invoke(this, new HullSectionDestroyedEventArgs(hull));
            BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(hull.maxHealth * 0.3f));
        }
    }

    public virtual void SetupHulls(Hull[] hulls)
    {
        _hulls = hulls;
        if (_hulls?[0] != null)
            maxHealth = _hulls[0].maxHealth;
    }
}
```

**Hull.cs** (formerly HullSection - generic component):
```csharp
public class Hull : MonoBehaviour, ITarget
{
    public Cruiser ParentCruiser;      // Generic reference
    public string HullId;
    public bool IsPrimary;              // Only one = true

    // Notifies parent when destroyed
    private void OnHealthGone()
    {
        ParentCruiser?.OnHullDestroyed(this);
    }
}
```

---

## 🎮 How to Build a Multi-Hull Level

### Step 1: Create Base Cruiser Prefab

```
In Scene:
1. Create empty GameObject: "EnemyBoss"
2. Add Component: Cruiser (yes, just "Cruiser", no "Chain")
3. Configure:
   - stringKeyBase: "Boss"
   - numOfDrones: 6
   - hullType: Cruiser
```

### Step 2: Create Hull Children

For each hull section you want (primary + secondaries):

```
EnemyBoss/
├─ PrimaryHull (Child GameObject)
│  ├─ Add Component: Hull
│  ├─ Add Component: SpriteRenderer (assign sprite)
│  ├─ Add Component: PolygonCollider2D (trace outline)
│  ├─ Configure Hull component:
│  │  ├─ HullId: "Primary"
│  │  ├─ IsPrimary: TRUE ⚠️ (only one!)
│  │  ├─ maxHealth: 3000
│  │  └─ healthGainPerDroneS: 1.0
│  └─ Assign DeathPrefab: (explosion effect)
│
├─ LeftWing (Child GameObject)
│  ├─ Add Component: Hull
│  ├─ Add Component: SpriteRenderer
│  ├─ Add Component: PolygonCollider2D
│  ├─ Configure Hull component:
│  │  ├─ HullId: "LeftWing"
│  │  ├─ IsPrimary: FALSE
│  │  ├─ maxHealth: 1500
│  │  └─ healthGainPerDroneS: 0.8
│  └─ Assign DeathPrefab: (explosion)
│
└─ RightWing (Child GameObject)
   ├─ Add Component: Hull
   ├─ [same as LeftWing]
```

### Step 3: Assign Hull Array

```
1. Select: EnemyBoss (root)
2. In Inspector, Cruiser component:
   ├─ Hulls size: 3
   ├─ Hulls[0]: Drag PrimaryHull
   ├─ Hulls[1]: Drag LeftWing
   └─ Hulls[2]: Drag RightWing
```

**Order matters**: Primary hull MUST be first (index 0)

### Step 4: Configure Each Hull

For each hull in the Hulls array, assign:
- **SpriteRenderer** field: The renderer component
- **PrimaryCollider** field: The PolygonCollider2D component
- **DeathPrefab** field: Explosion effect for that hull

### Step 5: Save as Prefab

```
1. Drag EnemyBoss to: Assets/Resources/Cruisers/
2. Name: "Boss_MultiHull.prefab" (or your boss name)
```

### Step 6: Use in Level

```csharp
// In BattleSceneGod or level config:
Cruiser boss = PrefabFactory.GetCruiserPrefab("Boss_MultiHull");
// Automatically works - Cruiser handles all hull setup
```

---

## 🧪 Testing Multi-Hull Combat

```
1. Load level with your multi-hull boss
2. Target each hull individually:
   - Click PrimaryHull: Should select boss, show primary health
   - Click LeftWing: Should select boss, still show primary health
   - Click RightWing: Should select boss, still show primary health
3. Damage secondary hull:
   - Attack LeftWing for ~1500 damage
   - LeftWing should hide/explode
   - Battle continues
   - Score awarded (~450 points)
4. Damage primary hull:
   - Attack PrimaryHull for ~3000 damage
   - PrimaryHull hides/explodes
   - GAME OVER/VICTORY
```

---

## 🎨 Example Boss Configurations

### Simple 3-Hull Boss
```
PrimaryHull (HP: 3000, IsPrimary: true)
LeftWing   (HP: 1500, IsPrimary: false)
RightWing  (HP: 1500, IsPrimary: false)

Total perceived health: 3000 (primary shown in UI)
Secondary destruction: Continues battle + awards points
```

### Complex 5-Hull Boss
```
MainHull   (HP: 4000, IsPrimary: true)  - center
Engine1    (HP: 1200, IsPrimary: false) - rear left
Engine2    (HP: 1200, IsPrimary: false) - rear right
CannonLeft (HP: 800,  IsPrimary: false) - side
CannonRight(HP: 800,  IsPrimary: false) - side

Total: 8000 possible damage, but game ends at 4000 (primary)
```

---

## 🔧 Technical Details

### How It All Works

#### When Battle Starts
```
1. Cruiser GameObject instantiated
2. StaticInitialise():
   ├─ Load Hull[] children from prefab
   └─ SetupHulls(_hulls) called
3. Initialise(args):
   ├─ Initialize each hull in _hulls[]
   └─ Subscribe to destruction events
4. Ready for combat
```

#### When Hull Takes Damage
```
1. Player targets LeftWing hull
2. LeftWing.TakeDamage(100)
3. LeftWing._healthTracker.RemoveHealth(100)
4. LeftWing health: 1500 → 1400
5. UI shows cruiser health (still primary: 3000)
```

#### When Secondary Hull Dies
```
1. LeftWing.TakeDamage(1500) [remaining health]
2. LeftWing._healthTracker reaches 0
3. Hull.OnHealthGone() triggered
4. Spawn DeathPrefab (explosion)
5. Call ParentCruiser.OnHullDestroyed(leftWing)
6. Cruiser.OnHullDestroyed() checks:
   ├─ if (IsPrimary) → Destroy() → Game over
   └─ else → SecondaryHullDestroyed event → Score awarded → Battle continues
```

#### When Primary Hull Dies
```
1. PrimaryHull.TakeDamage(3000)
2. Hull.OnHealthGone() triggered
3. Call ParentCruiser.OnHullDestroyed(primaryHull)
4. Cruiser.OnHullDestroyed() checks:
   └─ if (IsPrimary) → Destroy() → Triggers victory
```

### Properties Always Route Through Primary Hull

```csharp
// These all return primary hull values
boss.Health           // ← Primary hull health
boss.MaxHealth        // ← Primary hull max health
boss.IsDestroyed      // ← Primary hull destroyed?
boss.IsAlive          // ← Primary hull alive?
boss.Size             // ← Primary hull collider size

// These apply to ALL hulls
boss.Color = red      // ← All hulls turn red
boss.MakeInvincible() // ← All hulls invincible
```

---

## 📋 Checklist for Creating Multi-Hull Boss

- [ ] Create Cruiser GameObject
- [ ] Add Cruiser component
- [ ] Create Hull children (at least 2: 1 primary, 1+ secondary)
- [ ] For each Hull:
  - [ ] Add Hull component
  - [ ] Set HullId unique name
  - [ ] Set IsPrimary (only ONE = true)
  - [ ] Set maxHealth
  - [ ] Add SpriteRenderer, assign sprite
  - [ ] Add PolygonCollider2D, trace outline
  - [ ] Assign to Hull component fields
  - [ ] Assign DeathPrefab
- [ ] In root Cruiser, assign Hulls array
- [ ] Save as prefab
- [ ] Test in scene

---

## ❓ FAQ

**Q: Can I have more than 3 hulls?**
A: Yes, any number. Just add more Hull children and extend the Hulls array.

**Q: What if I have no HullSections in my prefab?**
A: Not recommended - Cruiser expects at least one. Will fall back to legacy single-hull logic.

**Q: Do secondary hulls need SlotWrapperController?**
A: Optional. Primary usually has slots, secondaries can have their own or none.

**Q: Can I change which hull is primary at runtime?**
A: Not recommended. Set IsPrimary in prefab before instantiation.

**Q: How does targeting work?**
A: GlobalTargetFinder emits each Hull as an independent target. Clicking any hull selects the cruiser, but targeting info goes to that specific hull.

**Q: What's the score for secondary destruction?**
A: `(int)(hull.maxHealth * 0.3f)` - 30% of hull's max health as points.

---

## Summary

### Old Way (Deleted)
- ChainCruiser class extends Cruiser
- HullSection components
- Special initialization, property overrides
- Complex inheritance

### New Way (Current) ✅
- One Cruiser class
- Hull[] array in base class
- Prefab determines complexity
- Zero inheritance specialization

**That's it.** Create a Cruiser prefab with multiple Hull children, save it, and use it. The system handles the rest.
