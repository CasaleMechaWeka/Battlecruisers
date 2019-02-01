using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IItemDetailsDisplayer<IBuilding> _buildingDetails;
        private readonly IItemDetailsDisplayer<IUnit> _unitDetails;
        private readonly IItemDetailsDisplayer<ICruiser> _cruiserDetails;

        public ItemFamily? SelectedItemFamily { get; private set; }

        private readonly ISettableBroadcastingProperty<int> _numOfDetailsShown;
        public IBroadcastingProperty<int> NumOfDetailsShown { get; private set; }

        public ItemDetailsManager(
            IItemDetailsDisplayer<IBuilding> buildingDetails,
            IItemDetailsDisplayer<IUnit> unitDetails,
            IItemDetailsDisplayer<ICruiser> cruiserDetails)
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, cruiserDetails);

            _buildingDetails = buildingDetails;
            _unitDetails = unitDetails;
            _cruiserDetails = cruiserDetails;

            SelectedItemFamily = null;

            _numOfDetailsShown = new SettableBroadcastingProperty<int>(initialValue: 0);
            NumOfDetailsShown = new BroadcastingProperty<int>(_numOfDetailsShown);
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
            where TItemDetails : IItemDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);

            HideDetails();
            SelectedItemFamily = itemFamily;
            itemDetails.SelectItem(item);
            _numOfDetailsShown.Value = 1;
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
            where TItemDetails : IItemDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);
            Assert.AreEqual(SelectedItemFamily, itemFamily);

            itemDetails.CompareWithSelectedItem(item);
            _numOfDetailsShown.Value = 2;
        }

        public void HideDetails()
        {
            _buildingDetails.HideDetails();
            _unitDetails.HideDetails();
            _cruiserDetails.HideDetails();
            _numOfDetailsShown.Value = 0;
        }
    }
}