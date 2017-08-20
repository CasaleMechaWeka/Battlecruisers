using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public interface IBuildingKeyHelper
	{
        bool CanConstructBuilding(IPrefabKey buildingKey);
	}
}