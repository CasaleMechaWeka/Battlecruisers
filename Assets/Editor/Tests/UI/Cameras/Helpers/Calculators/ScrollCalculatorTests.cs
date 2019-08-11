using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class ScrollCalculatorTests
    {
        private IScrollCalculator _calculator;
        private ICamera _camera;
        private ITime _time;
        private IRange<float> _validOrthographicSizes;
        private ISettingsManager _settingsManager;
        private ILevelToMultiplierConverter _scrollConverter;
        private float _scrollDeltaMultiplier;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _time = Substitute.For<ITime>();
            _validOrthographicSizes = new Range<float>(5, 40);
            _settingsManager = Substitute.For<ISettingsManager>();
            _scrollConverter = Substitute.For<ILevelToMultiplierConverter>();

            _calculator = new ScrollCalculator(_camera, _time, _validOrthographicSizes, _settingsManager, _scrollConverter);

            _camera.OrthographicSize.Returns(20);
            _time.UnscaledDeltaTime.Returns(0.1f);
            _settingsManager.ScrollSpeedLevel.Returns(2);
            _scrollConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel).Returns(0.25f);

            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float directionMultiplier = -1;
            _scrollDeltaMultiplier
                = directionMultiplier *
                    orthographicProportion *
                    ScrollCalculator.SCROLL_SCALE *
                    _time.UnscaledDeltaTime *
                    _scrollConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }

        [Test]
        public void FindScrollDelta_NegativeMouseScrollDeltaX()
        {
            float mouseScrollDeltaX = -1;
            float expectedScrollDelta = mouseScrollDeltaX * _scrollDeltaMultiplier;

            Assert.AreEqual(expectedScrollDelta, _calculator.FindScrollDelta(mouseScrollDeltaX));
        }

        [Test]
        public void FindScrollDelta_PositiveMouseScrollDeltaX()
        {
            float mouseScrollDeltaX = 1;
            float expectedScrollDelta = mouseScrollDeltaX * _scrollDeltaMultiplier;

            Assert.AreEqual(expectedScrollDelta, _calculator.FindScrollDelta(mouseScrollDeltaX));
        }
    }
}