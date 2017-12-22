using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IHighestPriorityTargetProvider : ITargetProvider, ITargetConsumer, IManagedDisposable { }
}
