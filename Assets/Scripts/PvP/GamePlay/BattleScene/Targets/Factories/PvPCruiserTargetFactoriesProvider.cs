using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPCruiserTargetFactoriesProvider : IPvPCruiserTargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public IPvPTargetTrackerFactory TrackerFactory { get; }
        public IPvPTargetDetectorFactory DetectorFactory { get; }
        public IPvPTargetProviderFactory ProviderFactory { get; }

        public PvPCruiserTargetFactoriesProvider(
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, cruiserSpecificFactories, parentCruiser, enemyCruiser, userChosenTargetTracker);

            ProcessorFactory = new PvPTargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            TrackerFactory = new PvPTargetTrackerFactory(userChosenTargetTracker);
            DetectorFactory = new PvPTargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, factoryProvider.UpdaterProvider);
            ProviderFactory = new PvPTargetProviderFactory(cruiserSpecificFactories, factoryProvider.Targets);
        }
    }
}