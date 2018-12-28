using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class ItemDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem>, IHidable
        where TItem : class, ITarget, IComparableItem
	{
		private Text _itemName, _itemDescription;
		private Image _itemImage;
        private StatsController<TItem> _statsController;
        
        protected TItem _item;

        public event EventHandler Dismissed;

		public void Initialise()
        {
            _itemName = transform.FindNamedComponent<Text>("LeftColumn/ItemName");
            _itemDescription = transform.FindNamedComponent<Text>("LeftColumn/ItemDescription");
            _itemImage = transform.FindNamedComponent<Image>("RightColumn/ItemImage");

            _statsController = GetStatsController();
            Assert.IsNotNull(_statsController);
            _statsController.Initialise();
        }

        protected abstract StatsController<TItem> GetStatsController();

        public virtual void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem))
        {
			Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;
			
            _statsController.ShowStats(item, itemToCompareTo);
            _itemName.text = item.Name;
            _itemDescription.text = item.Description;
            _itemImage.sprite = item.Sprite;

            _item.Destroyed += _item_Destroyed;

			gameObject.SetActive(true);
		}

        private void _item_Destroyed(object sender, DestroyedEventArgs e)
        {
            _item.Destroyed -= _item_Destroyed;
            Hide();
        }

        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);

            if (Dismissed != null)
            {
                Dismissed.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void CleanUp() { }
    }
}
