using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutItem<TItem> : BaseItem<TItem> where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		private TItem _item;
		public override TItem Item 
		{
			get { return _item; }
			protected set
			{
				_item = value;
				itemImage.sprite = _item.Sprite;
			}
		}

		public void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
		{
			Item = item;
			_itemDetailsManager = itemDetailsManager;
		}

		public void SelectItem()
		{
			_itemDetailsManager.SelectItem(this);
		}
	}
}
