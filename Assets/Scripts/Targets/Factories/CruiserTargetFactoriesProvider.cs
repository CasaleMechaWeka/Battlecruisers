using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Targets.Factories
{
    public class CruiserTargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public TargetTrackerFactory TrackerFactory { get; }
        public TargetDetectorFactory DetectorFactory { get; }
        public TargetProviderFactory ProviderFactory { get; }

        public CruiserTargetFactoriesProvider(
            IFactoryProvider factoryProvider,
            CruiserSpecificFactories cruiserSpecificFactories,
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(factoryProvider, cruiserSpecificFactories, parentCruiser, enemyCruiser, userChosenTargetTracker);

            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            TrackerFactory = new TargetTrackerFactory(userChosenTargetTracker);
            DetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, factoryProvider.UpdaterProvider);
            ProviderFactory = new TargetProviderFactory(cruiserSpecificFactories, factoryProvider.Targets);
        }
    }
}