using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraTargetLegacy
    {
        Vector3 Position { get; }
        float OrthographicSize { get; }
        CameraState State { get; }
        bool IsInstantTransition(CameraState fromState);
    }
}
