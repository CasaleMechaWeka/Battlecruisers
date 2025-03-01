using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public class PvPCruiserDamageMonitor : ICruiserDamageMonitor, IManagedDisposable
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

        private void OnCruiserOrBuildingDamaged(object sender, DamagedEventArgs e)
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