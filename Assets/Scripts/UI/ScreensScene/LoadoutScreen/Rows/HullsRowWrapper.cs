using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class HullsRowWrapper : MonoBehaviour
    {
        public void Initialise(IItemsRowArgs<ICruiser> args)
        {
            LoadoutHullItem loadoutHullItem = GetComponentInChildren<LoadoutHullItem>();
            Assert.IsNotNull(loadoutHullItem);

            UnlockedHullItemsRow unlockedHullsRow = GetComponentInChildren<UnlockedHullItemsRow>();
            Assert.IsNotNull(unlockedHullsRow);

            IItemsRow<ICruiser> hullsRow = new HullItemsRow(args, loadoutHullItem, unlockedHullsRow);
            hullsRow.SetupUI();
        }
    }
}
