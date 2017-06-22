using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
	public class CruiserDetailsManager : ItemDetailsManager<Cruiser>
	{
		public CruiserDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		void Start()
		{
			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
