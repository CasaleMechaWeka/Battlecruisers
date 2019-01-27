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

        public override void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(itemDetailsDisplayer, itemFamilyToCompare);

            Assert.IsNotNull(cruiser);
            cruiser.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemFamilyToCompare.Value == null)
            {
                _itemDetailsDisplayer.ShowDetails(cruiser);
            }
            else
            {
                _itemDetailsDisplayer.CompareWithSelectedItem(cruiser);
                _itemFamilyToCompare.Value = null;
            }
        }
    }
}