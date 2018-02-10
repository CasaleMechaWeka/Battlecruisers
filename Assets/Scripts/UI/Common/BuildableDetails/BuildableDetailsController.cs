using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BuildableDetailsController<TItem> : ItemDetails<TItem>, IBuildableDetails<TItem> where TItem : class, IBuildable
    {
        private IDroneManager _droneManager;
        private RepairButtonController _repairButton;
        private ToggleDroneButtonController _toggleDronesButton;
        private DeleteButtonController _deleteButton;
        private BuildableProgressBarController _buildProgressController;

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneManager, repairManager);

            _droneManager = droneManager;

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(_droneManager, repairManager);

            _toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            Assert.IsNotNull(_toggleDronesButton);
            _toggleDronesButton.Initialise();

            _deleteButton = GetComponentInChildren<DeleteButtonController>(includeInactive: true);
            Assert.IsNotNull(_deleteButton);
            _deleteButton.Initialise(this);

            _buildProgressController = GetComponentInChildren<BuildableProgressBarController>(includeInactive: true);
            Assert.IsNotNull(_buildProgressController);
        }

        public virtual void ShowBuildableDetails(TItem buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            _buildProgressController.Buildable = buildable;
            _toggleDronesButton.Buildable = buildable;
            _repairButton.Repairable = buildable;

            _deleteButton.gameObject.SetActive(allowDelete);
            _deleteButton.Buildable = buildable;
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
                _buildProgressController.Buildable = null;
                _toggleDronesButton.Buildable = null;
                _repairButton.Repairable = null;
                _deleteButton.Buildable = null;
			}
		}
	}
}
