using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Handle being disabled during comparison.  Follow BuildableButtonController's use of IBroadcastingFilter??
    public class ItemCategoryButton : MonoBehaviour, IPointerClickHandler
    {
        private IItemPanelsController _itemPanels;

        public ItemType itemType;

        public void Initialise(IItemPanelsController itemPanels)
        {
            Assert.IsNotNull(itemPanels);
            _itemPanels = itemPanels;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemPanels.ShowItemsPanel(itemType);
        }
    }
}