using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetTrackers
{
    public interface ITargetTracker : IManagedDisposable
    {
        event EventHandler TargetsChanged;

        bool ContainsTarget(ITarget target);
    }
}
