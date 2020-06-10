using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Update tests
    // FELIX  Avoid duplicate code with SwipeCTP?
    public class PinchZoomCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IZoomCalculator _zoomCalculator;
        private readonly IDirectionalZoom _directionalZoom;
        private readonly IPinchTracker _pinchTracker;
        private bool _inUserInputSession;

        public override int Priority => 3;

        public PinchZoomCameraTargetProvider(
            IZoomCalculator zoomCalculator,
            IDirectionalZoom directionalZoom,
            IPinchTracker pinchTracker)
        {
            Helper.AssertIsNotNull(zoomCalculator, directionalZoom, pinchTracker);

            _zoomCalculator = zoomCalculator;
            _directionalZoom = directionalZoom;
            _pinchTracker = pinchTracker;

            _pinchTracker.PinchEnd += _pinchTracker_PinchEnd;
            _pinchTracker.Pinch += _pinchTracker_Pinch;

            _inUserInputSession = false;
        }

        private void _pinchTracker_PinchEnd(object sender, EventArgs e)
        {
            if (_inUserInputSession)
            {
                RaiseUserInputEnded();
            }
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

            // Don't use PinchStart event, because we want to wait until we have a valid camera target.
            if (!_inUserInputSession)
            {
                _inUserInputSession = true;
                RaiseUserInputStarted();
            }
        }
    }
}