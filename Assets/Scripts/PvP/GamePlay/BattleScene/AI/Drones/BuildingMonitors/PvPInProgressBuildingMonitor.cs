using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public class PvPInProgressBuildingMonitor : IPvPInProgressBuildingMonitor, IPvPManagedDisposable
    {
        private readonly IPvPCruiserController _cruiser;
        private readonly IList<IPvPBuilding> _inProgressBuildings;

        public ReadOnlyCollection<IPvPBuilding> InProgressBuildings { get; }

        public PvPInProgressBuildingMonitor(IPvPCruiserController cruiser)
        {
            PvPHelper.AssertIsNotNull(cruiser, cruiser.DroneManager);

            _cruiser = cruiser;
            _inProgressBuildings = new List<IPvPBuilding>();
            InProgressBuildings = new ReadOnlyCollection<IPvPBuilding>(_inProgressBuildings);

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
        }

        private void _cruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            e.StartedBuilding.CompletedBuildable += Buildable_CompletedBuildable;
            e.StartedBuilding.Destroyed += Buildable_Destroyed;

            _inProgressBuildings.Add(e.StartedBuilding);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            IPvPBuilding completedBuilding = sender.Parse<IPvPBuilding>();
            RemoveInProgressBuilding(completedBuilding);
        }

        private void Buildable_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPBuilding destroyedBuildable = e.DestroyedTarget.Parse<IPvPBuilding>();
            RemoveInProgressBuilding(destroyedBuildable);
        }

        private void RemoveInProgressBuilding(IPvPBuilding building)
        {
            Assert.IsTrue(_inProgressBuildings.Contains(building));
            _inProgressBuildings.Remove(building);

            UnsubsribeFromBuildingEvents(building);
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;

            foreach (IPvPBuilding building in _inProgressBuildings)
            {
                UnsubsribeFromBuildingEvents(building);
            }
            _inProgressBuildings.Clear();
        }

        private void UnsubsribeFromBuildingEvents(IPvPBuilding building)
        {
            building.CompletedBuildable -= Buildable_CompletedBuildable;
            building.Destroyed -= Buildable_Destroyed;
        }
    }
}
