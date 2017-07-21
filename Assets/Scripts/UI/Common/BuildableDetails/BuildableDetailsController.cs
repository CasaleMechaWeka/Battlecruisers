using System;
using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildableDetailsController : BaseBuildableDetails<Buildable>
	{
		private IDroneManager _droneManager;
		private bool _allowDelete;

        protected override StatsController<Buildable> StatsController { get { return buildableStatsController; } }
		
        public BuildableStatsController buildableStatsController;
		public Button deleteButton;
		public Button toggleDroneButton;
		public BuildableProgressBarController buildProgressController;

		public void Initialise(IDroneManager droneManager, ISpriteFetcher spriteFetcher)
		{
			base.Initialise(spriteFetcher);
			_droneManager = droneManager;
		}

		public void ShowBuildableDetails(Buildable buildable, bool allowDelete)
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

			// Toggle drone button
			bool showDroneRelatedUI = buildable.DroneConsumer != null && buildable.Faction == Faction.Blues;
			toggleDroneButton.gameObject.SetActive(showDroneRelatedUI);
			if (showDroneRelatedUI)
			{
				toggleDroneButton.onClick.AddListener(ToggleBuildableDrones);
				_item.CompletedBuildable += Buildable_CompletedBuildable;
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
				buildProgressController.Cleanup();
			}
		}
	}
}
