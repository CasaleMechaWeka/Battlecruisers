using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Utils.Factories
{
    public class CruiserSpecificFactories
    {
        public AircraftProvider AircraftProvider { get; }
        public IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        public IDroneFeedbackFactory DroneFeedbackFactory { get; }
        public GlobalBoostProviders GlobalBoostProviders { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }
        public CruiserTargetFactoriesProvider Targets { get; }

        public CruiserSpecificFactories(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            IRankedTargetTracker userChosenTargetTracker,
            IUpdaterProvider updaterProvider,
            Faction faction,
            bool isTutorial)
        {
            Helper.AssertIsNotNull(parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new AircraftProvider(parentCruiser.Position, enemyCruiser.Position, isTutorial);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new TurretStatsFactory(GlobalBoostProviders);
            BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? FactoryProvider.Sound.PrioritisedSoundPlayer : FactoryProvider.Sound.DummySoundPlayer;
            Targets = new CruiserTargetFactoriesProvider(this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    new SpawnPositionFinder(Constants.WATER_LINE),
                    FactoryProvider.DroneFactory,
                    faction);
        }
    }
}