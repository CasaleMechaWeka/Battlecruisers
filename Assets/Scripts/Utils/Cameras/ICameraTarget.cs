using UnityEngine;

namespace BattleCruisers.Utils.Cameras
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        float OrthographicSize { get; }
        CameraState State { get; }
        bool IsInstantTransition(CameraState nextState);
    }
}
