using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class BuildingButton : ItemButton
    {
        public BuildingWrapper building;

        public override void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            base.Initialise(itemDetailsDisplayer);

            Assert.IsNotNull(building);

            building.Initialise();
            building.Buildable.StaticInitialise();
        }

        protected override void ShowItemDetails()
        {
            _itemDetailsDisplayer.ShowDetails(building.Buildable);
        }
    }
}