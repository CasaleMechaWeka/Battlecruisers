using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine.Assertions;
using System.Collections;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPFactoryProvider : IPvPFactoryProvider
    {
        // private readonly IPvPBattleSceneGodComponentsServer _components;
        private readonly IPvPBattleSceneGodComponents _components;

        public IPvPBoostFactory BoostFactory { get; }
        public IPvPDamageApplierFactory DamageApplierFactory { get; }
        public IPvPDeferrerProvider DeferrerProvider { get; }
        public IPvPDroneMonitor DroneMonitor { get; private set; }
        public IPvPFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IPvPMovementControllerFactory MovementControllerFactory { get; }
        public IPvPPrefabFactory PrefabFactory { get; }
        public IPvPSpawnDeciderFactory SpawnDeciderFactory { get; }
        public IPvPSpriteChooserFactory SpriteChooserFactory { get; }
        public IPvPTargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        public IPvPTargetFactoriesProvider Targets { get; }
        public IPvPTurretFactoryProvider Turrets { get; }
        public IPvPUpdaterProvider UpdaterProvider { get; }
        public ISettingsManager SettingsManager { get; }

        // Circular dependencies :/
        public IPvPPoolProviders PoolProviders { get; private set; }
        public IPvPSoundFactoryProvider Sound { get; private set; }


        public PvPFactoryProvider(
            IPvPBattleSceneGodComponents components,
            IPvPPrefabFactory prefabFactory,
            IPvPSpriteProvider spriteProvider
        )
        {
            PvPHelper.AssertIsNotNull(components, prefabFactory, spriteProvider /*, settingsManager*/);

            _components = components;
            PrefabFactory = prefabFactory;
            // SettingsManager = settingsManager;
            Targets = new PvPTargetFactoriesProvider();
            TargetPositionPredictorFactory = new PvPTargetPositionPredictorFactory();
            MovementControllerFactory = new PvPMovementControllerFactory();
            FlightPointsProviderFactory = new PvPFlightPointsProviderFactory();
            BoostFactory = new PvPBoostFactory();
            DamageApplierFactory = new PvPDamageApplierFactory(Targets.FilterFactory);
            SpriteChooserFactory
                = new PvPSpriteChooserFactory(
                    new PvPAssignerFactory(),
                    spriteProvider);
            DeferrerProvider = new PvPDeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            SpawnDeciderFactory = new PvPSpawnDeciderFactory();
            UpdaterProvider = components.UpdaterProvider;

            Turrets = new PvPTurretFactoryProvider();
        }

        public PvPFactoryProvider(
            IPvPBattleSceneGodComponents components,
            IPvPPrefabFactory prefabFactory,
            IPvPSpriteProvider spriteProvider,
            ISettingsManager settingsManager
            )
        {
            PvPHelper.AssertIsNotNull(components, prefabFactory, spriteProvider /*, settingsManager*/);

            _components = components;
            PrefabFactory = prefabFactory;
            SettingsManager = settingsManager;
            Targets = new PvPTargetFactoriesProvider();
            TargetPositionPredictorFactory = new PvPTargetPositionPredictorFactory();
            MovementControllerFactory = new PvPMovementControllerFactory();
            FlightPointsProviderFactory = new PvPFlightPointsProviderFactory();
            BoostFactory = new PvPBoostFactory();
            DamageApplierFactory = new PvPDamageApplierFactory(Targets.FilterFactory);
            SpriteChooserFactory
                = new PvPSpriteChooserFactory(
                    new PvPAssignerFactory(),
                    spriteProvider);
            DeferrerProvider = new PvPDeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            SpawnDeciderFactory = new PvPSpawnDeciderFactory();
            UpdaterProvider = components.UpdaterProvider;

            Turrets = new PvPTurretFactoryProvider();
        }

        // Not in constructor because of circular dependency
        public async Task Initialise( /* IPvPUIManager uiManager */)
        {
            // Assert.IsNotNull(uiManager);


            IPvPDroneFactory droneFactory = new PvPDroneFactory(PrefabFactory);
            DroneMonitor = new PvPDroneMonitor(droneFactory);

            PvPPoolProviders poolProviders = new PvPPoolProviders(this, droneFactory);
            PoolProviders = poolProviders;
            await poolProviders.SetInitialCapacities();

            //    Sound = new PvPSoundFactoryProvider(_components, poolProviders);
        }

        /*        IEnumerator iInitialise()
                {
                    yield return null;
                    IPvPDroneFactory droneFactory = new PvPDroneFactory(PrefabFactory);
                    DroneMonitor = new PvPDroneMonitor(droneFactory);

                    PvPPoolProviders poolProviders = new PvPPoolProviders(this, droneFactory);
                    PoolProviders = poolProviders;
                    poolProviders.SetInitialCapacities();
                }*/

        public void Initialise(IPvPUIManager uiManager)
        {
            Assert.IsNotNull(uiManager);

            /*            IPvPDroneFactory droneFactory = new PvPDroneFactory(PrefabFactory);
                        DroneMonitor = new PvPDroneMonitor(droneFactory);

                        PvPPoolProviders poolProviders = new PvPPoolProviders(this, uiManager, droneFactory);
                        PoolProviders = poolProviders;
                        poolProviders.SetInitialCapacities();*/

            Sound = new PvPSoundFactoryProvider(_components, this /*, poolProviders */);
        }

    }
}
