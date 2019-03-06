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
        private readonly IBuildableDetails<IBuilding> _buildingDetails;
        private readonly IBuildableDetails<IUnit> _unitDetails;
        private readonly ICruiserDetails _cruiserDetails;

        private ISettableBroadcastingProperty<ITarget> _selectedItem;
        public IBroadcastingProperty<ITarget> SelectedItem { get; private set; }

        public ItemDetailsManager(IInformatorPanel informator)
        {
            Helper.AssertIsNotNull(informator);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _selectedItem = new SettableBroadcastingProperty<ITarget>(initialValue: null);
            SelectedItem = new BroadcastingProperty<ITarget>(_selectedItem);

            _informatorPanel.Hide();
        }

        public void ShowDetails(IBuilding building)
        {
            HideDetails();

            _informatorPanel.Show();
            _buildingDetails.ShowBuildableDetails(building);
            _selectedItem.Value = building;
        }

        public void ShowDetails(IUnit unit)
        {
            HideDetails();

            _informatorPanel.Show();
            _unitDetails.ShowBuildableDetails(unit);
            _selectedItem.Value = unit;
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideDetails();

            _informatorPanel.Show();
            _cruiserDetails.ShowCruiserDetails(cruiser);
            _selectedItem.Value = cruiser;
        }
		
        public void HideDetails()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
            _informatorPanel.Hide();
            _selectedItem.Value = null;
        }
    }
}
