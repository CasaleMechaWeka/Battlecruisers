using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.InputHandlers
{
	public class MouseZoomHandlerTests
    {
		private IMouseZoomHandler _zoomHandler;
		private ISettingsManager _settingsManager;
        private IDeltaTimeProvider _deltaTimeProvider;

        [SetUp]
        public void SetuUp()
        {
			_settingsManager = Substitute.For<ISettingsManager>();
			_settingsManager.ZoomSpeed.Returns(0.5f);

            _deltaTimeProvider = Substitute.For<IDeltaTimeProvider>();
            // * ZOOM_SPEED_MULTIPLIER = 1 :)
            _deltaTimeProvider.UnscaledDeltaTime.Returns(0.03333333f);

            _zoomHandler = new MouseZoomHandler(_settingsManager, _deltaTimeProvider, CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE, CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE);
        }

        [Test]
		public void FindCameraOrthographicSize_NonZeroScroll()
        {
			float currentSize = 10;
			float yScroll = -5;
			float expectedSize = currentSize - _settingsManager.ZoomSpeed * yScroll;

			Assert.AreEqual(expectedSize, _zoomHandler.FindCameraOrthographicSize(currentSize, yScroll));
        }

        [Test]
        public void FindCameraOrthographicSize_NonZeroScroll_TooBigOrthographicSize_Clamped()
        {
			float currentSize = CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE;
			float yScroll = -5;
            
			Assert.AreEqual(CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE, _zoomHandler.FindCameraOrthographicSize(currentSize, yScroll));
        }

        [Test]
        public void FindCameraOrthographicSize_NonZeroScroll_TooSmallOrthographicSize_Clamped()
        {
			float currentSize = CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE;
            float yScroll = 5;

            Assert.AreEqual(CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE, _zoomHandler.FindCameraOrthographicSize(currentSize, yScroll));
        }

        [Test]
        public void FindCameraOrthographicSize_ZeroScroll()
        {
			float currentSize = 10;
            float yScroll = 0;

            Assert.AreEqual(currentSize, _zoomHandler.FindCameraOrthographicSize(currentSize, yScroll));
        }
    }
}
