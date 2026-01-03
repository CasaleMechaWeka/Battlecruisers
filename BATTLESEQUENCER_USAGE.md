# BattleSequencer Usage Guide

## Testing Chain Battles

To test level 32 (or any chain battle) directly in BattleSceneGod:

1. Open `BattleSceneGod` in Unity Inspector
2. Set `defaultLevel = 32`
3. Set `isTutorial = false`
4. Play the scene

**This will now properly:**
- Set `ApplicationModel.Mode = GameMode.Campaign`
- Set `ApplicationModel.SelectedLevel = 32`
- Load the chain battle sequencer
- Initialize additional phases

## Understanding IgnoreDroneReq and IgnoreBuildTime

### IgnoreDroneReq (Default: TRUE)
- **false**: Requires ALL drones specified by building
  - ❌ **CAUSES ASSERTION ERROR** if cruiser doesn't have enough drones
  - Example: Building needs 3 drones, but cruiser only has 2 → CRASH
- **true**: Uses only 1 drone, multiplies build time accordingly
  - ✅ Always works, never crashes
  - Building still constructs normally, just with 1 drone doing all the work

### IgnoreBuildTime (Default: TRUE)
- **false**: Normal build time (builds over time)
  - Building constructs at normal speed
  - You'll see construction progress
- **true**: Instant construction
  - Sets boost multiplier to 1e9 (effectively instant)
  - Building appears immediately

### Recommended Settings for Sequencer

```csharp
public bool IgnoreDroneReq = true;  // Avoid drone assertions
public bool IgnoreBuildTime = true; // Instant for cinematics
```

Only set to `false` if:
- You're **certain** the cruiser has enough drones (IgnoreDroneReq)
- You want to show construction time for dramatic effect (IgnoreBuildTime)

## DroneConsumerProvider Assertion (Line 25)

**Error**: `AssertionException: Assertion failure. Value was False`

**Cause**: `_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired)` returns false

**This happens when:**
1. IgnoreDroneReq = false (requires all drones)
2. Building needs X drones
3. Cruiser doesn't have X available drones

**Fix**: Set `IgnoreDroneReq = true` (now the default)

## Factory Requirement Checking

The `RequiredFactory` field in UnitAction is **currently unused** in the code.

If you need to check if a cruiser has a specific factory before spawning units, you would need to implement logic like:

```csharp
// Check if cruiser has required factory
bool hasFactory = false;
foreach (Slot slot in cruiser.SlotWrapperController.Slots)
{
    if (slot.Building.Value != null)
    {
        string factoryName = unitAction.RequiredFactory.ToString().Split('_')[1];
        if (slot.Building.Value.keyName == factoryName)
        {
            hasFactory = true;
            break;
        }
    }
}

if (!hasFactory)
{
    Debug.LogWarning($"Cruiser missing required factory: {unitAction.RequiredFactory}");
    continue; // Skip this unit
}
```

## Chain Battle Setup Example

See `Sequencer32.prefab` for a working example of:
- Multiple enemy phases
- Timed building spawns
- Unit spawns
- Boost applications

## Common Pitfalls

1. **Setting defaultLevel without setting Mode** → Fixed! Now automatic
2. **IgnoreDroneReq = false** → Assertion crashes. Use true.
3. **Trying to access building.PrefabKey** → Doesn't exist. Use building.keyName
4. **Using PrefabKeyName.None** → Doesn't exist. Enum starts at Hull_Trident (0)
