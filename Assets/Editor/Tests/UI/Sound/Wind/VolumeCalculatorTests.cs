using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.Wind
{
    public class VolumeCalculatorTests
    {
        private IVolumeCalculator _calculator;
        private IProportionCalculator _proportionCalculator;
        private IRange<float> _validOrthographicSizes;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _proportionCalculator = Substitute.For<IProportionCalculator>();
            _validOrthographicSizes = new Range<float>(2, 10);
            _settingsManager = Substitute.For<ISettingsManager>();
            _calculator = new VolumeCalculator(_proportionCalculator, _validOrthographicSizes, _settingsManager);
        }

        [Test]
        public void FindVolume()
        {
            float cameraOrthographicSize = 3.2f;
            float proportion = 0.12f;
            _proportionCalculator
                .FindProportion(cameraOrthographicSize, _validOrthographicSizes)
                .Returns(proportion);
            _settingsManager.EffectVolume.Returns(0.75f);

            float actualVolume = _calculator.FindVolume(cameraOrthographicSize);

            float expectedVolume = proportion * _settingsManager.EffectVolume;
            Assert.AreEqual(expectedVolume, actualVolume);
        }
    }
}