using System;
using BattleCruisers.Buildables.BuildProgress;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public class ChangeCruiserBuildSpeedStepNEW : TutorialStepNEW
    {
        private readonly IBuildSpeedController _buildSpeedController;
        private readonly BuildSpeed _buildSpeed;

        public ChangeCruiserBuildSpeedStepNEW(ITutorialStepArgsNEW args, IBuildSpeedController buildSpeedController, BuildSpeed buildSpeed)
            : base(args)
        {
            Assert.IsNotNull(buildSpeedController);

            _buildSpeedController = buildSpeedController;
            _buildSpeed = buildSpeed;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _buildSpeedController.BuildSpeed = _buildSpeed;

            OnCompleted();
        }
    }
}
