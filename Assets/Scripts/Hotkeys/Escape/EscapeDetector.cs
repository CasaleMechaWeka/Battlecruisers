using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.Hotkeys.Escape
{
    public class EscapeDetector : IEscapeDetector
    {
        private readonly IInput _input;
        private readonly IUpdater _updater;

        public event EventHandler EscapePressed;

        public EscapeDetector(IInput input, IUpdater updater)
        {
            Helper.AssertIsNotNull(input, updater);

            _input = input;
            _updater = updater;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.GetKeyUp(KeyCode.Escape))
            {
                EscapePressed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}