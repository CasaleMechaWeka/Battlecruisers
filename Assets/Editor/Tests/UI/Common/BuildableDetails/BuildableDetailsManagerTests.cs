using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails
{
    public class BuildableDetailsManagerTests
    {
        private IBuildableDetailsManager _detailsManager;
        private IBuildableDetails _buildableDetails;
        private IInBattleCruiserDetails _cruiserDetails;
        private IBuilding _building;
        private IUnit _unit;
        private ICruiser _cruiser;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _buildableDetails = Substitute.For<IBuildableDetails>();
            _cruiserDetails = Substitute.For<IInBattleCruiserDetails>();

            _detailsManager = new BuildableDetailsManager(_buildableDetails, _cruiserDetails);

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

            _cruiserDetails.Received().Hide();
            _buildableDetails.Received().ShowBuildableDetails(_unit, allowDelete: false);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _detailsManager.ShowDetails(_cruiser);

            _buildableDetails.Received().Hide();
            _cruiserDetails.Received().ShowCruiserDetails(_cruiser);
        }

        [Test]
        public void Hide()
        {
            _detailsManager.HideDetails();

            _buildableDetails.Received().Hide();
            _cruiserDetails.Received().Hide();
        }

        private void ShowBuildingDetails(bool allowDelete)
        {
            _detailsManager.ShowDetails(_building, allowDelete);

            _cruiserDetails.Received().Hide();
            _buildableDetails.Received().ShowBuildableDetails(_building, allowDelete);
        }
    }
}
