using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;

namespace BattleCruisers.Data.Static
{
    /// <summary>
    /// Provides data that does not change throughout the game.
    /// 
    /// This is in contrast to the GameModel, which changes as the player
    /// progresses and unlocks new prefabs.
    /// </summary>
    public interface IStaticData
    {
        bool IsDemo { get; }
        GameModel InitialGameModel { get; }
        ReadOnlyCollection<ILevel> Levels { get; }
		ReadOnlyCollection<HullKey> HullKeys { get; }
		ReadOnlyCollection<UnitKey> UnitKeys { get; }
        ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        ReadOnlyCollection<BuildingKey> AIBannedUltrakeys{ get; }
        int LastLevelWithLoot { get; }
        ILevelStrategies Strategies { get; }

        bool IsUnitAvailable(UnitKey unitKey, int levelNum);
        IList<UnitKey> GetAvailableUnits(UnitCategory category, int levelNum);

        bool IsBuildingAvailable(BuildingKey buildingKey, int levelNum);
        IList<BuildingKey> GetAvailableBuildings(BuildingCategory category, int levelNum);

        ILoot GetLevelLoot(int levelCompleted);
	}
}
	