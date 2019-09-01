using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CategoryButtonsPanel : MonoBehaviour
    {
        public void Initialise(
            IItemPanelsController itemPanels, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, soundPlayer);

            ItemCategoryButton[] buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in buttons)
            {
                button.Initialise(soundPlayer, itemPanels, itemFamilyToCompare);
            }
        }
    }
}