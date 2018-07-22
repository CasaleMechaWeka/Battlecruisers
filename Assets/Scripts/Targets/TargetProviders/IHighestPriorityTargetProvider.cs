using System;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IHighestPriorityTargetProvider : IBroadcastingTargetProvider, ITargetConsumer, IManagedDisposable 
    {
        event EventHandler NewInRangeTarget;
    }
}
