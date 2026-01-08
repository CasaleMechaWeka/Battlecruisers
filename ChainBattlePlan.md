# Chain Battle System - Technical Reference

---

## LLM Context Block

```yaml
system: BattleSequencer
purpose: Spawn buildings, units, and apply boosts during battle via timed sequence points
key_files:
  - Assets/Scripts/Scenes/BattleScene/BattleSequencer.cs  # Main sequencer class
  - Assets/Scripts/Cruisers/Slots/Slot.cs                 # Building placement logic
  - Assets/Scripts/Buildables/Buildings/Building.cs       # Building click handling
  - Assets/Scripts/UI/Common/Click/ClickHandlerWrapper.cs # Click detection
  - Assets/Scripts/Data/Static/StaticData.cs              # Level configuration
  - Assets/Scripts/Levels/ChainBattleController.cs        # Battle utility methods
  - Assets/Scripts/Data/ChainBattleBonus.cs               # Bonus data class

click_chain_for_buildings:
  1_raycast: "Physics2DRaycaster on Main Camera (event mask 688129 = layers 0,15,17,19)"
  2_collider: "BoxCollider2D on inner building GameObject (e.g., 'ArtilleryBuilding')"
  3_handler: "ClickHandlerWrapper implements IPointerClickHandler"
  4_event: "ClickHandler.SingleClick event fires"
  5_building: "Building.OnSingleClick() called"
  6_ui: "UIManager.SelectBuilding(this) called"

building_hierarchy:
  wrapper: "BuildingWrapper (e.g., 'Artillery') - Layer 0, contains pool management"
  inner: "Building (e.g., 'ArtilleryBuilding') - Layer 15, has Collider2D + ClickHandlerWrapper"

slot_building_placement:
  method: "Slot.SlotBuilding setter"
  steps:
    - "Set building rotation to match slot"
    - "Calculate position offset from PuzzleRootPoint"
    - "Set building world position at slot's BuildingPlacementPoint"
  critical: "DO NOT parent building to slot - breaks click detection"

layers:
  0: "Default"
  15: "Units (buildings placed here)"
  17: "Aircraft"
  19: "Slots"

physics2d_raycaster_event_mask: 688129  # Binary: includes layers 0, 15, 17, 19
```

---

## Building Click Detection - How It Works

### Components Required on Building Prefab

1. **BoxCollider2D** - On the inner building GameObject (e.g., "ArtilleryBuilding")
2. **ClickHandlerWrapper** - Implements `IPointerClickHandler`
3. **Layer 15** (Units) - Must be in the Physics2DRaycaster event mask

### Click Flow

```
User Click
    ↓
Physics2DRaycaster (Main Camera, event mask 688129)
    ↓
Raycast hits BoxCollider2D on building (Layer 15)
    ↓
EventSystem calls ClickHandlerWrapper.OnPointerClick()
    ↓
ClickHandler determines single/double click
    ↓
Building.OnSingleClick() called
    ↓
UIManager.SelectBuilding(this) - shows selection UI
```

## Slot.SlotBuilding - Building Placement Logic

The correct implementation (matching commit d6ee442b4f):

```csharp
private IBuilding SlotBuilding
{
    set
    {
        // ... cleanup old building ...
        
        _baseBuilding.Value = value;
        
        if (_baseBuilding.Value != null)
        {
            IBuilding building = _baseBuilding.Value;
            
            // Calculate offset from puzzle root
            float verticalChange = building.Position.y - building.PuzzleRootPoint.y;
            float horizontalChange = building.Position.x - building.PuzzleRootPoint.x;
            
            // Set rotation first
            _baseBuilding.Value.Rotation = Transform.Rotation;
            
            // Position at slot's placement point with offset
            building.Position = BuildingPlacementPoint
                                + (Transform.Up * verticalChange)
                                + (Transform.Right * horizontalChange);
            
            // Handle health bar offset mirroring...
        }
    }
}
```

---

## BattleSequencer Usage

### Creating a Chain Battle

**Option 1: Standalone Sequencer Prefab**
1. Create prefab with `BattleSequencer` component on root (e.g., `SequencerSQ023.prefab`)
2. Configure `sequencePoints` array in inspector
3. Reference in StaticData for automatic loading

**Option 2: Custom Cruiser with Embedded Sequencer**
1. Create custom cruiser prefab (e.g., `CB32Raptor.prefab`)
2. Add `BattleSequencer` component to cruiser GameObject
3. Configure sequence points directly on cruiser
4. Reference in StaticData - loads cruiser with built-in battle sequence

**Note**: Chain battles no longer use ChainCruiser or CruiserSection classes. The system now uses standard Cruiser class with BattleSequencer for scripted events.

### Sequence Point Structure

```csharp
[Serializable]
public class SequencePoint
{
    public int DelayMS = 0;           // Delay before executing
    public Faction Faction;            // Blues (player) or Reds (AI)
    public List<BuildingAction> BuildingActions;
    public List<BoostAction> BoostActions;
    public List<UnitAction> UnitActions;
    public ScriptCallAction ScriptCallActions;  // Unity events
}
```

### BuildingAction

```csharp
public enum BuildingOp { Add = 0, Destroy = 1 }

public class BuildingAction
{
    public BuildingOp Operation;
    public PrefabKeyName PrefabKeyName;
    public byte SlotID;
    public bool IgnoreDroneReq = true;   // Use 1 drone, multiply build time
    public bool IgnoreBuildTime = true;  // Instant construction
}
```

### Default Settings

- `IgnoreDroneReq = true` - Avoids assertion errors when cruiser lacks drones
- `IgnoreBuildTime = true` - Instant spawning for cinematic sequences

---

## ChainBattleController

Utility class for battle scripting. Attach to BattleSequencer prefab and use via ScriptCallActions:

```csharp
public class ChainBattleController : MonoBehaviour
{
    public static void ApplyDamageToEnemy(float amount);
    public static void MakeAICruiserInvincible();
    public static void MakeAICruiserDamagable();
}
```

---

## Troubleshooting

### Building Cannot Be Selected (No Click Response)

**Debug Steps:**
1. Click building, check console for:
   - `[Slot.OnPointerClick]` - Slot received click (building didn't)
   - `[Building.OnSingleClick]` - Building received click (working)

2. If only Slot logs appear:
   - Check building has ClickHandlerWrapper component
   - Check building has BoxCollider2D on correct layer (15)
   - **Check Slot.cs for SetParent call - REMOVE IT if present**

3. If Building logs appear but no UI:
   - Check `_uiManager` is not null in Building.OnSingleClick()

### Building Spawns at Wrong Position

**Check:**
- BuildingPlacementPoint child exists on slot prefab
- PuzzleRootPoint exists on building prefab
- No SetParent call interfering with position

### NullReferenceException in Buildable.Update()

**Cause:** Building's ParentCruiser or BuildProgressCalculator is null

**Fix:** Ensure BattleSequencer.Cruisers array is set by BattleSceneGod before StartF()

---

## Multi-Section Cruiser System

### Overview

The system enables a single `Cruiser` class to handle 1-N independent, targetable sections.

### Key Files

| File | Purpose |
|------|---------|
| Cruiser.cs | Base class, handles both single and multi-section |
| CruiserSection.cs | Individual targetable section component |
| CruiserFactory.cs | Automatic section detection and initialization |
| GlobalTargetFinder.cs | Emits sections as targets |

### Detection Pattern

- **Single-section:** Cruiser root has SpriteRenderer + Collider2D, no CruiserSection children
- **Multi-section:** Cruiser root has NO renderer/collider, has 2+ CruiserSection children

### Health Behavior

- `Cruiser.Health` returns primary section's health (for UI)
- Each section tracks health independently
- Primary section destruction = game over
- Secondary section destruction = battle continues + score

---

## Version History

### v4.0 - Click Detection Fix (January 6, 2026)
- **Fixed:** Removed SetParent call from Slot.SlotBuilding that broke click detection
- **Restored:** Original building placement logic (position only, no parenting)
- **Reference commit:** d6ee442b4f - known working state

### v3.x - Multi-Section Cruisers (January 2-4, 2026)
- Inverted architecture from inheritance to composition
- **REVERTED** - Deleted ChainCruiser and CruiserSection classes
- Returned to standard Cruiser class for all cruiser types

---

## Key Files Reference

| File | Purpose |
|------|---------|
| BattleSequencer.cs | Main chain battle sequencer |
| Slot.cs | Building placement on slots |
| Building.cs | Building click handling |
| ClickHandlerWrapper.cs | Unity EventSystem click detection |
| ChainBattleController.cs | Battle utility methods |
| ChainBattleBonus.cs | Bonus data class |
| StaticData.cs | Level and chain battle configuration |

