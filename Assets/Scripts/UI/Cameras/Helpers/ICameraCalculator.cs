using BattleCruisers.Cruisers;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface ICameraCalculator
    {
        float FindCameraOrthographicSize(ICruiser cruiser);
        float FindCameraYPosition(float desiredOrthographicSize);
		float FindScrollSpeed(float orthographicSize, float timeDelta);
        Vector3 FindCruiserCameraPosition(ICruiser cruiser, float orthographicSize, float zValue);

        /// <summary>
        /// Returns the camera position required for the zoomTarget's viewport position
        /// to remain the same.
        /// </summary>
        Vector3 FindZoomingCameraPosition(
            Vector2 zoomTarget, 
            Vector2 targetViewportPosition, 
            float cameraOrthographicSize, 
            float cameraAspectRatio,
            float cameraPositionZ);
	}
}
