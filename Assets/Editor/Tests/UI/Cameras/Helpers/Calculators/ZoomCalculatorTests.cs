using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class ZoomCalculatorTests
    {
        private IZoomCalculator _calculator;
        private ICamera _camera;
        private IDeltaTimeProvider _deltaTimeProvider;
        private IRange<float> _validOrthographicSizes;
        private ISettingsManager _settingsManager;
        private IZoomConverter _zoomConverter;
        private float _zoomDeltaMultiplier;
        private const float _zoomScale = 1.23f;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _deltaTimeProvider = Substitute.For<IDeltaTimeProvider>();
            _validOrthographicSizes = new Range<float>(5, 40);
            _settingsManager = Substitute.For<ISettingsManager>();
            _zoomConverter = Substitute.For<IZoomConverter>();

            _calculator = new ZoomCalculator(_camera, _deltaTimeProvider, _validOrthographicSizes, _settingsManager, _zoomConverter, _zoomScale);

            _camera.OrthographicSize.Returns(20);
            _deltaTimeProvider.UnscaledDeltaTime.Returns(0.1f);
            _settingsManager.ZoomSpeedLevel.Returns(2);
            _zoomConverter.LevelToSpeed(_settingsManager.ZoomSpeedLevel).Returns(0.25f);

            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            _zoomDeltaMultiplier
                = orthographicProportion *
                    _zoomScale *
                    _deltaTimeProvider.UnscaledDeltaTime *
                    _zoomConverter.LevelToSpeed(_settingsManager.ZoomSpeedLevel);
        }

        [Test]
        public void FindZoomDelta_NegativeMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = -1;
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float expectedZoomDelta = Mathf.Abs(mouseScrollDeltaY) * _zoomDeltaMultiplier;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindZoomDelta(mouseScrollDeltaY));
        }

        [Test]
        public void FindZoomDelta_PositiveMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = 1;
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float expectedZoomDelta = Mathf.Abs(mouseScrollDeltaY) * _zoomDeltaMultiplier;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindZoomDelta(mouseScrollDeltaY));
        }
    }
}