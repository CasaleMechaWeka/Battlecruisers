using BattleCruisers.Buildables;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildableDetailsManager<TItem> : ItemDetailsManager<TItem> where TItem : class, IBuildable
	{
		public void Initialise()
		{
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());

            BaseBuildableDetails<TItem> singleItemDetails = transform.FindNamedComponent<BaseBuildableDetails<TItem>>("SingleItemDetails");
			singleItemDetails.Initialise(spriteProvider);

            BaseBuildableDetails<TItem> leftComparableItemDetails = transform.FindNamedComponent<BaseBuildableDetails<TItem>>("LeftItemDetails");
			leftComparableItemDetails.Initialise(spriteProvider);

            BaseBuildableDetails<TItem> rightComparableItemDetails = transform.FindNamedComponent<BaseBuildableDetails<TItem>>("RightItemDetails");
			rightComparableItemDetails.Initialise(spriteProvider);

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
