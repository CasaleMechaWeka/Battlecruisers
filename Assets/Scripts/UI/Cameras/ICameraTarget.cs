using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        float OrthographicSize { get; }
        CameraState State { get; }
        bool IsInstantTransition(CameraState nextState);
    }
}
