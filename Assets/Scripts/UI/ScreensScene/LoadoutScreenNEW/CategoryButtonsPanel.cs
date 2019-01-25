using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class CategoryButtonsPanel : MonoBehaviour
    {
        public void Initialise(IItemPanelsController itemPanels)
        {
            Assert.IsNotNull(itemPanels);

            ItemCategoryButton[] buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in buttons)
            {
                button.Initialise(itemPanels);
            }
        }
    }
}