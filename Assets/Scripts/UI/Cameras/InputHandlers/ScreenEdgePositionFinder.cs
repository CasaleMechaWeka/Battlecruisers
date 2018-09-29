using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    // FELIX  Test :)  (Copy from ScrollHandler?)
    /// <summary>
    /// Moves the camera towards the screen edge where the mouse is.
    /// </summary>
    public class ScreenEdgePositionFinder : IScrollPositionFinder
    {
        private readonly IScreen _screen;

        // Cannot have a scroll boundary of 0, otherwise only left and upwards
        // scroll work on Mac.  Right and downwards do not.
        private const float SCROLL_BOUNDARY_IN_PIXELS = 2;

        public ScreenEdgePositionFinder(IScreen screen)
        {
            Assert.IsNotNull(screen);
            _screen = screen;
        }

        public Vector3 FindDesiredPosition(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInM)
        {
            return
                new Vector3(
                    FindDesiredX(cameraPosition, mousePosition, scrollSpeedInM),
                    FindDesiredY(cameraPosition, mousePosition, scrollSpeedInM),
                    cameraPosition.z);
        }

        private float FindDesiredX(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInM)
        {
            if (mousePosition.x >= _screen.Width - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x + scrollSpeedInM;
            }
            else if (mousePosition.x <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x - scrollSpeedInM;
            }
            return cameraPosition.x;
        }

        private float FindDesiredY(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInM)
        {
            if (mousePosition.y >= _screen.Height - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y + scrollSpeedInM;
            }
            else if (mousePosition.y <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y - scrollSpeedInM;
            }
            return cameraPosition.y;
        }

    }
}