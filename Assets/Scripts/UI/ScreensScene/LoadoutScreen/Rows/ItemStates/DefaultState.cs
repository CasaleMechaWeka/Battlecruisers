using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public abstract class DefaultState<TItem> : ItemState<TItem> where TItem : IComparableItem
	{
		protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.DEFAULT; } }

        protected DefaultState(IItem<TItem> item)
			: base(item)
		{
		}
	}
}
