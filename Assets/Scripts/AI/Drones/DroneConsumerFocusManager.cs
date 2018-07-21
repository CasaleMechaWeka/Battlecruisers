using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    /// <summary>
    /// FELIX  Update tests :)
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
        private readonly ICruiserController _aiCruiser;
        private readonly IDroneManager _droneManager;
        private readonly IFactoriesMonitor _factoriesMonitor;
        private readonly IList<IFactory> _completedFactories;
        private readonly IBuildingMonitor _buildingMonitor;

        public DroneConsumerFocusManager(
            IDroneFocusingStrategy strategy, 
            ICruiserController aiCruiser, 
            IFactoriesMonitor factoriesMonitor,
            IBuildingMonitor buildingMonitor)
        {
            Helper.AssertIsNotNull(strategy, aiCruiser, aiCruiser.DroneManager, factoriesMonitor, buildingMonitor);

            _strategy = strategy;
            _aiCruiser = aiCruiser;
            _droneManager = _aiCruiser.DroneManager;
            _factoriesMonitor = factoriesMonitor;
            _buildingMonitor = buildingMonitor;

            _completedFactories = new List<IFactory>();

            _aiCruiser.StartedConstruction += _aiCruiser_StartedConstruction;
            _aiCruiser.BuildingCompleted += _aiCruiser_BuildingCompleted;
        }

        private void _aiCruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                factory.StartedBuildingUnit += Factory_StartedBuildingFirstUnit;

                Assert.IsFalse(_completedFactories.Contains(factory));
                _completedFactories.Add(factory);

                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void _aiCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            if (_strategy.EvaluateWhenBuildingStarted)
            {
                FocusOnNonFactoryDroneConsumer();
			}
        }

        /// <summary>
        /// The first time a factory starts building a unit, toggle its drone focus
        /// so it becomes the highest priority drone consumer.  This means it has 
        /// higher priority than previously built factories.
        /// </summary>
        private void Factory_StartedBuildingFirstUnit(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = sender.Parse<IFactory>();
            factory.StartedBuildingUnit -= Factory_StartedBuildingFirstUnit;

            Assert.IsNotNull(factory.DroneConsumer);
            _droneManager.ToggleDroneConsumerFocus(factory.DroneConsumer);
        }

        private void Factory_StartedBuildingUnit(object sender, StartedConstructionEventArgs e)
        {
			Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "Factory_StartedBuildingUnit()");

            if (_strategy.EvaluateWhenUnitStarted)
            {
                FocusOnNonFactoryDroneConsumer();
			}
        }

        private void FocusOnNonFactoryDroneConsumer()
        {
			Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()");

            if (!_factoriesMonitor.AreAnyFactoriesWronglyUsingDrones)
            {
                // No factories wrongly using drones, no need to reassign drones
                return;
            }

            IBuildable affordableBuilding = _buildingMonitor.GetNonFocusedAffordableBuilding();
            if (affordableBuilding == null)
            {
                // No affordable buildings, so no buildings to assign wrongly used drones to
                return;
            }

			Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()  Going to focus on: " + affordableBuilding);
            IDroneConsumer affordableDroneConsumer = affordableBuilding.DroneConsumer;

            // Try to upgrade: Idle => Active
            if (affordableDroneConsumer.State == DroneConsumerState.Idle)
            {
                _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
			}

			// Try to upgrade: Active => Focused
            if (affordableDroneConsumer.State == DroneConsumerState.Active
                && _strategy.ForceInProgressBuildingToFocused)
			{
				_droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
			}
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory destroyedFactory = e.DestroyedTarget.Parse<IFactory>();

            Assert.IsTrue(_completedFactories.Contains(destroyedFactory));
            _completedFactories.Remove(destroyedFactory);

            UnsubscribeFromFactoryEvents(destroyedFactory);
        }

        // FELIX  Add test for dispose :)
        public void DisposeManagedState()
        {
            _aiCruiser.StartedConstruction -= _aiCruiser_StartedConstruction;
            _aiCruiser.BuildingCompleted -= _aiCruiser_BuildingCompleted;

            foreach (IFactory factory in _completedFactories)
            {
                UnsubscribeFromFactoryEvents(factory);
            }
            _completedFactories.Clear();
        }

        private void UnsubscribeFromFactoryEvents(IFactory factory)
        {
            factory.StartedBuildingUnit -= Factory_StartedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }
    }
}
