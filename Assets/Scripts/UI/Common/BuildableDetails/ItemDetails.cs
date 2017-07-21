using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class ItemDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem> where TItem : Target, IComparableItem
	{
        protected abstract StatsController<TItem> StatsController { get; }
		
        protected TItem _item;

        public Text itemName;
        public Text itemDescription;
        public Image itemImage;

        public virtual void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem))
        {
			Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;
			
            StatsController.ShowStats(item, itemToCompareTo);
            itemName.text = item.name;
            itemDescription.text = item.Description;
            itemImage.sprite = item.Sprite;
            
			gameObject.SetActive(true);
		}

        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);
        }

        protected virtual void CleanUp() { }
    }
}
