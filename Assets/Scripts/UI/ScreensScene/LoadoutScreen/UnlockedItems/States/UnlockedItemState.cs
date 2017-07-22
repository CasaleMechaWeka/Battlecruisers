using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
    public abstract class UnlockedItemState<TItem> : IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		protected readonly UnlockedItem<TItem> _item;

		protected abstract Color BackgroundColour { get; }

		public UnlockedItemState(UnlockedItem<TItem> item)
		{
			_item = item;
			_item.backgroundImage.color = BackgroundColour;
		}

		public abstract void HandleSelection();
	}
}
