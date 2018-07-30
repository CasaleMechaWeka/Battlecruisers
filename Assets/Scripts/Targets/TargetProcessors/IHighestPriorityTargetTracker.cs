using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface IHighestPriorityTargetTracker : IManagedDisposable
    {
        ITarget HighestPriorityTarget { get; }

        event EventHandler HighestPriorityTargetChanged;

        void StartTrackingTargets();
    }
}