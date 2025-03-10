using BattleCruisers.Targets;
using BattleCruisers.Utils;

namespace BattleCruisers.Movement.Deciders
{
    public interface IMovementDecider : ITargetConsumer, IManagedDisposable { }
}
