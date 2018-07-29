using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Targets.TargetFinders
{
    public class DummyUserChosenTargetManager : IUserChosenTargetManager
    {
        public ITarget Target { get; set; }

#pragma warning disable 67  // Unused event
        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;
#pragma warning restore 67  // Unused event

        public void StartFindingTargets()
        {
            // empty
        }

        public void DisposeManagedState()
        {
            // empty
        }
    }
}