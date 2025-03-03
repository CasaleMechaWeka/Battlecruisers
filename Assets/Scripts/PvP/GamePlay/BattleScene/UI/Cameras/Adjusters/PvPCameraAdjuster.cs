using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public abstract class PvPCameraAdjuster : ICameraAdjuster
    {
        protected readonly ICameraTargetProvider _cameraTargetProvider;

        public event EventHandler CompletedAdjustment;

        public PvPCameraAdjuster(ICameraTargetProvider cameraTargetProvider)
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