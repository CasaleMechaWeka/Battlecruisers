using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class HullItemWrapper : ItemWrapper<ICruiser, HullKey>
    {
        private UnlockedHullItem _unlockedHullItem;
        protected override IItem<ICruiser> UnlockedItem { get { return _unlockedHullItem; } }

        public void Initialise(
            IItemDetailsManager<ICruiser> hullDetailsManager,
            IHullItemsRow hullItemsRow,
            IGameModel gameModel,
            ICruiser hull,
            HullKey hullKey)
        {
            base.Initialise(gameModel, hullKey);

            Helper.AssertIsNotNull(hullDetailsManager, hullItemsRow, hull);

            _unlockedHullItem = GetComponentInChildren<UnlockedHullItem>();
            Assert.IsNotNull(_unlockedHullItem);
            bool isInLoadout = gameModel.PlayerLoadout.Hull.Equals(hullKey);
            _unlockedHullItem.Initialise(hull, hullDetailsManager, hullItemsRow, isInLoadout);
        }

        public void OnNewHullSelected(ICruiser selectedHull)
        {
            _unlockedHullItem.OnNewHullSelected(selectedHull);
        }

        protected override bool IsItemUnlocked()
        {
            return _gameModel.UnlockedHulls.Contains(_itemKey);
        }
    }
}