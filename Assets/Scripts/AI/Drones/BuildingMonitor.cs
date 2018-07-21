using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    // FELIX  Rename to InProgressBuildingMonitor (also interface) :)
    // FELIX   Test
    public class BuildingMonitor : IBuildingMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;
        private readonly IList<IBuildable> _inProgressBuildings;

        public BuildingMonitor(ICruiserController cruiser)
        {
            Helper.AssertIsNotNull(cruiser, cruiser.DroneManager);

            _cruiser = cruiser;
            _inProgressBuildings = new List<IBuildable>();

            _cruiser.StartedConstruction += _cruiser_StartedConstruction;
        }

        private void _cruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            e.Buildable.CompletedBuildable += Buildable_CompletedBuildable;
            e.Buildable.Destroyed += Buildable_Destroyed;

            _inProgressBuildings.Add(e.Buildable);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuildable completedBuildable = sender.Parse<IBuildable>();
            RemoveInProgressBuilding(completedBuildable);
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuildable destroyedBuildable = e.DestroyedTarget.Parse<IBuildable>();
            RemoveInProgressBuilding(destroyedBuildable);
        }

        private void RemoveInProgressBuilding(IBuildable buildable)
        {
            Assert.IsTrue(_inProgressBuildings.Contains(buildable));
            _inProgressBuildings.Remove(buildable);

            UnsubsribeFromBuildingEvents(buildable);
        }

        public IBuildable GetNonFocusedAffordableBuilding()
        {
            IBuildable affordableBuilding = null;

            if (_inProgressBuildings.Count != 0
                && _inProgressBuildings.All(building => building.DroneConsumer.State != DroneConsumerState.Focused))
            {
                affordableBuilding
                    = _inProgressBuildings
                        .FirstOrDefault(building => building.DroneConsumer.NumOfDronesRequired <= _cruiser.DroneManager.NumOfDrones);
            }

            return affordableBuilding;
        }

        public void DisposeManagedState()
        {
            _cruiser.StartedConstruction -= _cruiser_StartedConstruction;

            foreach (IBuildable building in _inProgressBuildings)
            {
                UnsubsribeFromBuildingEvents(building);
            }
            _inProgressBuildings.Clear();
        }

        private void UnsubsribeFromBuildingEvents(IBuildable buildable)
        {
            buildable.CompletedBuildable -= Buildable_CompletedBuildable;
            buildable.Destroyed -= Buildable_Destroyed;
        }
    }
}
