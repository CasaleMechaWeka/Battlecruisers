using BattleCruisers.Buildables;
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
	public class BuildableDetailsManager : ItemDetailsManager<Buildable>
	{
		public ComparableBuildableDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		void Start()
		{
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			
			singleItemDetails.Initialise(spriteFetcher);
			leftComparableItemDetails.Initialise(spriteFetcher);
			rightComparableItemDetails.Initialise(spriteFetcher);

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
