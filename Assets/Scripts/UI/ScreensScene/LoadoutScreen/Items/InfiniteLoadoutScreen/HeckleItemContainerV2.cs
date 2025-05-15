using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using IItemDetailsManager = BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.IItemDetailsManager;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleItemContainerV2 : ItemContainer
    {
        public HeckleData heckleData;
        public SelectHeckleButton toggleSelectionButton;

        protected override ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            SingleSoundPlayer soundPlayer,
            GameModel gameModel)
        {
            HeckleButtonV2 heckleButton = GetComponentInChildren<HeckleButtonV2>(includeInactive: true);
            Assert.IsNotNull(heckleButton);
            heckleButton.Initialise(_itemsPanel, soundPlayer, heckleData, itemDetailsManager, comparingFamilyTracker, gameModel);
            heckleButton.selectHeckleButton = toggleSelectionButton;
            return heckleButton;
        }

        // Heckle button is not working as like other category buttons
        protected override bool IsUnlocked(GameModel gameModel)
        {
            return true;
        }

        protected override bool IsNew(GameModel gameModel)
        {
            return false;
        }

        protected override void MakeOld(GameModel gameModel)
        {

        }
    }
}
