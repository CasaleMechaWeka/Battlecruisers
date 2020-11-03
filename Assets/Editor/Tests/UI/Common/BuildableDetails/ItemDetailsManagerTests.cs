
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using NSubstitute;
using NUnit.Framework;

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
        }

        [Test]
        public void ShowBuildingDetails()
        {
            _detailsManager.ShowDetails(_building);

            ReceivedHideEverything();
            _buildingDetails.Received().ShowBuildableDetails(_building);
            _informatorPanel.Received().Show();
            Assert.AreSame(_building, _detailsManager.SelectedItem.Value);
        }

        [Test]
        public void ShowUnitDetails()
        {
            _detailsManager.ShowDetails(_unit);

            ReceivedHideEverything();
            _unitDetails.Received().ShowBuildableDetails(_unit);
            _informatorPanel.Received().Show();
            Assert.AreSame(_unit, _detailsManager.SelectedItem.Value);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _detailsManager.ShowDetails(_cruiser);

            ReceivedHideEverything();
            _cruiserDetails.Received().ShowCruiserDetails(_cruiser);
            _informatorPanel.Received().Show();
            Assert.AreSame(_cruiser, _detailsManager.SelectedItem.Value);
        }

        [Test]
        public void Hide()
        {
            _detailsManager.HideDetails();

            ReceivedHideInformator();
            Assert.IsNull(_detailsManager.SelectedItem.Value);
        }

        private void ReceivedHideEverything()
        {
            ReceivedHideInformator();
            ReceivedHideInformatorContent();
        }

        private void ReceivedHideInformator()
        {
            _informatorPanel.Hide();
        }

        private void ReceivedHideInformatorContent()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
