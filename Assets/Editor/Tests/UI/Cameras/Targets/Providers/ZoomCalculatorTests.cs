using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class ZoomCalculatorTests
    {
        private IZoomCalculator _calculator;
        private ICamera _camera;
        private IDeltaTimeProvider _deltaTimeProvider;
        private IRange<float> _validOrthographicSizes;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _deltaTimeProvider = Substitute.For<IDeltaTimeProvider>();
            _validOrthographicSizes = new Range<float>(5, 40);
            _settingsManager = Substitute.For<ISettingsManager>();

            _calculator = new ZoomCalculator(_camera, _deltaTimeProvider, _validOrthographicSizes, _settingsManager);

            _camera.OrthographicSize.Returns(20);
            _deltaTimeProvider.UnscaledDeltaTime.Returns(0.1f);
            _settingsManager.ZoomSpeed.Returns(2);
        }

        [Test]
        public void FindZoomDelta_NegativeMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = -1;
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float expectedZoomDelta 
                = Mathf.Abs(mouseScrollDeltaY) * 
                    orthographicProportion * 
                    ZoomCalculator.ZOOM_SCALE * 
                    _deltaTimeProvider.UnscaledDeltaTime *
                    _settingsManager.ZoomSpeed;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindZoomDelta(mouseScrollDeltaY));
        }

        [Test]
        public void FindZoomDelta_PositiveMouseScrollDeltaY()
        {
            float mouseScrollDeltaY = 1;
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float expectedZoomDelta 
                = Mathf.Abs(mouseScrollDeltaY) * 
                    orthographicProportion * 
                    ZoomCalculator.ZOOM_SCALE * 
                    _deltaTimeProvider.UnscaledDeltaTime *
                    _settingsManager.ZoomSpeed;

            Assert.AreEqual(expectedZoomDelta, _calculator.FindZoomDelta(mouseScrollDeltaY));
        }
    }
}