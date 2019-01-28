using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class BuildingButton : ItemButton
    {
        public BuildingWrapper building;

        public override void Initialise(IItemDetailsManager itemDetailsManager, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(itemDetailsManager, itemFamilyToCompare);

            Assert.IsNotNull(building);

            building.Initialise();
            building.Buildable.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_itemFamilyToCompare.Value == null)
            {
                _itemDetailsManager.ShowDetails(building.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(building.Buildable);
                _itemFamilyToCompare.Value = null;
            }
        }
    }
}