using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables
{
    public class RepairManagerTests
	{
        private RepairManager _repairManager;
        private ICruiser _cruiser;
        private IDroneConsumerProvider _droneConsumerProvider;
        private IDroneConsumer _droneConsumer;
        private IBuilding _building;
        private IRepairCommand _cruiserRepairCommand, _buildingRepairCommand;
        private float _repairAmount;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;
		private const float DELTA_TIME_IN_S = 1;
        private const float REPAIRABLE_HEALTH_GAIN_PER_DRONE_S = 3;

		[SetUp]
		public void SetuUp()
		{
            _droneConsumer = Substitute.For<IDroneConsumer>();
            _droneConsumer.NumOfDronesRequired.Returns(2);
            _droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
            _droneConsumerProvider.RequestDroneConsumer(numOfDronesRequired: -99, isHighPriority: false).ReturnsForAnyArgs(_droneConsumer);
            _cruiserRepairCommand = Substitute.For<IRepairCommand>();
            _cruiser = Substitute.For<ICruiser>();
            _cruiser.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _cruiser.DroneConsumerProvider.Returns(_droneConsumerProvider);
            _cruiser.RepairCommand.Returns(_cruiserRepairCommand);
            _cruiserRepairCommand.Repairable.Returns(_cruiser);

            _buildingRepairCommand = Substitute.For<IRepairCommand>();
            _building = Substitute.For<IBuilding>();
            _building.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _building.RepairCommand.Returns(_buildingRepairCommand);
            _buildingRepairCommand.Repairable.Returns(_building);

            _repairAmount = DELTA_TIME_IN_S * _droneConsumer.NumOfDrones * REPAIRABLE_HEALTH_GAIN_PER_DRONE_S;

            // FELIX
            _repairManager = new RepairManager(new DummyDeferrer(), null);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Initialise_AddsCruiserAsRepairable_NotRepairable_DoesNotCreateDroneConsumer()
		{
            _cruiserRepairCommand.CanExecute.Returns(false);

            _repairManager.Initialise(_cruiser);

            _droneConsumerProvider.DidNotReceiveWithAnyArgs().RequestDroneConsumer(numOfDronesRequired: -99, isHighPriority: false);
		}

		[Test]
        public void Initialise_AddsCruiserAsRepairable_Repairable_CreatesDroneConsumer()
		{
			_cruiserRepairCommand.CanExecute.Returns(true);

			_repairManager.Initialise(_cruiser);

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR, isHighPriority: false);
            _droneConsumerProvider.Received().ActivateDroneConsumer(_droneConsumer);
		}

        [Test]
        public void BuildingStarted_AddsAsRepairable()
        {
            AddUnrepairableCruiser();
            AddRepairableBuilding();
        }

        [Test]
        public void BuildingDestroyed_RemovesDroneConsumer()
        {
            BuildingStarted_AddsAsRepairable();

            _building.Destroyed += Raise.EventWith(_building, new DestroyedEventArgs(_building));

            _droneConsumerProvider.Received().ReleaseDroneConsumer(_droneConsumer);
        }

        [Test]
        public void CruiserDestroyed_RemovesAllDroneConsumers()
        {
            // Drone consumer 1
            AddRepairableCruiser();

            // Drone consumer 2
            AddRepairableBuilding();

            // Destroy cruiser
            _cruiser.Destroyed += Raise.EventWith(_cruiser, new DestroyedEventArgs(_cruiser));

            // Released 2 drone consumers, 1 for the cruiser and 1 for the building
            _droneConsumerProvider.Received(requiredNumberOfCalls: 2).ReleaseDroneConsumer(_droneConsumer);
        }

        [Test]
        public void Dispose_RemovesAllDroneConsumers()
        {
			// Drone consumer 1
			AddRepairableCruiser();

			// Drone consumer 2
			AddRepairableBuilding();

            _repairManager.Dispose();

			// Released 2 drone consumers, 1 for the cruiser and 1 for the building
			_droneConsumerProvider.Received(requiredNumberOfCalls: 2).ReleaseDroneConsumer(_droneConsumer);
        }

		#region Repair()
		[Test]
        public void Repair_DoesNotRepairUnrepairable()
        {
            AddUnrepairableCruiser();

            _repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
        }

        [Test]
        public void Repair_RepairsRepairable()
        {
            AddRepairableCruiser();

			_repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsMultiple()
        {
            AddRepairableCruiser();
            AddRepairableBuilding();

			_repairManager.Repair(DELTA_TIME_IN_S);

			_cruiserRepairCommand.Received().Execute(_repairAmount);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsSome()
        {
            AddUnrepairableCruiser();
            AddRepairableBuilding();

            _repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }
		#endregion Repair()

		#region RepairCommand_CanExecuteChanged
        [Test]
        public void RepairCommand_CanExecuteChanged_Repairable_NoExistingDroneConsumer_RequestsDroneConsumer()
        {
            // Add repairable without requesting drone consumer
            AddUnrepairableCruiser();

            _droneConsumerProvider.ClearReceivedCalls();

            _cruiserRepairCommand.CanExecute.Returns(true);
            _cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR, isHighPriority: false);
        }

		[Test]
		public void RepairCommand_CanExecuteChanged_Repairable_ExistingDroneConsumer_DoesNothing()
		{
            AddRepairableCruiser();

			_droneConsumerProvider.ClearReceivedCalls();

			_cruiserRepairCommand.CanExecute.Returns(true);
			_cruiserRepairCommand.CanExecuteChanged += Raise.Event();

			_droneConsumerProvider.DidNotReceiveWithAnyArgs().RequestDroneConsumer(numOfDronesRequired: -99, isHighPriority: false);
            _droneConsumerProvider.DidNotReceiveWithAnyArgs().ReleaseDroneConsumer(droneConsumer: null);
		}

		[Test]
		public void RepairCommand_CanExecuteChanged_NotRepairable_NoExistingDroneConsumer_DoesNothing()
		{
			// Add repairable without requesting drone consumer
			AddUnrepairableCruiser();

			_droneConsumerProvider.ClearReceivedCalls();

			_cruiserRepairCommand.CanExecute.Returns(false);
			_cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.DidNotReceiveWithAnyArgs().RequestDroneConsumer(numOfDronesRequired: -99, isHighPriority: false);
			_droneConsumerProvider.DidNotReceiveWithAnyArgs().ReleaseDroneConsumer(droneConsumer: null);
		}

		[Test]
		public void RepairCommand_CanExecuteChanged_NotRepairable_ExistingDroneConsumer_ReleasesDroneConsumer()
		{
			AddRepairableCruiser();

			_droneConsumerProvider.ClearReceivedCalls();

			_cruiserRepairCommand.CanExecute.Returns(false);
			_cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().ReleaseDroneConsumer(_droneConsumer);
		}
        #endregion RepairCommand_CanExecuteChanged

        #region GetDroneConsumer
        [Test]
        public void GetDroneConsumer_IsRepairable_ReturnsDroneConsumer()
        {
            AddRepairableCruiser();

            IDroneConsumer droneConsumer = _repairManager.GetDroneConsumer(_cruiser);
            Assert.AreSame(_droneConsumer, droneConsumer);
        }

        [Test]
        public void GetDroneConsumer_IsNotRepairable_ReturnsNull()
        {
            AddUnrepairableCruiser();

            IDroneConsumer droneConsumer = _repairManager.GetDroneConsumer(_cruiser);
            Assert.IsNull(droneConsumer);
        }

        [Test]
        public void GetDroneConsumer_NonExistantRepairable_Throws()
        {
            AddUnrepairableCruiser();
            Assert.Throws<UnityAsserts.AssertionException>(() => _repairManager.GetDroneConsumer(_building));
        }
        #endregion GetDroneConsumer

        private void AddRepairableBuilding()
        {
            _buildingRepairCommand.CanExecute.Returns(true);
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_building));
            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR, isHighPriority: false);
			_droneConsumerProvider.Received().ActivateDroneConsumer(_droneConsumer);
		}

        private void AddRepairableCruiser()
        {
            _cruiserRepairCommand.CanExecute.Returns(true);
			_repairManager.Initialise(_cruiser);
            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR, isHighPriority: false);
			_droneConsumerProvider.Received().ActivateDroneConsumer(_droneConsumer);
		}

        private void AddUnrepairableCruiser()
        {
			_cruiserRepairCommand.CanExecute.Returns(false);
			_repairManager.Initialise(_cruiser);
        }
	}
}
