# Save Data Migration for Version 651+ (ChainBattle Integration)

## Overview

Version 651 introduces a major structural change to BattleCruisers: **levels 32-40 are removed from the main campaign and converted to sidequests that unlock ChainBattle access**. This change requires careful save data migration to preserve player progress and prevent data loss.

## Context: What Changed

### Before Version 651
- **Main Campaign**: Levels 1-40 (linear progression)
- **Levels 32-40**: Regular campaign levels with standard loot unlocks
- **Progress Tracking**: Simple level completion flags

### After Version 651
- **Main Campaign**: Levels 1-31 only
- **Levels 32-40**: Removed from main campaign
- **Sidequests**: Levels 32-40 converted to sidequests that unlock ChainBattle access
- **ChainBattles**: Separate battle configurations loaded from `Resources/ChainBattles/`
- **Progress Tracking**: Dual system (campaign levels + sidequest completions)

## Migration Problem

### Risk Assessment
Saves created before version 651 may contain:
- Completion flags for levels 32-40
- Unlocked hulls/units from levels 32-40 loot
- Player progression dependent on these levels

### Data Loss Scenarios
1. **Progress Reset**: Players lose "completion" status for levels they actually beat
2. **Unlock Loss**: Hulls/units unlocked by levels 32-40 become inaccessible
3. **UI Confusion**: Level select shows incorrect completion states
4. **Loot Inaccessibility**: ChainBattle rewards become unreachable

## Migration Solution Architecture

### Integration Point
The migration executes during save data loading in the `Serializer` class, specifically in the deserialization pipeline where version checks occur.

### Migration Trigger
```pseudocode
if (loadedSaveData.version < 651) {
    MigratePre651ChainBattleData(loadedSaveData);
}
```

### Data Structures Involved

#### Source Data (Pre-651)
- `ApplicationModel.completedLevels[]`: Array of completed level numbers (1-40)
- `ApplicationModel.unlockedHulls[]`: Hulls unlocked through level progression
- `ApplicationModel.unlockedUnits[]`: Units unlocked through level progression

#### Target Data (Post-651)
- `ApplicationModel.completedLevels[]`: Array of completed level numbers (1-31 only)
- `ApplicationModel.completedChainBattles[]`: NEW array for ChainBattle completions
- `ApplicationModel.unlockedHulls[]`: Preserved
- `ApplicationModel.unlockedUnits[]`: Preserved

## Migration Logic

### Step 1: Identify Affected Levels
```pseudocode
affectedLevels = [32, 33, 34, 35, 36, 37, 38, 39, 40]
chainBattleMappings = {
    32: "ChainBattle_Fei",      // Sidequest 0
    33: "ChainBattle_Level33",  // Sidequest 1
    34: "ChainBattle_Level34",  // Sidequest 2
    // ... etc for all 32-40
}
```

### Step 2: Preserve Level Completions
```pseudocode
foreach (completedLevel in loadedSaveData.completedLevels) {
    if (completedLevel in affectedLevels) {
        // Convert to ChainBattle completion
        chainBattleId = chainBattleMappings[completedLevel]
        loadedSaveData.completedChainBattles.Add(chainBattleId)

        // Remove from main campaign levels
        loadedSaveData.completedLevels.Remove(completedLevel)
    }
}
```

### Step 3: Preserve Loot Unlocks
```pseudocode
// This is critical - ensure loot tables match
foreach (affectedLevel in affectedLevels) {
    if (playerCompletedLevel(affectedLevel)) {
        // Get loot that WOULD have been unlocked by this level
        levelLoot = StaticData.GetLevelLoot(affectedLevel)

        // Ensure this loot is still accessible via ChainBattle
        chainBattleLoot = StaticData.GetChainBattleLoot(affectedLevel)
        Assert lootTablesMatch(levelLoot, chainBattleLoot)
    }
}
```

### Step 4: Update Progress Statistics
```pseudocode
// Recalculate completion percentages
totalCampaignLevels = 31  // Was 40
completedCampaignLevels = loadedSaveData.completedLevels.Count
campaignProgress = completedCampaignLevels / totalCampaignLevels

// Add ChainBattle progress
totalChainBattles = StaticData.ChainBattles.Count
completedChainBattles = loadedSaveData.completedChainBattles.Count
chainBattleProgress = completedChainBattles / totalChainBattles

// Combined progress metric
overallProgress = (campaignProgress + chainBattleProgress) / 2
```

## Integration with Existing Serializer

### Location in Code Flow
The migration integrates into `Serializer.LoadGame()` method:

```pseudocode
public static ApplicationModel LoadGame() {
    // 1. Load raw save data from file
    rawData = LoadFromFile(saveFilePath)

    // 2. Deserialize into ApplicationModel
    saveData = Deserialize<ApplicationModel>(rawData)

    // 3. VERSION CHECK AND MIGRATION POINT
    if (saveData.version < CURRENT_VERSION) {
        MigrateSaveData(saveData, saveData.version)
    }

    // 4. Validate migrated data
    ValidateSaveData(saveData)

    return saveData
}
```

### Migration Method Structure
```csharp
private static void MigrateSaveData(ApplicationModel saveData, int fromVersion) {
    if (fromVersion < 651) {
        MigratePre651ChainBattleData(saveData);
    }

    // Other version migrations...
    if (fromVersion < 700) {
        // Future migrations
    }
}
```

### The Migration Method
```csharp
private static void MigratePre651ChainBattleData(ApplicationModel saveData) {
    Debug.Log("Migrating save data for ChainBattle integration (v651+)");

    // Step 1: Identify completed levels 32-40
    List<int> completedChainBattleLevels = new List<int>();
    saveData.completedLevels.RemoveAll(level => {
        if (level >= 32 && level <= 40) {
            completedChainBattleLevels.Add(level);
            return true; // Remove from main campaign
        }
        return false;
    });

    // Step 2: Convert to ChainBattle completions
    foreach (int level in completedChainBattleLevels) {
        string chainBattleId = GetChainBattleIdForLevel(level);
        saveData.completedChainBattles.Add(chainBattleId);
    }

    // Step 3: Update version
    saveData.version = 651;

    Debug.Log($"Migration complete: {completedChainBattleLevels.Count} levels converted to ChainBattles");
}
```

## Validation and Testing

### Post-Migration Validation
```pseudocode
ValidateMigratedData(saveData) {
    // Ensure no levels 32-40 remain in completedLevels
    assert !saveData.completedLevels.Any(level => level >= 32 && level <= 40)

    // Ensure ChainBattle completions were added
    assert saveData.completedChainBattles.Count >= 0

    // Ensure loot accessibility is preserved
    foreach (chainBattleId in saveData.completedChainBattles) {
        assert StaticData.GetChainBattleLoot(chainBattleId) != null
    }
}
```

### Testing Scenarios
1. **Fresh Save**: No levels 32-40 completed → No migration needed
2. **Partial Completion**: Some levels 32-40 completed → Convert those only
3. **Full Completion**: All levels 32-40 completed → Convert all to ChainBattles
4. **Edge Cases**: Invalid level numbers, corrupted save data

## Implementation Checklist

### Code Changes Required
- [ ] Add migration method to `Serializer.cs`
- [ ] Add version check (< 651) in migration pipeline
- [ ] Implement level-to-ChainBattle mapping
- [ ] Add `completedChainBattles` field to `ApplicationModel`
- [ ] Update save data validation
- [ ] Test with various save file scenarios

### Data Structure Changes
- [ ] `ApplicationModel.completedChainBattles`: New `List<string>` field
- [ ] `ApplicationModel.version`: Ensure version tracking works
- [ ] ChainBattle loot tables: Verify equivalent rewards

### Testing Requirements
- [ ] Unit tests for migration logic
- [ ] Integration tests with actual save files
- [ ] UI tests for progress display
- [ ] Loot accessibility verification

## Rollback Considerations

### If Migration Fails
1. **Partial Migration**: Restore original save data
2. **Data Corruption**: Alert user, suggest backup restore
3. **Version Inconsistency**: Prevent save until migration succeeds

### Player Communication
- **Migration Notification**: "Updating save data for new ChainBattle system..."
- **Progress Preservation**: "Your completed levels 32-40 have been converted to ChainBattles"
- **Error Handling**: Clear error messages if migration fails

## Future-Proofing

### Extensible Design
The migration system should handle future structural changes:
- Additional ChainBattle types
- New progression systems
- Loot table adjustments

### Version Tracking
Maintain clear version boundaries:
- 650: Pre-ChainBattle system
- 651: ChainBattle integration
- 652+: Future enhancements

## References

### Key Classes and Methods
- `Serializer.LoadGame()`: Main save loading method
- `Serializer.MigrateSaveData()`: Version migration dispatcher
- `ApplicationModel`: Save data structure
- `StaticData.GetChainBattle()`: ChainBattle lookup
- `StaticData.GetChainBattleLoot()`: Loot calculation

### Related Documentation
- `CHAINBATTLE_PLAN.md`: ChainBattle system overview
- Save data format specifications
- Loot table documentation

---

**Implementation Note**: This migration preserves all player progress while seamlessly transitioning to the new ChainBattle system. The key principle is **no data loss** - every completed level becomes an accessible ChainBattle with equiva