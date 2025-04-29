using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPCruiserSpecificFactories : IPvPCruiserSpecificFactories
    {
        public AircraftProvider AircraftProvider { get; }
        public IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        public DroneFeedbackFactory DroneFeedbackFactory { get; }
        public GlobalBoostProviders GlobalBoostProviders { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }
        public ICruiserTargetFactoriesProvider Targets { get; }

        public PvPCruiserSpecificFactories(
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker,
            IUpdaterProvider updaterProvider,
            Faction faction,
            bool isTutorial)
        {
            PvPHelper.AssertIsNotNull(parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new AircraftProvider(parentCruiser.Position, enemyCruiser.Position, isTutorial);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(GlobalBoostProviders);
            //   BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            //  BuildableEffectsSoundPlayer = factoryProvider.Sound.PrioritisedSoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    new SpawnPositionFinder(Constants.WATER_LINE),
                    PvPFactoryProvider.DroneFactory,
                    faction);
        }

        public PvPCruiserSpecificFactories(
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker,
            IDroneFactory droneFactory,
            IUpdaterProvider updaterProvider,
            Faction faction)
        {
            PvPHelper.AssertIsNotNull(parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new AircraftProvider(parentCruiser.Position, enemyCruiser.Position);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new PvPTurretStatsFactory(GlobalBoostProviders);
            //    BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            Targets = new PvPCruiserTargetFactoriesProvider(parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    new SpawnPositionFinder(Constants.WATER_LINE),
                    droneFactory,
                    faction);
        }
    }
}