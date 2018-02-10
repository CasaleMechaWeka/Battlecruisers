using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class ItemDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem> where TItem : class, ITarget, IComparableItem
	{
        protected abstract StatsController<TItem> StatsController { get; }
		
        protected TItem _item;

        // FELIX  Retrieve programmatically???
        public Text itemName;
        public Text itemDescription;
        public Image itemImage;

        public void Initialise()
        {
            StatsController.Initialise();
        }

        public virtual void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem))
        {
			Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;
			
            StatsController.ShowStats(item, itemToCompareTo);
            itemName.text = item.Name;
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
