using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public interface IBuildingKeyHelper
	{
        bool CanConstructBuilding(IPrefabKey buildingKey);
        IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category);
	}
}