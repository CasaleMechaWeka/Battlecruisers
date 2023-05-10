using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Pinch;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPPinchZoomCameraTargetProvider : PvPUserInputCameraTargetProvider
    {
        private readonly IPvPZoomCalculator _zoomCalculator;
        private readonly IPvPDirectionalZoom _directionalZoom;
        private readonly IPvPPinchTracker _pinchTracker;

        public override int Priority => 4;

        public PvPPinchZoomCameraTargetProvider(
            IPvPZoomCalculator zoomCalculator,
            IPvPDirectionalZoom directionalZoom,
            IPvPPinchTracker pinchTracker)
        {
            PvPHelper.AssertIsNotNull(zoomCalculator, directionalZoom, pinchTracker);

            _zoomCalculator = zoomCalculator;
            _directionalZoom = directionalZoom;
            _pinchTracker = pinchTracker;

            _pinchTracker.Pinch += _pinchTracker_Pinch;
            _pinchTracker.PinchEnd += _pinchTracker_PinchEnd;
        }

        private void _pinchTracker_Pinch(object sender, PvPPinchEventArgs e)
        {
            float orthographicSizeDelta = _zoomCalculator.FindPinchZoomOrthographicSizeDelta(e.DeltaInM);

            if (e.DeltaInM < 0)
            {
                Target = _directionalZoom.ZoomOut(orthographicSizeDelta);
            }
            else
            {
                Target = _directionalZoom.ZoomIn(orthographicSizeDelta, e.Position);
            }
        }

        private void _pinchTracker_PinchEnd(object sender, EventArgs e)
        {
            UserInputEnd();
        }
    }
}