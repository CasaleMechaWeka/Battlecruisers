using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // NEWUI  Update tests :)
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IInformatorPanel _informator;
        private readonly IBuildableDetails<IBuilding> _buildingDetails;
        private readonly IBuildableDetails<IUnit> _unitDetails;
        private readonly ICruiserDetails _cruiserDetails;

        public ItemDetailsManager(IInformatorPanel informator)
        {
            Helper.AssertIsNotNull(informator);

            _informator = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _informator.Hide();
        }

        public void ShowDetails(IBuilding building)
        {
            HideDetails();

            _informator.Show();
            _buildingDetails.ShowBuildableDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            HideDetails();

            _informator.Show();
            _unitDetails.ShowBuildableDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideDetails();

            _informator.Show();
            _cruiserDetails.ShowCruiserDetails(cruiser);
        }
		
        public void HideDetails()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
            _informator.Hide();
        }
    }
}
