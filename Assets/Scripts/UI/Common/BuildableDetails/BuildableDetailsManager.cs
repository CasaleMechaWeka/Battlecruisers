using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildableDetailsManager : IBuildableDetailsManager
    {
        private readonly IBuildableDetails<IBuilding> _buildingDetails;
        private readonly IBuildableDetails<IUnit> _unitDetails;
        private readonly ICruiserDetails _cruiserDetails;

        public BuildableDetailsManager(IHUDCanvasController hudCanvas)
        {
            Helper.AssertIsNotNull(hudCanvas);

            _buildingDetails = hudCanvas.BuildingDetails;
            _unitDetails = hudCanvas.UnitDetails;
            _cruiserDetails = hudCanvas.CruiserDetails;
        }

        public void ShowDetails(IBuilding building, bool allowDelete)
        {
            HideDetails();
            _buildingDetails.ShowBuildableDetails(building, allowDelete);
        }

        public void ShowDetails(IUnit unit)
        {
            HideDetails();
            _unitDetails.ShowBuildableDetails(unit, allowDelete: false);
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
