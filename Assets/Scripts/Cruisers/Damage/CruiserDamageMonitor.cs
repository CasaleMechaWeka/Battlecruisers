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
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiser.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private void _cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            e.Buildable.Damaged += OnCruiserOrBuildingDamaged;
        }

        private void _cruiser_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
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
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
            _cruiser.BuildingDestroyed -= _cruiser_BuildingDestroyed;
        }
    }
}