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
        private IDroneConsumerFocusHelper _focusHelper;

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

            _focusHelper = Substitute.For<IDroneConsumerFocusHelper>();

            _inProgressBuildingDroneConsumer = Substitute.For<IDroneConsumer>();
            // FELIX  Remove?
            //int numOfDronesRequired = _droneManager.NumOfDrones - 1;
            //_inProgressBuildingDroneConsumer.NumOfDronesRequired.Returns(numOfDronesRequired);

            _inProgressBuilding = Substitute.For<IBuilding>();
            _inProgressBuilding.DroneConsumer.Returns(_inProgressBuildingDroneConsumer);

            _focusManager = new DroneConsumerFocusManager(_strategy, _aiCruiser, _focusHelper);

            _factory = Substitute.For<IFactory>();

			UnityAsserts.Assert.raiseExceptions = true;
		}

        #region UnitStartedConstruction
        [Test]
        public void UnitStartedConstruction_TriggersFocus()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(true);

            FactoryStartBuildingUnit();

            _focusHelper.Received().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        [Test]
        public void UnitStartedConstruction_DoesNotTriggerFocus()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(false);

            FactoryStartBuildingUnit();

            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }
        #endregion UnitStartedConstruction

        #region BuildingStartedConstruction
        [Test]
        public void BuildingStartedConstruction_TriggersFocus()
        {
            _strategy.EvaluateWhenBuildingStarted.Returns(true);

            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(buildable: null));

            _focusHelper.Received().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        [Test]
        public void BuildingStartedConstruction_DoesNotTrigger()
        {
            _strategy.EvaluateWhenBuildingStarted.Returns(false);

            _aiCruiser.StartedConstruction += Raise.EventWith(_aiCruiser, new StartedConstructionEventArgs(buildable: null));

            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }
        #endregion BuildingStartedConstruction

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
            _aiCruiser.BuildingCompleted += Raise.EventWith(new CompletedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
