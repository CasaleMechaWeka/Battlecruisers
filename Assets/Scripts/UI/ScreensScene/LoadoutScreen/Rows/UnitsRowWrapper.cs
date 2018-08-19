using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class UnitsRowWrapper : MonoBehaviour
    {
        public UnitCategory category;

        public IStatefulUIElement UnitsRow { get; private set; }

        public void Initialise(IItemsRowArgs<IUnit> args)
        {
            Assert.IsNotNull(args);

            LoadoutUnitItemsRow loadoutRow = GetComponentInChildren<LoadoutUnitItemsRow>();
            Assert.IsNotNull(loadoutRow);
            loadoutRow.Initialise(args, category);

            UnitsRow = loadoutRow;
        }
    }
}
