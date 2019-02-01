using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class BaseItem<TItem> : MonoBehaviourWrapper, IItem<TItem> 
        where TItem : class, IComparableItem
	{
		public static class Colors
		{
			public readonly static Color HIGHLIGHTED = Color.green;
            public readonly static Color DEFAULT = Color.grey;
		}

        private TItem _item;
        public TItem Item
        {
            get { return _item; }
            protected set
            {
                Assert.IsNotNull(value);

                _item = value;
                itemImage.sprite = _item.Sprite;
            }
        }

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

        public abstract ItemType Type { get; }

        public virtual void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
        {
            Helper.AssertIsNotNull(item, itemDetailsManager, itemImage, selectedFeedbackImage, BackgroundImage);

            Item = item;

            _itemDetailsManager = itemDetailsManager;
        }

        public void SelectItem()
        {
            _state.SelectItem();
        }

        public void GoToState(UIState state)
        {
            _state = CreateState(state);
        }

        private IItemState<TItem> CreateState(UIState state)
        {
			switch (state)
			{
				case UIState.Default:
					return CreateDefaultState();
				case UIState.Highlighted:
                    return new HighlightedState<TItem>(_itemDetailsManager, this);
                case UIState.Disabled:
                    return new DisabledState<TItem>(this);
                default:
                    throw new ArgumentException();
			}
        }

        protected abstract IItemState<TItem> CreateDefaultState();
	}
}
