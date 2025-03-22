using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPFactoryProvider : IPvPFactoryProvider
    {
        // private readonly IPvPBattleSceneGodComponentsServer _components;
        private readonly IPvPBattleSceneGodComponents _components;

        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public DeferrerProvider DeferrerProvider { get; }
        public IDroneMonitor DroneMonitor { get; private set; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IPvPMovementControllerFactory MovementControllerFactory { get; }
        public IPvPPrefabFactory PrefabFactory { get; }
        public IPvPSpawnDeciderFactory SpawnDeciderFactory { get; }
        public IPvPSpriteChooserFactory SpriteChooserFactory { get; }
        public IPvPTargetFactoriesProvider Targets { get; }
        public IUpdaterProvider UpdaterProvider { get; }
        public ISettingsManager SettingsManager { get; }

        // Circular dependencies :/
        public IPvPPoolProviders PoolProviders { get; private set; }
        public ISoundFactoryProvider Sound { get; private set; }


        private PvPPoolProviders poolProviders;
        public PvPFactoryProvider(
            IPvPBattleSceneGodComponents components,
            IPvPPrefabFactory prefabFactory,
            ISettingsManager settingsManager
            )
        {
            PvPHelper.AssertIsNotNull(components, prefabFactory, settingsManager);

            _components = components;
            PrefabFactory = prefabFactory;
            SettingsManager = settingsManager;
            Targets = new PvPTargetFactoriesProvider();
            MovementControllerFactory = new PvPMovementControllerFactory();
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new PvPDamageApplierFactory(Targets.FilterFactory);
            SpriteChooserFactory
                = new PvPSpriteChooserFactory();
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            SpawnDeciderFactory = new PvPSpawnDeciderFactory();
            UpdaterProvider = components.UpdaterProvider;
        }
        // Not in constructor because of circular dependency
        public void Initialise( /* IPvPUIManager uiManager */)
        {
            IDroneFactory droneFactory = new PvPDroneFactory(PrefabFactory);
            DroneMonitor = new DroneMonitor(droneFactory);
            Sound = new PvPSoundFactoryProvider(_components, this /*, poolProviders */);
            poolProviders = new PvPPoolProviders(this, droneFactory);
            PoolProviders = poolProviders;
            poolProviders.SetInitialCapacities();
        }

        public void Initialise_Rest()
        {
            poolProviders.SetInitialCapacities_Rest();
        }

        public void Initialise(IPvPUIManager uiManager)
        {
            Assert.IsNotNull(uiManager);
            Sound = new PvPSoundFactoryProvider(_components, this /*, poolProviders */);
        }

    }
}
