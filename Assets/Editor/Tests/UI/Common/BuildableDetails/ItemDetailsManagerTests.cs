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
        private IComparableItemDetails<IBuilding> _buildingDetails;
        private IComparableItemDetails<IUnit> _unitDetails;
        private IComparableItemDetails<ICruiser> _cruiserDetails;
        private IBuilding _building;
        private IUnit _unit;
        private ICruiser _cruiser;

        [SetUp]
        public void SetuUp()
        {
            _buildingDetails = Substitute.For<IComparableItemDetails<IBuilding>>();
            _unitDetails = Substitute.For<IComparableItemDetails<IUnit>>();
            _cruiserDetails = Substitute.For<IComparableItemDetails<ICruiser>>();

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
            _buildingDetails.Received().ShowItemDetails(_building);
            _informatorPanel.Received().Show(_building);
            Assert.AreSame(_building, _detailsManager.SelectedItem.Value);
        }

        [Test]
        public void ShowUnitDetails()
        {
            _detailsManager.ShowDetails(_unit);

            ReceivedHideEverything();
            _unitDetails.Received().ShowItemDetails(_unit);
            _informatorPanel.Received().Show(_unit);
            Assert.AreSame(_unit, _detailsManager.SelectedItem.Value);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _detailsManager.ShowDetails(_cruiser);

            ReceivedHideEverything();
            _cruiserDetails.Received().ShowItemDetails(_cruiser);
            _informatorPanel.Received().Show(_cruiser);
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
