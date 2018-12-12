using BattleCruisers.UI.Cameras.Adjusters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when camera completes it's current adjustment.
    /// </summary>
    public class CameraAdjustmentWaitStep : TutorialStepNEW
    {
        private readonly ICameraAdjuster _cameraAdjuster;

        public CameraAdjustmentWaitStep(ITutorialStepArgsNEW args, ICameraAdjuster cameraAdjuster)
            : base(args)
        {
            Assert.IsNotNull(cameraAdjuster);
            _cameraAdjuster = cameraAdjuster;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _cameraAdjuster.CompletedAdjustment += _cameraAdjuster_CompletedAdjustment;
        }

        private void _cameraAdjuster_CompletedAdjustment(object sender, EventArgs e)
        {
            _cameraAdjuster.CompletedAdjustment -= _cameraAdjuster_CompletedAdjustment;
            OnCompleted();
        }
    }
}