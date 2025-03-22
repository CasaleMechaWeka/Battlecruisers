using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFactoriesProvider : IPvPTargetFactoriesProvider
    {
        public IPvPTargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public IPvPTargetHelperFactory HelperFactory { get; }
        public RangeCalculatorProvider RangeCalculatorProvider { get; }

        public PvPTargetFactoriesProvider()
        {
            FilterFactory = new PvPTargetFilterFactory();
            RankerFactory = new PvPTargetRankerFactory();
            HelperFactory = new PvPTargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}