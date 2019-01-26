using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    // FELIX  Test :)
    public class ItemDetailsDisplayer : IItemDetailsDisplayer
    {
        private readonly IItemDetailsPanel _itemDetailsPanel;

        private ItemFamily? _selectedItemFamily;
        public ItemFamily? SelectedItemFamily
        {
            get { return _selectedItemFamily; }
            private set
            {
                if (_selectedItemFamily != value)
                {
                    _selectedItemFamily = value;

                    if (SelectedItemFamilyChanged != null)
                    {
                        SelectedItemFamilyChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler SelectedItemFamilyChanged;

        public ItemDetailsDisplayer(IItemDetailsPanel itemDetailsPanel)
        {
            Assert.IsNotNull(itemDetailsPanel);

            _itemDetailsPanel = itemDetailsPanel;
            _selectedItemFamily = null;
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

            _selectedItemFamily = itemFamily;
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