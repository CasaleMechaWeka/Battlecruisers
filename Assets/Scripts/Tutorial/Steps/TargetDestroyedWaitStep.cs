using System;
using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    /// <summary>
    /// Completed when the target is destroyed.
    /// </summary>
    // FELIX  Test :D
    public class TargetDestroyedWaitStep : TutorialStep
    {
        private readonly ITarget _target;

        public TargetDestroyedWaitStep(ITutorialStepArgs args, ITarget target)
            : base(args)
        {
            Assert.IsNotNull(target);
            _target = target;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _target.Destroyed += _target_Destroyed;
        }

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            _target.Destroyed -= _target_Destroyed;
            base.OnCompleted();
        }
    }
}
