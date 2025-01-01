using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public interface IPvPTargetDetectorEventEmitter
    {
        void InvokeTargetEnteredEvent(ITarget target);
        void InvokeTargetExitedEvent(ITarget target);
    }
}