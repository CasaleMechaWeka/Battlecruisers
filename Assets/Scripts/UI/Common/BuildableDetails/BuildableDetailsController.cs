using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BuildableDetailsController : ItemDetails<IBuildable>, IBuildableDetails
    {
        private IDroneManager _droneManager;
        private RepairButtonController _repairButton;
        private ToggleDroneButtonController _toggleDronesButton;
        private DeleteButtonController _deleteButton;

        // FELIX  Retrieve programmatically
		public BuildableProgressBarController buildProgressController;

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
        }

        public virtual void ShowBuildableDetails(IBuildable buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            buildProgressController.Buildable = buildable;
            _toggleDronesButton.Buildable = buildable;
            _repairButton.Repairable = buildable;

            _deleteButton.gameObject.SetActive(allowDelete);
            _deleteButton.Buildable = buildable;
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
                buildProgressController.Buildable = null;
                _toggleDronesButton.Buildable = null;
                _repairButton.Repairable = null;
                _deleteButton.Buildable = null;
			}
		}
	}
}
