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
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
        private DeleteButtonController _deleteButton;
        private InformatorWidgetManager _buttonManager;

        public IButton DroneFocusButton => _buttonManager.ToggleDronesButton;

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

            _deleteButton = GetComponentInChildren<DeleteButtonController>(includeInactive: true);
            Assert.IsNotNull(_deleteButton);
            _deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, this);

            _buttonManager = GetComponentInChildren<InformatorWidgetManager>(includeInactive: true);
            Assert.IsNotNull(_buttonManager);
            // FELIX :P
            //_buttonManager.Initialise(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, commonStrings);
        }

        public virtual void ShowBuildableDetails(TItem buildable)
        {
            base.ShowItemDetails(buildable);

            _buttonManager.SelectedItem = buildable;
            _deleteButton.Buildable = buildable;
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
                _buttonManager.SelectedItem = null;
                _deleteButton.Buildable = null;
			}
		}
	}
}
