using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    /// <summary>
    /// Completed when the specified buildable completes construction.
    /// </summary>
    // FELIX  Test :D
    public class BuildableCompletedWait : TutorialStep
    {
        private readonly IBuildable _buildable;

        public BuildableCompletedWait(ITutorialStepArgs args, IBuildable buildable)
            : base(args)
        {
            Assert.IsNotNull(buildable);
            _buildable = buildable;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _buildable.CompletedBuildable += _buildable_CompletedBuildable;
        }

        private void _buildable_CompletedBuildable(object sender, EventArgs e)
        {
            _buildable.CompletedBuildable -= _buildable_CompletedBuildable;
            OnCompleted();
        }
    }
}
