using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Utils.Factories
{
    public class CruiserSpecificFactories : ICruiserSpecificFactories
    {
        public IAircraftProvider AircraftProvider { get; }
        public IGlobalBoostProviders GlobalBoostProviders { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }
        public IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        // FELIX  Create targets sub provider?
        public ITargetProcessorFactory ProcessorFactory { get; }  // FELIX  Rename to TargetProcessorFactory
        public ITargetTrackerFactory TrackerFactory { get; }  // FELIX  Rename to TargetTrackerFactory
        public ITargetDetectorFactory TargetDetectorFactory { get; }

        public CruiserSpecificFactories(
            IFactoryProvider factoryProvider,
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IRankedTargetTracker userChosenTargetTracker, 
            IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(factoryProvider, parentCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);

            AircraftProvider = new AircraftProvider(parentCruiser.Position, enemyCruiser.Position, new RandomGenerator());
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new TurretStatsFactory(factoryProvider.BoostFactory, GlobalBoostProviders);
            BuildableEffectsSoundPlayer = parentCruiser.IsPlayerCruiser ? factoryProvider.Sound.PrioritisedSoundPlayer : factoryProvider.Sound.DummySoundPlayer;
            ProcessorFactory = new TargetProcessorFactory(enemyCruiser, userChosenTargetTracker);
            TrackerFactory = new TargetTrackerFactory(userChosenTargetTracker);
            TargetDetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, updaterProvider);
        }
    }
}