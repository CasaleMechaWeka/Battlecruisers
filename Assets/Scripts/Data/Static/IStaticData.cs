using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;

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
        GameModel InitialGameModel { get; }
        // FELIX  Change to ReadOnlyCollection
        IList<ILevel> Levels { get; }
        ReadOnlyCollection<IPrefabKey> BuildingKeys { get; }
        ReadOnlyCollection<IPrefabKey> AIBannedUltrakeys{ get; }

		bool IsUnitAvailable(IPrefabKey unitKey, int levelNum);
        IList<IPrefabKey> GetAvailableUnits(UnitCategory category, int levelNum);

		bool IsBuildingAvailable(IPrefabKey buildingKey, int levelNum);
		IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category, int levelNum);

        ILoot GetLevelLoot(int levelCompleted);

        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
	}
}
	