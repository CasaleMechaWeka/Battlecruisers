using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public interface ITeleporterHelper
    {
        Vector2 FindTeleportTargetPosition(ICloudNEW onScreenCloud, ICloudNEW offScreenCloud);
        bool ShouldTeleportCloud(ICloudNEW rightCloud);
    }
}