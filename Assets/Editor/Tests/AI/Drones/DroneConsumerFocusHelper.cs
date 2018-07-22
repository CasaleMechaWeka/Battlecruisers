using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Drones
{
    public class DroneConsumerFocusHelperTests
    {
        private IDroneConsumerFocusHelper _focusHelper;

        private IDroneManager _droneManager;
        private IFactoriesMonitor _factoriesMonitor;
        private IBuildingProvider _buildingProvider;
        private IBuilding _inProgressBuilding;
        private IDroneConsumer _inProgressBuildingDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _droneManager = Substitute.For<IDroneManager>();
            _factoriesMonitor = Substitute.For<IFactoriesMonitor>();

            _inProgressBuilding = Substitute.For<IBuilding>();
            _inProgressBuildingDroneConsumer = Substitute.For<IDroneConsumer>();
            _inProgressBuilding.DroneConsumer.Returns(_inProgressBuildingDroneConsumer);

            _buildingProvider = Substitute.For<IBuildingProvider>();

            _focusHelper = new DroneConsumerFocusHelper(_droneManager, _factoriesMonitor, _buildingProvider);
		}

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoFactoriesWronglyUsingDrones_DoesNothing()
        {
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(false);

            _focusHelper.FocusOnNonFactoryDroneConsumer(forceInProgressBuildingToFocused: false);

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoAffordableNonFocusedBuildings_DoesNothing()
        {
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(true);
            _buildingProvider.Building.Returns((IBuildable)null);

            _focusHelper.FocusOnNonFactoryDroneConsumer(forceInProgressBuildingToFocused: false);

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_GoesActive()
        {
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(true);
            _buildingProvider.Building.Returns(_inProgressBuilding);
            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);

            _focusHelper.FocusOnNonFactoryDroneConsumer(forceInProgressBuildingToFocused: false);

            // Idle => Active
            _droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }

		[Test]
		public void FocusOnNonFactoryDroneConsumer_GoesFocused()
		{
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(true);
            _buildingProvider.Building.Returns(_inProgressBuilding);
            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle, DroneConsumerState.Active);

            _focusHelper.FocusOnNonFactoryDroneConsumer(forceInProgressBuildingToFocused: true);

			// Idle => Active, Active => Focused
            _droneManager
                .Received(requiredNumberOfCalls: 2)
                .ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
		}
    }
}
