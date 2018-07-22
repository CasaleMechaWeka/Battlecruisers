using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.AI.Drones
{
    public class AffordableInProgressNonAffordableProviderTests
    {
        private IBuildingProvider _provider;
        private IDroneManager _droneManager;
        private IInProgressBuildingMonitor _buildingMonitor;
        private IList<IBuildable> _inProgressBuildings;
        private IBuilding _building;
        private IDroneConsumer _buildingDroneConsumer;

        [SetUp]
        public void TestSetup()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 6;
            _buildingMonitor = Substitute.For<IInProgressBuildingMonitor>();

            _provider = new AffordableInProgressNonFocusedProvider(_droneManager, _buildingMonitor);

            _inProgressBuildings = new List<IBuildable>();
            ReadOnlyCollection<IBuildable> readonlyInProgressBuildings = new ReadOnlyCollection<IBuildable>(_inProgressBuildings);
            _buildingMonitor.InProgressBuildings.Returns(readonlyInProgressBuildings);

            _building = Substitute.For<IBuilding>();
            _buildingDroneConsumer = Substitute.For<IDroneConsumer>();
            _building.DroneConsumer.Returns(_buildingDroneConsumer);
        }

        [Test]
        public void NoInProgressBuildings_ReturnsNull()
        {
            Assert.IsNull(_provider.Building);
        }

        [Test]
        public void NoNonFocusedBuildings_ReturnsNull()
        {
            _buildingDroneConsumer.State.Returns(DroneConsumerState.Focused);
            _inProgressBuildings.Add(_building);

            Assert.IsNull(_provider.Building);
        }

        [Test]
        public void NoAffordableBuildings_ReturnsNull()
        {
            _buildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
            int unAffordableDroneNum = _droneManager.NumOfDrones + 1;
            _buildingDroneConsumer.NumOfDronesRequired.Returns(unAffordableDroneNum);
            _inProgressBuildings.Add(_building);

            Assert.IsNull(_provider.Building);
        }

        [Test]
        public void InProgress_NonFocused_AffordableBuilding_ReturnsBuilding()
        {
            _buildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
            int affordableDroneNum = _droneManager.NumOfDrones - 1;
            _buildingDroneConsumer.NumOfDronesRequired.Returns(affordableDroneNum);
            _inProgressBuildings.Add(_building);

            Assert.AreSame(_building, _provider.Building);
        }
    }
}