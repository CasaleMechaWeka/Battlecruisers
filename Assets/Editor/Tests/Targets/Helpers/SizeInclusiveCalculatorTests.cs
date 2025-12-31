using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Targets.Helpers
{
    public class SizeInclusiveCalculatorTests
    {
        private IRangeCalculator _calculator;
        private ITransform _parentTransform;
        private ITarget _target;
        private float _rangeInM, _adjustedRangeEdge;

        [SetUp]
        public void TestSetup()
        {
            _calculator = new SizeInclusiveCalculator();

            _parentTransform = Substitute.For<ITransform>();
            _parentTransform.Position.Returns(new Vector3(0, 0, 0));

            _target = Substitute.For<ITarget>();
            _target.Size.Returns(new Vector2(2, 1));

            _rangeInM = 4.5f;
            _adjustedRangeEdge = _rangeInM + _target.Size.x / 2;
        }

        [Test]
        public void IsInRange_TargetToRight_InRange()
        {
            _target.Position.Returns(new Vector2(_adjustedRangeEdge, 0));
            Assert.IsTrue(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }

        [Test]
        public void IsInRange_TargetToRight_OutOfRange()
        {
            _target.Position.Returns(new Vector2(_adjustedRangeEdge + 0.01f, 0));
            Assert.IsFalse(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }

        [Test]
        public void IsInRange_TargetToLeft_InRange()
        {
            _target.Position.Returns(new Vector2(-_adjustedRangeEdge, 0));
            Assert.IsTrue(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }

        [Test]
        public void IsInRange_TargetToLeft_OutOfRange()
        {
            _target.Position.Returns(new Vector2(-_adjustedRangeEdge - 0.01f, 0));
            Assert.IsFalse(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }
    }
}