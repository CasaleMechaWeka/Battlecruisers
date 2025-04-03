using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFactoriesProvider
    {
        public IPvPTargetFilterFactory FilterFactory { get; }
        public TargetRankerFactory RankerFactory { get; }
        public IPvPTargetHelperFactory HelperFactory { get; }
        public RangeCalculatorProvider RangeCalculatorProvider { get; }

        public PvPTargetFactoriesProvider()
        {
            FilterFactory = new PvPTargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            HelperFactory = new PvPTargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}