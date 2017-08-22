using BattleCruisers.AI.Providers.Strategies.Requests;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    // FELIX  Delete :D
    public interface IBuildingKeyProviderFactory
	{
        IBuildingKeyProvider CreateBuildingKeyProvider(OffensiveType offensiveType, int levelNum);
	}
}
