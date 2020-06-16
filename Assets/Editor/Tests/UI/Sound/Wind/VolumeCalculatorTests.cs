using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.Wind
{
    public class VolumeCalculatorTests
    {
        private IVolumeCalculator _calculator;
        private IRange<float> _validOrthographicSizes;

        [SetUp]
        public void TestSetup()
        {
            _validOrthographicSizes = new Range<float>(2, 10);
            _calculator = new VolumeCalculator(_validOrthographicSizes);
        }

        [Test]
        public void FindVolume_ArgTooSmall()
        {
            float expectedVolume = 0;
            float actualVolume = _calculator.FindVolume(_validOrthographicSizes.Min - 0.1f);
            Assert.AreEqual(expectedVolume, actualVolume);
        }

        [Test]
        public void FindVolume_ArgTooLarge()
        {
            float expectedVolume = 1;
            float actualVolume = _calculator.FindVolume(_validOrthographicSizes.Max + 0.1f);
            Assert.AreEqual(expectedVolume, actualVolume);
        }

        [Test, Sequential]
        public void FindVolume(
            [Values(2, 5, 7.5f, 10)] float cameraOrthographicSize,
            [Values(0, 0, 0.5f, 1)] float expectedVolume)
        {
            float actualVolume = _calculator.FindVolume(cameraOrthographicSize);
            Assert.AreEqual(expectedVolume, actualVolume);
        }
    }
}