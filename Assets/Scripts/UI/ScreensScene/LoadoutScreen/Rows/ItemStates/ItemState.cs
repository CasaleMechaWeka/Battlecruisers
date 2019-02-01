using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    /// <summary>
    /// Handles:
    /// 1. What should happen when the item is selected (clicked on).
    /// 2. What background colour the item should be.
    /// </summary>
    public abstract class ItemState<TItem> : IItemState<TItem> where TItem : IComparableItem
	{
        protected readonly IItem<TItem> _item;

		protected abstract Color BackgroundColour { get; }

        protected ItemState(IItem<TItem> item)
		{
            Assert.IsNotNull(item);

			_item = item;
            _item.BackgroundImage.color = BackgroundColour;
		}

		public abstract void SelectItem();
	}
}
