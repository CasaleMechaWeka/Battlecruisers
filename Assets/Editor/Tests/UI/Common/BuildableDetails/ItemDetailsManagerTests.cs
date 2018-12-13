using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails
{
    public class ItemDetailsManagerTests
    {
        private IItemDetailsManager _detailsManager;
        private IInformatorPanel _informatorPanel;
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

            _informatorPanel = Substitute.For<IInformatorPanel>();
            _informatorPanel.BuildingDetails.Returns(_buildingDetails);
            _informatorPanel.UnitDetails.Returns(_unitDetails);
            _informatorPanel.CruiserDetails.Returns(_cruiserDetails);

            _detailsManager = new ItemDetailsManager(_informatorPanel);

            _building = Substitute.For<IBuilding>();
            _unit = Substitute.For<IUnit>();
            _cruiser = Substitute.For<ICruiser>();

            _informatorPanel.Received().Hide();   
        }

        [Test]
        public void ShowBuildingDetails()
        {
            _detailsManager.ShowDetails(_building);

            AllDetails_ReceivedHide();
            _buildingDetails.Received().ShowBuildableDetails(_building);
        }

        [Test]
        public void ShowUnitDetails()
        {
            _detailsManager.ShowDetails(_unit);

            AllDetails_ReceivedHide();
            _unitDetails.Received().ShowBuildableDetails(_unit);
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

        private void AllDetails_ReceivedHide()
        {
            _buildingDetails.Received().Hide();
            _unitDetails.Received().Hide();
            _cruiserDetails.Received().Hide();
            _informatorPanel.Received().Hide();
        }
    }
}
