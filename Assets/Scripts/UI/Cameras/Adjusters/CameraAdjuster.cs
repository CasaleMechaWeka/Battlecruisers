using BattleCruisers.UI.Cameras.Targets.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public abstract class CameraAdjuster : ICameraAdjuster
    {
        protected readonly ICameraTargetProvider _cameraTargetProvider;

        public event EventHandler CompletedAdjustment;

        public CameraAdjuster(ICameraTargetProvider cameraTargetProvider)
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