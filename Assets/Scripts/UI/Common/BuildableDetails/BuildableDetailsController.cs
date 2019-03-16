using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
        private RectTransform _rectTransform;
        private DeleteButtonController _deleteButton;
        private InformatorWidgetManager _buttonManager;

        public IButton DroneFocusButton => _buttonManager.ToggleDronesButton;

        public void Initialise(
            IUIManager uiManager,
            IDroneFocuser droneFocuser,
            IRepairManager repairManager,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters);

            _rectTransform = transform.Parse<RectTransform>();

            _deleteButton = GetComponentInChildren<DeleteButtonController>(includeInactive: true);
            Assert.IsNotNull(_deleteButton);
            _deleteButton.Initialise(uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            _buttonManager = GetComponentInChildren<InformatorWidgetManager>(includeInactive: true);
            Assert.IsNotNull(_buttonManager);
            _buttonManager.Initialise(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters);
        }

        public virtual void ShowBuildableDetails(TItem buildable)
        {
            base.ShowItemDetails(buildable);

            _buttonManager.Buildable = buildable;
            _deleteButton.Buildable = buildable;
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
                _buttonManager.Buildable = null;
                _deleteButton.Buildable = null;
			}
		}
	}
}
