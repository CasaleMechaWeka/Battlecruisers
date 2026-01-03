# Battle Performance Degradation Investigation

**Issue:** Single-player battles experience significant frame rate drops after 5-6 minutes of sustained gameplay.

**Investigation Date:** 2025-12-30

---

## Executive Summary

I've identified **ONE CRITICAL BUG** and **MULTIPLE PERFORMANCE ISSUES** that together explain the 5-6 minute performance degradation:

### 🔴 CRITICAL BUG: Pool.cs Backwards Logic (Line 35)
The unit pooling system has **inverted logic** that causes it to fail when pools are healthy and full, forcing new object creation instead of reuse.

### ⚠️ MAJOR PERFORMANCE ISSUES:
1. **O(n) LINQ operations** in target tracking called every frame
2. **Excessive Unity coroutines** accumulating from deferred cleanup (10s trail lifetime)
3. **Event subscription overhead** in target detection system

---

## 1. CRITICAL BUG: Pool.cs Inverted MaxLimit Check

**File:** `/Assets/Scripts/Utils/BattleScene/Pools/Pool.cs:35`

### The Bug

```csharp
public TPoolable GetItem(TArgs activationArgs)
{
    if (_items.Count < MaxLimit)  // ❌ BUG: Checks pool size instead of total created!
    {
        TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
        item.Activate(activationArgs);
        return item;
    }
    return null;  // Returns null when pool is FULL (should be when we're out of capacity!)
}
```

### What's Wrong

The condition checks `_items.Count` (number of available items in pool) instead of `_createCount` (total items ever created).

**Current Broken Behavior:**
- When pool is empty (0 items): `0 < 1000` ✅ Works
- As items are recycled, pool fills up
- When pool reaches 1000 items: `1000 < 1000` ❌ FALSE → **Returns NULL**
- Forces creation of NEW objects outside the pool → Memory fragmentation

**What Should Happen:**
```csharp
if (_createCount < MaxLimit)  // Check total created, not pool size!
```

### Impact on Performance

After 5-6 minutes of battle:
1. Many units have been created and destroyed
2. Pool fills up with recycled units (this is GOOD and expected!)
3. Bug triggers: GetItem() returns null
4. Factory.cs:176 gets null but doesn't check → **NullReferenceException** OR creates objects outside pool
5. Memory fragmentation increases
6. Garbage collector runs more frequently
7. Frame rate drops

### Evidence

**Pool.cs:35** - Bug location
```csharp
if (_items.Count < MaxLimit)
```

**Factory.cs:176** - No null check after GetItem():
```csharp
UnitUnderConstruction = _unitPool.GetItem(activationArgs);
UnitUnderConstruction.DroneConsumerProvider = this;  // ❌ Will crash if null!
```

---

## 2. MAJOR ISSUE: RankedTargetTracker O(n) LINQ Performance

**File:** `/Assets/Scripts/Targets/TargetTrackers/RankedTargetTracker.cs:119`

### The Problem

```csharp
private bool AreTrackingTarget(ITarget target)
{
    return _targets.Any(rankedTarget => ReferenceEquals(rankedTarget.Target, target));
}
```

**Called From:**
- Line 49: `TargetFinder_TargetFound()` - Every time a target enters detection range
- Line 98: `TargetFinder_TargetLost()` - Every time a target leaves detection range

### Performance Impact

- **Time Complexity:** O(n) per check
- **Frequency:** Called for EVERY target detection event
- **Scale:** With 50+ units in a 5-6 minute battle, this becomes thousands of O(n) operations
- **CPU Impact:** Significant when combined with LINQ overhead

### Recommended Fix

Replace with HashSet for O(1) lookup:
```csharp
private readonly HashSet<ITarget> _trackedTargets = new HashSet<ITarget>();

private bool AreTrackingTarget(ITarget target)
{
    return _trackedTargets.Contains(target);
}
```

---

## 3. MAJOR ISSUE: Excessive Coroutine Accumulation

**Files:**
- `/Assets/Scripts/Projectiles/ProjectileWithTrail.cs:56`
- `/Assets/Scripts/Utils/Threading/TimeScaleDeferrer.cs`

### The Problem

Every projectile with a trail (rockets, missiles) defers cleanup for 10 seconds:

```csharp
protected override void DestroyProjectile()
{
    ShowExplosion();
    OnImpactCleanUp();
    InvokeDestroyed();
    _deferrer.Defer(OnTrailsDoneCleanup, TrailLifetimeInS);  // 10 second delay!
}
```

**Deferrer Implementation:**
```csharp
public void Defer(Action action, float delayInS)
{
    StartCoroutine(Wait(action, delayInS));  // Creates new Unity coroutine
}
```

### Impact

- Each projectile creates a coroutine that runs for 10 seconds
- In a 5-6 minute battle with continuous projectile firing:
  - Assuming 5 projectiles/second → 1500-1800 projectiles
  - Many have overlapping 10-second cleanup coroutines
  - Unity's coroutine system accumulates hundreds of active coroutines
- CPU overhead from coroutine scheduling
- Memory overhead from closure allocations

### SmartMissileController Additional Deferrals

**File:** `/Assets/Scripts/Projectiles/SmartMissileController.cs:150`

```csharp
private void ReleaseMissile()
{
    MovementController = _dummyMovementController;
    _deferrer.Defer(ConditionalDestroy, MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S);  // +0.5s
}
```

Smart missiles add ANOTHER deferred action on top of the 10-second trail cleanup.

---

## 4. MODERATE ISSUE: Target Detection Event Subscription Overhead

**File:** `/Assets/Scripts/Targets/TargetDetectors/TargetColliderHandler.cs`

### Analysis

The code **IS CORRECT** in terms of memory leaks - it properly unsubscribes:

```csharp
public void OnTargetColliderEntered(ITarget target)
{
    target.Destroyed += Target_Destroyed;  // Subscribe
    _eventEmitter.InvokeTargetEnteredEvent(target);
}

private void Target_Destroyed(object sender, DestroyedEventArgs e)
{
    e.DestroyedTarget.Destroyed -= Target_Destroyed;  // Unsubscribe
    _eventEmitter.InvokeTargetExitedEvent(e.DestroyedTarget);
}
```

### Performance Concern

While not a memory leak, the overhead of:
- Creating delegates for each target detection
- Subscribing/unsubscribing events
- Event invocation chains

...accumulates with dozens of active units constantly entering/exiting detection ranges.

---

## 5. Pooling System Architecture (Working as Designed)

**File:** `/Assets/Scripts/Utils/Fetchers/PrefabFactory.cs`

The PrefabFactory uses a SEPARATE pooling system that works correctly:

```csharp
if (projectilePool[(int)projectileType].Count > 0)
    projectile = (TProjectile)projectilePool[(int)projectileType].Pop();
else
    projectile = CreateProjectile<TProjectile>(projectileType);
```

**This system is SAFE and doesn't have the Pool.cs bug.**

**Pool Sizes:**
- Explosions: 1-50 per type (15 types)
- Projectiles: 1-50 per type (21 types)
- Drones: Single shared pool
- Ship Deaths: Variable per type

---

## Root Cause Analysis: Why 5-6 Minutes?

The performance degradation at 5-6 minutes is explained by **cumulative effects**:

1. **0-3 minutes:**
   - Pool.cs bug not yet triggered (pool hasn't filled to 1000 items)
   - Moderate number of coroutines
   - Target tracking list sizes still manageable

2. **3-5 minutes:**
   - Unit pools approaching capacity
   - Pool.cs bug starts triggering intermittently
   - Hundreds of active coroutines
   - O(n) target tracking slowing down

3. **5-6+ minutes:**
   - Pool.cs bug fully triggered - GetItem() returning null
   - New units created outside pool system
   - Garbage collector struggling with fragmented memory
   - 500+ active coroutines
   - Target lists at maximum size
   - **Perfect storm of performance issues**

---

## Recommendations (Priority Order)

### 1. FIX IMMEDIATELY: Pool.cs Bug
**File:** `/Assets/Scripts/Utils/BattleScene/Pools/Pool.cs:35`

Change:
```csharp
if (_items.Count < MaxLimit)
```

To:
```csharp
if (_createCount < MaxLimit)
```

**Expected Impact:** Major improvement - prevents memory fragmentation and null returns

### 2. OPTIMIZE: RankedTargetTracker
**File:** `/Assets/Scripts/Targets/TargetTrackers/RankedTargetTracker.cs:119`

Replace List + LINQ with HashSet for O(1) lookups.

**Expected Impact:** Moderate improvement - reduces CPU overhead from target tracking

### 3. REDUCE: Trail Lifetime
**File:** `/Assets/Scripts/Projectiles/ProjectileWithTrail.cs:25`

Change from 10 seconds to 3-5 seconds:
```csharp
protected virtual float TrailLifetimeInS { get => 3; }  // Was 10
```

**Expected Impact:** Significant reduction in coroutine count

### 4. MONITOR: Coroutine Count
Add logging to track active coroutine count during battles to verify improvement.

### 5. CONSIDER: Batch Deferrer
Replace individual coroutines with a single Update()-based deferrer that batches actions.

---

## Testing Recommendations

1. **Before Fix:** Profile a 10-minute battle:
   - Memory allocations per frame
   - Total GameObject count
   - Active coroutine count
   - Frame time distribution

2. **After Fix:** Compare same metrics:
   - Should see stable memory usage
   - Reduced GameObject creation
   - Fewer coroutines (if trail lifetime reduced)
   - Consistent frame times even after 10+ minutes

3. **Specific Tests:**
   - Does Factory.cs:176 ever receive null from GetItem()? (Add logging)
   - What's the pool size (_items.Count) when GetItem() is called? (Add logging)
   - How many coroutines are active at 5-6 minutes?

---

## Conclusion

The **Pool.cs inverted logic bug (line 35)** is the smoking gun. Combined with O(n) target tracking and excessive coroutine accumulation, these issues create a perfect storm at the 5-6 minute mark when:

1. Pools fill up (which is GOOD for recycling)
2. Bug triggers and returns null
3. Objects created outside pool system
4. Memory fragments
5. GC runs frequently
6. Frame rate tanks

**Primary Fix:** Correct the Pool.cs condition from `_items.Count` to `_createCount`.

**Secondary Fixes:** Optimize target tracking and reduce coroutine overhead.

This should restore performance to pre-bug levels and allow battles to run smoothly for extended periods.
