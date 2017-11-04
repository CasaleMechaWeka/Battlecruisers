using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class InBattleCruiserDetailsController : CruiserDetailsController, IInBattleCruiserDetails
    {
        private RepairButtonController _repairButton;

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise();

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneManager, repairManager);
        }

        public void ShowCruiserDetails(ICruiser cruiser)
        {
            base.ShowItemDetails(cruiser);
            _repairButton.Repairable = cruiser;
        }
    }
}
