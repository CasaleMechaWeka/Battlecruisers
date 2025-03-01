using BattleCruisers.UI.Cameras.Targets;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public interface IPvPStaticCameraTargetProvider : IPvPUserInputCameraTargetProvider
    {
        void SetTarget(ICameraTarget target);
    }
}