using BattleCruisers.Cruisers;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraCalculator
    {
        float FindCameraOrthographicSize(ICruiser cruiser);
        float FindCameraYPosition(float desiredOrthographicSize);
		float FindScrollSpeed(float orthographicSize, float timeDelta);
        Vector3 FindCruiserCameraPosition(ICruiser cruiser, float orthographicSize, float zValue);
	}
}
