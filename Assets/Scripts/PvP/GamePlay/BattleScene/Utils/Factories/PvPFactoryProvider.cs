using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public static class PvPFactoryProvider
    {
        // private readonly IPvPBattleSceneGodComponentsServer _components;
        private static PvPBattleSceneGodComponents _components;

        public static DeferrerProvider DeferrerProvider { get; private set; }
        public static DroneMonitor DroneMonitor { get; private set; }
        public static FlightPointsProviderFactory FlightPointsProviderFactory { get; private set; }
        public static IUpdaterProvider UpdaterProvider { get; private set; }

        public static IPvPPoolProviders PoolProviders { get; private set; }
        public static ISoundFactoryProvider Sound { get; private set; }

        private static PvPPoolProviders poolProviders;

        public static void Setup(PvPBattleSceneGodComponents components)
        {
            PvPHelper.AssertIsNotNull(components);

            _components = components;
            FlightPointsProviderFactory = new FlightPointsProviderFactory();
            DeferrerProvider = new DeferrerProvider(components.Deferrer, components.RealTimeDeferrer);
            UpdaterProvider = components.UpdaterProvider;
        }

        public static void Initialise()
        {
            IDroneFactory droneFactory = new PvPDroneFactory();
            DroneMonitor = new DroneMonitor(droneFactory);
            Sound = new PvPSoundFactoryProvider(_components /*, poolProviders */);
            poolProviders = new PvPPoolProviders(droneFactory);
            PoolProviders = poolProviders;
            poolProviders.SetInitialCapacities();
        }

        public static void Initialise_Rest()
        {
            poolProviders.SetInitialCapacities_Rest();
        }

        public static void Clear()
        {
            DeferrerProvider = null;
            DroneMonitor = null;
            FlightPointsProviderFactory = null;
            UpdaterProvider = null;
            PoolProviders = null;
            Sound = null;
        }

        public static void Initialise_Sound()
        {
            Sound = new PvPSoundFactoryProvider(_components /*, poolProviders */);
        }
    }
}
