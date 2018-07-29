using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
    // FELIX  Test, use
    public class CompositeTargetFinder : ITargetFinder
    {
        private readonly ITargetFinder[] _targetFinders;

        private const int MIN_NUM_OF_TARGET_FINDERS = 2;

        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;

        public CompositeTargetFinder(params ITargetFinder[] targetFinders)
        {
            Assert.IsNotNull(targetFinders);
            Assert.IsTrue(targetFinders.Length >= MIN_NUM_OF_TARGET_FINDERS);

            _targetFinders = targetFinders;

            foreach (ITargetFinder targetFinder in _targetFinders)
            {
                targetFinder.TargetFound += TargetFinder_TargetFound;
                targetFinder.TargetLost += TargetFinder_TargetLost;
            }
        }

        private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
        {
            if (TargetFound != null)
            {
                TargetFound.Invoke(this, e);
            }
        }

        private void TargetFinder_TargetLost(object sender, TargetEventArgs e)
        {
            if (TargetLost != null)
            {
                TargetLost.Invoke(this, e);
            }
        }

        public void StartFindingTargets()
        {
            foreach (ITargetFinder targetFinder in _targetFinders)
            {
                targetFinder.StartFindingTargets();
            }
        }

        public void DisposeManagedState()
        {
            foreach (ITargetFinder targetFinder in _targetFinders)
            {
                targetFinder.TargetFound -= TargetFinder_TargetFound;
                targetFinder.TargetLost -= TargetFinder_TargetLost;
                targetFinder.DisposeManagedState();
            }
        }
    }
}