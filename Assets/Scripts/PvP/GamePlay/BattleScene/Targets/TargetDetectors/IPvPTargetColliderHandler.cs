using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public interface IPvPTargetColliderHandler
    {
        void OnTargetColliderEntered(ITarget target);
        void OnTargetColliderExited(ITarget target);
    }
}