using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildableDetailsController : BaseBuildableDetails<IBuildable>
    {
        private IDroneManager _droneManager;
        private IRepairManager _repairManager;
        private bool _allowDelete;
		
		public BuildableStatsController buildableStatsController;
		public Button deleteButton;
		public Button toggleDroneButton;
		public Button repairButton;
		public BuildableProgressBarController buildProgressController;

        protected override StatsController<IBuildable> StatsController { get { return buildableStatsController; } }

        // Only show repair button for user repairlabes, not opponent repairables
        private bool ShowRepairButton { get { return _item.Faction == Faction.Blues && _item.RepairCommand.CanExecute; } }
		
        public void Initialise(ISpriteFetcher spriteFetcher, IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise(spriteFetcher);

            Helper.AssertIsNotNull(droneManager, repairManager);

            _droneManager = droneManager;
            _repairManager = repairManager;
        }

        public void ShowBuildableDetails(IBuildable buildable, bool allowDelete)
        {
            base.ShowItemDetails(buildable);

            _allowDelete = allowDelete;
            buildProgressController.Initialise(_item);

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

            // Toggle repair drone button (should only be visible for player repairables)
            repairButton.gameObject.SetActive(ShowRepairButton);
            if (ShowRepairButton)
            {
                repairButton.onClick.AddListener(ToggleRepairButton);
            }
            buildable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
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
            Debug.Log("ToggleBuildableDrones");

            _droneManager.ToggleDroneConsumerFocus(_item.DroneConsumer);
        }

        public void ToggleRepairButton()
        {
            IDroneConsumer repairDroneConsumer = _repairManager.GetDroneConsumer(_item);
            _droneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            _item.CompletedBuildable -= Buildable_CompletedBuildable;
            toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);
            toggleDroneButton.gameObject.SetActive(false);
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            repairButton.gameObject.SetActive(ShowRepairButton);

            if (ShowRepairButton)
            {
                repairButton.onClick.AddListener(ToggleRepairButton);
            }
            else
            {
                repairButton.onClick.RemoveListener(ToggleRepairButton);
            }
        }

        protected override void CleanUp()
		{
			if (_item != null)
			{
				deleteButton.onClick.RemoveListener(DeleteBuildable);
				toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);
				
                repairButton.onClick.RemoveListener(ToggleRepairButton);
                _item.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;

				buildProgressController.Cleanup();
			}
		}
	}
}
