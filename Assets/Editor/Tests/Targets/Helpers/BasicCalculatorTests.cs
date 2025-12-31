using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Targets.Helpers
{
    public class BasicCalculatorTests
    {
        private IRangeCalculator _calculator;
        private ITransform _parentTransform;
        private ITarget _target;
        private float _rangeInM = 7.23f;

        [SetUp]
        public void TestSetup()
        {
            _calculator = new BasicCalculator();

            _parentTransform = Substitute.For<ITransform>();
            _parentTransform.Position.Returns(new Vector3(0, 0, 0));

            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void IsInRange_True()
        {
            _target.Position.Returns(new Vector2(-_rangeInM, 0));
            Assert.IsTrue(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }

        [Test]
        public void IsInRange_False()
        {
            _target.Position.Returns(new Vector2(-_rangeInM - 0.01f, 0));
            Assert.IsTrue(_calculator.IsInRange(_parentTransform, _target, _rangeInM));
        }
    }
}