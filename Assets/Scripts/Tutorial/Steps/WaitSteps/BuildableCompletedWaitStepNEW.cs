using System;
using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when the specified buildable completes construction.
    /// </summary>
    public class BuildableCompletedWaitStepNEW : TutorialStep
    {
        private readonly IItemProvider<IBuildable> _buildableProvider;
        private IBuildable _buildable;

        public BuildableCompletedWaitStepNEW(ITutorialStepArgs args, IItemProvider<IBuildable> buildableProvider)
            : base(args)
        {
            Assert.IsNotNull(buildableProvider);
            _buildableProvider = buildableProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _buildable = _buildableProvider.FindItem();
            Assert.IsNotNull(_buildable);
            _buildable.CompletedBuildable += _buildable_CompletedBuildable;
        }

        private void _buildable_CompletedBuildable(object sender, EventArgs e)
        {
            _buildable.CompletedBuildable -= _buildable_CompletedBuildable;
            OnCompleted();
        }
    }
}
