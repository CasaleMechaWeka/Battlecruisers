using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CategoryButtonsPanel : MonoBehaviour, IManagedDisposable
    {
        private ItemCategoryButton[] _buttons;

        public void Initialise(
            IItemPanelsController itemPanels, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            ISingleSoundPlayer soundPlayer,
            IGameModel gameModel,
            IList<IItemButton> itemButtons,
            IComparingItemFamilyTracker itemFamilyTracker)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, soundPlayer, gameModel, itemButtons);

            _buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in _buttons)
            {
                button.Initialise(soundPlayer, itemPanels, itemFamilyToCompare, gameModel, itemButtons, itemFamilyTracker);
            }
        }

        public void DisposeManagedState()
        {
            foreach (ItemCategoryButton button in _buttons)
            {
                button.DisposeManagedState();
            }
        }
    }
}