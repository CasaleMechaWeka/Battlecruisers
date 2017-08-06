using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildingKeyProviderFactory
	{
        IBuildingKeyProvider CreateBuildingKeyProvider(BuildingCategory buildingCategory, int levelNum);
        IBuildingKeyProvider CreateDummyBuildingKeyProvider(IPrefabKey buildingKey);
	}
}
