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
        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public IDeferrerProvider DeferrerProvider { get; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IMovementControllerFactory MovementControllerFactory { get; }
        public IPrefabFactory PrefabFactory { get; }
        public ISoundFactoryProvider Sound { get; }
        public ISpawnDeciderFactory SpawnDeciderFactory { get; }
        public ISpriteChooserFactory SpriteChooserFactory { get; }
        public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        public ITargetFactoriesProvider Targets { get; }
        public ITurretFactoryProvider Turrets { get; }
        public IUpdaterProvider UpdaterProvider { get; }

        // Have setter because of circular dependency with FactoryProvider :/
        public IPoolProviders PoolProviders { get; set; }

        public FactoryProvider(
            IBattleSceneGodComponents components,
            IPrefabFactory prefabFactory, 
            ISpriteProvider spriteProvider)
		{
            Helper.AssertIsNotNull(components, prefabFactory, spriteProvider);

			PrefabFactory = prefabFactory;
            Targets = new TargetFactoriesProvider();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new DamageApplierFactory(Targets.FilterFactory);
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
