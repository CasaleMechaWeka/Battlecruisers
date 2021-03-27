using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Remove?
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
        public void Initialise(
            IUIManager uiManager,
            IDroneFocuser droneFocuser,
            IRepairManager repairManager,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, commonStrings);
        }

        public virtual void ShowBuildableDetails(TItem buildable)
        {
            base.ShowItemDetails(buildable);
        }
	}
}
