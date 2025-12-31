using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonVisibilityFilterTests
    {
        private IFilter<ITarget> _filter;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _filter = new ChooseTargetButtonVisibilityFilter();
            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void IsMatch_Null_ReturnsFalse()
        {
            _target = null;
            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_PlayerFaction_ReturnsFalse()
        {
            _target.Faction.Returns(Faction.Blues);
            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_AIFaction_TargetIsAircraft_ReturnsFalse()
        {
            _target.Faction.Returns(Faction.Reds);
            _target.TargetType.Returns(TargetType.Aircraft);

            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_AIFaction_TargetIsBuilding_ReturnsTrue()
        {
            _target.Faction.Returns(Faction.Reds);
            _target.TargetType.Returns(TargetType.Buildings);

            Assert.IsTrue(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_AIFaction_TargetIsCruiser_ReturnsTrue()
        {
            _target.Faction.Returns(Faction.Reds);
            _target.TargetType.Returns(TargetType.Cruiser);

            Assert.IsTrue(_filter.IsMatch(_target));
        }
    }
}