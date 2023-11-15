using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public class PvPScrollWheelCameraTargetProvider : PvPUserInputCameraTargetProvider
    {
        private readonly IPvPInput _input;
        private readonly IPvPUpdater _updater;
        private readonly IPvPZoomCalculator _zoomCalculator;
        private readonly IPvPDirectionalZoom _directionalZoom;

        public override int Priority => 5;

        public PvPScrollWheelCameraTargetProvider(
            IPvPInput input,
            IPvPUpdater updater,
            IPvPZoomCalculator zoomCalculator,
            IPvPDirectionalZoom directionalZoom)
        {
            PvPHelper.AssertIsNotNull(input, updater, zoomCalculator, directionalZoom);

            _input = input;
            _updater = updater;
            _zoomCalculator = zoomCalculator;
            _directionalZoom = directionalZoom;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.MouseScrollDelta.y == 0)
            {
                UserInputEnd();
                return;
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