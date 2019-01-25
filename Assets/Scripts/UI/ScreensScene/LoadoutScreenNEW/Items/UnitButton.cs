using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class UnitButton : ItemButton
    {
        public UnitWrapper unit;

        public override void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            base.Initialise(itemDetailsDisplayer);

            Assert.IsNotNull(unit);

            unit.Initialise();
            unit.Buildable.StaticInitialise();
        }

        protected override void ShowItemDetails()
        {
            _itemDetailsDisplayer.ShowDetails(unit.Buildable);
        }
    }
}