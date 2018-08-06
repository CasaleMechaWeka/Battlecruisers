using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class CruiserDetailsController : ItemDetails<ICruiser>, ICruiserDetails
    {
        private RepairButtonController _repairButton;
        private ChooseTargetButtonController _chooseTargetButton;

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager, IUserChosenTargetHelper userChosenTargetHelper)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneManager, repairManager, userChosenTargetHelper);

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneManager, repairManager);

            _chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            Assert.IsNotNull(_chooseTargetButton);
            _chooseTargetButton.Initialise(userChosenTargetHelper);
        }

        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponentInChildren<CruiserStatsController>();
        }

        public void ShowCruiserDetails(ICruiser cruiser)
        {
            base.ShowItemDetails(cruiser);

            _repairButton.Repairable = cruiser;
            _chooseTargetButton.Target = cruiser;
        }
    }
}
