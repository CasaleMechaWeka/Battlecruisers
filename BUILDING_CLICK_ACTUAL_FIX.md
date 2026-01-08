# Building Click Detection - ROOT CAUSE FOUND ✅

## **THE ACTUAL PROBLEM:**

**`ClickHandler.cs` was refactored to add triple-click support but broke single-click detection!**

---

## Working vs Broken Code

### ✅ **Production Version (WORKS)**

```csharp
public void OnClick(float timeSinceGameStartInS)
{
    if ((timeSinceGameStartInS - _lastClickTime) <= _doubleClickThresholdInS)
    {
        // Double click - FIRES IMMEDIATELY
        DoubleClick?.Invoke(this, EventArgs.Empty);
        _lastClickTime = float.MinValue;
    }
    else
    {
        // Single click - FIRES IMMEDIATELY
        SingleClick?.Invoke(this, EventArgs.Empty);
        _lastClickTime = timeSinceGameStartInS;
    }
}
```

### ❌ **Current Version (BROKEN)**

```csharp
public void OnClick(float timeSinceGameStartInS)
{
    _clickCount++;
    
    if (_clickCount == 1)
    {
        // First click - DOES NOTHING!
        _firstClickTime = timeSinceGameStartInS;
        _lastClickTime = timeSinceGameStartInS;
        return;  // ← SingleClick NEVER FIRES!
    }
    
    // Complex debouncing logic for triple-click...
    // SingleClick only fires when you click AGAIN after threshold
}
```

---

## Why It Broke

The new implementation tried to add triple-click support with a debouncing mechanism:

1. **First click**: `_clickCount = 1`, then `return` → **No event fired**
2. **Second click (fast)**: `_clickCount = 2`, waits for possible triple-click → **No event fired yet**
3. **Third click OR next click after threshold**: Finally processes previous clicks

**The problem**: `SingleClick` only fires when you click a **second time** (outside the threshold). If you click once and don't click again, **NOTHING HAPPENS**.

This is why:
- ✅ **Double-clicks work** (second click triggers double-click event)
- ❌ **Single-clicks broken** (need a second slow click to trigger)
- ✅ **Slots work** (use `IPointerClickHandler` directly, not `ClickHandler`)

---

## The Fix

**Reverted `ClickHandler.cs` to the working production version** from commit `8d985d3354239b7c9ef7350fe88348b099e31037`.

```bash
git checkout 8d985d3354239b7c9ef7350fe88348b099e31037 -- Assets/Scripts/UI/Common/Click/ClickHandler.cs
```

---

## Symptoms Explained

| Symptom | Explanation |
|---------|-------------|
| Can't select buildings (single-click) | SingleClick event never fires on first click |
| Can't select cruisers (single-click) | Same issue - both use ClickHandler |
| CAN target enemies (double-click) | DoubleClick event works because it triggers on second click |
| CAN click slots | Slots use `IPointerClickHandler` directly, bypass `ClickHandler` |

---

## Investigation Red Herrings

These were investigated but were NOT the cause:

- ❌ Slot colliders blocking buildings - Both have colliders, both in raycast mask, but this wasn't the issue
- ❌ `SetParent` call breaking hierarchy - Already removed in earlier commit
- ❌ Layer configuration - Correct (Layer 15 for buildings, in event mask 688129)
- ❌ `ClickHandlerWrapper` missing - Present and working correctly
- ❌ `BoxCollider2D` missing - Present on all building prefabs
- ❌ `Physics2DRaycaster` misconfigured - Correct event mask, working fine
- ❌ Prefab instantiation issues - No problems with instantiation
- ❌ `Artillery.cs` deletion - Red herring from refactoring, not the issue

---

## How Click Detection Actually Works

```
User Click
    ↓
Physics2DRaycaster (Main Camera, event mask 688129)
    ↓
Raycast hits BoxCollider2D on building (Layer 15)
    ↓
EventSystem calls ClickHandlerWrapper.OnPointerClick()
    ↓
ClickHandlerWrapper.OnPointerClick() calls _clickHandler.OnClick()
    ↓
ClickHandler.OnClick() determines single/double click
    ↓ [BROKEN HERE IN CURRENT VERSION]
    ↓
ClickHandler.SingleClick event fires
    ↓
Building.ClickHandler_SingleClick() called
    ↓
Building.OnSingleClick() called
    ↓
UIManager.SelectBuilding(this) - shows selection UI
```

---

## Lessons Learned

1. **When debugging click issues, check the event firing logic, not just the raycasting**
2. **Simple click detection is often better than complex debouncing**
3. **Triple-click support requires a timer/coroutine, not just state tracking**
4. **User feedback is critical** - "works for double-click but not single-click" was the key clue

---

Generated: January 6, 2026
**Fixed by reverting ClickHandler.cs to production version**

