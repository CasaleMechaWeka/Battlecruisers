using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class HullButton : ItemButton
    {
        public Cruiser cruiser;

        public override void Initialise(IItemDetailsManager itemDetailsManager, ISettableBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(itemDetailsManager, itemFamilyToCompare);

            Assert.IsNotNull(cruiser);
            cruiser.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemFamilyToCompare.Value == null)
            {
                _itemDetailsManager.ShowDetails(cruiser);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(cruiser);
                _itemFamilyToCompare.Value = null;
            }
        }
    }
}