using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
{
    public interface IPvPRankedTargetTracker : IManagedDisposable
    {
        /// <summary>
        /// The highest priority target, or null if there are no targets.
        /// </summary>
        PvPRankedTarget HighestPriorityTarget { get; }

        event EventHandler HighestPriorityTargetChanged;
    }
}