using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider : ITargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public ITargetFinderFactory FinderFactory { get; }
        public ITargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public ITargetProviderFactory ProviderFactory { get; }
        public ITargetHelperFactory HelperFactory { get; }
        public ITargetDetectorFactory TargetDetectorFactory { get; }
        public IRangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider(ICruiser parentCruiser, ICruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker, IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            FinderFactory = new TargetFinderFactory();
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();

            // FELIX  Create new implementation that.  dang it
            //ProviderFactory = new TargetProviderFactory(this);

            HelperFactory = new TargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
            TargetDetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, updaterProvider);
        }
    }
}