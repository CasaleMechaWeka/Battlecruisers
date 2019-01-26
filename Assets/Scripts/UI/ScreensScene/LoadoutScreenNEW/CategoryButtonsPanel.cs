using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class CategoryButtonsPanel : MonoBehaviour
    {
        public void Initialise(IItemPanelsController itemPanels, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare);

            ItemCategoryButton[] buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in buttons)
            {
                button.Initialise(itemPanels, itemFamilyToCompare);
            }
        }
    }
}