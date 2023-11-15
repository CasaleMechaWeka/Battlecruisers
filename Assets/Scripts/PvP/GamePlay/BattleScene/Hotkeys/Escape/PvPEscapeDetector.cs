using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.Escape
{
    public class PvPEscapeDetector : IPvPEscapeDetector
    {
        private readonly IPvPInput _input;
        private readonly IPvPUpdater _updater;

        public event EventHandler EscapePressed;

        public PvPEscapeDetector(IPvPInput input, IPvPUpdater updater)
        {
            PvPHelper.AssertIsNotNull(input, updater);

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