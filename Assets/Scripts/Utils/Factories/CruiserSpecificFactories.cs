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
        public ICruiserTargetFactoriesProvider Targets { get; }
        // FELIX  Create targets sub provider? => Yes!  Then can pass subprovider instead of whole ICruiserSpecificFactories :)
        public ITargetTrackerFactory TrackerFactory { get; }
        public ITargetProviderFactory TargetProviderFactory { get; }  // FELIX  Remove Target prefix once part of sub provider :)

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
            Targets = new CruiserTargetFactoriesProvider(factoryProvider, this, parentCruiser, enemyCruiser, userChosenTargetTracker);

            // FELIX  Delete :)
            TrackerFactory = new TargetTrackerFactory(userChosenTargetTracker);
            TargetProviderFactory = new TargetProviderFactory(this, factoryProvider.TargetFactories);
        }
    }
}