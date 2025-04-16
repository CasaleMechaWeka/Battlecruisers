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
    public static class FactoryProvider
    {
        private static BattleSceneGodComponents _components;

        public static DeferrerProvider DeferrerProvider { get; private set; }
        public static DroneMonitor DroneMonitor { get; private set; }
        public static FlightPointsProviderFactory FlightPointsProviderFactory { get; private set; }
        public static TargetFactoriesProvider Targets { get; private set; }
        public static IUpdaterProvider UpdaterProvider { get; private set; }
        public static SettingsManager SettingsManager { get; private set; }

        // Circular dependencies :/
        public static PoolProviders PoolProviders { get; private set; }
        public static ISoundFactoryProvider Sound { get; private set; }
        public static IDroneFactory DroneFactory { get; private set; }

        public static void Initialise(
            BattleSceneGodComponents components,
            SettingsManager settingsManager,
            IUIManager uiManager)
        {
            Helper.AssertIsNotNull(components, settingsManager);

            _components = components;
            SettingsManager = settingsManager;
            Targets = new TargetFactoriesProvider();
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            UpdaterProvider = components.UpdaterProvider;

            Assert.IsNotNull(uiManager);

            DroneFactory = new DroneFactory();
            DroneMonitor = new DroneMonitor(DroneFactory);

            PoolProviders poolProviders = new PoolProviders(uiManager);
            PoolProviders = poolProviders;

            Sound = new SoundFactoryProvider(_components, poolProviders);
        }

        public static void Clear()
        {
            DeferrerProvider = null;
            DroneMonitor = null;
            FlightPointsProviderFactory = null;
            Targets = null;
            UpdaterProvider = null;
            SettingsManager = null;
            PoolProviders = null;
            Sound = null;
        }
    }
}
