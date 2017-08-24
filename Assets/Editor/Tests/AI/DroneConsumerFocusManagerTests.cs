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
            _aiCruiser = Substitute.For<ICruiserController>();
            _droneManager = Substitute.For<IDroneManager>();

            new DroneConsumerFocusManager(_aiCruiser, _droneManager);

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
		
		private void EmitFactoryStartedBuildingUnit()
		{
			_factory.StartedConstruction += Raise.Event();
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
