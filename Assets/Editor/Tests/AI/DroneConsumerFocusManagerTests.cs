using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI
{
    public class DroneConsumerFocusManagerTests
	{
        private ICruiserController _aiCruiser;
        private IDroneManager _droneManager;
        private IFactory _factory;
        private IBuilding _inProgressBuilding;
        private IDroneConsumer _factoryDroneConsumer, _inProgressBuildingDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 12;
			_aiCruiser = Substitute.For<ICruiserController>();
            _aiCruiser.DroneManager.Returns(_droneManager);

            new DroneConsumerFocusManager(_aiCruiser);

            _factoryDroneConsumer = Substitute.For<IDroneConsumer>();
            _factory = Substitute.For<IFactory>();
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);

            _inProgressBuildingDroneConsumer = Substitute.For<IDroneConsumer>();
            _inProgressBuilding = Substitute.For<IBuilding>();
            _inProgressBuilding.DroneConsumer.Returns(_inProgressBuildingDroneConsumer);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void FocusOnNonFactoryDroneConsumer_FactoryIdle_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Idle);

            EmitFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoInProgressBuildings_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            EmitFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

		[Test]
		public void FocusOnNonFactoryDroneConsumer_FocusedInProgressBuildingExists_DoesNothing()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Focused);
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			EmitFocusOnNonFactoryDroneConsumer();

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

			EmitFocusOnNonFactoryDroneConsumer();

			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}

        [Test]
        public void FocusOnNonFactoryDroneConsumer_FactoryNotIdle_AndHaveInProgessBuildings_AndNoFocusedInProgressBuilding_AndCanAffordInProgressBuilding_FocusesOnInProgressBuilding()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
            int numOfDronesRequired = _droneManager.NumOfDrones - 1;
            _inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

            EmitFocusOnNonFactoryDroneConsumer();

            // Idle => Active
            _droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }

        [Test]
        public void BuildingStartedConstruction_AlsoTriggers_FocusOnNonFactoryDroneConsumer()
        {
            // Create active factory
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
            _factory.CompletedBuildable += Raise.Event();
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

			// Setup in progress building
			_inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
			int numOfDronesRequired = _droneManager.NumOfDrones - 1;
			_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);

            // Trigger FocusOnNonFactoryDroneConsumer()
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			// Idle => Active
			_droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
		}
		
		private void EmitFocusOnNonFactoryDroneConsumer()
		{
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
