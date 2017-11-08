using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Turrets.AngleLimiters
{
    public class AngleLimiterTests
    {
        private IAngleLimiter _limiter;
        private float _minAngleInDegrees, _maxAngleInDegrees;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _minAngleInDegrees = 12;
            _maxAngleInDegrees = 90;

            _limiter = new AngleLimiter(_minAngleInDegrees, _maxAngleInDegrees);
        }

        [Test]
        public void MinAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_minAngleInDegrees);
            Assert.AreEqual(_minAngleInDegrees, limitedAngle);
        }

        [Test]
        public void MaxAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }

        [Test]
        public void AngleWithinLimi_ReturnsSame()
        {
            float inLimitAngle = _minAngleInDegrees + 3;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void TooSmallAngle_ReturnsMinAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_minAngleInDegrees - 1);
            Assert.AreEqual(_minAngleInDegrees, limitedAngle);
        }

        [Test]
        public void TooLargeAngle_ReturnsMaxAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees + 1);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }
    }
}
