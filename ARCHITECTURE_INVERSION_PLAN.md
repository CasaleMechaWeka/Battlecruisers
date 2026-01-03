# Cruiser Architecture Inversion: From Specialized to Flexible

## Vision

**FROM**: "Make ChainCruiser a special Cruiser that has multiple hulls"
**TO**: "Make Cruiser flexible enough to have 1-N hulls natively"

This inverts the hierarchy: instead of ChainCruiser overriding everything, the base Cruiser becomes hull-agnostic and just works with whatever hull configuration you give it.

---

## Current Problems (Why Inversion Needed)

### 1. **Inheritance Explosion**
```csharp
// Current: ChainCruiser overrides 10+ properties
public class ChainCruiser : Cruiser
{
    public override Color { set { /* apply to all hulls */ } }
    public new Sprite { get { /* return primary hull sprite */ } }
    public new float Health { get { /* return primary hull health */ } }
    public new float MaxHealth { /* ... */ }
    public new bool IsDestroyed { /* ... */ }
    public override Vector2 Size { /* ... */ }
    public new void MakeInvincible() { /* ... */ }
    public new void MakeDamagable() { /* ... */ }
    public override void FixedUpdate() { /* ... */ }
    public override void StaticInitialise() { /* ... */ }
    public override async void Initialise() { /* ... */ }
}
```

Every new multi-hull cruiser needs a new subclass. Every property access routes through nullable checks.

### 2. **Null Check Hell**
Properties return null-safe values but still require defensive checks:
- `_primaryHull?.Health ?? 0`
- `_primaryHull?.MaxHealth ?? maxHealth`
- `_primaryHull?.IsDestroyed ?? true`

This leaks implementation details everywhere.

### 3. **Tight Coupling to ChainCruiser**
HullSection is hardcoded to expect a ChainCruiser parent:
```csharp
public ChainCruiser ParentCruiser;  // ← Only works with ChainCruiser
public void OnHullSectionDestroyed(HullSection hull) { /* ... */ }
```

This breaks if we want generic Hull components.

### 4. **Prefab Duplication**
Single-hull and multi-hull cruisers are fundamentally different prefab structures:
- Single-hull: Root GameObject with SpriteRenderer, Collider
- Multi-hull: Root GameObject with children HullSections

Can't share initialization logic or prefab variants.

---

## Solution Architecture

### **Core Principle: Composition Over Inheritance**

All Cruisers, regardless of hull count, work with `Hull[] _hulls`:
- **Single-hull cruiser**: `_hulls.Length == 1`
- **Multi-hull cruiser (ChainCruiser)**: `_hulls.Length > 1`
- **Same code path**: No specialization needed

### **Key Changes**

#### 1. **Rename HullSection → Hull**
```csharp
public class Hull : MonoBehaviour, ITarget
{
    // Same as current HullSection
    // No reference to specific parent type
    // Generic parent reference only
    public Cruiser ParentCruiser;  // ← Generic, not ChainCruiser
}
```

#### 2. **Add Hull Array to Base Cruiser**
```csharp
public class Cruiser : Target, ICruiser
{
    // Instead of:
    // protected SpriteRenderer _renderer;

    // Use:
    protected Hull[] _hulls;

    // Property routing
    public virtual float Health => _hulls?[0]?.Health ?? maxHealth;
    public virtual float MaxHealth => _hulls?[0]?.MaxHealth ?? maxHealth;
    public virtual bool IsDestroyed => _hulls?[0]?.IsDestroyed ?? false;
    public override bool IsAlive => _hulls != null && _hulls.Length > 0 && !_hulls[0].IsDestroyed;
    public override Vector2 Size => _hulls?[0]?.Size ?? Vector2.one;
    public override Color Color { set { foreach(var h in _hulls) h.Color = value; } }

    // And more...
}
```

#### 3. **Remove ChainCruiser Entirely (or Empty Subclass)**

Option A: Delete ChainCruiser class entirely
- All logic is in base Cruiser
- Prefabs use "Cruiser" with multiple Hull children

Option B: Keep empty ChainCruiser for backwards compatibility
```csharp
public class ChainCruiser : Cruiser
{
    // Empty - base Cruiser handles everything
}
```

#### 4. **Generic Hull Events and Callbacks**

Instead of `ChainCruiser.OnHullSectionDestroyed()`, use generic events:
```csharp
public class Cruiser : Target
{
    public event EventHandler<HullDestroyedEventArgs> HullDestroyed;

    public virtual void OnHullDestroyed(Hull hull)
    {
        if (hull.IsPrimary)
        {
            Destroy();  // Primary hull death = cruiser death
        }
        else
        {
            HullDestroyed?.Invoke(this, new HullDestroyedEventArgs(hull));
        }
    }
}
```

Hull calls back generically:
```csharp
public class Hull : MonoBehaviour, ITarget
{
    private void OnHealthGone(object sender, EventArgs e)
    {
        ParentCruiser?.OnHullDestroyed(this);  // ← Generic callback
    }
}
```

---

## Implementation Steps

### **Phase 1: Rename and Decouple (1 hour)**
1. Rename HullSection.cs → Hull.cs
2. Update class name: `class HullSection` → `class Hull`
3. Change ParentCruiser type from `ChainCruiser` to `Cruiser`
4. Update all references in HullSection/Hull code
5. Update ChainCruiser to reference `Hull[]` instead of `HullSection[]`

### **Phase 2: Add Hull Array to Base Cruiser (2 hours)**
1. Add `protected Hull[] _hulls` to Cruiser
2. Create `SetupHulls(Hull[])` method
3. Route properties through `_hulls[0]`:
   - Health, MaxHealth, IsDestroyed, IsAlive
   - Color (apply to all)
   - Size, Sprite
   - MakeInvincible, MakeDamagable
4. Update FixedUpdate to guard against null _hulls
5. Remove SpriteRenderer requirement from StaticInitialise

### **Phase 3: Unified Death Logic (1 hour)**
1. Add `OnHullDestroyed(Hull hull)` virtual method to Cruiser
2. Update Hull.OnHealthGone to call `ParentCruiser.OnHullDestroyed(this)`
3. Remove ChainCruiser.OnHullSectionDestroyed
4. Move primary/secondary hull logic to base Cruiser

### **Phase 4: Update Initialization (1 hour)**
1. Create Hull[] during StaticInitialise for both single and multi-hull
2. For single-hull: create wrapper Hull from root SpriteRenderer
3. For multi-hull: load Hull components from children
4. Call SetupHulls() before Enable
5. Update CruiserFactory to handle both cases

### **Phase 5: Remove ChainCruiser Specialization (30 min)**
1. Delete FixedUpdate override from ChainCruiser (move to base)
2. Delete all property overrides (Color, Sprite, Health, etc)
3. Delete StaticInitialise override
4. Delete Initialise override
5. Delete click handling methods (move to base)

### **Phase 6: Testing & Documentation (1.5 hours)**
1. Test regular cruisers (1 hull)
2. Test ChainCruiser (N hulls)
3. Test clicking on individual hulls
4. Test primary vs secondary hull death
5. Create comprehensive architecture documentation

---

## File Changes Summary

| File | Action | Why |
|------|--------|-----|
| HullSection.cs | Rename to Hull.cs | Generic hull component, not section-specific |
| Hull.cs (renamed) | Update type: ParentCruiser | Decouple from ChainCruiser |
| Cruiser.cs | Add `_hulls[]`, route properties | Become hull-agnostic |
| Cruiser.cs | Add OnHullDestroyed() | Generic death callback |
| Cruiser.cs | Remove SpriteRenderer requirement | Support multi-hull |
| ChainCruiser.cs | Delete most overrides | Base Cruiser handles it |
| ChainCruiser.cs | Rename HullSections → Hulls | Consistency |
| CruiserFactory.cs | Update initialization | Setup Hull array |
| Prefabs | Restructure if needed | Ensure Hull[] is populated |

---

## Code Examples: Before & After

### **Property Routing**

**BEFORE** (ChainCruiser override hell):
```csharp
public class ChainCruiser : Cruiser
{
    public HullSection[] HullSections;
    private HullSection _primaryHull;

    public new float Health => _primaryHull?.Health ?? 0;
    public new float MaxHealth => _primaryHull?.MaxHealth ?? maxHealth;
    public new bool IsDestroyed => _primaryHull?.IsDestroyed ?? true;

    public override Color Color {
        set {
            if (HullSections != null) {
                foreach (var hullSection in HullSections) {
                    if (hullSection != null) {
                        hullSection.Color = value;
                    }
                }
            }
        }
    }
}
```

**AFTER** (Base Cruiser, no specialization):
```csharp
public class Cruiser : Target, ICruiser
{
    protected Hull[] _hulls;

    public virtual float Health => _hulls?[0]?.Health ?? maxHealth;
    public virtual float MaxHealth => _hulls?[0]?.MaxHealth ?? maxHealth;
    public virtual bool IsDestroyed => _hulls?[0]?.IsDestroyed ?? false;

    public override Color Color {
        set {
            if (_hulls != null) {
                foreach (var hull in _hulls) {
                    if (hull != null) {
                        hull.Color = value;
                    }
                }
            }
        }
    }
}

// ChainCruiser becomes:
public class ChainCruiser : Cruiser
{
    // Empty - or just exists for backwards compatibility
}
```

### **Death Logic**

**BEFORE** (ChainCruiser-specific):
```csharp
public class ChainCruiser : Cruiser
{
    public void OnHullSectionDestroyed(HullSection hull)
    {
        if (hull.IsPrimary)
        {
            Destroy();
        }
        else
        {
            SecondaryHullDestroyed?.Invoke(this, new HullSectionDestroyedEventArgs(hull));
            BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(hull.maxHealth * 0.3f));
        }
    }
}

public class HullSection : MonoBehaviour, ITarget
{
    private void OnHealthGone(object sender, EventArgs e)
    {
        ParentCruiser?.OnHullSectionDestroyed(this);  // Expects ChainCruiser
    }
}
```

**AFTER** (Generic, works for any Cruiser):
```csharp
public class Cruiser : Target, ICruiser
{
    public event EventHandler<HullDestroyedEventArgs> HullDestroyed;

    public virtual void OnHullDestroyed(Hull hull)
    {
        if (hull.IsPrimary)
        {
            Destroy();  // Same for all
        }
        else
        {
            HullDestroyed?.Invoke(this, new HullDestroyedEventArgs(hull));

            if (Faction == Faction.Reds)
            {
                BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(hull.maxHealth * 0.3f));
            }
        }
    }
}

public class Hull : MonoBehaviour, ITarget
{
    public Cruiser ParentCruiser;  // Generic reference

    private void OnHealthGone(object sender, EventArgs e)
    {
        ParentCruiser?.OnHullDestroyed(this);  // Works with any Cruiser
    }
}
```

### **Setup**

**BEFORE** (Specialized per cruiser type):
```csharp
// Regular Cruiser uses base StaticInitialise
public class Cruiser : Target
{
    protected virtual void StaticInitialise()
    {
        _renderer = GetComponent<SpriteRenderer>();  // Assumes root renderer
        Assert.IsNotNull(_renderer, "Cruiser needs SpriteRenderer");
        // ...
    }
}

// ChainCruiser overrides completely
public class ChainCruiser : Cruiser
{
    public override void StaticInitialise()
    {
        // Completely different - no root renderer, load from children
        if (HullSections != null && HullSections.Length > 0)
        {
            _primaryHull = HullSections.FirstOrDefault(h => h.IsPrimary);
            maxHealth = _primaryHull.maxHealth;
        }
        // ...
    }
}
```

**AFTER** (Generic, handles both):
```csharp
public class Cruiser : Target
{
    protected virtual void StaticInitialise()
    {
        // Load hulls - could be 1 or N
        _hulls = LoadHulls();

        if (_hulls != null && _hulls.Length > 0)
        {
            // Set maxHealth from primary hull
            maxHealth = _hulls[0].maxHealth;

            // If we're a traditional single-hull, we might have a root renderer
            _renderer = GetComponent<SpriteRenderer>();  // Can be null now
        }
        // ...
    }

    protected virtual Hull[] LoadHulls()
    {
        // Try to load from children first (multi-hull)
        var childHulls = GetComponentsInChildren<Hull>();
        if (childHulls.Length > 0) return childHulls;

        // If no children, create from root renderer (single-hull legacy)
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            return new[] { CreateHullFromRenderer(renderer) };
        }

        return new Hull[0];
    }
}
```

---

## Event Changes

**New Events** (Generic across all Cruiser types):
```csharp
// In Cruiser.cs
public event EventHandler<HullDestroyedEventArgs> HullDestroyed;
public event EventHandler<HullDamagedEventArgs> HullDamaged;
```

**Removed Events** (ChainCruiser-specific):
```csharp
// Goodbye to:
// public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;
// public event EventHandler<HullSectionDestroyedEventArgs> SecondaryHullDestroyed;
// public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;
```

Listeners can subscribe to `Cruiser.HullDestroyed` instead, works for any number of hulls.

---

## Backwards Compatibility

### ✅ Works Out of the Box
- All existing regular (single-hull) cruisers
- All existing ChainCruisers with multiple HullSections
- BattleSequencer (doesn't care about hull structure)

### ⚠️ Code Changes Needed
- Any code checking `cruiser._renderer` → check `cruiser._hulls[0]?.SpriteRenderer`
- Any code checking `ChainCruiser.HullSections` → check `Cruiser._hulls`
- Any code listening to `ChainCruiser.SecondaryHullDestroyed` → listen to `Cruiser.HullDestroyed`

### ✅ No Breaking Changes
- Public API of Cruiser stays mostly the same
- Health, MaxHealth, IsDestroyed, IsAlive all work the same
- Click behavior unchanged

---

## Success Criteria

✅ Regular cruisers work with 1 Hull in array
✅ ChainCruisers work with N Hulls in array
✅ Clicking on individual hulls selects the cruiser
✅ Triple-click on hull targets that specific hull
✅ Primary hull death = cruiser death = game over
✅ Secondary hull death = score awarded, battle continues
✅ All existing UI (health bars, selection, etc) works
✅ No ChainCruiser-specific overrides in base Cruiser
✅ Hull array is completely generic
✅ Can add 3+ hull cruisers without code changes

---

## Timeline Estimate
- Phase 1 (Rename): 1 hour
- Phase 2 (Hull Array): 2 hours
- Phase 3 (Death Logic): 1 hour
- Phase 4 (Initialization): 1 hour
- Phase 5 (Cleanup): 30 minutes
- Phase 6 (Testing & Docs): 1.5 hours

**Total: ~7 hours of focused work**

---

## Next Steps

1. Review this plan
2. Create Plan2.md with final strategy
3. Begin Phase 1: Rename HullSection → Hull
4. Execute phases in order
5. Test after each phase
6. Document as we go
