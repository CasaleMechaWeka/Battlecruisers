using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTarget
    {
        Vector3 Position { get; }
        float OrthographicSize { get; }
    }
}
