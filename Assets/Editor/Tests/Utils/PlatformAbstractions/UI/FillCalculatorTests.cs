using BattleCruisers.Utils.PlatformAbstractions.UI;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.PlatformAbstractions.UI
{
    public class FillCalculatorTests
    {
        private IFillCalculator _calculator;
        private float _minProportion, _maxProportion;

        [SetUp]
        public void TestSetup()
        {
            _minProportion = 0.2f;
            _maxProportion = 0.7f;
            _calculator = new FillCalculator(_minProportion, _maxProportion);
        }

        [Test]
        public void RawToAdjusted()
        {
            float rawFillAmount = 0.25f;
            float expectedAdjustedFillAmount = rawFillAmount * (_maxProportion - _minProportion) + _minProportion;
            Assert.AreEqual(expectedAdjustedFillAmount, _calculator.RawToAdjusted(rawFillAmount));
        }

        [Test]
        public void AdjustedToRaw()
        {
            float adjustedFillAmount = 0.25f;
            float expectedRawFillAmount = (adjustedFillAmount - _minProportion) / (_maxProportion - _minProportion);
            Assert.AreEqual(expectedRawFillAmount, _calculator.AdjustedToRaw(adjustedFillAmount));
        }
    }
}