using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider : ITargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; private set; }
        public ITargetFinderFactory FinderFactory { get; private set; }
        public ITargetTrackerFactory TrackerFactory { get; private set; }
        public ITargetFilterFactory FilterFactory { get; private set; }
        public ITargetRankerFactory RankerFactory { get; private set; }
        public ITargetProviderFactory ProviderFactory { get; private set; }
        public ITargetHelperFactory HelperFactory { get; private set; }

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