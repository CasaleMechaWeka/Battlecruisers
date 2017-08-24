using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
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

        public DroneConsumerFocusManager(ICruiserController aiCruiser, IDroneManager droneManager)
        {
            Helper.AssertIsNotNull(aiCruiser, droneManager);

            _aiCruiser = aiCruiser;
            _droneManager = droneManager;

            _completedFactories = new List<IFactory>();
            _inProgressBuildings = new List<IBuildable>();

            _aiCruiser.StartedConstruction += _aiCruiser_StartedConstruction;
        }

        private void _aiCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            e.Buildable.CompletedBuildable += Buildable_CompletedBuildable;
            e.Buildable.Destroyed += Buildable_Destroyed;

            _inProgressBuildings.Add(e.Buildable);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuildable completedBuildable = sender as IBuildable;
            Assert.IsNotNull(completedBuildable);

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
            // FELIX  Create generic helper method :P
            IFactory factory = sender as IFactory;
            Assert.IsNotNull(factory);

            if (factory.DroneConsumer.State != DroneConsumerState.Idle)
            {
                IBuildable affordableBuilding = GetAffordableInProgressBuilding();

                if (affordableBuilding != null)
                {
                    IDroneConsumer affordableDroneConsumer = affordableBuilding.DroneConsumer;

                    Assert.IsTrue(
                        affordableDroneConsumer.State != DroneConsumerState.Focused,
                        "Only a single drone consumer should be focused at a time, and code should only be reached if a factory is focused.  Hnece this in progress building should not be focused as well.");

                    if (affordableDroneConsumer.State == DroneConsumerState.Idle)
                    {
                        // Try to upgrade to active
                        _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
                    }

                    if (affordableDroneConsumer.State == DroneConsumerState.Active)
					{
						// Try to upgrade to focused
						_droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
					}
                }
            }
        }

        private IBuildable GetAffordableInProgressBuilding()
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
            IFactory destroyedFactory = e.DestroyedTarget as IFactory;
            Assert.IsNotNull(destroyedFactory);

            RemoveFactory(destroyedFactory);
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuildable destroyedBuildable = e.DestroyedTarget as IBuildable;
            Assert.IsNotNull(destroyedBuildable);

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
