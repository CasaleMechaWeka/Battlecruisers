using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class BuildingButton : ItemButton
    {
        public IBuilding building;

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            base.Initailise(itemDetailsDisplayer);
            Assert.IsNotNull(building);
        }

        protected override void ShowItemDetails()
        {
            _itemDetailsDisplayer.ShowDetails(building);
        }
    }
}