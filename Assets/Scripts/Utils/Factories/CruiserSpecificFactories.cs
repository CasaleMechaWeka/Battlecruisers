using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Utils.Factories
{
    public class CruiserSpecificFactories : ICruiserSpecificFactories
    {
        public IAircraftProvider AircraftProvider { get; }
        public IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        public IDroneFeedbackFactory DroneFeedbackFactory { get; }
        public IGlobalBoostProviders GlobalBoostProviders { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }
        public ICruiserTargetFactoriesProvider Targets { get; }

        public CruiserSpecificFactories(
            IFactoryProvider factoryProvider,
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IRankedTargetTracker userChosenTargetTracker, 
            IUpdaterProvider updaterProvider, 
            Faction faction)
        {
            Helper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new AircraftProvider(parentCruiser.Position, enemyCruiser.Position, RandomGenerator.Instance);
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new TurretStatsFactory(factoryProvider.BoostFactory, GlobalBoostProviders);
            BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            Targets = new CruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            DroneFeedbackFactory
                = new DroneFeedbackFactory(
                    factoryProvider.PoolProviders.DronePool,
                    new SpawnPositionFinder(RandomGenerator.Instance, Constants.WATER_LINE),
                    faction);
        }
    }
}