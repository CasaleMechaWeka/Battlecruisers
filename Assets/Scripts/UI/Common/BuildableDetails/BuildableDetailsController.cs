using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class BuildableDetailsController : BaseBuildableDetails<Buildable>
	{
		private IDroneManager _droneManager;
		private bool _allowDelete;
		
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
			buildProgressController.Initialise(_buildable);

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
				_buildable.CompletedBuildable += Buildable_CompletedBuildable;
			}
		}

		public void DeleteBuildable()
		{
			Assert.IsTrue(_allowDelete);
			Assert.IsNotNull(_buildable);

			_buildable.InitiateDelete();
			Hide();
		}

		public void ToggleBuildableDrones()
		{
			_droneManager.ToggleDroneConsumerFocus(_buildable.DroneConsumer);
		}
		
		private void Buildable_CompletedBuildable(object sender, EventArgs e)
		{
			_buildable.CompletedBuildable -= Buildable_CompletedBuildable;
			toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);
			toggleDroneButton.gameObject.SetActive(false);
		}

		protected override void CleanUp()
		{
			if (_buildable != null)
			{
				deleteButton.onClick.RemoveListener(DeleteBuildable);
				toggleDroneButton.onClick.RemoveListener(ToggleBuildableDrones);
				buildProgressController.Cleanup();
			}
		}
	}
}
