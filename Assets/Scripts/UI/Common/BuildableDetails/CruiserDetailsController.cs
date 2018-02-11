using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class CruiserDetailsController : ItemDetails<ICruiser>, ICruiserDetails
    {
        private RepairButtonController _repairButton;

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise();

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneManager, repairManager);
        }

        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponentInChildren<CruiserStatsController>();
        }

        public void ShowCruiserDetails(ICruiser cruiser)
        {
            base.ShowItemDetails(cruiser);
            _repairButton.Repairable = cruiser;
        }
    }
}
