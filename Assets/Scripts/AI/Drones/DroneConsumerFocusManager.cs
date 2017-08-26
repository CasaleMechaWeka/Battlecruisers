using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
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
    public class DroneConsumerFocusManager : IDisposable
    {
        private readonly ICruiserController _aiCruiser;
        private readonly IDroneManager _droneManager;
        private readonly IList<IFactory> _completedFactories;
        private readonly IList<IBuildable> _inProgressBuildings;

        public DroneConsumerFocusManager(ICruiserController aiCruiser)
        {
            Helper.AssertIsNotNull(aiCruiser, aiCruiser.DroneManager);

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

            FocusOnNonFactoryDroneConsumer();
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
			
            FocusOnNonFactoryDroneConsumer();
        }

        private void FocusOnNonFactoryDroneConsumer()
        {
			Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()");
			
            if (_completedFactories.Any(factory => factory.DroneConsumer.State != DroneConsumerState.Idle))
            {
                IBuildable idleAffordableBuilding = GetIdleAffordableBuilding();

                if (idleAffordableBuilding != null)
                {
					Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()  Going to focus on: " + idleAffordableBuilding);
     
                    IDroneConsumer idleAffordableDroneConsumer = idleAffordableBuilding.DroneConsumer;

                    // Try to upgrade to Active, but NOT Focused, so that any left over
                    // drones can be used by completed factories.
					_droneManager.ToggleDroneConsumerFocus(idleAffordableDroneConsumer);
                }
            }
        }

        private IBuildable GetIdleAffordableBuilding()
        {
            IBuildable affordableBuilding = null;

            if (_inProgressBuildings.Count != 0
                && _inProgressBuildings.All(building => building.DroneConsumer.State == DroneConsumerState.Idle))
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

        public void Dispose()
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
