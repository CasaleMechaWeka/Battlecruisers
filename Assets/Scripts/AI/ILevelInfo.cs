using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    public interface ILevelInfo
	{
        ICruiserController AICruiser { get; }
        ICruiserController PlayerCruiser { get; }

        bool CanConstructBuilding(BuildingKey buildingKey);
        IList<BuildingKey> GetAvailableBuildings(BuildingCategory category);
	}
}
