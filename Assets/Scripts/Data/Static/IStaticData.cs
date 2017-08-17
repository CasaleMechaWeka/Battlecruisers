using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.AI.Providers.Strategies;
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
        IList<ILevel> Levels { get; }
        ReadOnlyCollection<IPrefabKey> BuildingKeys { get; }
        bool IsBuildableAvailable(IPrefabKey buildableKey, int levelNum);
        IList<IPrefabKey> GetAvailableUnits(UnitCategory category, int levelNum);
		IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category, int levelNum);
        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
	}
}
	