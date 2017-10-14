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
        private IRepairManager _repairManager;
		private bool _allowDelete;

        protected override StatsController<IBuildable> StatsController { get { return buildableStatsController; } }
		
        public BuildableStatsController buildableStatsController;
		public Button deleteButton;
		public Button toggleDroneButton;
        public Button repairButton;
		public BuildableProgressBarController buildProgressController;

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

            // FELIX  Extract bool expression to method?
            // Toggle drone button (should only be visible for player buildings)
			bool showDroneRelatedUI = buildable.DroneConsumer != null && buildable.Faction == Faction.Blues;
			toggleDroneButton.gameObject.SetActive(showDroneRelatedUI);
			if (showDroneRelatedUI)
			{
				toggleDroneButton.onClick.AddListener(ToggleBuildableDrones);
				_item.CompletedBuildable += Buildable_CompletedBuildable;
			}

            // FELIX  Extract bool expression to method?
            // FELIX  Handle can execute change for repair command :D
            // Toggle repair drone button (should only be visible for player repairables)
            bool showRepairButton = buildable.Faction == Faction.Blues && buildable.RepairCommand.CanExecute;
            repairButton.gameObject.SetActive(showRepairButton);
            if (showRepairButton)
            {
                repairButton.onClick.AddListener(ToggleRepairButton);
            }
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

        public void ToggleRepairButton()
        {
            IDroneConsumer repairDroneConsumer = _repairManager.GetDroneConsumer(_item);
            Assert.IsNotNull(repairDroneConsumer);

            _droneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
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
                repairButton.onClick.RemoveListener(ToggleRepairButton);
				buildProgressController.Cleanup();
			}
		}
	}
}
