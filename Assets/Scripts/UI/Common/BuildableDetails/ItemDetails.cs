using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class ItemDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem> where TItem : class, ITarget, IComparableItem
	{
		private Text _itemName, _itemDescription;
		private Image _itemImage;
		
        protected abstract StatsController<TItem> StatsController { get; }
        
        protected TItem _item;

		public void Initialise()
        {
            _itemName = transform.FindNamedComponent<Text>("ItemName");
            _itemDescription = transform.FindNamedComponent<Text>("ItemDescription");
            _itemImage = transform.FindNamedComponent<Image>("ItemImage");

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
            _itemName.text = item.Name;
            _itemDescription.text = item.Description;
            _itemImage.sprite = item.Sprite;
            
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
