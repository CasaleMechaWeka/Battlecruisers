using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetDetectors
{
    public interface ITargetDetectorEventEmitter
    {
        void InvokeTargetEnteredEvent(ITarget target);
        void InvokeTargetExitedEvent(ITarget target);
    }
}