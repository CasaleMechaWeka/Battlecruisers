using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Damage
{
    public class CruiserDamageMonitor : ICruiserDamageMonitor, IManagedDisposable
    {
        private readonly ICruiser _cruiser;

        public event EventHandler CruiserOrBuildingDamaged;

        public CruiserDamageMonitor(ICruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.Damaged += OnCruiserOrBuildingDamaged;
            _cruiser.BuildingMonitor.BuildingStarted += BuildingMonitor_BuildingStarted;
            _cruiser.BuildingMonitor.BuildingDestroyed += BuildingMonitor_BuildingDestroyed;
        }

        private void BuildingMonitor_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            e.StartedBuilding.Damaged += OnCruiserOrBuildingDamaged;
        }

        private void BuildingMonitor_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
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