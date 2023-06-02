using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys
{
    public class GameSpeedHotkeyListener : IManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly ISpeedComponents _speedComponents;

        public GameSpeedHotkeyListener(IHotkeyDetector hotkeyDetector, ISpeedComponents speedComponents)
        {
            Helper.AssertIsNotNull(hotkeyDetector, speedComponents);

            _hotkeyDetector = hotkeyDetector;
            _speedComponents = speedComponents;

            //_hotkeyDetector.PauseSpeed += _hotkeyDetector_PauseSpeed;
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
            //if (_speedComponents.PauseButton.IsSelected)
            //{
            //    _speedComponents.NormalSpeedButton.TriggerClick();
            //}
            //else{
            //    _speedComponents.PauseButton.TriggerClick();
            //}
           
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.SlowMotion -= _hotkeyDetector_SlowMotion;
            _hotkeyDetector.NormalSpeed -= _hotkeyDetector_NormalSpeed;
            _hotkeyDetector.FastForward -= _hotkeyDetector_FastForward;
            _hotkeyDetector.ToggleSpeed -= _hotkeyDetector_ToggleSpeed;
        }
    }
}