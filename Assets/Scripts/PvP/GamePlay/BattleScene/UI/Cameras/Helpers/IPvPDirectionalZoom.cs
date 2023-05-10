using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public interface IPvPDirectionalZoom
    {
        IPvPCameraTarget ZoomOut(float orthographicSizeDelta);
        IPvPCameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition);
    }
}