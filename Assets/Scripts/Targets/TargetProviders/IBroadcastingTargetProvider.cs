using System;

namespace BattleCruisers.Targets.TargetProviders
{
    public interface IBroadCastingTargetProvider : ITargetProvider
    {
        event EventHandler TargetChanged;
    }
}
