using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPGameSpeedHotkeyListener : IPvPManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly IPvPSpeedComponents _speedComponents;

        public PvPGameSpeedHotkeyListener(IHotkeyDetector hotkeyDetector, IPvPSpeedComponents speedComponents)
        {
            PvPHelper.AssertIsNotNull(hotkeyDetector, speedComponents);

            _hotkeyDetector = hotkeyDetector;
            _speedComponents = speedComponents;

            _hotkeyDetector.PauseSpeed += _hotkeyDetector_PauseSpeed;
            _hotkeyDetector.SlowMotion += _hotkeyDetector_SlowMotion;
            _hotkeyDetector.NormalSpeed += _hotkeyDetector_NormalSpeed;
            _hotkeyDetector.FastForward += _hotkeyDetector_FastForward;
            _hotkeyDetector.ToggleSpeed += _hotkeyDetector_ToggleSpeed;
        }

        private void _hotkeyDetector_PauseSpeed(object sender, EventArgs e)
        {
            _speedComponents.PauseButton.TriggerClick();
        }

        private void _hotkeyDetector_SlowMotion(object sender, EventArgs e)
        {
            _speedComponents.SlowMotionButton.TriggerClick();
        }

        private void _hotkeyDetector_NormalSpeed(object sender, EventArgs e)
        {
            _speedComponents.NormalSpeedButton.TriggerClick();
        }

        private void _hotkeyDetector_FastForward(object sender, EventArgs e)
        {
            _speedComponents.FastForwardButton.TriggerClick();
        }

        private void _hotkeyDetector_ToggleSpeed(object sender, EventArgs e)
        {
            if (_speedComponents.PauseButton.IsSelected)
            {
                _speedComponents.NormalSpeedButton.TriggerClick();
            }
            else
            {
                _speedComponents.PauseButton.TriggerClick();
            }

        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.SlowMotion -= _hotkeyDetector_SlowMotion;
            _hotkeyDetector.NormalSpeed -= _hotkeyDetector_NormalSpeed;
            _hotkeyDetector.FastForward -= _hotkeyDetector_FastForward;
            _hotkeyDetector.ToggleSpeed += _hotkeyDetector_ToggleSpeed;
        }
    }
}