using System;
using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when the target is destroyed.
    /// </summary>
    public class TargetDestroyedWaitStepNEW : TutorialStep
    {
        private readonly IItemProvider<ITarget> _targetProvider;
        private ITarget _target;

        public TargetDestroyedWaitStepNEW(ITutorialStepArgs args, IItemProvider<ITarget> targetProvider)
            : base(args)
        {
            Assert.IsNotNull(targetProvider);
            _targetProvider = targetProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _target = _targetProvider.FindItem();
            Assert.IsNotNull(_target);
            _target.Destroyed += _target_Destroyed;
        }

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            _target.Destroyed -= _target_Destroyed;
            base.OnCompleted();
        }
    }
}
