using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPFactoryProvider
    {
        // private readonly IPvPBattleSceneGodComponentsServer _components;
        private readonly IPvPBattleSceneGodComponents _components;

        public DeferrerProvider DeferrerProvider { get; }
        public IDroneMonitor DroneMonitor { get; private set; }
        public FlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IUpdaterProvider UpdaterProvider { get; }
        public SettingsManager SettingsManager { get; }

        // Circular dependencies :/
        public IPvPPoolProviders PoolProviders { get; private set; }
        public ISoundFactoryProvider Sound { get; private set; }


        private PvPPoolProviders poolProviders;
        public PvPFactoryProvider(
            IPvPBattleSceneGodComponents components,
            SettingsManager settingsManager
            )
        {
            PvPHelper.AssertIsNotNull(components, settingsManager);

            _components = components;
            SettingsManager = settingsManager;
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            UpdaterProvider = components.UpdaterProvider;
        }
        // Not in constructor because of circular dependency
        public void Initialise( /* IPvPUIManager uiManager */)
        {
            IDroneFactory droneFactory = new PvPDroneFactory();
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
