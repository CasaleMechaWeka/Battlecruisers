using BattleCruisers.UI.Cameras.Targets;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface ICameraNavigationWheelCalculator
    {
        /// <summary>
        /// Calculates the camera orthographic size needed for
        /// the current navigation wheel position.
        /// </summary>
        float FindOrthographicSize();

        /// <summary>
        /// Calculates the camera position needed for the current
        /// navigation wheel position.
        /// </summary>
        Vector2 FindCameraPosition();

        /// <summary>
        /// The reverse of the other methods.
        /// </summary>
        Vector2 FindNavigationWheelPosition(ICameraTarget cameraTarget);
    }
}