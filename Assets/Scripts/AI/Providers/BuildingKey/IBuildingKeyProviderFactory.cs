using BattleCruisers.AI.Providers.Strategies;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public interface IBuildingKeyProviderFactory
	{
        IBuildingKeyProvider CreateBuildingKeyProvider(OffensiveType offensiveType, int levelNum);
	}
}
