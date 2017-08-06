namespace BattleCruisers.AI.Providers
{
    public interface IBuildingKeyProviderFactory
	{
        IBuildingKeyProvider CreateBuildingKeyProvider(OffensiveType offensiveType, int levelNum);
	}
}
