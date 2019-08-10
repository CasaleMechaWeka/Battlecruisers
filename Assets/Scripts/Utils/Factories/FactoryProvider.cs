using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider : IFactoryProvider
	{
        public ITurretFactoryProvider Turrets { get; }
        public ISoundFactoryProvider Sound { get; }
        public IAircraftProvider AircraftProvider { get; }
        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public IDeferrerProvider DeferrerProvider { get; }
        public IExplosionManager ExplosionManager { get; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IGlobalBoostProviders GlobalBoostProviders { get; }
        public IMovementControllerFactory MovementControllerFactory { get; }
        public IPrefabFactory PrefabFactory { get; }
        public ISpawnDeciderFactory SpawnDeciderFactory { get; }
        public ISpriteChooserFactory SpriteChooserFactory { get; }
        public ITargetFactoriesProvider TargetFactories { get; }
        public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        public ITrackerFactory TrackerFactory { get; }
        public IUpdaterProvider UpdaterProvider { get; }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider,
            IDeferrer deferrer,
            IRankedTargetTracker userChosenTargetTracker,
            ICamera soleCamera,
            bool isPlayerCruiser,
            IAudioSource audioSource,
            IMarkerFactory markerFactory,
            IUpdaterProvider updaterProvider)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider, deferrer, userChosenTargetTracker, soleCamera, audioSource, markerFactory, updaterProvider);

			PrefabFactory = prefabFactory;
            TargetFactories = new TargetFactoriesProvider(friendlyCruiser, enemyCruiser, userChosenTargetTracker, updaterProvider);
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory(TimeBC.Instance);
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            GlobalBoostProviders = new GlobalBoostProviders();
            DamageApplierFactory = new DamageApplierFactory(TargetFactories.FilterFactory);
            ExplosionManager = new ExplosionManager(PrefabFactory);
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);

            Turrets = new TurretFactoryProvider(BoostFactory, GlobalBoostProviders);
            Sound = new SoundFactoryProvider(deferrer, soleCamera, isPlayerCruiser, audioSource);
            DeferrerProvider = new DeferrerProvider(deferrer);
            TrackerFactory = new TrackerFactory(markerFactory, soleCamera);
            SpawnDeciderFactory = new SpawnDeciderFactory();
            UpdaterProvider = updaterProvider;
        }
	}
}
