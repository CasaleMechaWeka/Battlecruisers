using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPCruiserSpecificFactories : IPvPCruiserSpecificFactories
    {
        public IAircraftProvider AircraftProvider { get; }
        public IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        public IDroneFeedbackFactory DroneFeedbackFactory { get; }
        public IGlobalBoostProviders GlobalBoostProviders { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }
        public IPvPCruiserTargetFactoriesProvider Targets { get; }

        public PvPCruiserSpecificFactories(
            IPvPFactoryProvider factoryProvider,
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker,
            IUpdaterProvider updaterProvider,
            Faction faction,
            bool isTutorial)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new PvPAircraftProvider(parentCruiser.Position, enemyCruiser.Position, isTutorial);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(GlobalBoostProviders);
            //   BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            //  BuildableEffectsSoundPlayer = factoryProvider.Sound.PrioritisedSoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    factoryProvider.PoolProviders.DronePool,
                    new SpawnPositionFinder(Constants.WATER_LINE),
                    faction);
        }

        public PvPCruiserSpecificFactories(
            IPvPFactoryProvider factoryProvider,
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker,
            IUpdaterProvider updaterProvider,
            Faction faction)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new PvPAircraftProvider(parentCruiser.Position, enemyCruiser.Position);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(GlobalBoostProviders);
            //    BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    factoryProvider.PoolProviders.DronePool,
                    new SpawnPositionFinder(Constants.WATER_LINE),
                    faction);
        }
    }
}