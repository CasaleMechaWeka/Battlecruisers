using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables
{
    public class RepairManagerTests
	{
        private ICruiser _cruiser;
        private IDroneConsumerProvider _droneConsumerProvider;
        private IDroneConsumer _cruiserDroneConsumer, _buildingDroneConsumer;
        private IBuilding _building;
        private IRepairCommand _cruiserRepairCommand, _buildingRepairCommand;
		private IDroneFeedbackFactory _feedbackFactory;
        private IDroneFeedback _cruiserFeedback, _buildingFeedback;
        private float _repairAmount;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;
		private const float DELTA_TIME_IN_S = 1;
        private const float REPAIRABLE_HEALTH_GAIN_PER_DRONE_S = 3;

		[SetUp]
		public void SetuUp()
		{
            // Drones
            _cruiserDroneConsumer = Substitute.For<IDroneConsumer>();
            _cruiserDroneConsumer.NumOfDronesRequired.Returns(1);
            _buildingDroneConsumer = Substitute.For<IDroneConsumer>();
            _buildingDroneConsumer.NumOfDronesRequired.Returns(1);
            _droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();

            // Feedback
            _cruiserFeedback = Substitute.For<IDroneFeedback>();
            _cruiserFeedback.DroneConsumer.Returns(_cruiserDroneConsumer);
            _buildingFeedback = Substitute.For<IDroneFeedback>();
            _buildingFeedback.DroneConsumer.Returns(_buildingDroneConsumer);
            _feedbackFactory = Substitute.For<IDroneFeedbackFactory>();

            // Cruiser repairable
            _cruiserRepairCommand = Substitute.For<IRepairCommand>();
            _cruiser = Substitute.For<ICruiser>();
            _cruiser.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _cruiser.RepairCommand.Returns(_cruiserRepairCommand);
            _cruiserRepairCommand.Repairable.Returns(_cruiser);
            _cruiser.Position.Returns(new Vector2(17, 93));
            _cruiser.Size.Returns(new Vector2(28, 82));

            // Building repairable
            _buildingRepairCommand = Substitute.For<IRepairCommand>();
            _building = Substitute.For<IBuilding>();
            _building.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _building.RepairCommand.Returns(_buildingRepairCommand);
            _buildingRepairCommand.Repairable.Returns(_building);
            _building.Position.Returns(new Vector2(7, 3));
            _building.Size.Returns(new Vector2(8, 2));
            _repairAmount = DELTA_TIME_IN_S * _cruiserDroneConsumer.NumOfDrones * REPAIRABLE_HEALTH_GAIN_PER_DRONE_S * BuildSpeedMultipliers.DEFAULT;

            UnityAsserts.Assert.raiseExceptions = true;
		}

        [Test]
        public void BuildingStarted_AddsAsRepairable()
        {
            IRepairManager repairManager = CreateRepairManager();
            AddRepairableBuilding();
        }

        [Test]
        public void BuildingDestroyed_RemovesDroneConsumer()
        {
            BuildingStarted_AddsAsRepairable();

            _building.Destroyed += Raise.EventWith(_building, new DestroyedEventArgs(_building));

            _droneConsumerProvider.Received().ReleaseDroneConsumer(_buildingDroneConsumer);
        }

        [Test]
        public void CruiserDestroyed_RemovesAllDroneConsumers()
        {
            // Drone consumer 1
            IRepairManager repairManager = CreateRepairManager();

            // Drone consumer 2
            AddRepairableBuilding();

            // Destroy cruiser
            _cruiser.Destroyed += Raise.EventWith(_cruiser, new DestroyedEventArgs(_cruiser));

            // Released 2 drone consumers, 1 for the cruiser and 1 for the building
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_cruiserDroneConsumer);
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_buildingDroneConsumer);
        }

        [Test]
        public void Dispose_RemovesAllDroneConsumers()
        {
            // Drone consumer 1
            IRepairManager repairManager = CreateRepairManager();

			// Drone consumer 2
			AddRepairableBuilding();

            repairManager.DisposeManagedState();

			// Released 2 drone consumers, 1 for the cruiser and 1 for the building
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_cruiserDroneConsumer);
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_buildingDroneConsumer);
        }

		#region Repair()
		[Test]
        public void Repair_Unrepairable_DroneConsumerIdle_DoesNotRepair()
        {
            IRepairManager repairManager = CreateRepairManager();

            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Idle);

            repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
        }

        [Test]
        public void Repair_Unrepairable_DroneConsumerActive_Throws()
        {
            IRepairManager repairManager = CreateRepairManager();

            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

            Assert.Throws<UnityAsserts.AssertionException>(() => repairManager.Repair(DELTA_TIME_IN_S));
        }

        [Test]
        public void Repair_Repairable_DroneConsumerActive_Repairs()
        {
            IRepairManager repairManager = CreateRepairManager();

            _cruiserRepairCommand.CanExecute.Returns(true);
            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

			repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsMultiple()
        {

            IRepairManager repairManager = CreateRepairManager();
            _cruiserRepairCommand.CanExecute.Returns(true);
            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

            AddRepairableBuilding();
            _buildingDroneConsumer.State.Returns(DroneConsumerState.Active);

			repairManager.Repair(DELTA_TIME_IN_S);

			_cruiserRepairCommand.Received().Execute(_repairAmount);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsSome()
        {
            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Idle);
            IRepairManager repairManager = CreateRepairManager();

            _buildingDroneConsumer.State.Returns(DroneConsumerState.Active);
            AddRepairableBuilding();

            repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }
		#endregion Repair()

		#region RepairCommand_CanExecuteChanged
        [Test]
        public void RepairCommand_CanExecuteChanged_Repairable_ActivatesDroneConsumer()
        {
            // Add repairable without activatingdrone consumer
            IRepairManager repairManager = CreateRepairManager();

            _droneConsumerProvider.ClearReceivedCalls();

            _cruiserRepairCommand.CanExecute.Returns(true);
            _cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().ActivateDroneConsumer(_cruiserDroneConsumer);
        }

		[Test]
		public void RepairCommand_CanExecuteChanged_NotRepairable_ReleasesDroneConsumer()
		{
            // Add repairable and activating drone consumer
            IRepairManager repairManager = CreateRepairManager();

            _droneConsumerProvider.ClearReceivedCalls();

			_cruiserRepairCommand.CanExecute.Returns(false);
			_cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().ReleaseDroneConsumer(_cruiserDroneConsumer);
		}
        #endregion RepairCommand_CanExecuteChanged

        #region GetDroneConsumer
        [Test]
        public void GetDroneConsumer_ReturnsDroneConsumer()
        {
            IRepairManager repairManager = CreateRepairManager();

            IDroneConsumer droneConsumer = repairManager.GetDroneConsumer(_cruiser);
            Assert.AreSame(_cruiserDroneConsumer, droneConsumer);
        }

        [Test]
        public void GetDroneConsumer_NonExistantRepairable_Throws()
        {
            IRepairManager repairManager = CreateRepairManager();
            Assert.Throws<UnityAsserts.AssertionException>(() => repairManager.GetDroneConsumer(_building));
        }
        #endregion GetDroneConsumer

        private void AddRepairableBuilding()
        {
            _buildingRepairCommand.CanExecute.Returns(true);
            _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR).Returns(_buildingDroneConsumer);
            _feedbackFactory.CreateFeedback(_buildingDroneConsumer, _building.Position, _building.Size).Returns(_buildingFeedback);

            _cruiser.StartConstructingBuilding(_building);

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
            _feedbackFactory.Received().CreateFeedback(_buildingDroneConsumer, _building.Position, _building.Size);
            _droneConsumerProvider.Received().ActivateDroneConsumer(_buildingDroneConsumer);
		}

        private IRepairManager CreateRepairManager()
        {
            _cruiserRepairCommand.CanExecute.Returns(false);
            _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR).Returns(_cruiserDroneConsumer);
            // FELIX  Fix.
            _feedbackFactory.CreateFeedback(_cruiserDroneConsumer, _cruiser.Position, _cruiser.Size).Returns(_cruiserFeedback);

            IRepairManager repairManager = new RepairManager(_feedbackFactory, _droneConsumerProvider, _cruiser);

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
            _feedbackFactory.Received().CreateFeedback(_cruiserDroneConsumer, _cruiser.Position, _cruiser.Size);

            return repairManager;
		}
	}
}
