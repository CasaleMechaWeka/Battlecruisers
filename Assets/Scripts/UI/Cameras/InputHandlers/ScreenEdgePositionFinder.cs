using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    // FELIX  Test :)  (Copy from ScrollHandler?)
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

        public Vector3 FindDesiredPosition(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInMPerS)
        {
            return
                new Vector3(
                    FindDesiredX(cameraPosition, mousePosition, scrollSpeedInMPerS),
                    FindDesiredY(cameraPosition, mousePosition, scrollSpeedInMPerS),
                    cameraPosition.z);
        }

        private float FindDesiredX(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInMPerS)
        {
            if (mousePosition.x >= _screen.Width - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x + scrollSpeedInMPerS;
            }
            else if (mousePosition.x <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.x - scrollSpeedInMPerS;
            }
            return cameraPosition.x;
        }

        private float FindDesiredY(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInMPerS)
        {
            if (mousePosition.y >= _screen.Height - SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y + scrollSpeedInMPerS;
            }
            else if (mousePosition.y <= 0 + SCROLL_BOUNDARY_IN_PIXELS)
            {
                return cameraPosition.y - scrollSpeedInMPerS;
            }
            return cameraPosition.y;
        }

    }
}