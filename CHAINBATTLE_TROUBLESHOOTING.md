# ChainBattle Troubleshooting Plan - Building Spawn Issues

## 🔴 Problem Summary

**Symptom**: Cannons built via BattleSequencer sequence points build but cannot fire, with NullReferenceException errors.

**Error**: `NullReferenceException` in `Buildable.cs:476` during `Update()` method
**Root Cause**: Buildings spawned via sequence points are missing critical initialization

---

## 🔍 Analysis

### The Error Chain

1. **Line 476** (`if (BuildProgress > MAX_BUILD_PROGRESS)`) - Error occurs here
2. **Line 471** is the actual problem:
   ```csharp
   float buildProgressInDroneS = ParentCruiser.BuildProgressCalculator.CalculateBuildProgressInDroneS(this, _time.DeltaTime);
   ```
3. **Likely null references**:
   - `ParentCruiser` is null, OR
   - `ParentCruiser.BuildProgressCalculator` is null, OR
   - `_time` is null

### Why This Happens

**Current BattleSequencer Code** (BattleSequencer.cs:102):
```csharp
cruiser.ConstructBuilding(building, slot, buildingAction.IgnoreDroneReq, buildingAction.IgnoreBuildTime);
```

**What ConstructBuilding Does** (Cruiser.cs:333-365):
1. Creates building via `PrefabFactory.CreateBuilding()` ✅
2. Calls `building.Activate(new BuildingActivationArgs(...))` ✅
3. Sets `slot.SetBuilding(building)` ✅
4. Calls `building.StartConstruction()` ✅
5. If `ignoreBuildTime=true`, sets `BuildProgressBoostable.BoostMultiplier = 1e9f` ⚠️

**The Problem**:
- `ignoreBuildTime` speeds up construction but building still goes through Update() loop
- Building enters `BuildableState.InProgress` state
- `Update()` tries to calculate build progress but references are null
- Building gets stuck in limbo - not fully constructed, can't fire

---

## 🎯 Root Cause: ChainBattle Cruiser Not Fully Initialized

### Theory 1: Enemy Cruiser from Prefab vs BattleSceneGod

**Normal Campaign Flow**:
```
BattleSceneGod.Initialise()
├── Creates playerCruiser (CruiserFactory)
├── Creates aiCruiser (CruiserFactory)
├── InitialisePlayerCruiser(playerCruiser, aiCruiser) ← Sets up enemy refs
├── InitialiseAICruiser(aiCruiser, playerCruiser) ← Sets up enemy refs
└── Loads BattleSequencer
    └── battleSequencer.Cruisers = [playerCruiser, aiCruiser]
```

**ChainBattle Flow**:
```
BattleSceneGod.Initialise()
├── Creates playerCruiser (CruiserFactory)
├── Creates aiCruiser (CruiserFactory) ← From BattleSceneGod, NOT prefab
├── InitialisePlayerCruiser(playerCruiser, aiCruiser)
├── InitialiseAICruiser(aiCruiser, playerCruiser)
└── Loads ChainBattle Prefab
    ├── Prefab has child cruiser GameObject ⚠️ DIFFERENT cruiser instance!
    └── battleSequencer.Cruisers = [playerCruiser, aiCruiser] ← BattleSceneGod's aiCruiser
```

**The Mismatch**:
- BattleSequencer.Cruisers[1] = aiCruiser from BattleSceneGod
- ChainBattle prefab's child cruiser GameObject = DIFFERENT cruiser instance
- Sequence points use `Cruisers[(int)sq.Faction]` to get cruiser
- If Faction=Enemy, gets BattleSceneGod's aiCruiser (correct)
- But if ChainBattle prefab has its own cruiser child, that one is never initialized!

### Theory 2: Missing BuildProgressCalculator

**BuildProgressCalculator Setup** (should happen in Cruiser.Activate):
```csharp
// CruiserArgs contains everything needed for activation
public virtual void Activate(CruiserArgs args)
{
    // Sets up BuildProgressCalculator, DroneConsumerProvider, etc.
}
```

**If BattleSceneGod's aiCruiser is used**, it should be properly activated.
**If ChainBattle prefab's child cruiser is used**, it might NOT be activated.

### Theory 3: Time Reference Missing

Buildings need `_time` reference (ITime) for delta time calculations.
This is set in `Buildable.Activate()` but might not be passed correctly.

---

## 📋 Detailed Troubleshooting Steps

### Phase 1: Verify ChainBattle Prefab Structure ✅

**Task**: Check what's actually in the ChainBattle prefab

**Steps**:
1. Open ChainBattle_032.prefab in Unity
2. Check Hierarchy structure
3. Document EXACTLY what's there

**Questions to Answer**:
- ❓ Is there a child cruiser GameObject in the prefab?
- ❓ If yes, does it have Cruiser component attached?
- ❓ Does BattleSequencer reference this child cruiser or BattleSceneGod's cruiser?
- ❓ Are there any buildings pre-placed in the prefab?

**Expected Correct Structure**:
```
ChainBattle_032 (root)
└── BattleSequencer component
    └── sequencePoints array (configured)

NO child cruiser GameObject!
```

**Why**: The enemy cruiser should come from BattleSceneGod, NOT the prefab.

---

### Phase 2: Verify BattleSceneGod Loading ✅

**Task**: Confirm how ChainBattle prefabs are loaded

**File**: `BattleSceneGod.cs`

**Check Loading Code** (around line 534-545):
```csharp
if (ApplicationModel.Mode == GameMode.Campaign &&
    StaticData.IsChainBattleLevel(ApplicationModel.SelectedLevel))
{
    string path = StaticData.GetChainBattleSequencerPath(levelNum);
    var handle = Addressables.LoadAssetAsync<GameObject>(path);
    await handle.Task;

    if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
    {
        battleSequencer = Instantiate(handle.Result, transform)
            .GetComponent<BattleSequencer>();
        battleSequencer.Cruisers = new[] { playerCruiser, aiCruiser }; ← KEY LINE
        battleSequencer.StartF();
    }
}
```

**Questions**:
- ❓ Does this code execute for level 32?
- ❓ Are `playerCruiser` and `aiCruiser` the SAME instances that were initialized earlier?
- ❓ Does the instantiated prefab have child cruiser GameObjects that override these?

**Add Debug Logging**:
```csharp
Debug.Log($"[ChainBattle] Loading sequencer for level {levelNum}");
Debug.Log($"[ChainBattle] playerCruiser: {playerCruiser.name} (ID: {playerCruiser.GetInstanceID()})");
Debug.Log($"[ChainBattle] aiCruiser: {aiCruiser.name} (ID: {aiCruiser.GetInstanceID()})");
Debug.Log($"[ChainBattle] aiCruiser._enemyCruiser: {(aiCruiser as Cruiser)?._enemyCruiser?.name ?? "NULL"}");
Debug.Log($"[ChainBattle] aiCruiser.BuildProgressCalculator: {aiCruiser.BuildProgressCalculator != null}");
```

---

### Phase 3: Verify Sequence Point Execution ✅

**Task**: Confirm which cruiser is used when spawning buildings

**File**: `BattleSequencer.cs`

**Add Debug Logging** (line 69 in ProcessSequencePoint):
```csharp
public async Task ProcessSequencePoint(SequencePoint sq)
{
    await Task.Delay(sq.DelayMS);
    Cruiser cruiser = Cruisers[(int)sq.Faction];

    // ADD THIS DEBUG LOGGING
    Debug.Log($"[BattleSequencer] Processing sequence point for {sq.Faction}");
    Debug.Log($"[BattleSequencer] Cruiser: {cruiser.name} (ID: {cruiser.GetInstanceID()})");
    Debug.Log($"[BattleSequencer] Cruiser._enemyCruiser: {cruiser._enemyCruiser?.name ?? "NULL"}");
    Debug.Log($"[BattleSequencer] Cruiser.BuildProgressCalculator: {cruiser.BuildProgressCalculator != null}");

    // Execute Building Actions
    if (sq.BuildingActions != null)
        foreach (SequencePoint.BuildingAction buildingAction in sq.BuildingActions)
        {
            Debug.Log($"[BattleSequencer] Building action: {buildingAction.Operation} {buildingAction.PrefabKeyName} in slot {buildingAction.SlotID}");
            // ... rest of code
```

---

### Phase 4: Verify Building Activation ✅

**Task**: Confirm building is properly activated with all references

**File**: `BattleSequencer.cs` (after cruiser.ConstructBuilding call)

**Add Debug Logging** (around line 102):
```csharp
if ((prefabID >= (int)Building_AirFactory) && (prefabID < (int)Unit_Bomber))
{
    IBuildableWrapper<IBuilding> building = PrefabFactory.GetBuildingWrapperPrefab(...);
    IList<Slot> slots = cruiser.SlotAccessor.GetFreeSlots(...);
    Slot slot = slots[Math.Min(slots.Count - 1, buildingAction.SlotID)];

    if (slot == null)
    {
        Debug.LogError($"{slot.type} Slot #{buildingAction.SlotID} is already occupied!");
        return;
    }

    // ADD DEBUG BEFORE CONSTRUCT
    Debug.Log($"[BattleSequencer] About to construct {buildingAction.PrefabKeyName}");
    Debug.Log($"[BattleSequencer] Cruiser for construction: {cruiser.name}");
    Debug.Log($"[BattleSequencer] Cruiser.CruiserSpecificFactories: {cruiser.CruiserSpecificFactories != null}");

    IBuilding constructedBuilding = cruiser.ConstructBuilding(building, slot,
        buildingAction.IgnoreDroneReq, buildingAction.IgnoreBuildTime);

    // ADD DEBUG AFTER CONSTRUCT
    Debug.Log($"[BattleSequencer] Building constructed: {constructedBuilding.Name}");
    Debug.Log($"[BattleSequencer] Building.ParentCruiser: {constructedBuilding.ParentCruiser?.name ?? "NULL"}");
    Debug.Log($"[BattleSequencer] Building.EnemyCruiser: {constructedBuilding.EnemyCruiser?.name ?? "NULL"}");
    Debug.Log($"[BattleSequencer] Building.BuildableState: {constructedBuilding.BuildableState}");
}
```

---

### Phase 5: Fix IgnoreBuildTime Logic ⚠️

**Current Problem**: `ignoreBuildTime` still lets building go through Update() loop

**File**: `Cruiser.cs:354-355`

**Current Code**:
```csharp
if (ignoreBuildTime)
    building.BuildProgressBoostable.BoostMultiplier = 1e9f;
```

**This approach**:
- Building still enters InProgress state
- Update() still runs every frame
- Relies on massive multiplier to finish "instantly"
- BUT if any reference is null, Update() crashes before completion

**Better Approach - Option A**: Skip construction entirely
```csharp
if (ignoreBuildTime)
{
    // Skip construction, mark as complete immediately
    building.FinishConstruction(); // Might need to call this differently
}
else
{
    building.StartConstruction();
}
```

**Better Approach - Option B**: Check for completion immediately
```csharp
if (ignoreBuildTime)
{
    building.BuildProgressBoostable.BoostMultiplier = 1e9f;
    building.StartConstruction();

    // Force immediate completion (manually invoke completion)
    // This might require exposing OnBuildableCompleted() or similar
}
else
{
    building.StartConstruction();
}
```

**Investigation Needed**:
- ❓ Does `Buildable` have a public method to force completion?
- ❓ Can we bypass StartConstruction entirely for instant builds?
- ❓ Is there a `FinishConstruction()` method?

---

### Phase 6: Check Buildable Lifecycle ✅

**Task**: Understand building construction states

**File**: `Buildables/Buildable.cs`

**Key Methods to Check**:
```csharp
public void Activate(...) // Sets up ParentCruiser, EnemyCruiser, _time
public void StartConstruction() // Enters InProgress state
protected void OnBuildableCompleted() // Completes construction
public void FinishConstruction() // ← Does this exist?
```

**Search for**:
- How to bypass construction entirely
- Public methods to force completion
- State transitions

---

## 🛠️ Proposed Solutions

### Solution 1: Remove Cruiser from ChainBattle Prefab ✅ (Recommended)

**Reason**: ChainBattle prefab should ONLY contain BattleSequencer component

**Steps**:
1. Open all ChainBattle prefabs
2. Delete any child cruiser GameObjects
3. Ensure only root GameObject with BattleSequencer exists
4. BattleSceneGod provides the aiCruiser at runtime

**Why This Works**:
- Uses properly initialized cruiser from BattleSceneGod
- All references (enemy cruiser, factories, calculators) already set up
- No mismatch between prefab cruiser and runtime cruiser

---

### Solution 2: Fix IgnoreBuildTime to Bypass Update() ✅

**Reason**: Buildings shouldn't enter InProgress state if they're instant

**Approach A - Immediate Completion**:
```csharp
// In Cruiser.ConstructSelectedBuilding()
if (ignoreBuildTime)
{
    // Don't start construction - complete immediately
    building.CompletedBuildable += Building_CompletedBuildable;
    building.Destroyed += Building_Destroyed;

    // Manually set to completed state
    ((Buildable<BuildingActivationArgs>)building).FinishConstruction();

    _helper.OnBuildingConstructionStarted(building, SlotAccessor, SlotHighlighter);
    BuildingStarted?.Invoke(this, new BuildingStartedEventArgs(building));
}
else
{
    // Normal construction flow
    building.CompletedBuildable += Building_CompletedBuildable;
    building.Destroyed += Building_Destroyed;
    building.StartConstruction();
    _helper.OnBuildingConstructionStarted(building, SlotAccessor, SlotHighlighter);
    BuildingStarted?.Invoke(this, new BuildingStartedEventArgs(building));
}
```

**Approach B - Validate Before Update** (Safer):
```csharp
// In Buildable.cs Update()
public void Update()
{
    if (BuildableState == BuildableState.InProgress)
    {
        // ADD SAFETY CHECK
        if (ParentCruiser == null || ParentCruiser.BuildProgressCalculator == null || _time == null)
        {
            Debug.LogError($"[Buildable] {Name} has null references during Update! ParentCruiser={ParentCruiser != null}, Calculator={ParentCruiser?.BuildProgressCalculator != null}, Time={_time != null}");
            return; // Skip update if references missing
        }

        Assert.IsTrue(DroneConsumer.State != DroneConsumerState.Idle);
        // ... rest of Update()
```

---

### Solution 3: Enhanced BattleSequencer Validation ✅

**Add pre-flight checks before spawning buildings**:

```csharp
// In BattleSequencer.ProcessSequencePoint() before BuildingActions
if (sq.BuildingActions != null && sq.BuildingActions.Count > 0)
{
    // Validate cruiser is ready for building construction
    if (cruiser.CruiserSpecificFactories == null)
    {
        Debug.LogError($"[BattleSequencer] Cruiser {cruiser.name} missing CruiserSpecificFactories!");
        return;
    }

    if (cruiser.BuildProgressCalculator == null)
    {
        Debug.LogError($"[BattleSequencer] Cruiser {cruiser.name} missing BuildProgressCalculator!");
        return;
    }

    if ((cruiser as Cruiser)?._enemyCruiser == null)
    {
        Debug.LogError($"[BattleSequencer] Cruiser {cruiser.name} missing enemy cruiser reference!");
        return;
    }

    Debug.Log($"[BattleSequencer] Cruiser validation passed, proceeding with building spawns");
}
```

---

## 📊 Testing Plan

### Test 1: Prefab Structure
1. Open ChainBattle_032.prefab
2. Verify NO child cruiser GameObjects
3. Verify ONLY BattleSequencer component on root

### Test 2: Debug Logging
1. Add all debug logs from Phase 2-4
2. Play level 32
3. Capture console output
4. Analyze:
   - Which cruiser instance is used?
   - Are references non-null?
   - Does building activate correctly?

### Test 3: Building Spawn
1. Create sequence point with building at 0ms
2. Play level
3. Verify:
   - Building appears in slot
   - No NullReferenceException
   - Building can fire at player

### Test 4: Animation Trigger
1. Add animation trigger to sequence point
2. Verify animation plays
3. Verify no errors

---

## 🎯 Recommended Execution Order

1. **First**: Verify prefab structure (Phase 1)
   - If prefab has child cruiser → Delete it

2. **Second**: Add debug logging (Phases 2-4)
   - Run test, capture logs
   - Identify which references are null

3. **Third**: Implement Solution 1 (remove cruiser from prefab)
   - Test again with debug logs

4. **Fourth**: If still broken, implement Solution 2 (fix ignoreBuildTime)
   - Check if FinishConstruction() exists
   - Bypass Update() loop for instant buildings

5. **Fifth**: Add Solution 3 (validation) as safety net
   - Prevents crashes with clear error messages

---

## ❓ Questions for User

Before proceeding, please confirm:

1. **ChainBattle Prefab Structure**:
   - Does your ChainBattle_032.prefab have a child cruiser GameObject?
   - If yes, did you manually place it there or was it added by code?

2. **Expected Behavior**:
   - Should buildings spawn instantly (0 build time) or have visible construction?
   - Should buildings spawn at battle start (0ms) or during battle?

3. **Error Timing**:
   - Does the error occur immediately when battle starts?
   - Or does it occur when you try to fire the cannon?
   - Can you share the FULL error stack trace?

4. **Testing Approach**:
   - Do you want me to implement debug logging first (non-invasive)?
   - Or go straight to fixing the prefab structure?

---

## 📝 Next Steps

Once you review this plan and answer the questions, I'll:

1. Make the appropriate code changes
2. Add comprehensive debug logging
3. Test and verify the fix
4. Update documentation with correct workflow
5. Commit and push fixes

Let me know which approach you'd like to take!
