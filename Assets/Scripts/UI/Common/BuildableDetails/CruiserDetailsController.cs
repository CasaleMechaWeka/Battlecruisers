using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class CruiserDetailsController : ItemDetails<ICruiser>, ICruiserDetails
    {
        private RepairButtonController _repairButton;
        private ChooseTargetButtonController _chooseTargetButton;

        public void Initialise(
            IDroneFocuser droneFocuser, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);

            // FELIX  Remove all buttons :)
            //_repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            //Assert.IsNotNull(_repairButton);
            //_repairButton.Initialise(soundPlayer, droneFocuser, repairManager);

            //_chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            //Assert.IsNotNull(_chooseTargetButton);
            //_chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter, commonStrings);
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
