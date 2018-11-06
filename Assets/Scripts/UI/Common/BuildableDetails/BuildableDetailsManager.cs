using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildableDetailsManager : IBuildableDetailsManager
    {
        private readonly IBuildableDetails<IBuilding> _buildingDetails;
        private readonly IBuildableDetails<IUnit> _unitDetails;
        private readonly ICruiserDetails _cruiserDetails;

        public BuildableDetailsManager(IInformatorPanel informator)
        {
            Helper.AssertIsNotNull(informator);

            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;
        }

        public void ShowDetails(IBuilding building)
        {
            HideDetails();
            _buildingDetails.ShowBuildableDetails(building);
        }

        public void ShowDetails(IUnit unit)
        {
            HideDetails();
            _unitDetails.ShowBuildableDetails(unit);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideDetails();
            _cruiserDetails.ShowCruiserDetails(cruiser);
        }
		
        public void HideDetails()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
