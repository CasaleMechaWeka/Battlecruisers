using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IInformatorPanel _informatorPanel;
        private readonly IBuildableDetails<IBuilding> _buildingDetails;
        private readonly IBuildableDetails<IUnit> _unitDetails;
        private readonly ICruiserDetails _cruiserDetails;

        public ItemDetailsManager(IInformatorPanel informator)
        {
            Helper.AssertIsNotNull(informator);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _informatorPanel.Hide();
        }

        public void ShowDetails(IBuilding building)
        {
            HideDetails();

            _informatorPanel.Show();
            _buildingDetails.ShowBuildableDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            HideDetails();

            _informatorPanel.Show();
            _unitDetails.ShowBuildableDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideDetails();

            _informatorPanel.Show();
            _cruiserDetails.ShowCruiserDetails(cruiser);
        }
		
        public void HideDetails()
        {
            _buildingDetails.Hide();
            // FELIX  Uncomment :P
            //_unitDetails.Hide();
            //_cruiserDetails.Hide();
            _informatorPanel.Hide();
        }
    }
}
