using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using System;

namespace BattleCruisers.Targets.TargetTrackers
{
    public class DummyUserChosenTargetManager : IUserChosenTargetManager
    {
        public ITarget Target { get; set; }
        public RankedTarget HighestPriorityTarget { get; set; }

#pragma warning disable 67  // Unused event
        public event EventHandler HighestPriorityTargetChanged;
#pragma warning restore 67  // Unused event

        public void DisposeManagedState()
        {
            // empty
        }
    }
}