using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public interface IPvPStaticCameraTargetProvider : IUserInputCameraTargetProvider
    {
        void SetTarget(ICameraTarget target);
    }
}