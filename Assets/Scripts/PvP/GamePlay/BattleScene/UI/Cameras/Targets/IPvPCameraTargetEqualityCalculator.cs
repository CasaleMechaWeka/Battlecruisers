using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTargetEqualityCalculator
    {
        bool IsOnTarget(ICameraTarget target, ICamera camera);
    }
}