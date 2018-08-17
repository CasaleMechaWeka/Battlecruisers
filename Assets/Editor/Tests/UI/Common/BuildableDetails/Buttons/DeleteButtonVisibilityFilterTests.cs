using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonVisibilityFilterTests
    {
        private IFilter<ITarget> _filter;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _filter = new DeleteButtonVisibilityFilter();
            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void IsMatch_Null_ReturnsFalse()
        {
            _target = null;
            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_AIFaction_ReturnsFalse()
        {
            _target.Faction.Returns(Faction.Reds);
            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_PlayerFaction_TargetIsNotBuilding_ReturnsFalse()
        {
            _target.Faction.Returns(Faction.Blues);
            _target.TargetType.Returns(TargetType.Aircraft);

            Assert.IsFalse(_filter.IsMatch(_target));
        }

        [Test]
        public void IsMatch_NonNull_PlayerFaction_TargetIsBuilding_ReturnsTrue()
        {
            _target.Faction.Returns(Faction.Blues);
            _target.TargetType.Returns(TargetType.Buildings);

            Assert.IsTrue(_filter.IsMatch(_target));
        }
    }
}