using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.UI.Common.BuildableDetails;
using UnityEngine.Assertions;
using IItemDetailsManager = BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.IItemDetailsManager;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleItemContainerV2 : ItemContainer
    {
        public IHeckleData heckleData;

        protected override ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IGameModel gameModel)
        {
            HeckleButtonV2 heckleButton = GetComponentInChildren<HeckleButtonV2>(includeInactive: true);
            Assert.IsNotNull(heckleButton);
            heckleButton.Initialise(soundPlayer, heckleData, itemDetailsManager, comparingFamilyTracker);
            return heckleButton;
        }



        // Heckle button is not working as like other category buttons
        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return true;
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return false;
        }

        protected override void MakeOld(IGameModel gameModel)
        {

        }
    }
}
