using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildableDetailsController : BaseBuildableDetails<IBuildable>
    {
        private IDroneManager _droneManager;
        private bool _allowDelete;
        private RepairButtonController _repairButton;
        private ToggleDroneButtonController _toggleDronesButton;
		
		public BuildableStatsController buildableStatsController;
		public Button deleteButton;
		public BuildableProgressBarController buildProgressController;

        protected override StatsController<IBuildable> StatsController { get { return buildableStatsController; } }
		
        public void Initialise(ISpriteFetcher spriteFetcher, IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise(spriteFetcher);

            Helper.AssertIsNotNull(droneManager, repairManager);

            _droneManager = droneManager;

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(_droneManager, repairManager);

            _toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            Assert.IsNotNull(_toggleDronesButton);
            _toggleDronesButton.Initialise();
        }

        // FELIX  Created button controllers for all 3 buttons?
        public void ShowBuildableDetails(IBuildable buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            _allowDelete = allowDelete;

            // FELIX  Handle in delete button controller?  Implement cancelling delete?
            // Hmm, delete should dismiss details.  Pressing building should cancel delete.
            // Delete buildable button
            deleteButton.gameObject.SetActive(allowDelete);
            if (allowDelete)
            {
                deleteButton.onClick.AddListener(DeleteBuildable);
            }

            buildProgressController.Buildable = buildable;
            _toggleDronesButton.Buildable = buildable;
            _repairButton.Repairable = buildable;
        }

        public void DeleteBuildable()
        {
            Assert.IsTrue(_allowDelete);
            Assert.IsNotNull(_item);

            _item.InitiateDelete();
            Hide();
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
				deleteButton.onClick.RemoveListener(DeleteBuildable);

                buildProgressController.Buildable = null;
                _toggleDronesButton.Buildable = null;
                _repairButton.Repairable = null;
			}
		}
	}
}
