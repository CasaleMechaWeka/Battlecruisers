using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public static class PvPTargetFactoriesProvider
    {
        public static TargetRankerFactory RankerFactory = new TargetRankerFactory();
        public static RangeCalculatorProvider RangeCalculatorProvider = new RangeCalculatorProvider();
    }
}