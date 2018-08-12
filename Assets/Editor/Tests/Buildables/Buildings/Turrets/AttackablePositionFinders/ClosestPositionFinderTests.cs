using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class ClosestPositionFinderTests
    {
        private IAttackablePositionFinder _positionFinder;
        private float _cutoffWidthInM, _bufferInM;
        private ITarget _target;
        private Vector2 _sourcePosition;

        [SetUp]
        public void TestSetup()
        {
            _cutoffWidthInM = 5;
            _bufferInM = 0.5f;

            _positionFinder = new ClosestPositionFinder(_cutoffWidthInM, _bufferInM);

            _sourcePosition = new Vector2(0, 0);

            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void FindClosestAttackablePosition_TargetTooSmall_ReturnsTargetCenter()
        {
            _target.Size.Returns(new Vector2(_cutoffWidthInM, 121));
            _target.Position.Returns(new Vector2(17, 71));

            Assert.AreEqual(_target.Position, _positionFinder.FindClosestAttackablePosition(_sourcePosition, _target));
        }

        [Test]
        public void FindClosestAttackablePosition_TargetBigEnough_TargetToRight()
        {
            _target.Size.Returns(new Vector2(_cutoffWidthInM + 1, 121));
            _target.Position.Returns(new Vector2(10, 71));

            float expectedXPosition = _target.Position.x - (_target.Size.x / 2) + _bufferInM;
            Vector2 expectedPosition = new Vector2(expectedXPosition, _target.Position.y);

            Assert.AreEqual(expectedPosition, _positionFinder.FindClosestAttackablePosition(_sourcePosition, _target));
        }

        [Test]
        public void FindClosestAttackablePosition_TargetBigEnough_TargetToLeft()
        {
            _target.Size.Returns(new Vector2(_cutoffWidthInM + 1, 121));
            _target.Position.Returns(new Vector2(-10, 71));

            float expectedXPosition = _target.Position.x + (_target.Size.x / 2) - _bufferInM;
            Vector2 expectedPosition = new Vector2(expectedXPosition, _target.Position.y);

            Assert.AreEqual(expectedPosition, _positionFinder.FindClosestAttackablePosition(_sourcePosition, _target));
        }
    }
}