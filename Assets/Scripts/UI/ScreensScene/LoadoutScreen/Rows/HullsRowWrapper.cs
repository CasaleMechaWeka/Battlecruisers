using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class HullsRowWrapper : MonoBehaviour
    {
        public void Initialise(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory)
        {
            LoadoutHullItem loadoutHullItem = GetComponentInChildren<LoadoutHullItem>();
            Assert.IsNotNull(loadoutHullItem);

            UnlockedHullItemsRow unlockedHullsRow = GetComponentInChildren<UnlockedHullItemsRow>();
            Assert.IsNotNull(unlockedHullsRow);

            CruiserDetailsManager cruiserDetailsManager = GetComponentInParent<CruiserDetailsManager>();
            Assert.IsNotNull(cruiserDetailsManager);

            IItemsRowArgs<ICruiser> args = new ItemsRowArgs<ICruiser>(gameModel, prefabFactory, uiFactory, cruiserDetailsManager);
            IItemsRow<ICruiser> hullsRow = new HullItemsRow(args, loadoutHullItem, unlockedHullsRow);
            hullsRow.SetupUI();
        }
    }
}
