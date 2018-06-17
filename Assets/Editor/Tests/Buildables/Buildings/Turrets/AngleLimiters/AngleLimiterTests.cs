using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiterTests
    {
        private IAngleLimiter _limiter;
        private float _minNonNegativeAngle, _maxAngleInDegrees;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            float minAngleInDegrees = -45;
            _minNonNegativeAngle = minAngleInDegrees + 360;
            _maxAngleInDegrees = 45;

            _limiter = new AngleLimiter(minAngleInDegrees, _maxAngleInDegrees);
        }

        [Test]
        public void MinAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_minNonNegativeAngle);
            Assert.AreEqual(_minNonNegativeAngle, limitedAngle);
        }

        [Test]
        public void MaxAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }

        [Test]
        public void AngleWithinLimit_CloseToMin_ReturnsSame()
        {
            float inLimitAngle = _minNonNegativeAngle + 1;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void AngleWithinLimit_CloseToMax_ReturnsSame()
        {
            float inLimitAngle = _maxAngleInDegrees - 1;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void TooSmallAngle_ReturnsMinAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_minNonNegativeAngle - 1);
            Assert.AreEqual(_minNonNegativeAngle, limitedAngle);
        }

        [Test]
        public void TooLargeAngle_ReturnsMaxAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees + 1);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }
    }
}
