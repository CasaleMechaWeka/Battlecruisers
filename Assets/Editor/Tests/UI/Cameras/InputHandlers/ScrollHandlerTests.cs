using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
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

		private float _orthographicSize = 71;
		private float _scrollSpeed = 17;
		private float _timeDelta = 171;
		private Vector3 _cameraPosition;

		[SetUp]
		public void SetuUp()
		{
			_calculator = Substitute.For<ICameraCalculator>();
			_screen = Substitute.For<IScreen>();
			_clamper = Substitute.For<IPositionClamper>();

			_scrollHandler = new ScrollHandler(_calculator, _screen, _clamper);

			_screen.Width.Returns(1920);
			_screen.Height.Returns(1200);

			_calculator.FindScrollSpeed(_orthographicSize, _timeDelta).Returns(_scrollSpeed);

			// Simply return the parameter, ie, don't clamp :P
			_clamper.Clamp(default(Vector3)).ReturnsForAnyArgs(x => x.Arg<Vector3>());

			_cameraPosition = new Vector3(0, 0, -10);
		}

        [Test]
        public void FindCameraPosition_NoScroll()
        {
			Vector3 mousePosition = new Vector3(1, 1, 1);

			Assert.AreEqual(_cameraPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition, _timeDelta));

			_calculator.Received().FindScrollSpeed(_orthographicSize, _timeDelta);
			_clamper.Received().Clamp(_cameraPosition);
        }

		[Test]
		public void FindCameraPosition_XScroll_Negative()
		{
			Vector3 mousePosition = new Vector3(0, 1);
			float expectedX = _cameraPosition.x - _scrollSpeed;
			Vector3 expectedPosition = new Vector3(expectedX, _cameraPosition.y, _cameraPosition.z);

			Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition, _timeDelta));
		}

        [Test]
        public void FindCameraPosition_XScroll_Positive()
        {
			Vector3 mousePosition = new Vector3(_screen.Width, 1);
            float expectedX = _cameraPosition.x + _scrollSpeed;
            Vector3 expectedPosition = new Vector3(expectedX, _cameraPosition.y, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition, _timeDelta));
        }

        [Test]
        public void FindCameraPosition_YScroll_Negative()
        {
			Vector3 mousePosition = new Vector3(1, 0);
			float expectedY = _cameraPosition.y - _scrollSpeed;
			Vector3 expectedPosition = new Vector3(_cameraPosition.x, expectedY, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition, _timeDelta));
        }

        [Test]
        public void FindCameraPosition_YScroll_Positive()
        {
			Vector3 mousePosition = new Vector3(1, _screen.Height);
            float expectedY = _cameraPosition.y + _scrollSpeed;
            Vector3 expectedPosition = new Vector3(_cameraPosition.x, expectedY, _cameraPosition.z);

            Assert.AreEqual(expectedPosition, _scrollHandler.FindCameraPosition(_orthographicSize, _cameraPosition, mousePosition, _timeDelta));
        }
	}
}
