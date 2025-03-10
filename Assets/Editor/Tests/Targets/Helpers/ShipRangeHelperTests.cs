using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Targets.Helpers
{
    public class ShipRangeHelperTests
    {
        private ITargetRangeHelper _rangeHelper;
        private IShip _ship;
        private ITarget _target;
        private float _perfectRangeInM;

		private const float IN_RANGE_LEEWAY_IN_M = 0.01f;

        [SetUp]
        public void SetuUp()
        {
            _ship = Substitute.For<IShip>();
            _ship.Position.Returns(new Vector2(0, 0));

            _target = Substitute.For<ITarget>();
            _target.Position.Returns(new Vector2(3, 4));
            _target.Size.Returns(new Vector2(2, 1));

            _rangeHelper = new ShipRangeHelper(_ship);

            _perfectRangeInM = Vector2.Distance(_ship.Position, _target.Position) - (_target.Size.x / 2) - IN_RANGE_LEEWAY_IN_M;
        }

        [Test]
        public void JustInRange()
        {
            _ship.OptimalArmamentRangeInM.Returns(_perfectRangeInM);
            Assert.IsTrue(_rangeHelper.IsTargetInRange(_target));
        }

        [Test]
        public void JustOutOfRange()
        {
            _ship.OptimalArmamentRangeInM.Returns(_perfectRangeInM - 0.001f);
            Assert.IsFalse(_rangeHelper.IsTargetInRange(_target));
        }
    }
}
