using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.Strategies;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Drones
{
    public class DroneConsumerFocusManagerTests
	{
        private DroneConsumerFocusManager _focusManager;

        private IDroneFocusingStrategy _strategy;
        private IDroneManager _droneManager;
        private ICruiserBuildingMonitor _aiBuildingMonitor;
        private IDroneConsumerFocusHelper _focusHelper;
        private IFactory _factory;
        private IDroneConsumer _factoryDroneConsumer;

		[SetUp]
		public void SetuUp()
		{
            _strategy = Substitute.For<IDroneFocusingStrategy>();

            ICruiserController aiCruiser = Substitute.For<ICruiserController>();
            _droneManager = aiCruiser.DroneManager;
            _droneManager.NumOfDrones = 12;
            _aiBuildingMonitor = aiCruiser.BuildingMonitor;

            _focusHelper = Substitute.For<IDroneConsumerFocusHelper>();

            _focusManager = new DroneConsumerFocusManager(_strategy, aiCruiser, _focusHelper);

            _factory = Substitute.For<IFactory>();
            _factoryDroneConsumer = Substitute.For<IDroneConsumer>();
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);
		}

        #region UnitStartedConstruction
        [Test]
        public void UnitStartedConstruction_TriggersFocus()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(true);

            BuildFactory();
            BuildUnit();

            _focusHelper.Received().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        [Test]
        public void UnitStartedConstruction_DoesNotTriggerFocus()
        {
            _strategy.EvaluateWhenUnitStarted.Returns(false);

            BuildFactory();
            BuildUnit();

            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }
        #endregion UnitStartedConstruction

        #region BuildingStartedConstruction
        [Test]
        public void BuildingStartedConstruction_TriggersFocus()
        {
            _strategy.EvaluateWhenBuildingStarted.Returns(true);

            _aiBuildingMonitor.EmitBuildingStarted(buildingToStart: null);

            _focusHelper.Received().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        [Test]
        public void BuildingStartedConstruction_DoesNotTrigger()
        {
            _strategy.EvaluateWhenBuildingStarted.Returns(false);

            _aiBuildingMonitor.EmitBuildingStarted(buildingToStart: null);

            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }
        #endregion BuildingStartedConstruction

        #region Factory first unit
        [Test]
        public void Factory_StartedUnitConstruction_FirstUnit_Focuses()
        {
            BuildFactory();
            BuildUnit();
            _droneManager.Received().ToggleDroneConsumerFocus(_factoryDroneConsumer);
        }

        [Test]
        public void Factory_StartedUnitConstruction_NotFirstUnit_DoesNotFocus()
        {
            // First unit, focuses
            BuildFactory();
            BuildUnit();
            _droneManager.Received().ToggleDroneConsumerFocus(_factoryDroneConsumer);

            // Second unit, does not focus
            _droneManager.ClearReceivedCalls();
            BuildUnit();
            _droneManager.DidNotReceive().ToggleDroneConsumerFocus(_factoryDroneConsumer);
        }
        #endregion Factory first unit

        [Test]
        public void FactoryDestroyed_UnsubscribesFromEvents()
        {
            BuildFactory();

            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));

            // Build unit for destroyed factory, should no longer trigger focus
            BuildUnit();
            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        [Test]
        public void Dispose_Unsubscribes()
        {
            BuildFactory();

            _focusManager.DisposeManagedState();

            // Factory started construction ignored
            _strategy.EvaluateWhenUnitStarted.Returns(true);
            BuildUnit();
            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
            _droneManager.DidNotReceive().ToggleDroneConsumerFocus(_factoryDroneConsumer);

            // Cruiser started construction ignored
            _strategy.EvaluateWhenBuildingStarted.Returns(true);
            BuildUnit();
            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);

            // Cruiser completed construction ignored
            BuildFactory();
            BuildUnit();
            _focusHelper.DidNotReceive().FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
        }

        private void BuildFactory()
        {
            _aiBuildingMonitor.EmitBuildingCompleted(_factory);
        }

        private void BuildUnit()
        {
            _factory.StartBuildingUnit(unitToStart: null);
        }
    }
}
