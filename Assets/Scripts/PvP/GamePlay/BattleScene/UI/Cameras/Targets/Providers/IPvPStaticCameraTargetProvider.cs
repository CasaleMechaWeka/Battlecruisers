namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public interface IPvPStaticCameraTargetProvider : IPvPUserInputCameraTargetProvider
    {
        void SetTarget(IPvPCameraTarget target);
    }
}