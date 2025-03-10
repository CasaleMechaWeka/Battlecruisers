using BattleCruisers.UI.Cameras.Targets;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IDirectionalZoom
    {
        ICameraTarget ZoomOut(float orthographicSizeDelta);
        ICameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition);
    }
}