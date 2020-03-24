using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public class ScrollWheelCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IInput _input;
        private readonly IUpdater _updater;
        private readonly IZoomCalculator _zoomCalculator;
        private readonly IDirectionalZoom _directionalZoom;
        private bool _duringUserInput;

        public override int Priority => 4;

        public ScrollWheelCameraTargetProvider(
            IInput input, 
            IUpdater updater,
            IZoomCalculator zoomCalculator,
            IDirectionalZoom directionalZoom)
        {
            Helper.AssertIsNotNull(input, updater, zoomCalculator, directionalZoom);

            _input = input;
            _updater = updater;
            _zoomCalculator = zoomCalculator;
            _duringUserInput = false;
            _directionalZoom = directionalZoom;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.MouseScrollDelta.y == 0)
            {
                if (_duringUserInput)
                {
                    _duringUserInput = false;
                    RaiseUserInputEnded();
                }

                return;
            }

            if (!_duringUserInput)
            {
                _duringUserInput = true;
                RaiseUserInputStarted();
            }

            float orthographicSizeDelta = _zoomCalculator.FindMouseScrollOrthographicSizeDelta(_input.MouseScrollDelta.y);

            if (_input.MouseScrollDelta.y < 0)
            {
                Target = _directionalZoom.ZoomOut(orthographicSizeDelta);
            }
            else
            {
                Target = _directionalZoom.ZoomIn(orthographicSizeDelta, _input.MousePosition);
            }
        }
    }
}