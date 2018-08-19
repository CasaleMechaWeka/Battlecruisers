using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class UnitsRowWrapper : BuildablesRowWrapper<IUnit, UnitKey>
    {
        public UnitCategory category;

        public void Initialise(IItemsRowArgs<IUnit> args)
        {
            Assert.IsNotNull(args);

            LoadoutUnitItemsRow loadoutRow = GetComponentInChildren<LoadoutUnitItemsRow>();
            Assert.IsNotNull(loadoutRow);
            loadoutRow.Initialise(args, category);

            BuildablesRow = loadoutRow;
        }
    }
}
