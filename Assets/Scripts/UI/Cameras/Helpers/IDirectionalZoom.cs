using BattleCruisers.UI.Cameras.Targets;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IDirectionalZoom
    {
        CameraTarget ZoomOut(float orthographicSizeDelta);
        CameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition);
    }
}