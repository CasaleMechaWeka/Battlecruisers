using BattleCruisers.PlatformAbstractions;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	/// <summary>
	/// When the mouse touches the edge of the screen we want the screen to scroll
	/// in that direction.  Similar to real time strategy games like Age of Empires.
	/// </summary>
	/// FELIX  Test :D
	public class ScrollHandler : IScrollHandler
	{
		private readonly ICameraCalculator _calculator;
		private readonly IScreen _screen;
		private readonly IPositionClamper _clamper;

		// FELIX  Remove, of 0 works while fullscreen :)
		private const float SCROLL_BOUNDARY_IN_PIXELS = 0;

		public ScrollHandler(ICameraCalculator calculator, IScreen screen, IPositionClamper clamper)
		{
			Helper.AssertIsNotNull(calculator, screen, clamper);

			_calculator = calculator;
			_screen = screen;
			_clamper = clamper;
		}

		public Vector3 FindCameraPosition(float cameraOrthographicSize, Vector3 cameraPosition, Vector3 mousePosition, float timeDelta)
		{
			float scrollSpeed = _calculator.FindScrollSpeed(cameraOrthographicSize, timeDelta);

            Vector3 desiredPosition
                = new Vector3(
    				FindDesiredX(cameraPosition, mousePosition, scrollSpeed),
    				FindDesiredY(cameraPosition, mousePosition, scrollSpeed),
				    cameraPosition.z);

			_clamper.Clamp(desiredPosition);

			return desiredPosition;
		}

		private float FindDesiredX(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
        {
            if (mousePosition.x > _screen.Width - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x + scrollSpeed;
            }
            else if (mousePosition.x < 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x - scrollSpeed;
            }
            return cameraPosition.x;
        }

        private float FindDesiredY(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
        {
            if (mousePosition.y > _screen.Height - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y + scrollSpeed;
            }
            else if (mousePosition.y < 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y - scrollSpeed;
            }
			return cameraPosition.y;
        }
	}
}
