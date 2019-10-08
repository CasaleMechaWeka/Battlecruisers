using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CategoryButtonsPanel : MonoBehaviour, IManagedDisposable
    {
        private ItemCategoryButton[] _buttons;

        public void Initialise(
            IItemPanelsController itemPanels, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            ISoundPlayer soundPlayer,
            IGameModel gameModel)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, soundPlayer, gameModel);

            _buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in _buttons)
            {
                button.Initialise(soundPlayer, itemPanels, itemFamilyToCompare, gameModel);
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