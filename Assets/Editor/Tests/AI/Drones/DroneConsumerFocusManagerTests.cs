using BattleCruisers.AI.Drones;
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
        private IFactory _factory;
        private IDroneConsumer _factoryDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _strategy = Substitute.For<IDroneFocusingStrategy>();

            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 12;
			_aiCruiser = Substitute.For<ICruiserController>();
            _aiCruiser.DroneManager.Returns(_droneManager);

            _focusHelper = Substitute.For<IDroneConsumerFocusHelper>();

            _focusManager = new DroneConsumerFocusManager(_strategy, _aiCruiser, _focusHelper);

            _factory = Substitute.For<IFactory>();
            _factoryDroneConsumer = Substitute.For<IDroneConsumer>();
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);

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

        #region Factory first unit
        [Test]
        public void Factory_StartedUnitConstruction_FirstUnit_Focuses()
        {
            FactoryStartBuildingUnit();
            _droneManager.Received().ToggleDroneConsumerFocus(_factoryDroneConsumer);
        }

        [Test]
        public void Factory_StartedUnitConstruction_NotFirstUnit_DoesNotFocus()
        {
            // First unit, focuses
            FactoryStartBuildingUnit();
            _droneManager.Received().ToggleDroneConsumerFocus(_factoryDroneConsumer);

            // Second unit, does not focus
            _droneManager.ClearReceivedCalls();
            _factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
            _droneManager.DidNotReceive().ToggleDroneConsumerFocus(_factoryDroneConsumer);
        }
        #endregion Factory first unit

        // Factory destroyed

        // Dispose

        private void FactoryStartBuildingUnit()
		{
            _aiCruiser.BuildingCompleted += Raise.EventWith(new CompletedConstructionEventArgs(_factory));
			_factory.CompletedBuildable += Raise.Event();
			_factory.StartedBuildingUnit += Raise.EventWith(_factory, new StartedConstructionEventArgs(buildable: null));
		}
    }
}
