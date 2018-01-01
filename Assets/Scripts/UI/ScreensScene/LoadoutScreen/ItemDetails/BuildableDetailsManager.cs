using BattleCruisers.Buildables;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildableDetailsManager<TItem> : ItemDetailsManager<TItem> where TItem : class, IBuildable
	{
        // FELIX  Initialise programmatically?
        public BaseBuildableDetails<TItem> singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

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
