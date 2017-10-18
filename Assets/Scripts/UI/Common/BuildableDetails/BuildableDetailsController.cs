using System;
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
		
		public BuildableStatsController buildableStatsController;
		public Button deleteButton;
		public Button toggleDroneButton;
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
        }

        // FELIX  Created button controllers for all 3 buttons?
        public void ShowBuildableDetails(IBuildable buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            _allowDelete = allowDelete;
            buildProgressController.Initialise(_item);

            // FELIX  Handle in delete button controller?  Implement cancelling delete?
            // Hmm, delete should dismiss details.  Pressing building should cancel delete.
            // Delete buildable button
            deleteButton.gameObject.SetActive(allowDelete);
            if (allowDelete)
            {
                deleteButton.onClick.AddListener(DeleteBuildable);
            }

            // Toggle drone button (should only be visible for player buildings)
            bool showDroneRelatedUI = buildable.DroneConsumer != null && buildable.Faction == Faction.Blues;
            toggleDroneButton.gameObject.SetActive(showDroneRelatedUI);
            if (showDroneRelatedUI)
            {
                toggleDroneButton.onClick.AddListener(ToggleBuildableDrones);
                _item.CompletedBuildable += Buildable_CompletedBuildable;
            }

            _repairButton.Repairable = buildable;
        }

        public void DeleteBuildable()
        {
            Assert.IsTrue(_allowDelete);
            Assert.IsNotNull(_item);

            _item.InitiateDelete();
            Hide();
        }

        public void ToggleBuildableDrones()
        {
            _droneManager.ToggleDroneConsumerFocus(_item.DroneConsumer);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            _item.CompletedBuildable -= Buildable_CompletedBuildable;
            toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);
            toggleDroneButton.gameObject.SetActive(false);
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
				deleteButton.onClick.RemoveListener(DeleteBuildable);
				toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);

                _repairButton.Repairable = null;

				buildProgressController.Cleanup();
			}
		}
	}
}
