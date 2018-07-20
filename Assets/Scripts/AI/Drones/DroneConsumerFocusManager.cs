using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    /// <summary>
    /// Manages which drone consumer should be in focus, thus having the most
    /// drones.
    /// 
    /// Ensures the drone consumers of factories that are producing units
    /// (completed factories) are never the highest priority drone
    /// consumer, unless they are the only drone consumer.
    /// 
    /// This ensures that the AI does not pour all their drones into unit
    /// production, while they are also trying to produce additional buildings
    /// that may otherwise never complete.
    /// </summary>
    public class DroneConsumerFocusManager : IManagedDisposable
    {
        private readonly IDroneFocusingStrategy _strategy;
        private readonly ICruiserController _aiCruiser;
        private readonly IDroneManager _droneManager;
        private readonly IList<IFactory> _completedFactories;
        private readonly IList<IBuildable> _inProgressBuildings;

        public DroneConsumerFocusManager(IDroneFocusingStrategy strategy, ICruiserController aiCruiser)
        {
            Helper.AssertIsNotNull(strategy, aiCruiser, aiCruiser.DroneManager);

            _strategy = strategy;
            _aiCruiser = aiCruiser;
            _droneManager = _aiCruiser.DroneManager;

            _completedFactories = new List<IFactory>();
            _inProgressBuildings = new List<IBuildable>();

            _aiCruiser.StartedConstruction += _aiCruiser_StartedConstruction;
        }

        private void _aiCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            e.Buildable.CompletedBuildable += Buildable_CompletedBuildable;
            e.Buildable.Destroyed += Buildable_Destroyed;

            _inProgressBuildings.Add(e.Buildable);

            if (_strategy.EvaluateWhenBuildingStarted)
            {
                FocusOnNonFactoryDroneConsumer();
			}
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuildable completedBuildable = sender.Parse<IBuildable>();

            RemoveInProgressBuilding(completedBuildable);

            IFactory factory = completedBuildable as IFactory;
            if (factory != null)
            {
                Assert.IsFalse(_completedFactories.Contains(factory));
                _completedFactories.Add(factory);

                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
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
			
            // FELIX  Use returns to avoid nesting :)
            if (_completedFactories.Any(SelectFactoryUsingDrones))
            {
                IBuildable affordableBuilding = GetNonFocusedAffordableBuilding();

                if (affordableBuilding != null)
                {
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
            }
        }

        private bool SelectFactoryUsingDrones(IFactory factory)
        {
            return 
                !factory.IsDestroyed
                && factory.DroneConsumer != null
                && factory.DroneConsumer.State != DroneConsumerState.Idle;
        }

        private IBuildable GetNonFocusedAffordableBuilding()
        {
            IBuildable affordableBuilding = null;

            if (_inProgressBuildings.Count != 0
                && _inProgressBuildings.All(building => building.DroneConsumer.State != DroneConsumerState.Focused))
            {
                affordableBuilding
				    = _inProgressBuildings
						.FirstOrDefault(building => building.DroneConsumer.NumOfDronesRequired <= _droneManager.NumOfDrones);
            }

            return affordableBuilding;
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory destroyedFactory = e.DestroyedTarget.Parse<IFactory>();
            RemoveFactory(destroyedFactory);
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuildable destroyedBuildable = e.DestroyedTarget.Parse<IBuildable>();
            RemoveInProgressBuilding(destroyedBuildable);
		}

        public void DisposeManagedState()
        {
            _aiCruiser.StartedConstruction -= _aiCruiser_StartedConstruction;

            foreach (IFactory factory in _completedFactories)
            {
                RemoveFactory(factory);
            }

            foreach (IBuildable building in _inProgressBuildings)
            {
                RemoveInProgressBuilding(building);
            }
        }

        private void RemoveFactory(IFactory factory)
        {
            Assert.IsTrue(_completedFactories.Contains(factory));
            _completedFactories.Remove(factory);

            factory.StartedBuildingUnit -= Factory_StartedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }
		
        private void RemoveInProgressBuilding(IBuildable buildable)
        {
            Assert.IsTrue(_inProgressBuildings.Contains(buildable));
            _inProgressBuildings.Remove(buildable);
            
            buildable.CompletedBuildable -= Buildable_CompletedBuildable;
            buildable.Destroyed -= Buildable_Destroyed;
        }
    }
}
