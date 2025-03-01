using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Teleporters
{
    public interface IPvPTeleporterHelper
    {
        Vector3 FindTeleportTargetPosition(ICloud onScreenCloud, ICloud offScreenCloud);
        bool ShouldTeleportCloud(ICloud rightCloud);
    }
}