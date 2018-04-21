using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class BaseItem<TItem> : MonoBehaviour, 
        IStatefulUIElement,    
        IItem<TItem> where TItem : IComparableItem
	{
		public static class Colors
		{
			public readonly static Color HIGHLIGHTED = Color.green;
            public readonly static Color DEFAULT = Color.grey;
		}

        protected TItem _item;
        public TItem Item { get { return _item; } }

        protected IItemDetailsManager<TItem> _itemDetailsManager;
        protected IItemState<TItem> _state;

        public Image itemImage;
        public Image selectedFeedbackImage;

        public Image backgroundImage;
        public Image BackgroundImage { get { return backgroundImage; } }

        public bool ShowSelectedFeedback
        {
            set { selectedFeedbackImage.gameObject.SetActive(value); }
        }

        public virtual void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
        {
            Helper.AssertIsNotNull(item, itemDetailsManager, itemImage, selectedFeedbackImage, BackgroundImage);

            _item = item;
            itemImage.sprite = _item.Sprite;

            _itemDetailsManager = itemDetailsManager;
        }

        public abstract void GoToDefaultState();

        public void GoToHighlightedState()
        {
            _state = new HighlightedState<TItem>(_itemDetailsManager, this);
        }

        public void GoToDisabledState()
        {
            _state = new DisabledState<TItem>(this);
        }

        public void SelectItem()
        {
            _state.SelectItem();
        }
	}
}
