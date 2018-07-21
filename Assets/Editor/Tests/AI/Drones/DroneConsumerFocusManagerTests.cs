using BattleCruisers.AI.Drones;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Drones
{
    public class DroneConsumerFocusManagerTests
	{
        private DroneConsumerFocusManager _focusManager;

        private IDroneFocusingStrategy _strategy;
        private ICruiserController _aiCruiser;
        private IDroneManager _droneManager;
        private IFactoriesMonitor _factoriesMonitor;
        private IBuildingMonitor _buildingMonitor;
        
        // FELIX  Unused?
        private IFactory _factory;

        private IBuilding _inProgressBuilding;
        private IDroneConsumer _inProgressBuildingDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _strategy = Substitute.For<IDroneFocusingStrategy>();

            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 12;
			_aiCruiser = Substitute.For<ICruiserController>();
            _aiCruiser.DroneManager.Returns(_droneManager);

            _factoriesMonitor = Substitute.For<IFactoriesMonitor>();
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(true);

            _inProgressBuildingDroneConsumer = Substitute.For<IDroneConsumer>();
            // FELIX  Remove?
            //int numOfDronesRequired = _droneManager.NumOfDrones - 1;
            //_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);

            _inProgressBuilding = Substitute.For<IBuilding>();
            _inProgressBuilding.DroneConsumer.Returns(_inProgressBuildingDroneConsumer);

            _buildingMonitor = Substitute.For<IBuildingMonitor>();
            _buildingMonitor.GetNonFocusedAffordableBuilding().Returns(_inProgressBuilding);

            _focusManager = new DroneConsumerFocusManager(_strategy, _aiCruiser, _factoriesMonitor, _buildingMonitor);

            _factory = Substitute.For<IFactory>();

			UnityAsserts.Assert.raiseExceptions = true;
		}

        #region FocusOnNonFactoryDroneConsumer
        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoFactoriesWronglyUsingDrones_DoesNothing()
        {
            _factoriesMonitor.AreAnyFactoriesWronglyUsingDrones.Returns(false);

            TriggerFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_NoAffordableNonFocusedBuildings_DoesNothing()
        {
            _buildingMonitor.GetNonFocusedAffordableBuilding().Returns((IBuildable)null);

            TriggerFocusOnNonFactoryDroneConsumer();

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
        }

        [Test]
        public void FocusOnNonFactoryDroneConsumer_GoesActive()
        {
			_strategy.ForceInProgressBuildingToFocused.Returns(false);
            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);

            TriggerFocusOnNonFactoryDroneConsumer();

            // Idle => Active
			_droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
        }

		[Test]
		public void FocusOnNonFactoryDroneConsumer_GoesFocused()
		{
			_strategy.ForceInProgressBuildingToFocused.Returns(true);
            _inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle, DroneConsumerState.Active);
			
			TriggerFocusOnNonFactoryDroneConsumer();

			// Idle => Active, Active => Focused
            _droneManager
                .Received(requiredNumberOfCalls: 2)
                .ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
		}
		#endregion FocusOnNonFactoryDroneConsumer

		//#region UnitStartedConstruction
  //      [Test]
  //      public void UnitStartedConstruction_DoesNotEvaluate()
  //      {
  //          _strategy.EvaluateWhenUnitStarted.Returns(false);

  //          SetupInProgressBuilding();

  //          FactoryStartBuildingUnit();

		//	// No focusing
		//	_droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
  //      }
		//#endregion UnitStartedConstruction

		//#region BuildingStartedConstruction
		//[Test]
  //      public void BuildingStartedConstruction_AlsoTriggers_FocusOnNonFactoryDroneConsumer()
  //      {
  //          CreateActiveFactory();

  //          SetupInProgressBuilding();

		//	_strategy.ForceInProgressBuildingToFocused.Returns(false);
  //          _strategy.EvaluateWhenBuildingStarted.Returns(true);
  //          _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

  //          // Focusing:  Idle => Active
  //          _droneManager.Received().ToggleDroneConsumerFocus(_inProgressBuildingDroneConsumer);
  //      }

  //      [Test]
  //      public void BuildingStartedConstruction_DoesNotEvalute()
  //      {
		//	_strategy.EvaluateWhenBuildingStarted.Returns(false);

  //          CreateActiveFactory();

  //          SetupInProgressBuilding();

		//	_aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_inProgressBuilding));

  //          // No focusing
  //          _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(droneConsumer: null);
		//}
		//#endregion BuildingStartedConstruction

        // FELIX  Use/remove :)
		private void SetupInProgressBuilding()
		{
			_inProgressBuildingDroneConsumer.State.Returns(DroneConsumerState.Idle);
			int numOfDronesRequired = _droneManager.NumOfDrones - 1;
			_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);
		}
		
        private void TriggerFocusOnNonFactoryDroneConsumer()
        {
            _strategy.EvaluateWhenBuildingStarted.Returns(true);
            _aiCruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(buildable: null));
        }

        // FELIX  Remove
        private void FactoryStartBuildingUnit()
		{
            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
