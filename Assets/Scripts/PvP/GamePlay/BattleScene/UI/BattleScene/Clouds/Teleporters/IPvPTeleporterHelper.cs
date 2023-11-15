using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Teleporters
{
    public interface IPvPTeleporterHelper
    {
        Vector3 FindTeleportTargetPosition(IPvPCloud onScreenCloud, IPvPCloud offScreenCloud);
        bool ShouldTeleportCloud(IPvPCloud rightCloud);
    }
}