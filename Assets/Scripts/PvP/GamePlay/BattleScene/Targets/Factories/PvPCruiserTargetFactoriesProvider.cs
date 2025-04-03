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
        public TargetTrackerFactory TrackerFactory { get; }
        public TargetDetectorFactory DetectorFactory { get; }
        public IPvPTargetProviderFactory ProviderFactory { get; }

        public PvPCruiserTargetFactoriesProvider(
            PvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, cruiserSpecificFactories, parentCruiser, enemyCruiser, userChosenTargetTracker);

            ProcessorFactory = new PvPTargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            TrackerFactory = new TargetTrackerFactory(userChosenTargetTracker);
            DetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, factoryProvider.UpdaterProvider);
            ProviderFactory = new PvPTargetProviderFactory(cruiserSpecificFactories);
        }
    }
}