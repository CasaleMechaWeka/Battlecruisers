using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public interface IPvPTargetDetectorEventEmitter
    {
        void InvokeTargetEnteredEvent(IPvPTarget target);
        void InvokeTargetExitedEvent(IPvPTarget target);
    }
}