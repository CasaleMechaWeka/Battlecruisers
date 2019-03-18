using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
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
        private IDroneConsumer _cruiserDroneConsumer, _buildingDroneConsumer;
        private IBuilding _building;
        private IRepairCommand _cruiserRepairCommand, _buildingRepairCommand;
		private IDroneNumFeedbackFactory _feedbackFactory;
        private IDroneNumFeedback _cruiserFeedback, _buildingFeedback;
        private ITextMesh _numOfRepairDronesText;
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
            _cruiserFeedback = Substitute.For<IDroneNumFeedback>();
            _cruiserFeedback.DroneConsumer.Returns(_cruiserDroneConsumer);
            _buildingFeedback = Substitute.For<IDroneNumFeedback>();
            _buildingFeedback.DroneConsumer.Returns(_buildingDroneConsumer);
            _feedbackFactory = Substitute.For<IDroneNumFeedbackFactory>();
			_numOfRepairDronesText = Substitute.For<ITextMesh>();

            // Cruiser repairable
            _cruiserRepairCommand = Substitute.For<IRepairCommand>();
            _cruiser = Substitute.For<ICruiser>();
            _cruiser.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _cruiser.DroneConsumerProvider.Returns(_droneConsumerProvider);
			_cruiser.NumOfRepairDronesText.Returns(_numOfRepairDronesText);
            _cruiser.RepairCommand.Returns(_cruiserRepairCommand);
            _cruiserRepairCommand.Repairable.Returns(_cruiser);

            // Building repairable
            _buildingRepairCommand = Substitute.For<IRepairCommand>();
            _building = Substitute.For<IBuilding>();
            _building.HealthGainPerDroneS.Returns(REPAIRABLE_HEALTH_GAIN_PER_DRONE_S);
            _building.NumOfRepairDronesText.Returns(_numOfRepairDronesText);
            _building.RepairCommand.Returns(_buildingRepairCommand);
            _buildingRepairCommand.Repairable.Returns(_building);

            _repairAmount = DELTA_TIME_IN_S * _cruiserDroneConsumer.NumOfDrones * REPAIRABLE_HEALTH_GAIN_PER_DRONE_S * BuildSpeedMultipliers.DEFAULT;

            _repairManager = new RepairManager(_feedbackFactory);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Initialise_AddsCruiserAsRepairable_NotRepairable_DoesNotActivateDroneConsumer()
		{
            AddCruiser(isRepairable: true);
		}

		[Test]
        public void Initialise_AddsCruiserAsRepairable_Repairable_ActivatesDroneConsumer()
		{
            AddCruiser(isRepairable: false);
		}

        [Test]
        public void BuildingStarted_AddsAsRepairable()
        {
            AddCruiser();
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
            AddCruiser();

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
			AddCruiser();

			// Drone consumer 2
			AddRepairableBuilding();

            _repairManager.DisposeManagedState();

			// Released 2 drone consumers, 1 for the cruiser and 1 for the building
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_cruiserDroneConsumer);
            _droneConsumerProvider.Received().ReleaseDroneConsumer(_buildingDroneConsumer);
        }

		#region Repair()
		[Test]
        public void Repair_Unrepairable_DroneConsumerIdle_DoesNotRepair()
        {
            AddCruiser(isRepairable: false);

            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Idle);

            _repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
        }

        [Test]
        public void Repair_Unrepairable_DroneConsumerActive_Throws()
        {
            AddCruiser(isRepairable: false);

            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

            Assert.Throws<UnityAsserts.AssertionException>(() => _repairManager.Repair(DELTA_TIME_IN_S));
        }

        [Test]
        public void Repair_Repairable_DroneConsumerActive_Repairs()
        {
            AddCruiser(isRepairable: true);

            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

			_repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsMultiple()
        {
            AddCruiser(isRepairable: true);
            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Active);

            AddRepairableBuilding();
            _buildingDroneConsumer.State.Returns(DroneConsumerState.Active);

			_repairManager.Repair(DELTA_TIME_IN_S);

			_cruiserRepairCommand.Received().Execute(_repairAmount);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }

        [Test]
        public void Repair_RepairsSome()
        {
            _cruiserDroneConsumer.State.Returns(DroneConsumerState.Idle);
            AddCruiser(isRepairable: false);

            _buildingDroneConsumer.State.Returns(DroneConsumerState.Active);
            AddRepairableBuilding();

            _repairManager.Repair(DELTA_TIME_IN_S);

            _cruiserRepairCommand.DidNotReceiveWithAnyArgs().Execute(parameter: -99);
            _buildingRepairCommand.Received().Execute(_repairAmount);
        }
		#endregion Repair()

		#region RepairCommand_CanExecuteChanged
        [Test]
        public void RepairCommand_CanExecuteChanged_Repairable_ActivatesDroneConsumer()
        {
            // Add repairable without activatingdrone consumer
            AddCruiser(isRepairable: false);

            _droneConsumerProvider.ClearReceivedCalls();

            _cruiserRepairCommand.CanExecute.Returns(true);
            _cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().ActivateDroneConsumer(_cruiserDroneConsumer);
        }

		[Test]
		public void RepairCommand_CanExecuteChanged_NotRepairable_ReleasesDroneConsumer()
		{
            // Add repairable and activating drone consumer
            AddCruiser(isRepairable: true);

			_droneConsumerProvider.ClearReceivedCalls();

			_cruiserRepairCommand.CanExecute.Returns(false);
			_cruiserRepairCommand.CanExecuteChanged += Raise.Event();

            _droneConsumerProvider.Received().ReleaseDroneConsumer(_cruiserDroneConsumer);
		}
        #endregion RepairCommand_CanExecuteChanged

        #region GetDroneConsumer
        [Test]
        public void GetDroneConsumer_Repairable_ReturnsDroneConsumer()
        {
            AddCruiser(isRepairable: true);

            IDroneConsumer droneConsumer = _repairManager.GetDroneConsumer(_cruiser);
            Assert.AreSame(_cruiserDroneConsumer, droneConsumer);
        }

        [Test]
        public void GetDroneConsumer_NotRepairable_ReturnsDroneConsumer()
        {
            AddCruiser(isRepairable: false);

            IDroneConsumer droneConsumer = _repairManager.GetDroneConsumer(_cruiser);
            Assert.AreSame(_cruiserDroneConsumer, droneConsumer);
        }

        [Test]
        public void GetDroneConsumer_NonExistantRepairable_Throws()
        {
            AddCruiser();

            Assert.Throws<UnityAsserts.AssertionException>(() => _repairManager.GetDroneConsumer(_building));
        }
        #endregion GetDroneConsumer

        private void AddRepairableBuilding()
        {
            _buildingRepairCommand.CanExecute.Returns(true);
            _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR).Returns(_buildingDroneConsumer);
            _feedbackFactory.CreateFeedback(_buildingDroneConsumer, _numOfRepairDronesText).Returns(_buildingFeedback);

            _cruiser.StartConstructingBuilding(_building);

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
            _feedbackFactory.Received().CreateFeedback(_buildingDroneConsumer, _numOfRepairDronesText);
            _droneConsumerProvider.Received().ActivateDroneConsumer(_buildingDroneConsumer);
		}

        private void AddCruiser(bool isRepairable = false)
        {
            _cruiserRepairCommand.CanExecute.Returns(isRepairable);
            _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR).Returns(_cruiserDroneConsumer);
            _feedbackFactory.CreateFeedback(_cruiserDroneConsumer, _numOfRepairDronesText).Returns(_cruiserFeedback);
			
            _repairManager.Initialise(_cruiser);

            _droneConsumerProvider.Received().RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
            _feedbackFactory.Received().CreateFeedback(_cruiserDroneConsumer, _numOfRepairDronesText);

            // Only activate drone consumer if repairable is currently repairable
            if (isRepairable)
            {
				_droneConsumerProvider.Received().ActivateDroneConsumer(_cruiserDroneConsumer);
            }
            else
            {
                _droneConsumerProvider.DidNotReceive().ActivateDroneConsumer(_cruiserDroneConsumer);
            }
		}
	}
}
