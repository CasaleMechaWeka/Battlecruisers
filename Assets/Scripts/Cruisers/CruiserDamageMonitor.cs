using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Plays sounds to the user when:
    /// 1. The crusier (or its buildings) are damaged (have a cooldown so this doesn't get annoying)
    /// 2. The cruiser reaches critical health (say a third)
    /// </summary>
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
            if (CruiserOrBuildingDamaged != null)
            {
                CruiserOrBuildingDamaged.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisposeManagedState()
        {
            _cruiser.Damaged -= OnCruiserOrBuildingDamaged;
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
            _cruiser.BuildingDestroyed -= _cruiser_BuildingDestroyed;
        }
    }
}