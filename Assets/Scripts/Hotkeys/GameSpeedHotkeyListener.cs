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

            _hotkeyDetector.SlowMotion += _hotkeyDetector_SlowMotion;
            _hotkeyDetector.Play += _hotkeyDetector_Play;
            _hotkeyDetector.FastForward += _hotkeyDetector_FastForward;
        }

        private void _hotkeyDetector_SlowMotion(object sender, EventArgs e)
        {
            _speedComponents.SlowMotionButton.TriggerClick();
        }

        private void _hotkeyDetector_Play(object sender, EventArgs e)
        {
            _speedComponents.PlayButton.TriggerClick();
        }

        private void _hotkeyDetector_FastForward(object sender, EventArgs e)
        {
            _speedComponents.FastForwardButton.TriggerClick();
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.SlowMotion -= _hotkeyDetector_SlowMotion;
            _hotkeyDetector.Play -= _hotkeyDetector_Play;
            _hotkeyDetector.FastForward -= _hotkeyDetector_FastForward;
        }
    }
}