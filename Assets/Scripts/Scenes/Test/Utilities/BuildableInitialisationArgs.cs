using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using NSubstitute;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class BuildableInitialisationArgs
    {
        public IUIManager UiManager { get; private set; }
        public ICruiser ParentCruiser { get; private set; }
        public ICruiser EnemyCruiser { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }

        public BuildableInitialisationArgs(
            Helper helper,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IAircraftProvider aircraftProvider = null,
            IPrefabFactory prefabFactory = null,
            ITargetsFactory targetsFactory = null,
            IMovementControllerFactory movementControllerFactory = null,
            IAngleCalculatorFactory angleCalculatorFactory = null,
            ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
            IFlightPointsProviderFactory flightPointsProviderFactory = null,
            IBoostFactory boostFactory = null,
            IBoostProvidersManager boostProvidersManager = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right)
        {
            ParentCruiser = parentCruiser ?? helper.CreateCruiser(parentCruiserDirection, faction);
            EnemyCruiser = enemyCruiser ?? helper.CreateCruiser(Direction.Left, BcUtils.Helper.GetOppositeFaction(faction));
            UiManager = uiManager ?? Substitute.For<IUIManager>();
            angleCalculatorFactory = angleCalculatorFactory ?? new AngleCalculatorFactory();
            targetPositionPredictorFactory = targetPositionPredictorFactory ?? new TargetPositionPredictorFactory();
            targetsFactory = targetsFactory ?? new TargetsFactory(EnemyCruiser);

            FactoryProvider
                = CreateFactoryProvider(
                    prefabFactory ?? new PrefabFactory(new PrefabFetcher()),
                    targetsFactory,
                    movementControllerFactory ?? new MovementControllerFactory(angleCalculatorFactory, targetPositionPredictorFactory),
                    angleCalculatorFactory,
                    targetPositionPredictorFactory,
                    aircraftProvider ?? helper.CreateAircraftProvider(),
                    flightPointsProviderFactory ?? new FlightPointsProviderFactory(),
                    boostFactory ?? new BoostFactory(),
                    boostProvidersManager ?? new BoostProvidersManager(),
                    damageApplierFactory ?? new DamageApplierFactory(targetsFactory));
        }

        private IFactoryProvider CreateFactoryProvider(
            IPrefabFactory prefabFactory,
            ITargetsFactory targetsFactory,
            IMovementControllerFactory movementControllerFactory,
            IAngleCalculatorFactory angleCalculatorFactory,
            ITargetPositionPredictorFactory targetPositionControllerFactory,
            IAircraftProvider aircraftProvider,
            IFlightPointsProviderFactory flightPointsProviderFactory,
            IBoostFactory boostFactory,
            IBoostProvidersManager boostProvidersManager,
            IDamageApplierFactory damageApplierFactory)
        {
            IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();

            factoryProvider.PrefabFactory.Returns(prefabFactory);
            factoryProvider.TargetsFactory.Returns(targetsFactory);
            factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);
            factoryProvider.AngleCalculatorFactory.Returns(angleCalculatorFactory);
            factoryProvider.TargetPositionPredictorFactory.Returns(targetPositionControllerFactory);
            factoryProvider.AircraftProvider.Returns(aircraftProvider);
            factoryProvider.FlightPointsProviderFactory.Returns(flightPointsProviderFactory);
            factoryProvider.BoostFactory.Returns(boostFactory);
            factoryProvider.BoostProvidersManager.Returns(boostProvidersManager);
            factoryProvider.DamageApplierFactory.Returns(damageApplierFactory);

            return factoryProvider;
        }
    }
}
