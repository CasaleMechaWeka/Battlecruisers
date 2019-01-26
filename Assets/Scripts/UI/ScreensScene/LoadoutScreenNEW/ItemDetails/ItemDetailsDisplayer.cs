using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    // FELIX  Test :)
    public class ItemDetailsDisplayer : IItemDetailsDisplayer
    {
        private readonly IItemDetailsPanel _itemDetailsPanel;

        public ItemDetailsDisplayer(IItemDetailsPanel itemDetailsPanel)
        {
            Assert.IsNotNull(itemDetailsPanel);

            _itemDetailsPanel = itemDetailsPanel;
            HideDetails();
        }

        public void ShowDetails(IBuilding building)
        {
            Assert.IsNotNull(building);

            HideDetails();
            _itemDetailsPanel.LeftBuildingDetails.ShowItemDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            Assert.IsNotNull(unit);

            HideDetails();
            _itemDetailsPanel.LeftUnitDetails.ShowItemDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            HideDetails();
            _itemDetailsPanel.LeftCruiserDetails.ShowItemDetails(cruiser);
        }

        private void HideDetails()
        {
            _itemDetailsPanel.LeftBuildingDetails.Hide();
            _itemDetailsPanel.LeftUnitDetails.Hide();
            _itemDetailsPanel.LeftCruiserDetails.Hide();
            // FELIX  Hide others :)
        }
    }
}