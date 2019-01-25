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
        }

        public void ShowDetails(IBuilding building)
        {
            HideDetails();
            _itemDetailsPanel.LeftBuildingDetails.ShowItemDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            throw new System.NotImplementedException();
        }

        public void ShowDetails(ICruiser cruiser)
        {
            throw new System.NotImplementedException();
        }

        private void HideDetails()
        {
            _itemDetailsPanel.LeftBuildingDetails.Hide();
            // FELIX  Hide others :)
        }
    }
}