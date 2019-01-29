using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class BuildingButton : ItemButton
    {
        public BuildingWrapper building;

        public override void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            base.Initialise(itemDetailsManager, comparingFamiltyTracker);

            Assert.IsNotNull(building);
            building.Initialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(building.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(building.Buildable);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}