namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFactoriesProvider
    {
        IPvPTargetFinderFactory FinderFactory { get; }
        IPvPTargetFilterFactory FilterFactory { get; }
        IPvPTargetRankerFactory RankerFactory { get; }
        IPvPTargetHelperFactory HelperFactory { get; }
        IPvPRangeCalculatorProvider RangeCalculatorProvider { get; }
    }
}