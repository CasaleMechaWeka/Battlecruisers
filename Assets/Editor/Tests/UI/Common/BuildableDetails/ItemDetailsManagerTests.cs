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

            IInformatorPanel informatorPanel = Substitute.For<IInformatorPanel>();
            informatorPanel.BuildingDetails.Returns(_buildingDetails);
            informatorPanel.UnitDetails.Returns(_unitDetails);
            informatorPanel.CruiserDetails.Returns(_cruiserDetails);

            _detailsManager = new ItemDetailsManager(informatorPanel);

            _building = Substitute.For<IBuilding>();
            _unit = Substitute.For<IUnit>();
            _cruiser = Substitute.For<ICruiser>();
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
        }
    }
}
