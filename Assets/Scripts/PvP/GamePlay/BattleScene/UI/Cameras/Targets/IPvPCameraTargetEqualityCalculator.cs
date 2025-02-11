using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTargetEqualityCalculator
    {
        bool IsOnTarget(IPvPCameraTarget target, ICamera camera);
    }
}