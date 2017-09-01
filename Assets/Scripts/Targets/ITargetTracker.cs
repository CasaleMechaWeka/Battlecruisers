using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets
{
    public interface ITargetTracker
    {
        event EventHandler TargetsChanged;

        bool ContainsTarget(ITarget target);
    }
}