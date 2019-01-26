using System;
using BattleCruisers.Buildables;
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

        private TargetType _selectedItemType;
        public TargetType SelectedItemType
        {
            get { return _selectedItemType; }
            private set
            {
                if (_selectedItemType != value)
                {
                    _selectedItemType = value;

                    if (SelectedItemTypeChanged != null)
                    {
                        SelectedItemTypeChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler SelectedItemTypeChanged;

        public ItemDetailsDisplayer(IItemDetailsPanel itemDetailsPanel)
        {
            Assert.IsNotNull(itemDetailsPanel);

            _itemDetailsPanel = itemDetailsPanel;
            // FELIX  Codesmell :/  Nullable?  Argh
            // Any type, as long as it's not an item type:  Cruiser, Aircraft, Ships
            _selectedItemType = TargetType.Rocket;
        }

        public void ShowDetails(IBuilding building)
        {
            ShowDetailsInternal(building);
            _itemDetailsPanel.LeftBuildingDetails.ShowItemDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            ShowDetailsInternal(unit);
            _itemDetailsPanel.LeftUnitDetails.ShowItemDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            ShowDetailsInternal(cruiser);
            _itemDetailsPanel.LeftCruiserDetails.ShowItemDetails(cruiser);
        }

        private void ShowDetailsInternal(ITarget item)
        {
            Assert.IsNotNull(item);

            _selectedItemType = item.TargetType;
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