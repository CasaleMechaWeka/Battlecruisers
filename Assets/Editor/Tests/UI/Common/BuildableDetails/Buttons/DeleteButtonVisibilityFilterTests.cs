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

        [Test, Sequential]
        public void IsMatch(
            [Values(Faction.Reds, Faction.Blues, Faction.Blues, Faction.Blues)] Faction faction,
            [Values(TargetType.Buildings, TargetType.Ships, TargetType.Buildings, TargetType.Buildings)] TargetType targetType,
            [Values(true, true, false, true)] bool isInScene,
            [Values(false, false, false, true)] bool expectedResult)
        {
            _target.Faction.Returns(faction);
            _target.TargetType.Returns(targetType);
            _target.IsInScene.Returns(isInScene);

            Assert.AreEqual(expectedResult, _filter.IsMatch(_target));
        }
    }
}