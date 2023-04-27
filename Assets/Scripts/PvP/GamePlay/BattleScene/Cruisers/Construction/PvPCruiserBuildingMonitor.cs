using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPCruiserBuildingMonitor : IPvPCruiserBuildingMonitor, IPvPManagedDisposable
    {
        private readonly IPvPCruiserController _cruiser;

        private readonly HashSet<IPvPBuilding> _aliveBuildings;
        public IReadOnlyCollection<IPvPBuilding> AliveBuildings => _aliveBuildings;

        public event EventHandler<PvPBuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<PvPBuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<PvPBuildingDestroyedEventArgs> BuildingDestroyed;

        public PvPCruiserBuildingMonitor(IPvPCruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;

            _aliveBuildings = new HashSet<IPvPBuilding>();
        }

        private void _cruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            Assert.IsFalse(_aliveBuildings.Contains(e.StartedBuilding));
            _aliveBuildings.Add(e.StartedBuilding);

            e.StartedBuilding.CompletedBuildable += Buildable_CompletedBuildable;
            e.StartedBuilding.Destroyed += Buildable_Destroyed;

            BuildingStarted?.Invoke(this, new PvPBuildingStartedEventArgs(e.StartedBuilding));
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IPvPBuilding completedBuilding = sender.Parse<IPvPBuilding>();
            completedBuilding.CompletedBuildable -= Buildable_CompletedBuildable;

            BuildingCompleted?.Invoke(this, new PvPBuildingCompletedEventArgs(completedBuilding));
        }

        private void Buildable_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPBuilding destroyedBuilding = e.DestroyedTarget.Parse<IPvPBuilding>();
            destroyedBuilding.Destroyed -= Buildable_Destroyed;
            destroyedBuilding.CompletedBuildable -= Buildable_CompletedBuildable;

            Assert.IsTrue(_aliveBuildings.Contains(destroyedBuilding));
            _aliveBuildings.Remove(destroyedBuilding);

            BuildingDestroyed?.Invoke(this, new PvPBuildingDestroyedEventArgs(destroyedBuilding));
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}