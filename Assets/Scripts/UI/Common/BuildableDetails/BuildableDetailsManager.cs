using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildableDetailsManager : IBuildableDetailsManager
    {
        private readonly IBuildableDetails _buildableDetails;
        private readonly IInBattleCruiserDetails _cruiserDetails;

        public BuildableDetailsManager(IBuildableDetails buildableDetails, IInBattleCruiserDetails cruiserDetails)
        {
            Helper.AssertIsNotNull(buildableDetails, cruiserDetails);

            _buildableDetails = buildableDetails;
            _cruiserDetails = cruiserDetails;
        }

        public void ShowDetails(IBuilding building, bool allowDelete)
        {
            _cruiserDetails.Hide();
            _buildableDetails.ShowBuildableDetails(building, allowDelete);
        }

        public void ShowDetails(IUnit unit)
        {
            _cruiserDetails.Hide();
            _buildableDetails.ShowBuildableDetails(unit, allowDelete: false);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            _buildableDetails.Hide();
            _cruiserDetails.ShowCruiserDetails(cruiser);
        }
		
        public void HideDetails()
        {
            _buildableDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
