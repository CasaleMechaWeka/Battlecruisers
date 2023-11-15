using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public interface IPvPTargetColliderHandler
    {
        void OnTargetColliderEntered(IPvPTarget target);
        void OnTargetColliderExited(IPvPTarget target);
    }
}