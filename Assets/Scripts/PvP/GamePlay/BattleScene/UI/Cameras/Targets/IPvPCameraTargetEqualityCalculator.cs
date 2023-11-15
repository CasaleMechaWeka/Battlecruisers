using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTargetEqualityCalculator
    {
        bool IsOnTarget(IPvPCameraTarget target, IPvPCamera camera);
    }
}