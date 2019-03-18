using BattleCruisers.AI.Drones.Strategies;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    /// <summary>
    /// Manages which drone consumer should be in focus, thus having the most
    /// drones.
    /// 
    /// Ensures the drone consumers of factories that are producing units
    /// (completed factories) are never the highest priority drone
    /// consumer, unless they are:
    /// 1. The only drone consumer
    /// 2. Or, producing the desired number of units (usually 3ish).  This means
    /// factories produce some units (otherwise they have no point :P) but stop 
    /// hogging drones after having produced some units (so that other buildings
    /// can be built).
    /// </summary>
    public class DroneConsumerFocusManager : IManagedDisposable
    {
        private readonly IDroneFocusingStrategy _strategy;
        private readonly ICruiserBuildingMonitor _aiBuildingMonitor;
        private readonly IDroneManager _droneManager;
        private readonly IList<IFactory> _completedFactories;
        private readonly IDroneConsumerFocusHelper _focusHelper;

        public DroneConsumerFocusManager(
            IDroneFocusingStrategy strategy, 
            ICruiserController aiCruiser, 
            IDroneConsumerFocusHelper focusHelper)
        {
            Helper.AssertIsNotNull(strategy, aiCruiser, aiCruiser.DroneManager, focusHelper);

            _strategy = strategy;
            _aiBuildingMonitor = aiCruiser.BuildingMonitor;
            _droneManager = aiCruiser.DroneManager;
            _focusHelper = focusHelper;

            _completedFactories = new List<IFactory>();

            _aiBuildingMonitor.BuildingStarted += _aiBuildingMonitor_BuildingStarted;
            _aiBuildingMonitor.BuildingCompleted += _aiBuildingMonitor_BuildingCompleted;
        }

        private void _aiBuildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            IFactory factory = e.CompletedBuilding as IFactory;

            if (factory != null)
            {
                factory.StartedBuildingUnit += Factory_StartedBuildingFirstUnit;

                Assert.IsFalse(_completedFactories.Contains(factory));
                _completedFactories.Add(factory);

                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void _aiBuildingMonitor_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            if (_strategy.EvaluateWhenBuildingStarted)
            {
                _focusHelper.FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
            }
        }

        /// <summary>
        /// The first time a factory starts building a unit, toggle its drone focus
        /// so it becomes the highest priority drone consumer.  This means it has 
        /// higher priority than previously built factories.
        /// </summary>
        private void Factory_StartedBuildingFirstUnit(object sender, UnitStartedEventArgs e)
        {
            IFactory factory = sender.Parse<IFactory>();
            factory.StartedBuildingUnit -= Factory_StartedBuildingFirstUnit;

            Assert.IsNotNull(factory.DroneConsumer);
            _droneManager.ToggleDroneConsumerFocus(factory.DroneConsumer);
        }

        private void Factory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
			Logging.LogDefault(Tags.DRONE_CONUMSER_FOCUS_MANAGER);

            if (_strategy.EvaluateWhenUnitStarted)
            {
                _focusHelper.FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
			}
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory destroyedFactory = e.DestroyedTarget.Parse<IFactory>();

            Assert.IsTrue(_completedFactories.Contains(destroyedFactory));
            _completedFactories.Remove(destroyedFactory);

            UnsubscribeFromFactoryEvents(destroyedFactory);
        }

        public void DisposeManagedState()
        {
            _aiBuildingMonitor.BuildingStarted -= _aiBuildingMonitor_BuildingStarted;
            _aiBuildingMonitor.BuildingCompleted -= _aiBuildingMonitor_BuildingCompleted;

            foreach (IFactory factory in _completedFactories)
            {
                UnsubscribeFromFactoryEvents(factory);
            }
            _completedFactories.Clear();
        }

        private void UnsubscribeFromFactoryEvents(IFactory factory)
        {
            factory.StartedBuildingUnit -= Factory_StartedBuildingUnit;
            factory.StartedBuildingUnit -= Factory_StartedBuildingFirstUnit;
            factory.Destroyed -= Factory_Destroyed;
        }
    }
}
