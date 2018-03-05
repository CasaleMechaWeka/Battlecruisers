using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI
{
    public interface ILevelInfo
	{
        int LevelNum { get; }
        ICruiserController AICruiser { get; }
        ICruiserController PlayerCruiser { get; }

        bool CanConstructBuilding(BuildingKey buildingKey);
        IList<BuildingKey> GetAvailableBuildings(BuildingCategory category);
	}
}
