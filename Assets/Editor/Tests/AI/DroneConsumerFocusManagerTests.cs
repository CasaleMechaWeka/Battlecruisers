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
		public void FactoryStartedBuildingUnit_FactoryIdle_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Idle);

            EmitFactoryStartedBuildingUnit();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FactoryStartedBuildingUnit_NoInProgressBuildings_DoesNothing()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            EmitFactoryStartedBuildingUnit();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

		[Test]
		public void FactoryStartedBuildingUnit_FocusedInProgressBuildingExists_DoesNothing()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Focused);
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			EmitFactoryStartedBuildingUnit();

			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}

        [Test]
        public void FactoryStartedBuildingUnit_NoAffordableInProgressBuilding_DoesNothing()
		{
			_factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Active);
            int numOfDronesRequired = _droneManager.NumOfDrones + 1;
            _inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);

			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

			EmitFactoryStartedBuildingUnit();

			_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		}

        [Test]
        public void FactoryStartedBuildingUnit_FactoryNotIdle_AndHaveInProgessBuildings_AndNoFocusedInProgressBuilding_AndCanAffordInProgressBuilding_FocusesOnInProgressBuilding()
        {
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle, DroneConsumerState.Idle, DroneConsumerState.Active);
            int numOfDronesRequired = _droneManager.NumOfDrones - 1;
            _inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);
			_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

            EmitFactoryStartedBuildingUnit();

            // One toggle for Idle => Active, another for Active => Focused
            _droneManager.Received(requiredNumberOfCalls: 2).ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }
		
		private void EmitFactoryStartedBuildingUnit()
		{
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
