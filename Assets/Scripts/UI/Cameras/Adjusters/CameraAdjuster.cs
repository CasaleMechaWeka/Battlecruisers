using BattleCruisers.UI.Cameras.Targets.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public abstract class CameraAdjuster
    {
        protected readonly CompositeCameraTargetProvider _cameraTargetProvider;

        public event EventHandler CompletedAdjustment;

        public CameraAdjuster(CompositeCameraTargetProvider cameraTargetProvider)
        {
            Assert.IsNotNull(cameraTargetProvider);
            _cameraTargetProvider = cameraTargetProvider;
        }

        public abstract void AdjustCamera();

        protected void InvokeCompletedAdjustmentEvent()
        {
            CompletedAdjustment?.Invoke(this, EventArgs.Empty);
        }
    }
}