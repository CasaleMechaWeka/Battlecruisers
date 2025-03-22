using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.Factories;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider
    {
        private readonly IBattleSceneGodComponents _components;

        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public DeferrerProvider DeferrerProvider { get; }
        public IDroneMonitor DroneMonitor { get; private set; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IMovementControllerFactory MovementControllerFactory { get; }
        public IPrefabFactory PrefabFactory { get; }
        public ISpawnDeciderFactory SpawnDeciderFactory { get; }
        public SpriteChooserFactory SpriteChooserFactory { get; }
        public TargetFactoriesProvider Targets { get; }
        public IUpdaterProvider UpdaterProvider { get; }
        public ISettingsManager SettingsManager { get; }

        // Circular dependencies :/
        public PoolProviders PoolProviders { get; private set; }
        public ISoundFactoryProvider Sound { get; private set; }

        public FactoryProvider(
            IBattleSceneGodComponents components,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(components, prefabFactory, spriteProvider, settingsManager);

            _components = components;
            PrefabFactory = prefabFactory;
            SettingsManager = settingsManager;
            Targets = new TargetFactoriesProvider();
            MovementControllerFactory = new MovementControllerFactory();
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new DamageApplierFactory();
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    spriteProvider);
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            SpawnDeciderFactory = new SpawnDeciderFactory();
            UpdaterProvider = components.UpdaterProvider;
        }

        // Not in constructor because of circular dependency
        public void Initialise(IUIManager uiManager)
        {
            Assert.IsNotNull(uiManager);

            IDroneFactory droneFactory = new DroneFactory(PrefabFactory);
            DroneMonitor = new DroneMonitor(droneFactory);

            PoolProviders poolProviders = new PoolProviders(this, uiManager, droneFactory);
            PoolProviders = poolProviders;
            poolProviders.SetInitialCapacities();

            Sound = new SoundFactoryProvider(_components, poolProviders);
        }
    }
}
