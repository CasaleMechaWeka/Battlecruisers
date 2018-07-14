using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets
{
    public interface ITargetTracker : IManagedDisposable
    {
        event EventHandler TargetsChanged;

        bool ContainsTarget(ITarget target);
    }
}
