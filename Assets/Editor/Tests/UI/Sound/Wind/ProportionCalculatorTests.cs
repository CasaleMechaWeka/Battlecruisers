using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.Wind
{
    public class ProportionCalculatorTests
    {
        private IProportionCalculator _calculator;
        private IRange<float> _validOrthographicSizes;

        [SetUp]
        public void TestSetup()
        {
            _validOrthographicSizes = new Range<float>(2, 10);
            _calculator = new ProportionCalculator();
        }

        [Test]
        public void FindProportion_ArgTooSmall()
        {
            float expectedVolume = 0;
            float actualVolume = _calculator.FindProportion(_validOrthographicSizes.Min - 0.1f, _validOrthographicSizes);
            Assert.AreEqual(expectedVolume, actualVolume);
        }

        [Test]
        public void FindProportion_ArgTooLarge()
        {
            float expectedVolume = 1;
            float actualVolume = _calculator.FindProportion(_validOrthographicSizes.Max + 0.1f, _validOrthographicSizes);
            Assert.AreEqual(expectedVolume, actualVolume);
        }

        [Test, Sequential]
        public void FindProportion(
            [Values(2, 6, 8, 10)] float cameraOrthographicSize,
            [Values(0, 0.5f, 0.75f, 1)] float expectedVolume)
        {
            float actualVolume = _calculator.FindProportion(cameraOrthographicSize, _validOrthographicSizes);
            Assert.AreEqual(expectedVolume, actualVolume);
        }
    }
}