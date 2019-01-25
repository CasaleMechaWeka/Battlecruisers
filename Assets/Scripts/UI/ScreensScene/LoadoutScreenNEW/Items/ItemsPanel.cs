using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        public ItemType itemType;
        public ItemType ItemType { get { return itemType; } }

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            Assert.IsNotNull(itemDetailsDisplayer);

            ItemButton[] buttons = GetComponentsInChildren<ItemButton>(includeInactive: true);

            foreach (ItemButton button in buttons)
            {
                button.Initialise(itemDetailsDisplayer);
            }
        }
    }
}