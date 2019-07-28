using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider : ITargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public ITargetFinderFactory FinderFactory { get; }
        public ITargetTrackerFactory TrackerFactory { get; }
        public ITargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public ITargetProviderFactory ProviderFactory { get; }
        public ITargetHelperFactory HelperFactory { get; }
        public ITargetDetectorFactory TargetDetectorFactory { get; }
        public IRangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider(ICruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker, IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(enemyCruiser, userChosenTargetTracker, updaterProvider);

            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            FinderFactory = new TargetFinderFactory();
            TrackerFactory = new TargetTrackerFactory(enemyCruiser, userChosenTargetTracker);
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            ProviderFactory = new TargetProviderFactory(this);
            HelperFactory = new TargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();

            IUnitTargets unitTargets = new UnitTargets(enemyCruiser.UnitMonitor);
            TargetDetectorFactory = new TargetDetectorFactory(unitTargets, updaterProvider);
        }
    }
}