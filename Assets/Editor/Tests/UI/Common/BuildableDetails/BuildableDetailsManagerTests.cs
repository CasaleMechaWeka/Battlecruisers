using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Common.BuildingDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails
{
    public class BuildableDetailsManagerTests
    {
        private IBuildableDetailsManager _detailsManager;
        private IBuildableDetails<IBuilding> _buildingDetails;
        private IBuildableDetails<IUnit> _unitDetails;
        private ICruiserDetails _cruiserDetails;
        private IBuilding _building;
        private IUnit _unit;
        private ICruiser _cruiser;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _buildingDetails = Substitute.For<IBuildableDetails<IBuilding>>();
            _unitDetails = Substitute.For<IBuildableDetails<IUnit>>();
            _cruiserDetails = Substitute.For<ICruiserDetails>();

            IHUDCanvasController hudCanvas = Substitute.For<IHUDCanvasController>();
            hudCanvas.BuildingDetails.Returns(_buildingDetails);
            hudCanvas.UnitDetails.Returns(_unitDetails);
            hudCanvas.CruiserDetails.Returns(_cruiserDetails);

            _detailsManager = new BuildableDetailsManager(hudCanvas);

            _building = Substitute.For<IBuilding>();
            _unit = Substitute.For<IUnit>();
            _cruiser = Substitute.For<ICruiser>();
        }

        [Test]
        public void ShowBuildingDetails_AllowDelete()
        {
            ShowBuildingDetails(allowDelete: true);
        }

        [Test]
        public void ShowBuildingDetails_DoNotAllowDelete()
        {
            ShowBuildingDetails(allowDelete: false);
        }

        [Test]
        public void ShowUnitDetails()
        {
            _detailsManager.ShowDetails(_unit);

            AllDetails_ReceivedHide();
            _unitDetails.Received().ShowBuildableDetails(_unit, allowDelete: false);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _detailsManager.ShowDetails(_cruiser);

            AllDetails_ReceivedHide();
            _cruiserDetails.Received().ShowCruiserDetails(_cruiser);
        }

        [Test]
        public void Hide()
        {
            _detailsManager.HideDetails();
            AllDetails_ReceivedHide();
        }

        private void ShowBuildingDetails(bool allowDelete)
        {
            _detailsManager.ShowDetails(_building, allowDelete);

            AllDetails_ReceivedHide();
            _buildingDetails.Received().ShowBuildableDetails(_building, allowDelete);
        }

        private void AllDetails_ReceivedHide()
        {
            _buildingDetails.Received().Hide();
            _unitDetails.Received().Hide();
            _cruiserDetails.Received().Hide();            
        }
    }
}
