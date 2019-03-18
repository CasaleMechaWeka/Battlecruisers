using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class InProgressBuildingMonitor : IInProgressBuildingMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;
        private readonly IList<IBuilding> _inProgressBuildings;

        public ReadOnlyCollection<IBuilding> InProgressBuildings { get; }

        public InProgressBuildingMonitor(ICruiserController cruiser)
        {
            Helper.AssertIsNotNull(cruiser, cruiser.DroneManager);

            _cruiser = cruiser;
            _inProgressBuildings = new List<IBuilding>();
            InProgressBuildings = new ReadOnlyCollection<IBuilding>(_inProgressBuildings);

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
        }

        private void _cruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            e.Buildable.CompletedBuildable += Buildable_CompletedBuildable;
            e.Buildable.Destroyed += Buildable_Destroyed;

            _inProgressBuildings.Add(e.Buildable);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            RemoveInProgressBuilding(completedBuilding);
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuilding destroyedBuildable = e.DestroyedTarget.Parse<IBuilding>();
            RemoveInProgressBuilding(destroyedBuildable);
        }

        private void RemoveInProgressBuilding(IBuilding building)
        {
            Assert.IsTrue(_inProgressBuildings.Contains(building));
            _inProgressBuildings.Remove(building);

            UnsubsribeFromBuildingEvents(building);
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;

            foreach (IBuilding building in _inProgressBuildings)
            {
                UnsubsribeFromBuildingEvents(building);
            }
            _inProgressBuildings.Clear();
        }

        private void UnsubsribeFromBuildingEvents(IBuilding building)
        {
            building.CompletedBuildable -= Buildable_CompletedBuildable;
            building.Destroyed -= Buildable_Destroyed;
        }
    }
}
