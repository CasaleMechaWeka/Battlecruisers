using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils;
using UnityEngine;
using BattleCruisers.UI.Cameras.Helpers;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	/// <summary>
	/// When the mouse touches the edge of the screen we want the screen to scroll
	/// in that direction.  Similar to real time strategy games like Age of Empires.
	/// </summary>
	public class ScrollHandler : IScrollHandler
	{
		private readonly ICameraCalculator _calculator;
		private readonly IScreen _screen;
		private readonly IPositionClamper _clamper;
        private readonly IDeltaTimeProvider _deltaTimeProvider;

        // Cannot have a scroll boundary of 0, otherwise only left and upwards
        // scroll work on Mac.  Right and downwards do not.
		private const float SCROLL_BOUNDARY_IN_PIXELS = 2;

        public ScrollHandler(ICameraCalculator calculator, IScreen screen, IPositionClamper clamper, IDeltaTimeProvider deltaTimeProvider)
		{
            Helper.AssertIsNotNull(calculator, screen, clamper, deltaTimeProvider);

			_calculator = calculator;
			_screen = screen;
			_clamper = clamper;
            _deltaTimeProvider = deltaTimeProvider;
		}

		public Vector3 FindCameraPosition(float cameraOrthographicSize, Vector3 cameraPosition, Vector3 mousePosition)
		{
            float scrollSpeed = _calculator.FindScrollSpeed(cameraOrthographicSize, _deltaTimeProvider.UnscaledDeltaTime);

            Vector3 desiredPosition
                = new Vector3(
    				FindDesiredX(cameraPosition, mousePosition, scrollSpeed),
    				FindDesiredY(cameraPosition, mousePosition, scrollSpeed),
				    cameraPosition.z);

			return _clamper.Clamp(desiredPosition);
		}

		private float FindDesiredX(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
        {
            if (mousePosition.x >= _screen.Width - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x + scrollSpeed;
            }
            else if (mousePosition.x <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x - scrollSpeed;
            }
            return cameraPosition.x;
        }

        private float FindDesiredY(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
        {
            if (mousePosition.y >= _screen.Height - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y + scrollSpeed;
            }
            else if (mousePosition.y <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y - scrollSpeed;
            }
			return cameraPosition.y;
        }
	}
}
