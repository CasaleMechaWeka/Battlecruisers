using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFactoriesProvider
    {
        IPvPTargetFilterFactory FilterFactory { get; }
        TargetRankerFactory RankerFactory { get; }
        IPvPTargetHelperFactory HelperFactory { get; }
        RangeCalculatorProvider RangeCalculatorProvider { get; }
    }
}