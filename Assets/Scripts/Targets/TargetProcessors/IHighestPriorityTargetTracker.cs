using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface IHighestPriorityTargetTracker : IManagedDisposable
    {
        /// <summary>
        /// The highest priority target, or null if there are no targets.
        /// </summary>
        RankedTarget HighestPriorityTarget { get; }

        event EventHandler HighestPriorityTargetChanged;

        void StartTrackingTargets();
    }
}