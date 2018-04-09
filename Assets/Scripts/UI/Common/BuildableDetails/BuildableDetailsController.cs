using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
		private DeleteButtonController _deleteButton;
        private BuildableBottomBarController _bottomBar;

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneManager, repairManager);

            _deleteButton = GetComponentInChildren<DeleteButtonController>(includeInactive: true);
            Assert.IsNotNull(_deleteButton);
            _deleteButton.Initialise(this);

            _bottomBar = GetComponentInChildren<BuildableBottomBarController>(includeInactive: true);
            Assert.IsNotNull(_bottomBar);
            _bottomBar.Initialise(droneManager, repairManager);
        }

        public virtual void ShowBuildableDetails(TItem buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            _bottomBar.Buildable = buildable;

            // FELIX  Update height depending on whether bottom bar is visible!

            _deleteButton.gameObject.SetActive(allowDelete);
            _deleteButton.Buildable = buildable;
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
