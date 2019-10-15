using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButton : ItemButton
    {
        public UnitWrapper unit;
        public override IComparableItem Item => unit.Buildable;

        public override void Initialise(ISoundPlayer soundPlayer, IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);

            Assert.IsNotNull(unit);
            unit.StaticInitialise();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(unit.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(unit.Buildable);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}