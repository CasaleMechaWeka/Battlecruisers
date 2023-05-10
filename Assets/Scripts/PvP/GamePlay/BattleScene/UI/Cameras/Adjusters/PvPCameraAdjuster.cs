using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public abstract class PvPCameraAdjuster : IPvPCameraAdjuster
    {
        protected readonly IPvPCameraTargetProvider _cameraTargetProvider;

        public event EventHandler CompletedAdjustment;

        public PvPCameraAdjuster(IPvPCameraTargetProvider cameraTargetProvider)
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