using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public class PvPCruiserDamageMonitor : IPvPCruiserDamageMonitor, IPvPManagedDisposable
    {
        private readonly IPvPCruiser _cruiser;

        public event EventHandler CruiserOrBuildingDamaged;

        public PvPCruiserDamageMonitor(IPvPCruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.Damaged += OnCruiserOrBuildingDamaged;
            _cruiser.BuildingMonitor.BuildingStarted += BuildingMonitor_BuildingStarted;
            _cruiser.BuildingMonitor.BuildingDestroyed += BuildingMonitor_BuildingDestroyed;
        }

        private void BuildingMonitor_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            e.StartedBuilding.Damaged += OnCruiserOrBuildingDamaged;
        }

        private void BuildingMonitor_BuildingDestroyed(object sender, PvPBuildingDestroyedEventArgs e)
        {
            e.DestroyedBuilding.Damaged -= OnCruiserOrBuildingDamaged;
        }

        private void OnCruiserOrBuildingDamaged(object sender, PvPDamagedEventArgs e)
        {
            CruiserOrBuildingDamaged?.Invoke(this, EventArgs.Empty);
        }

        public void DisposeManagedState()
        {
            _cruiser.Damaged -= OnCruiserOrBuildingDamaged;
            _cruiser.BuildingMonitor.BuildingStarted -= BuildingMonitor_BuildingStarted;
            _cruiser.BuildingMonitor.BuildingDestroyed -= BuildingMonitor_BuildingDestroyed;
        }
    }
}