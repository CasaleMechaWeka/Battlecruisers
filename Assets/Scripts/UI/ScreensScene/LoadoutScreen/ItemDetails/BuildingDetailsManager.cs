﻿using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildingDetailsManager : ItemDetailsManager<Building>
	{
		public ComparableBuildingDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		public void Initialise()
		{
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			
			singleItemDetails.Initialise(spriteFetcher);
			leftComparableItemDetails.Initialise(spriteFetcher);
			rightComparableItemDetails.Initialise(spriteFetcher);

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
