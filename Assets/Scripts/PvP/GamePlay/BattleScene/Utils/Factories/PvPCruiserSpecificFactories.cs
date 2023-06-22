using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPCruiserSpecificFactories : IPvPCruiserSpecificFactories
    {
        public IPvPAircraftProvider AircraftProvider { get; }
        public IPvPPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        public IPvPDroneFeedbackFactory DroneFeedbackFactory { get; }
        public IPvPGlobalBoostProviders GlobalBoostProviders { get; }
        public IPvPTurretStatsFactory TurretStatsFactory { get; }
        public IPvPCruiserTargetFactoriesProvider Targets { get; }

        public PvPCruiserSpecificFactories(
            IPvPFactoryProvider factoryProvider,
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPUpdaterProvider updaterProvider,
            PvPFaction faction,
            bool isTutorial)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new PvPAircraftProvider(parentCruiser.Position, enemyCruiser.Position, PvPRandomGenerator.Instance, isTutorial);
            GlobalBoostProviders = new PvPGlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(factoryProvider.BoostFactory, GlobalBoostProviders);
         //   BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
          //  BuildableEffectsSoundPlayer = factoryProvider.Sound.PrioritisedSoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new PvPDroneFeedbackFactory(
                    factoryProvider.PoolProviders.DronePool,
                    new PvPSpawnPositionFinder(PvPRandomGenerator.Instance, Constants.WATER_LINE),
                    faction);
        }

        public PvPCruiserSpecificFactories(
    IPvPFactoryProvider factoryProvider,
    IPvPCruiser parentCruiser,
    IPvPCruiser enemyCruiser,
    IPvPRankedTargetTracker userChosenTargetTracker,
    IPvPUpdaterProvider updaterProvider,
    PvPFaction faction)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new PvPAircraftProvider(parentCruiser.Position, enemyCruiser.Position, PvPRandomGenerator.Instance);
            GlobalBoostProviders = new PvPGlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(factoryProvider.BoostFactory, GlobalBoostProviders);
        //    BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new PvPDroneFeedbackFactory(
                    factoryProvider.PoolProviders.DronePool,
                    new PvPSpawnPositionFinder(PvPRandomGenerator.Instance, Constants.WATER_LINE),
                    faction);
        }
    }
}