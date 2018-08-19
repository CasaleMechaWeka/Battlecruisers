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
    public class HullItem : MonoBehaviour, IStatefulUIElement
    {
        private IGameModel _gameModel;
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

            _gameModel = gameModel;
            _hullKey = hullKey;

            _unlockedHullItem = GetComponentInChildren<UnlockedHullItem>();
            Assert.IsNotNull(_unlockedHullItem);
            bool isInLoadout = gameModel.PlayerLoadout.Hull.Equals(hullKey);
            _unlockedHullItem.Initialise(hull, hullDetailsManager, hullItemsRow, isInLoadout);

            _lockedItem = GetComponentInChildren<LockedItem>();
            Assert.IsNotNull(_lockedItem);
        }

        public void RefreshLockedStatus()
        {
            bool isHullUnlocked = _gameModel.UnlockedHulls.Contains(_hullKey);

            _unlockedHullItem.IsVisible = isHullUnlocked;
            _lockedItem.IsVisible = !isHullUnlocked;
        }

        public void GoToState(UIState state)
        {
            _unlockedHullItem.GoToState(state);
        }

        public void OnNewHullSelected(ICruiser selectedHull)
        {
            _unlockedHullItem.OnNewHullSelected(selectedHull);
        }
    }
}