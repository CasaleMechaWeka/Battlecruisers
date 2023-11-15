using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPBackgroundImageCalculator
    {
        Vector3 FindPosition(IPvPBackgroundImageStats stats, float cameraAspectRatio);
    }
}