using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButton : ItemButton
    {
        public BuildingWrapper building;
        public override IComparableItem Item => building.Buildable;

        public override void Initialise(ISoundPlayer soundPlayer, IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);

            Assert.IsNotNull(building);
            building.Initialise();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

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