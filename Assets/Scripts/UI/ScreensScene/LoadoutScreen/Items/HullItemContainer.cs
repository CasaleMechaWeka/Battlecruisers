using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HullItemContainer : ItemContainer
    {
        public PrefabKeyName hullKeyName;

        private HullKey _hullKey;
        private HullKey HullKey
        {
            get
            {
                if (_hullKey == null)
                {
                    _hullKey = StaticPrefabKeyHelper.GetPrefabKey<HullKey>(hullKeyName);
                }
                return _hullKey;
            }
        }

        protected override ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamilyTracker, 
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory)
        {
            Cruiser cruiserPrefab = prefabFactory.GetCruiserPrefab(HullKey);
            HullButton hullButton = GetComponentInChildren<HullButton>(includeInactive: true);
            Assert.IsNotNull(hullButton);
            hullButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker, HullKey, cruiserPrefab, selectedHull);
            return hullButton;
        }

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedHulls.Contains(HullKey);
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return gameModel.NewHulls.Items.Contains(HullKey);
        }

        protected override void MakeOld(IGameModel gameModel)
        {
            gameModel.NewHulls.RemoveItem(HullKey);
        }
    }
}