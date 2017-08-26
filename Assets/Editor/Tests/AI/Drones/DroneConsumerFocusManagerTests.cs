using BattleCruisers.AI.Drones;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Drones
{
    public class DroneConsumerFocusManagerTests
	{
        private IDroneFocusingStrategy _strategy;
        private ICruiserController _aiCruiser;
        private IDroneManager _droneManager;
        private IFactory _factory;
        private IBuilding _inProgressBuilding;
        private IDroneConsumer _factoryDroneConsumer, _inProgressBuildingDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _strategy = Substitute.For<IDroneFocusingStrategy>();

            // FELIX  Remove!
			//_strategy.EvaluateWhenUnitStarted.Returns(true);
			//_strategy.EvaluateWhenBuildingStarted.Returns(true);
            //_strategy.ForceInProgressBuildingToFocused.Returns(false);

            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 12;
			_aiCruiser = Substitute.For<ICruiserController>();
            _aiCruiser.DroneManager.Returns(_droneManager);

            new DroneConsumerFocusManager(_strategy, _aiCruiser);

            _factoryDroneConsumer = Substitute.For<IDroneConsumer>();
            _factory = Substitute.For<IFactory>();
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);

            _inProgressBuildingDroneConsumer = Substitute.For<IDroneConsumer>();
            _inProgressBuilding = Substitute.For<IBuilding>();
            _inProgressBuilding.DroneConsumer.Returns(_inProgressBuildingDroneConsumer);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		#region FocusOnNonFactoryDroneConsumer
		[Test]
		public void FocusOnNonFactoryDroneConsumer_FactoryIdle_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Idle);

            TriggerFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoInProgressBuildings_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            TriggerFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

		[Test]
		public void FocusOnNonFactoryDroneConsumer_FocusedInProgressBuildingExists_DoesNothing()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Focused);
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			TriggerFocusOnNonFactoryDroneConsumer();

			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoAffordableInProgressBuilding_DoesNothing()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Active);
            int numOfDronesRequired = _droneManager.NumOfDrones + 1;
            _inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);

			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			TriggerFocusOnNonFactoryDroneConsumer();

			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}

        [Test]
        public void FocusOnNonFactoryDroneConsumer_GoesActive()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

			SetupInProgressBuilding();
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			_strategy.ForceInProgressBuildingToFocused.Returns(false);
            TriggerFocusOnNonFactoryDroneConsumer();

            // Idle => Active
			_droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }

		[Test]
		public void FocusOnNonFactoryDroneConsumer_GoesFocused()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle, DroneConsumerState.Idle, DroneConsumerState.Active);
			int numOfDronesRequired = _droneManager.NumOfDrones - 1;
			_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			_strategy.ForceInProgressBuildingToFocused.Returns(true);
			TriggerFocusOnNonFactoryDroneConsumer();

			// Idle => Active, Active => Focused
            _droneManager.Received(requiredNumberOfCalls: 2).ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
		}
		#endregion FocusOnNonFactoryDroneConsumer

		#region UnitStartedConstruction
        [Test]
        public void UnitStartedConstruction_DoesNotEvaluate()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(false);

            SetupInProgressBuilding();

            FactoryStartBuildingUnit();

			// No focusing
			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }
		#endregion UnitStartedConstruction

		#region BuildingStartedConstruction
		[Test]
        public void BuildingStartedConstruction_AlsoTriggers_FocusOnNonFactoryDroneConsumer()
        {
            CreateActiveFactory();

            SetupInProgressBuilding();

			_strategy.ForceInProgressBuildingToFocused.Returns(false);
            _strategy.EvaluateWhenBuildingStarted.Returns(true);
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

            // Focusing:  Idle => Active
            _droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }

        [Test]
        public void BuildingStartedConstruction_DoesNotEvalute()
        {
			_strategy.EvaluateWhenBuildingStarted.Returns(false);

            CreateActiveFactory();

            SetupInProgressBuilding();

			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

            // No focusing
            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}
		#endregion BuildingStartedConstruction

		private void SetupInProgressBuilding()
		{
			_inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
			int numOfDronesRequired = _droneManager.NumOfDrones - 1;
			_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);
		}
		
		private void CreateActiveFactory()
		{
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);
		}
		
        private void TriggerFocusOnNonFactoryDroneConsumer()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(true);

            FactoryStartBuildingUnit();
        }

        private void FactoryStartBuildingUnit()
		{
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
