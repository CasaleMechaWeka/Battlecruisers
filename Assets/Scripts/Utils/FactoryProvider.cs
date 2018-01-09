using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;

namespace BattleCruisers.Utils
{
    public class FactoryProvider : IFactoryProvider
	{
		public IPrefabFactory PrefabFactory { get; private set; }
		public ITargetsFactory TargetsFactory { get; private set; }
		public IMovementControllerFactory MovementControllerFactory { get; private set; }
		public IAngleCalculatorFactory AngleCalculatorFactory { get; private set; }
		public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; private set; }
		public IAircraftProvider AircraftProvider { get; private set; }
		public IFlightPointsProviderFactory FlightPointsProviderFactory { get; private set; } 
        public IBoostProvidersManager BoostProvidersManager { get; private set; }
        public IBoostFactory BoostFactory { get; private set; }
        public IDamageApplierFactory DamageApplierFactory { get; private set; }
        public IExplosionFactory ExplosionFactory { get; private set; }
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; private set; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; private set; }
        public IAngleLimiterFactory AngleLimiterFactory { get; private set; }
        public ISpriteChooserFactory SpriteChooserFactory { get; private set; }
        public ISoundFetcher SoundFetcher { get; private set; }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider);

			PrefabFactory = prefabFactory;
			TargetsFactory = new TargetsFactory(enemyCruiser);
			AngleCalculatorFactory = new AngleCalculatorFactory();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostProvidersManager = new BoostProvidersManager();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new DamageApplierFactory(TargetsFactory);
            ExplosionFactory = new ExplosionFactory(PrefabFactory);
            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            SoundFetcher = new SoundFetcher();
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);
		}
	}
}
