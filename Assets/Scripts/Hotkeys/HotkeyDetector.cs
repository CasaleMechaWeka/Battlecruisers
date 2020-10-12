using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Hotkeys
{
    // FELIX  Use, test
    public class HotkeyDetector : IHotkeyDetector
    {
        private readonly IHotkeyList _hotkeyList;
        private readonly IInput _input;
        private readonly IUpdater _updater;

        public event EventHandler PlayerCruiser;
        public event EventHandler Overview;
        public event EventHandler EnemyCruiser;

        public HotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater)
        {
            Helper.AssertIsNotNull(hotkeyList, input, updater);

            _hotkeyList = hotkeyList;
            _input = input;
            _updater = updater;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            // Navigation
            if (_input.GetKeyUp(_hotkeyList.PlayerCruiser))
            {
                PlayerCruiser?.Invoke(this, EventArgs.Empty);
            }
            else if (_input.GetKeyUp(_hotkeyList.Overview))
            {
                Overview?.Invoke(this, EventArgs.Empty);
            }
            else if (_input.GetKeyUp(_hotkeyList.EnemyCruiser))
            {
                EnemyCruiser?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}