using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFactoriesProvider : IPvPTargetFactoriesProvider
    {
        public IPvPTargetFinderFactory FinderFactory { get; }
        public IPvPTargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public IPvPTargetHelperFactory HelperFactory { get; }
        public IRangeCalculatorProvider RangeCalculatorProvider { get; }

        public PvPTargetFactoriesProvider()
        {
            FinderFactory = new PvPTargetFinderFactory();
            FilterFactory = new PvPTargetFilterFactory();
            RankerFactory = new PvPTargetRankerFactory();
            HelperFactory = new PvPTargetHelperFactory();
            RangeCalculatorProvider = new PvPRangeCalculatorProvider();
        }
    }
}