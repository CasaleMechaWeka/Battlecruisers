using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class UnitButton : ItemButton
    {
        public UnitWrapper unit;

        public override void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(itemDetailsDisplayer, itemFamilyToCompare);

            Assert.IsNotNull(unit);

            unit.Initialise();
            unit.Buildable.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemFamilyToCompare.Value == null)
            {
                _itemDetailsDisplayer.ShowDetails(unit.Buildable);
            }
            else
            {
                _itemDetailsDisplayer.CompareWithSelectedItem(unit.Buildable);
                _itemFamilyToCompare.Value = null;
            }
        }
    }
}