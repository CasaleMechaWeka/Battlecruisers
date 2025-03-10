using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IBroadcastingTargetProvider : ITargetProvider, IManagedDisposable
    {
        event EventHandler TargetChanged;
    }
}
