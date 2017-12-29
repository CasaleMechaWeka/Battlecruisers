﻿using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildingDetailsManager : ItemDetailsManager<IBuilding>
	{
        // FELIX  Initialise programmatically?
		public ComparableBuildingDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		public void Initialise()
		{
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
			
			singleItemDetails.Initialise(spriteProvider);
			leftComparableItemDetails.Initialise(spriteProvider);
			rightComparableItemDetails.Initialise(spriteProvider);

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
