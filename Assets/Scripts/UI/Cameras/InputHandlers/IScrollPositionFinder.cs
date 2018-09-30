using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    public interface IScrollPositionFinder
    {
        Vector3 FindDesiredPosition(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInMPerS);
    }
}