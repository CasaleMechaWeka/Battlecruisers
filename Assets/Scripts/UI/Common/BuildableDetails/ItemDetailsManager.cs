using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IInformatorPanel _informatorPanel;
        private readonly IComparableItemDetails<IBuilding> _buildingDetails;
        private readonly IComparableItemDetails<IUnit> _unitDetails;
        private readonly IComparableItemDetails<ICruiser> _cruiserDetails;

        private ISettableBroadcastingProperty<ITarget> _selectedItem;
        public IBroadcastingProperty<ITarget> SelectedItem { get; }

        public ItemDetailsManager(IInformatorPanel informator)
        {
            Helper.AssertIsNotNull(informator);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _selectedItem = new SettableBroadcastingProperty<ITarget>(initialValue: null);
            SelectedItem = new BroadcastingProperty<ITarget>(_selectedItem);
        }

        public void ShowDetails(IBuilding building)
        {
            HideInformatorContent();

            _informatorPanel.Show(building);
            _buildingDetails.ShowItemDetails(building);
            _selectedItem.Value = building;
        }

        public void ShowDetails(IUnit unit)
        {
            HideInformatorContent();

            _informatorPanel.Show(unit);
            _unitDetails.ShowItemDetails(unit);
            _selectedItem.Value = unit;
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideInformatorContent();

            _informatorPanel.Show(cruiser);
            _cruiserDetails.ShowItemDetails(cruiser);
            _selectedItem.Value = cruiser;
        }
		
        public void HideDetails()
        {
            _informatorPanel.Hide();
            _selectedItem.Value = null;
        }

        private void HideInformatorContent()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
