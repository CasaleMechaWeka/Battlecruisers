using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider : IFactoryProvider
    {
        public ISoundFactoryProvider Sound { get; }
        public ITurretFactoryProvider Turrets { get; }
        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public IDeferrerProvider DeferrerProvider { get; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IMovementControllerFactory MovementControllerFactory { get; }
        public IPrefabFactory PrefabFactory { get; }
        public ISpawnDeciderFactory SpawnDeciderFactory { get; }
        public ISpriteChooserFactory SpriteChooserFactory { get; }
        // FELIX  Rename to Targets?
        public ITargetFactoriesProvider TargetFactories { get; }
        public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        public IUpdaterProvider UpdaterProvider { get; }

        private IPoolProviders _poolProviders;
        public IPoolProviders PoolProviders
        {
            get
            {
                if (_poolProviders == null)
                {
                    _poolProviders = new PoolProviders(this);
                }
                return _poolProviders;
            }
        }

        public FactoryProvider(
            IBattleSceneGodComponents components,
            IPrefabFactory prefabFactory, 
            ISpriteProvider spriteProvider)
		{
            Helper.AssertIsNotNull(components, prefabFactory, spriteProvider);

			PrefabFactory = prefabFactory;
            TargetFactories = new TargetFactoriesProvider();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new DamageApplierFactory(TargetFactories.FilterFactory);
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);
            DeferrerProvider = new DeferrerProvider(components.Deferrer);
            SpawnDeciderFactory = new SpawnDeciderFactory();
            UpdaterProvider = components.UpdaterProvider;

            Sound = new SoundFactoryProvider(components.Deferrer, components.Camera, components.AudioSource);
            Turrets = new TurretFactoryProvider();
        }
	}
}
