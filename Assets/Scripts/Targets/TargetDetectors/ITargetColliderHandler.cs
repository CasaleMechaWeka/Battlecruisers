using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetDetectors
{
    public interface ITargetColliderHandler
    {
        void OnTargetColliderEntered(ITarget target);
        void OnTargetColliderExited(ITarget target);
    }
}