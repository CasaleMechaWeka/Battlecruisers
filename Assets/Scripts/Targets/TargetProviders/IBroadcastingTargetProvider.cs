using System;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IBroadcastingTargetProvider : ITargetProvider
    {
        event EventHandler TargetChanged;
    }
}
