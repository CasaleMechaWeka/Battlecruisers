using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
        private RectTransform _rectTransform;
		private float _maxHeight;
        private DeleteButtonController _deleteButton;
        private BuildableBottomBarController _bottomBar;

        public IButton DroneFocusButton { get { return _bottomBar.ToggleDronesButton; } }

        public void Initialise(
            IDroneFocuser droneFocuser, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IFilter<ITarget> chooseTargetButtonVisibilityFilter,
            IFilter<ITarget> deleteButtonVisibilityFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter, deleteButtonVisibilityFilter);

            _rectTransform = transform.Parse<RectTransform>();
            _maxHeight = _rectTransform.sizeDelta.y;

            _deleteButton = GetComponentInChildren<DeleteButtonController>(includeInactive: true);
            Assert.IsNotNull(_deleteButton);
            _deleteButton.Initialise(this, deleteButtonVisibilityFilter);

            _bottomBar = GetComponentInChildren<BuildableBottomBarController>(includeInactive: true);
            Assert.IsNotNull(_bottomBar);
            _bottomBar.Initialise(droneFocuser, repairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter);
        }

        public virtual void ShowBuildableDetails(TItem buildable)
        {
            base.ShowItemDetails(buildable);

            _bottomBar.Buildable = buildable;
            _deleteButton.Buildable = buildable;

            // Shrink details panel if bottom bar is invisble
            float desiredHeight = _bottomBar.IsVisible ? _maxHeight : _maxHeight - _bottomBar.Height;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, desiredHeight);
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
                _bottomBar.Buildable = null;
                _deleteButton.Buildable = null;
			}
		}
	}
}
