using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails.Buttons
{
    public class PlayerCruiserBuildingFilterTests
    {
        private IFilter<IBuilding> _filter;
        private IBuilding _building;
        private ICruiser _playerCruiser, _aiCruiser;

        [SetUp]
        public void SetuUp()
        {
            _playerCruiser = Substitute.For<ICruiser>();
            _aiCruiser = Substitute.For<ICruiser>();
            _building = Substitute.For<IBuilding>();

            _filter = new PlayerCruiserBuildingFilter(_playerCruiser);
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _building.ParentCruiser.Returns(_playerCruiser);
            Assert.IsTrue(_filter.IsMatch(_building));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            _building.ParentCruiser.Returns(_aiCruiser);
            Assert.IsFalse(_filter.IsMatch(_building));
        }
    }
}
