using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Test, use :)
    public class CruiserBuildingMonitor : ICruiserBuildingMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;

        private readonly HashSet<IBuilding> _aliveBuildings;
        public IReadOnlyCollection<IBuilding> AliveBuildings => _aliveBuildings;

        public event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
        public event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        public CruiserBuildingMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;

            _aliveBuildings = new HashSet<IBuilding>();
        }

        private void _cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            e.Buildable.CompletedBuildable += Buildable_CompletedBuildable;
            e.Buildable.Destroyed += Buildable_Destroyed;

            BuildingStarted?.Invoke(this, new StartedBuildingConstructionEventArgs(e.Buildable));
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            completedBuilding.CompletedBuildable -= Buildable_CompletedBuildable;

            Assert.IsFalse(_aliveBuildings.Contains(completedBuilding));
            _aliveBuildings.Add(completedBuilding);

            BuildingCompleted?.Invoke(this, new CompletedBuildingConstructionEventArgs(completedBuilding));
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuilding destroyedBuilding = e.DestroyedTarget.Parse<IBuilding>();
            destroyedBuilding.Destroyed -= Buildable_Destroyed;

            if (_aliveBuildings.Contains(destroyedBuilding))
            {
                _aliveBuildings.Remove(destroyedBuilding);
            }

            BuildingDestroyed?.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}