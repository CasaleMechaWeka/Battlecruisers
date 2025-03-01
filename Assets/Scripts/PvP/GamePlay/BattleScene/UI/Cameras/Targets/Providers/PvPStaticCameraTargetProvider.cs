using BattleCruisers.UI.Cameras.Targets;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPStaticCameraTargetProvider : PvPUserInputCameraTargetProvider, IPvPStaticCameraTargetProvider
    {
        public override int Priority { get; }

        public PvPStaticCameraTargetProvider(int priority)
        {
            Priority = priority;
        }

        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }
    }
}