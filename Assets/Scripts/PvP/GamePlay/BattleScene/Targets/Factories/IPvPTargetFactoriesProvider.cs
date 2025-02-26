using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFactoriesProvider
    {
        IPvPTargetFinderFactory FinderFactory { get; }
        IPvPTargetFilterFactory FilterFactory { get; }
        ITargetRankerFactory RankerFactory { get; }
        IPvPTargetHelperFactory HelperFactory { get; }
        IRangeCalculatorProvider RangeCalculatorProvider { get; }
    }
}