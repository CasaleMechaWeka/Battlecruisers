using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public interface ITeleporterHelper
    {
        Vector3 FindTeleportTargetPosition(ICloud onScreenCloud, ICloud offScreenCloud);
        bool ShouldTeleportCloud(ICloud rightCloud);
    }
}