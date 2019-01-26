using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    // FELIX  Test :)
    public class ItemDetailsDisplayer : IItemDetailsDisplayer
    {
        private readonly IItemDetailsPanel _itemDetailsPanel;

        public ItemFamily? SelectedItemFamily { get; private set; }

        public ItemDetailsDisplayer(IItemDetailsPanel itemDetailsPanel)
        {
            Assert.IsNotNull(itemDetailsPanel);

            _itemDetailsPanel = itemDetailsPanel;
            SelectedItemFamily = null;
        }

        public void ShowDetails(IBuilding building)
        {
            ShowDetailsInternal(building, ItemFamily.Buildings);
            _itemDetailsPanel.LeftBuildingDetails.ShowItemDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            ShowDetailsInternal(unit, ItemFamily.Units);
            _itemDetailsPanel.LeftUnitDetails.ShowItemDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            ShowDetailsInternal(cruiser, ItemFamily.Hulls);
            _itemDetailsPanel.LeftCruiserDetails.ShowItemDetails(cruiser);
        }

        private void ShowDetailsInternal(ITarget item, ItemFamily itemFamily)
        {
            Assert.IsNotNull(item);

            SelectedItemFamily = itemFamily;
            HideDetails();
        }

        public void HideDetails()
        {
            _itemDetailsPanel.LeftBuildingDetails.Hide();
            _itemDetailsPanel.LeftUnitDetails.Hide();
            _itemDetailsPanel.LeftCruiserDetails.Hide();
            // FELIX  Hide others :)
        }
    }
}