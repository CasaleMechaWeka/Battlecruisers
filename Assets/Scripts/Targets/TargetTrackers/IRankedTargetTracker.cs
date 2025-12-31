using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetTrackers
{
    public interface IRankedTargetTracker : IManagedDisposable
    {
        /// <summary>
        /// The highest priority target, or null if there are no targets.
        /// </summary>
        RankedTarget HighestPriorityTarget { get; }

        event EventHandler HighestPriorityTargetChanged;
    }
}