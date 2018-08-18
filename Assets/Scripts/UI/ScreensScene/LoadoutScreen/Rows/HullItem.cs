using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    // FELIX  Potentially avoid duplicate code with BuildableItems?
    public class HullItem : MonoBehaviour
    {
        // FELIX  Do not cache unused fields :/
        private IItemDetailsManager<ICruiser> _hullDetailsManager;
        private IHullItemsRow _hullItemsRow;
        private IGameModel _gameModel;
        private ICruiser _hull;
        private HullKey _hullKey;
        private UnlockedHullItem _unlockedHullItem;
        private LockedItem _lockedItem;

        public void Initialise(
            IItemDetailsManager<ICruiser> hullDetailsManager,
            IHullItemsRow hullItemsRow,
            IGameModel gameModel,
            ICruiser hull,
            HullKey hullKey)
        {
            Helper.AssertIsNotNull(hullDetailsManager, hullItemsRow, gameModel, hull, hullKey);

            _hullDetailsManager = hullDetailsManager;
            _hullItemsRow = hullItemsRow;
            _gameModel = gameModel;
            _hull = hull;
            _hullKey = hullKey;

            _unlockedHullItem = GetComponentInChildren<UnlockedHullItem>();
            Assert.IsNotNull(_unlockedHullItem);
            _unlockedHullItem.Initialise(_hull, _hullDetailsManager);

            _lockedItem = GetComponentInChildren<LockedItem>();
            Assert.IsNotNull(_lockedItem);
            _lockedItem.Initialise();
        }

        public void RefreshLockedStatus()
        {
            bool isHullUnlocked = _gameModel.UnlockedHulls.Contains(_hullKey);

            _unlockedHullItem.IsVisible = isHullUnlocked;
            _lockedItem.IsVisible = !isHullUnlocked;
        }
    }
}