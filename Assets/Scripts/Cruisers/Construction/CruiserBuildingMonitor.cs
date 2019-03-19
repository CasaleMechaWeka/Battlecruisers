using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class CruiserBuildingMonitor : ICruiserBuildingMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;

        private readonly HashSet<IBuilding> _aliveBuildings;
        public IReadOnlyCollection<IBuilding> AliveBuildings => _aliveBuildings;

        public event EventHandler<BuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<BuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        public CruiserBuildingMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;

            _aliveBuildings = new HashSet<IBuilding>();
        }

        private void _cruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            Assert.IsFalse(_aliveBuildings.Contains(e.StartedBuilding));
            _aliveBuildings.Add(e.StartedBuilding);

            e.StartedBuilding.CompletedBuildable += Buildable_CompletedBuildable;
            e.StartedBuilding.Destroyed += Buildable_Destroyed;

            BuildingStarted?.Invoke(this, new BuildingStartedEventArgs(e.StartedBuilding));
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            completedBuilding.CompletedBuildable -= Buildable_CompletedBuildable;

            BuildingCompleted?.Invoke(this, new BuildingCompletedEventArgs(completedBuilding));
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuilding destroyedBuilding = e.DestroyedTarget.Parse<IBuilding>();
            destroyedBuilding.Destroyed -= Buildable_Destroyed;
            destroyedBuilding.CompletedBuildable -= Buildable_CompletedBuildable;

            Assert.IsTrue(_aliveBuildings.Contains(destroyedBuilding));
            _aliveBuildings.Remove(destroyedBuilding);

            BuildingDestroyed?.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}