using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
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