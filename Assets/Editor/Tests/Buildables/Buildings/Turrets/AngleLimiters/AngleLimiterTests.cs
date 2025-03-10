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
            float minAngleInDegrees = -45;
            _minNonNegativeAngle = minAngleInDegrees + 360;
            _maxAngleInDegrees = 45;

            _limiter = new AngleLimiter(minAngleInDegrees, _maxAngleInDegrees);
        }

        [Test]
        public void LimitAngle_TooSmallDesiredAngle_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _limiter.LimitAngle(-1));
        }

        [Test]
        public void LimitAngle_TooLargeDesiredAngle_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _limiter.LimitAngle(361));
        }

        [Test]
        public void LimitAngle_MinAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_minNonNegativeAngle);
            Assert.AreEqual(_minNonNegativeAngle, limitedAngle);
        }

        [Test]
        public void LimitAngle_MaxAngle_ReturnsSame()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }

        [Test]
        public void LimitAngle_AngleWithinLimit_CloseToMin_ReturnsSame()
        {
            float inLimitAngle = _minNonNegativeAngle + 1;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void LimitAngle_AngleWithinLimit_CloseToMax_ReturnsSame()
        {
            float inLimitAngle = _maxAngleInDegrees - 1;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void LimitAngle_AngleWithinLimit_JustPositive_ReturnsSame()
        {
            float inLimitAngle = 1;
            float limitedAngle = _limiter.LimitAngle(inLimitAngle);
            Assert.AreEqual(inLimitAngle, limitedAngle);
        }

        [Test]
        public void LimitAngle_TooSmallAngle_ReturnsMinAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_minNonNegativeAngle - 1);
            Assert.AreEqual(_minNonNegativeAngle, limitedAngle);
        }

        [Test]
        public void LimitAngle_TooLargeAngle_ReturnsMaxAngle()
        {
            float limitedAngle = _limiter.LimitAngle(_maxAngleInDegrees + 1);
            Assert.AreEqual(_maxAngleInDegrees, limitedAngle);
        }
    }
}
