using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Targets.Factories
{
    public class CruiserTargetFactoriesProvider : ICruiserTargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public ITargetTrackerFactory TrackerFactory { get; }
        public ITargetDetectorFactory TargetDetectorFactory { get; }  // FELIX  Remove Target prefix once part of sub provider :)
        public ITargetProviderFactory TargetProviderFactory { get; }  // FELIX  Remove Target prefix once part of sub provider :)

        public CruiserTargetFactoriesProvider(
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(factoryProvider, cruiserSpecificFactories, parentCruiser, enemyCruiser, userChosenTargetTracker);

            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            TrackerFactory = new TargetTrackerFactory(userChosenTargetTracker);
            TargetDetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, factoryProvider.UpdaterProvider);
            TargetProviderFactory = new TargetProviderFactory(cruiserSpecificFactories, factoryProvider.TargetFactories);
        }
    }
}