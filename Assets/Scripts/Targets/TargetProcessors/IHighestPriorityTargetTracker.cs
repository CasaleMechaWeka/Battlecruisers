using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface IHighestPriorityTargetTracker : IManagedDisposable
    {
        RankedTarget HighestPriorityTarget { get; }

        event EventHandler HighestPriorityTargetChanged;

        void StartTrackingTargets();
    }
}