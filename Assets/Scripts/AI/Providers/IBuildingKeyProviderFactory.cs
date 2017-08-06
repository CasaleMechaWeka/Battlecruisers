using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildingKeyProviderFactory
	{
        IBuildingKeyProvider CreateBuildingKeyProvider(BuildingCategory buildingCategory, int levelNum);
	}
}
