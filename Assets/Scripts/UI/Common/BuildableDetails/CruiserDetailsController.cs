using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.ProgressBars;
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
        private IHealthDial<ICruiser> _healthDial;

        public void Initialise(
            IDroneFocuser droneFocuser, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters);

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneFocuser, repairManager);

            _chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            Assert.IsNotNull(_chooseTargetButton);
            _chooseTargetButton.Initialise(userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);

            CruiserHealthDialInitialiser healthDialInitialiser = GetComponentInChildren<CruiserHealthDialInitialiser>(includeInactive: true);
            Assert.IsNotNull(healthDialInitialiser);
            _healthDial = healthDialInitialiser.Initialise(buttonVisibilityFilters.HelpLabelsVisibilityFilter);
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
            _healthDial.Damagable = cruiser;
        }
    }
}
