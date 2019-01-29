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

        public override void Initialise(IItemDetailsManager itemDetailsManager, ISettableBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(itemDetailsManager, itemFamilyToCompare);

            Assert.IsNotNull(unit);

            unit.Initialise();
            unit.Buildable.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemFamilyToCompare.Value == null)
            {
                _itemDetailsManager.ShowDetails(unit.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(unit.Buildable);
                _itemFamilyToCompare.Value = null;
            }
        }
    }
}