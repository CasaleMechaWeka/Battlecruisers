using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Hotkeys
{
    // FELIX  Test
    public class HotkeyDetector : IHotkeyDetector
    {
        private readonly IHotkeyList _hotkeyList;
        private readonly IInput _input;
        private readonly IUpdater _updater;

        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Buildable buttons
        public event EventHandler AttackBoat, Frigate, Destroyer, Archon;

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
            if (_input.GetKeyUp(_hotkeyList.Overview))
            {
                Overview?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.EnemyCruiser))
            {
                EnemyCruiser?.Invoke(this, EventArgs.Empty);
            }
            // Boats
            if (_input.GetKeyUp(_hotkeyList.AttackBoat))
            {
                AttackBoat?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Frigate))
            {
                Frigate?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Destroyer))
            {
                Destroyer?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Archon))
            {
                Archon?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}