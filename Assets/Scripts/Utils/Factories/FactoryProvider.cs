using BattleCruisers.Buildables.Boost;
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
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider : IFactoryProvider
	{
        public ITurretFactoryProvider Turrets { get; private set; }
        public ISoundFactoryProvider Sound { get; private set; }
        public IAircraftProvider AircraftProvider { get; private set; }
        public IBoostFactory BoostFactory { get; private set; }
        public IDamageApplierFactory DamageApplierFactory { get; private set; }
        public IDeferrerProvider DeferrerProvider { get; private set; }
        public IExplosionManager ExplosionManager { get; private set; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; private set; }
        public IGlobalBoostProviders GlobalBoostProviders { get; private set; }
        public IMovementControllerFactory MovementControllerFactory { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public ISpriteChooserFactory SpriteChooserFactory { get; private set; }
        public ITargetFactoriesProvider TargetFactories { get; private set; }
        public ITargetsFactory TargetsFactory { get; private set; }
        public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; private set; }
        public ITrackerFactory TrackerFactory { get; private set; }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider,
            IVariableDelayDeferrer deferrer,
            IRankedTargetTracker userChosenTargetTracker,
            ICamera soleCamera,
            bool isPlayerCruiser,
            IAudioSource audioSource,
            IMarkerFactory markerFactory)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider, deferrer, userChosenTargetTracker, soleCamera, audioSource, markerFactory);

			PrefabFactory = prefabFactory;
            TargetFactories = new TargetFactoriesProvider(enemyCruiser, userChosenTargetTracker);
			TargetsFactory = new TargetsFactory(enemyCruiser, userChosenTargetTracker);
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory(new TimeBC());
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            GlobalBoostProviders = new GlobalBoostProviders();
            DamageApplierFactory = new DamageApplierFactory(TargetsFactory);
            ExplosionManager = new ExplosionManager(PrefabFactory);
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);

            Turrets = new TurretFactoryProvider(BoostFactory, GlobalBoostProviders);
            Sound = new SoundFactoryProvider(deferrer, soleCamera, isPlayerCruiser, audioSource);
            DeferrerProvider = new DeferrerProvider(deferrer);
            TrackerFactory = new TrackerFactory(markerFactory, soleCamera);
        }
	}
}
