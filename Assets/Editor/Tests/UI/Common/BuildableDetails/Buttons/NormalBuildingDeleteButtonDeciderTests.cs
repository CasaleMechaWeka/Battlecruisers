using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails.Buttons
{
    public class NormalBuildingDeleteButtonDeciderTests
    {
        private IActivenessDecider<IBuilding> _decider;
        private IBuilding _building;
        private ICruiser _playerCruiser, _aiCruiser;

        [SetUp]
        public void SetuUp()
        {
            _playerCruiser = Substitute.For<ICruiser>();
            _aiCruiser = Substitute.For<ICruiser>();
            _building = Substitute.For<IBuilding>();

            _decider = new NormalBuildingDeleteButtonDecider(_playerCruiser);
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _building.ParentCruiser.Returns(_playerCruiser);
            Assert.IsTrue(_decider.ShouldBeEnabled(_building));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            _building.ParentCruiser.Returns(_aiCruiser);
            Assert.IsFalse(_decider.ShouldBeEnabled(_building));
        }
    }
}
