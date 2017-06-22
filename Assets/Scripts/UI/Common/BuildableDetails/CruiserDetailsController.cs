using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	// FELIX  Avoid duplicate code with BaseBuildableDetails, create ItemDetails class?
	public abstract class CruiserDetailsController : MonoBehaviour, IComparableItemDetails<Cruiser>
	{
		public CruiserStatsController statsController;
		public Text cruiserName;
		public Text cruiserDescription;
		public Image cruiserImage;

		void Start()
		{
			Hide();
		}

		public void ShowItemDetails(Cruiser cruiser, Cruiser cruiserToCompareTo = null)
		{
			Assert.IsNotNull(cruiser);

			statsController.ShowStats(cruiser, cruiserToCompareTo);
			cruiserName.text = cruiser.name;
			cruiserDescription.text = cruiser.description;
			cruiserImage.sprite = cruiser.Sprite;

			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
