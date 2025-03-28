using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.Factories;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider
    {
        private readonly IBattleSceneGodComponents _components;

        public DeferrerProvider DeferrerProvider { get; }
        public IDroneMonitor DroneMonitor { get; private set; }
        public FlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public TargetFactoriesProvider Targets { get; }
        public IUpdaterProvider UpdaterProvider { get; }
        public SettingsManager SettingsManager { get; }

        // Circular dependencies :/
        public PoolProviders PoolProviders { get; private set; }
        public ISoundFactoryProvider Sound { get; private set; }

        public FactoryProvider(
            IBattleSceneGodComponents components,
            SettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(components, settingsManager);

            _components = components;
            SettingsManager = settingsManager;
            Targets = new TargetFactoriesProvider();
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            UpdaterProvider = components.UpdaterProvider;
        }

        // Not in constructor because of circular dependency
        public void Initialise(IUIManager uiManager)
        {
            Assert.IsNotNull(uiManager);

            IDroneFactory droneFactory = new DroneFactory();
            DroneMonitor = new DroneMonitor(droneFactory);

            PoolProviders poolProviders = new PoolProviders(this, uiManager, droneFactory);
            PoolProviders = poolProviders;
            poolProviders.SetInitialCapacities();

            Sound = new SoundFactoryProvider(_components, poolProviders);
        }
    }
}
