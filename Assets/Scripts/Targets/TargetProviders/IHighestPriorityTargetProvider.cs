using System;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IHighestPriorityTargetProvider : IBroadCastingTargetProvider, ITargetConsumer, IManagedDisposable 
    {
        event EventHandler NewInRangeTarget;
    }
}
