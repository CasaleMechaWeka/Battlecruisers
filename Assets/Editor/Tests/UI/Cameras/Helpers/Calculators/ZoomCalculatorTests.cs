using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class ZoomCalculatorTests
    {
        private IZoomCalculator _calculator;
        private ICamera _camera;
        private ITime _time;
        private IRange<float> _validOrthographicSizes;
        private float _zoomDeltaMultiplier;
        private const float _zoomScale = 1.23f;
        private const float _zoomSettingsMultiplier = 3.21f;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _time = Substitute.For<ITime>();
            _validOrthographicSizes = new Range<float>(5, 40);

            _calculator = new ZoomCalculator(_camera, _time, _validOrthographicSizes, _zoomScale, _zoomSettingsMultiplier);

            _camera.OrthographicSize.Returns(20);
            _time.UnscaledDeltaTime.Returns(0.1f);

            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            _zoomDeltaMultiplier
                = orthographicProportion *
                    _zoomScale *
                    _time.UnscaledDeltaTime *
                    _zoomSettingsMultiplier;
        }

        [Test]
        public void FindZoomDelta_NegativeMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = -1;
            float expectedZoomDelta = Mathf.Abs(mouseScrollDeltaY) * _zoomDeltaMultiplier;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindMouseScrollOrthographicSizeDelta(mouseScrollDeltaY));
        }

        [Test]
        public void FindZoomDelta_PositiveMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = 1;
            float expectedZoomDelta = Mathf.Abs(mouseScrollDeltaY) * _zoomDeltaMultiplier;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindMouseScrollOrthographicSizeDelta(mouseScrollDeltaY));
        }
    }
}