using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityEngine.Assertions;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class BuildableInitialisationArgs
    {
        public IUIManager UiManager { get; }
        public ICruiser ParentCruiser { get; }
        public ICruiser EnemyCruiser { get; }
        public CruiserSpecificFactories CruiserSpecificFactories { get; }
        public Direction ParentCruiserFacingDirection { get; }

        public BuildableInitialisationArgs(
            Helper helper,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            AircraftProvider aircraftProvider = null,
            ITargetFactories targetFactories = null,
            FlightPointsProviderFactory flightPointsProviderFactory = null,
            GlobalBoostProviders globalBoostProviders = null,
            Direction parentCruiserDirection = Direction.Right,
            IDeferrer deferrer = null,
            IDeferrer realTimeDeferrer = null,
            IUserChosenTargetManager userChosenTargetManager = null,
            IUpdaterProvider updaterProvider = null,
            ITurretStatsFactory turretStatsFactory = null,
            bool showDroneFeedback = false)
        {
            Assert.IsNotNull(helper);

            ParentCruiserFacingDirection = parentCruiserDirection;
            ParentCruiser = parentCruiser ?? helper.CreateCruiser(ParentCruiserFacingDirection, faction);
            EnemyCruiser = enemyCruiser ?? helper.CreateCruiser(Direction.Left, BcUtils.Helper.GetOppositeFaction(faction));
            UiManager = uiManager ?? Substitute.For<IUIManager>();
            userChosenTargetManager = userChosenTargetManager ?? new UserChosenTargetManager();
            updaterProvider = updaterProvider ?? Substitute.For<IUpdaterProvider>();
            TargetFactoriesProvider targetFactoriesProvider = targetFactories?.TargetFactoriesProvider ?? new TargetFactoriesProvider();
            deferrer = deferrer ?? Substitute.For<IDeferrer>();
            realTimeDeferrer = realTimeDeferrer ?? Substitute.For<IDeferrer>();
            globalBoostProviders = globalBoostProviders ?? new GlobalBoostProviders();

            IDroneFeedbackFactory droneFeedbackFactory = Substitute.For<IDroneFeedbackFactory>();
            if (showDroneFeedback)
            {
                droneFeedbackFactory
                    = new DroneFeedbackFactory(
                        new SpawnPositionFinder(Constants.WATER_LINE),
                        FactoryProvider.DroneFactory,
                        faction);
            }

            CruiserSpecificFactories = Substitute.For<CruiserSpecificFactories>();
            SetupCruiserSpecificFactories(
                CruiserSpecificFactories,
                aircraftProvider ?? helper.CreateAircraftProvider(),
                globalBoostProviders,
                turretStatsFactory ?? new TurretStatsFactory(globalBoostProviders),
                targetFactories?.TargetProcessorFactory ?? new TargetProcessorFactory(EnemyCruiser, userChosenTargetManager),
                targetFactories?.TargetTrackerFactory ?? new TargetTrackerFactory(userChosenTargetManager),
                targetFactories?.TargetDetectorFactory ?? new TargetDetectorFactory(EnemyCruiser.UnitTargets, ParentCruiser.UnitTargets, updaterProvider),
                targetFactories?.TargetProviderFactory ?? new TargetProviderFactory(CruiserSpecificFactories, targetFactoriesProvider),
                droneFeedbackFactory);
        }

        private void SetupCruiserSpecificFactories(
            CruiserSpecificFactories cruiserSpecificFactories,
            AircraftProvider aircraftProvider,
            GlobalBoostProviders globalBoostProviders,
            ITurretStatsFactory turretStatsFactory,
            ITargetProcessorFactory targetProcessorFactory,
            TargetTrackerFactory targetTrackerFactory,
            TargetDetectorFactory targetDetectorFactory,
            TargetProviderFactory targetProviderFactory,
            IDroneFeedbackFactory droneFeedbackFactory)
        {
            cruiserSpecificFactories.AircraftProvider.Returns(aircraftProvider);
            cruiserSpecificFactories.GlobalBoostProviders.Returns(globalBoostProviders);
            cruiserSpecificFactories.TurretStatsFactory.Returns(turretStatsFactory);
            cruiserSpecificFactories.Targets.ProcessorFactory.Returns(targetProcessorFactory);
            cruiserSpecificFactories.Targets.TrackerFactory.Returns(targetTrackerFactory);
            cruiserSpecificFactories.Targets.DetectorFactory.Returns(targetDetectorFactory);
            cruiserSpecificFactories.Targets.ProviderFactory.Returns(targetProviderFactory);
            cruiserSpecificFactories.DroneFeedbackFactory.Returns(droneFeedbackFactory);
        }
    }
}
