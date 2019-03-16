using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;

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

        public TargetFactoriesProvider(ICruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker)
        {
            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            FinderFactory = new TargetFinderFactory();
            TrackerFactory = new TargetTrackerFactory(enemyCruiser, userChosenTargetTracker);
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            ProviderFactory = new TargetProviderFactory(this);
            HelperFactory = new TargetHelperFactory();
        }
    }
}