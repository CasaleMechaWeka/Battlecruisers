using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public class HighlightedState<TItem> : ItemState<TItem> where TItem : class, IComparableItem
    {
		private readonly IItemDetailsManager<TItem> _itemDetailsManager;

        protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.HIGHLIGHTED; } }

        public HighlightedState(IItemDetailsManager<TItem> itemDetailsManager, IItem<TItem> item)
			: base(item)
		{
            Assert.IsNotNull(itemDetailsManager);
			_itemDetailsManager = itemDetailsManager;
		}

		public override void SelectItem()
		{
			_itemDetailsManager.SelectItem(_item);
		}
	}
}
