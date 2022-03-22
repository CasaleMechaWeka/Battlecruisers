using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
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
		public Text itemName, itemDescription;
		public Image itemImage;
        private StatsController<TItem> _statsController;
        
        protected TItem _item;

        public event EventHandler Dismissed;

		public virtual void Initialise()
        {
            Helper.AssertIsNotNull(itemName, itemDescription, itemImage);

            _statsController = GetStatsController();
            Assert.IsNotNull(_statsController);
            _statsController.Initialise();
        }

        protected abstract StatsController<TItem> GetStatsController();

        public virtual void ShowItemDetails(TItem item, TItem itemToCompareTo = default)
        {
			Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;
			
            _statsController.ShowStats(item, itemToCompareTo);
            itemName.text = item.Name.ToUpper();
            itemDescription.text = item.Description;
            itemImage.sprite = item.Sprite;

			gameObject.SetActive(true);
		}

        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);

            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void CleanUp() { }
    }
}
