using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.UI.Cameras.Targets.Providers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPPinchZoomCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IZoomCalculator _zoomCalculator;
        private readonly IDirectionalZoom _directionalZoom;
        private readonly IPinchTracker _pinchTracker;

        public override int Priority => 4;

        public PvPPinchZoomCameraTargetProvider(
            IZoomCalculator zoomCalculator,
            IDirectionalZoom directionalZoom,
            IPinchTracker pinchTracker)
        {
            PvPHelper.AssertIsNotNull(zoomCalculator, directionalZoom, pinchTracker);

            _zoomCalculator = zoomCalculator;
            _directionalZoom = directionalZoom;
            _pinchTracker = pinchTracker;

            _pinchTracker.Pinch += _pinchTracker_Pinch;
            _pinchTracker.PinchEnd += _pinchTracker_PinchEnd;
        }

        private void _pinchTracker_Pinch(object sender, PinchEventArgs e)
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