using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    // FELIX  Test :)
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IItemFamilyDetailsDisplayer<IBuilding> _buildingDetails;
        private readonly IItemFamilyDetailsDisplayer<IUnit> _unitDetails;
        private readonly IItemFamilyDetailsDisplayer<ICruiser> _cruiserDetails;

        public ItemFamily? SelectedItemFamily { get; private set; }

        private int _numOfDetailsShown;
        public int NumOfDetailsShown
        {
            get { return _numOfDetailsShown; }
            private set
            {
                if (_numOfDetailsShown != value)
                {
                    _numOfDetailsShown = value;

                    if (NumOfDetailsShownChanged != null)
                    {
                        NumOfDetailsShownChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler NumOfDetailsShownChanged;

        public ItemDetailsManager(
            IItemFamilyDetailsDisplayer<IBuilding> buildingDetails,
            IItemFamilyDetailsDisplayer<IUnit> unitDetails,
            IItemFamilyDetailsDisplayer<ICruiser> cruiserDetails)
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, cruiserDetails);

            _buildingDetails = buildingDetails;
            _unitDetails = unitDetails;
            _cruiserDetails = cruiserDetails;

            SelectedItemFamily = null;
            _numOfDetailsShown = 0;
        }

        public void ShowDetails(IBuilding building)
        {
            ShowDetails(building, _buildingDetails, ItemFamily.Buildings);
        }

        public void ShowDetails(IUnit unit)
        {
            ShowDetails(unit, _unitDetails, ItemFamily.Units);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            ShowDetails(cruiser, _cruiserDetails, ItemFamily.Hulls);
        }

        private void ShowDetails<TItem, TItemDetails>(TItem item, TItemDetails itemDetails, ItemFamily itemFamily)
            where TItem : class, IComparableItem
            where TItemDetails : IItemFamilyDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);

            HideDetails();
            SelectedItemFamily = itemFamily;
            itemDetails.SelectItem(item);
            NumOfDetailsShown = 1;
        }

        public void CompareWithSelectedItem(IBuilding building)
        {
            CompareWithSelectedItem(building, _buildingDetails, ItemFamily.Buildings);
        }

        public void CompareWithSelectedItem(IUnit unit)
        {
            CompareWithSelectedItem(unit, _unitDetails, ItemFamily.Units);
        }

        public void CompareWithSelectedItem(ICruiser cruiser)
        {
            CompareWithSelectedItem(cruiser, _cruiserDetails, ItemFamily.Hulls);
        }

        private void CompareWithSelectedItem<TItem, TItemDetails>(TItem item, TItemDetails itemDetails, ItemFamily itemFamily)
            where TItem : class, IComparableItem
            where TItemDetails : IItemFamilyDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);
            Assert.AreEqual(SelectedItemFamily, itemFamily);

            itemDetails.CompareWithSelectedItem(item);
            NumOfDetailsShown = 2;
        }

        public void HideDetails()
        {
            _buildingDetails.HideDetails();
            _unitDetails.HideDetails();
            _cruiserDetails.HideDetails();
            NumOfDetailsShown = 0;
        }
    }
}