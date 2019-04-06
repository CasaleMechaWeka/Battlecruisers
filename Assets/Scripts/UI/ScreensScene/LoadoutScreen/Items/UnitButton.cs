using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButton : ItemButton
    {
        public UnitWrapper unit;

        public override void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(itemDetailsManager, comparingItemFamily);

            Assert.IsNotNull(unit);
            unit.Initialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
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