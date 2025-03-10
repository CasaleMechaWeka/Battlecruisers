using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        float OrthographicSize { get; }
    }
}
