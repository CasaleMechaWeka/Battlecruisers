using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedHullItemsRow : MonoBehaviour, IStatefulUIElement
    {
        private IList<HullItem> _hullItems;

        public void Initialise(
            IItemDetailsManager<ICruiser> hullDetailsManager,
            IHullItemsRow hullItemsRow,
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
		{
            Helper.AssertIsNotNull(hullDetailsManager, hullItemsRow, dataProvider, prefabFactory); 

            _hullItems = GetComponentsInChildren<HullItem>().ToList();
            Assert.AreEqual(dataProvider.StaticData.HullKeys.Count, _hullItems.Count);

            for (int i = 0; i < _hullItems.Count; ++i)
            {
                HullItem hullItem = _hullItems[i];
                HullKey hullKey = dataProvider.StaticData.HullKeys[i];
                ICruiser hullPrefab = prefabFactory.GetCruiserPrefab(hullKey);

                hullItem.Initialise(hullDetailsManager, hullItemsRow, dataProvider.GameModel, hullPrefab, hullKey);
            }
        }

        public void RefreshLockedStatus()
        {
            foreach (HullItem hullItem in _hullItems)
            {
                hullItem.RefreshLockedStatus();
            }
        }

        public void GoToState(UIState state)
        {
            foreach (HullItem hullItem in _hullItems)
            {
                hullItem.GoToState(state);
            }
        }

        public void UpdateSelectedHull(ICruiser selectedHull)
        {
            foreach (HullItem hullItem in _hullItems)
            {
                hullItem.OnNewHullSelected(selectedHull);
            }
        }
    }
}