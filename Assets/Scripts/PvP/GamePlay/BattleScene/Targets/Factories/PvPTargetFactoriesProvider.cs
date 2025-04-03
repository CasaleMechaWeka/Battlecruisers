using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public static class PvPTargetFactoriesProvider
    {
        public static IPvPTargetFilterFactory FilterFactory = new PvPTargetFilterFactory();
        public static TargetRankerFactory RankerFactory = new TargetRankerFactory();
        public static IPvPTargetHelperFactory HelperFactory = new PvPTargetHelperFactory();
        public static RangeCalculatorProvider RangeCalculatorProvider = new RangeCalculatorProvider();
    }
}