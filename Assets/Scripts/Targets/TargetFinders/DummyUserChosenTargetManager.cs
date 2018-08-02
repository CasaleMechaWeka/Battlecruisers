using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using System;

namespace BattleCruisers.Targets.TargetFinders
{
    public class DummyUserChosenTargetManager : IUserChosenTargetManager
    {
        public ITarget Target { get; set; }
        public RankedTarget HighestPriorityTarget { get; set; }

#pragma warning disable 67  // Unused event
        public event EventHandler HighestPriorityTargetChanged;
#pragma warning restore 67  // Unused event

        public void StartTrackingTargets()
        {
            // empty
        }

        public void DisposeManagedState()
        {
            // empty
        }
    }
}