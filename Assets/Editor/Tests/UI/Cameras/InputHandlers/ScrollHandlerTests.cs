using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils.Clamper;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.InputHandlers
{
    public class ScrollHandlerTests
	{
		private IScrollHandler _scrollHandler;

		private ICameraCalculator _calculator;
        private IScreen _screen;
        private IPositionClamper _clamper;
        private IDeltaTimeProvider _deltaTimeProvider;

		private float _orthographicSize = 71;
		private float _scrollSpeed = 17;
		private Vector3 _cameraPosition;

		private float _nonScrollingValue;

		private const float SCROLL_BOUNDARY_IN_PIXELS = 2;
        
		[SetUp]
		public void SetuUp()
		{
			_calculator = Substitute.For<ICameraCalculator>();
			_screen = Substitute.For<IScreen>();
			_clamper = Substitute.For<IPositionClamper>();
            _deltaTimeProvider = Substitute.For<IDeltaTimeProvider>();

			_scrollHandler = new ScrollHandler(_calculator, _screen, _clamper, _deltaTimeProvider);

			_screen.Width.Returns(1920);
			_screen.Height.Returns(1200);

            _deltaTimeProvider.UnscaledDeltaTime.Returns(0.171f);

            _calculator.FindScrollSpeed(_orthographicSize, _deltaTimeProvider.UnscaledDeltaTime).Returns(_scrollSpeed);

            // Simply return the parameter, ie, don't clamp :P
            _clamper.Clamp(default(Vector3)).ReturnsForAnyArgs(x => x.Arg<Vector3>());


			_cameraPosition = new Vector3(0, 0, -10);

			_nonScrollingValue = SCROLL_BOUNDARY_IN_PIXELS + 1;
		}

        [Test]
        public void FindCameraPosition_NoScroll()
        {
			Vector3 mousePosition = new Vector3(_nonScrollingValue, _nonScrollingValue, 1);

			Assert.AreEqual(_cameraPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition));

            _calculator.Received().FindScrollSpeed(_orthographicSize, _deltaTimeProvider.UnscaledDeltaTime);
			_clamper.Received().Clamp(_cameraPosition);
        }

		[Test]
		public void FindCameraPosition_XScroll_Negative()
		{
			Vector3 mousePosition = new Vector3(SCROLL_BOUNDARY_IN_PIXELS, _nonScrollingValue);
			float expectedX = _cameraPosition.x - _scrollSpeed;
			Vector3 expectedPosition = new Vector3(expectedX, _cameraPosition.y, _cameraPosition.z);

			Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition));
		}

        [Test]
        public void FindCameraPosition_XScroll_Positive()
        {
			Vector3 mousePosition = new Vector3(_screen.Width - SCROLL_BOUNDARY_IN_PIXELS, _nonScrollingValue);
            float expectedX = _cameraPosition.x + _scrollSpeed;
            Vector3 expectedPosition = new Vector3(expectedX, _cameraPosition.y, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition));
        }

        [Test]
        public void FindCameraPosition_YScroll_Negative()
        {
			Vector3 mousePosition = new Vector3(_nonScrollingValue, SCROLL_BOUNDARY_IN_PIXELS);
			float expectedY = _cameraPosition.y - _scrollSpeed;
			Vector3 expectedPosition = new Vector3(_cameraPosition.x, expectedY, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition));
        }

        [Test]
        public void FindCameraPosition_YScroll_Positive()
        {
			Vector3 mousePosition = new Vector3(_nonScrollingValue, _screen.Height - SCROLL_BOUNDARY_IN_PIXELS);
            float expectedY = _cameraPosition.y + _scrollSpeed;
            Vector3 expectedPosition = new Vector3(_cameraPosition.x, expectedY, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition));
        }
	}
}
