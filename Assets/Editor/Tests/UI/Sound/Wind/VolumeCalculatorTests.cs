using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.Wind
{
    public class VolumeCalculatorTests
    {
        private VolumeCalculator _calculator;
        private IRange<float> _validOrthographicSizes;
        private SettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _validOrthographicSizes = new Range<float>(2, 10);
            _settingsManager = Substitute.For<SettingsManager>();
            _calculator = new VolumeCalculator(_validOrthographicSizes, _settingsManager);
        }

        [Test]
        public void FindVolume()
        {
            float cameraOrthographicSize = 3.2f;
            float proportion = 0.12f;
            _settingsManager.EffectVolume.Returns(0.75f);

            float actualVolume = _calculator.FindVolume(cameraOrthographicSize);

            float expectedVolume = proportion * _settingsManager.EffectVolume;
            Assert.AreEqual(expectedVolume, actualVolume);
        }
    }
}