using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class EdgeScrollCalculatorTests
    {
        private IEdgeScrollCalculator _calculator;
        private float _expectedDelta;

        [SetUp]
        public void TestSetup()
        {
            ICamera camera = Substitute.For<ICamera>();
            ITime time = Substitute.For<ITime>();
            IRange<float> validOrthographicSizes = new Range<float>(5, 40);
            ISettingsManager settingsManager = Substitute.For<ISettingsManager>();
            ILevelToMultiplierConverter scrollConverter = Substitute.For<ILevelToMultiplierConverter>();

            _calculator = new EdgeScrollCalculator(time, settingsManager, scrollConverter, camera, validOrthographicSizes);

            camera.OrthographicSize.Returns(20);
            time.UnscaledDeltaTime.Returns(0.1f);
            settingsManager.ScrollSpeedLevel.Returns(2);
            scrollConverter.LevelToMultiplier(settingsManager.ScrollSpeedLevel).Returns(0.25f);

            float orthographicProportion = camera.OrthographicSize / validOrthographicSizes.Max;
            _expectedDelta
                = EdgeScrollCalculator.SCROLL_SCALE *
                    time.UnscaledDeltaTime *
                    orthographicProportion *
                    scrollConverter.LevelToMultiplier(settingsManager.ScrollSpeedLevel);
        }

        [Test]
        public void FindCameraPositionDeltaMagnituteInM()
        {
            float actualDelta = _calculator.FindCameraPositionDeltaMagnituteInM();
            Assert.AreEqual(_expectedDelta, actualDelta);
        }
    }
}