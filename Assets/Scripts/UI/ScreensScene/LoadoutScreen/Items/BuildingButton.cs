using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButton : ItemButton
    {
        public BuildingWrapper building;
        public override IComparableItem Item => building.Buildable;

        public override void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            base.Initialise(itemDetailsManager, comparingFamiltyTracker);

            Assert.IsNotNull(building);
            building.Initialise();
        }

        protected override void OnClicked()
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