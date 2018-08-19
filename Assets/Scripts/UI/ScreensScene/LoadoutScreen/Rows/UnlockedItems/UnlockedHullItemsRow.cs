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
        private IList<HullItemWrapper> _hullItems;

        public void Initialise(
            IItemDetailsManager<ICruiser> hullDetailsManager,
            IHullItemsRow hullItemsRow,
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
		{
            Helper.AssertIsNotNull(hullDetailsManager, hullItemsRow, dataProvider, prefabFactory); 

            _hullItems = GetComponentsInChildren<HullItemWrapper>().ToList();
            Assert.AreEqual(dataProvider.StaticData.HullKeys.Count, _hullItems.Count);

            for (int i = 0; i < _hullItems.Count; ++i)
            {
                HullItemWrapper hullItem = _hullItems[i];
                HullKey hullKey = dataProvider.StaticData.HullKeys[i];
                ICruiser hullPrefab = prefabFactory.GetCruiserPrefab(hullKey);

                hullItem.Initialise(hullDetailsManager, hullItemsRow, dataProvider.GameModel, hullPrefab, hullKey);
            }
        }

        public void RefreshLockedStatus()
        {
            foreach (HullItemWrapper hullItem in _hullItems)
            {
                hullItem.OnPresenting(activationParameter: null);
            }
        }

        public void GoToState(UIState state)
        {
            foreach (HullItemWrapper hullItem in _hullItems)
            {
                hullItem.GoToState(state);
            }
        }

        public void UpdateSelectedHull(ICruiser selectedHull)
        {
            foreach (HullItemWrapper hullItem in _hullItems)
            {
                hullItem.OnNewHullSelected(selectedHull);
            }
        }
    }
}