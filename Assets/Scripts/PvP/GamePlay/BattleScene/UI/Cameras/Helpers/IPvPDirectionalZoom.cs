using BattleCruisers.UI.Cameras.Targets;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public interface IPvPDirectionalZoom
    {
        ICameraTarget ZoomOut(float orthographicSizeDelta);
        ICameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition);
    }
}