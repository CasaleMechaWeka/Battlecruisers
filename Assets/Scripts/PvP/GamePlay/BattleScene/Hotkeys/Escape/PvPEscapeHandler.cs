// using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.HelpLabels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.Escape
{
    public class PvPEscapeHandler
    {
        private readonly IPvPEscapeDetector _escapeDetector;
        private readonly IPvPMainMenuManager _mainMenuManager;

        public PvPEscapeHandler(IPvPEscapeDetector escapeDetector, IPvPMainMenuManager mainMenuManager)
        {
            PvPHelper.AssertIsNotNull(escapeDetector, mainMenuManager);

            _escapeDetector = escapeDetector;
            _mainMenuManager = mainMenuManager;


            _escapeDetector.EscapePressed += _escapeDetector_EscapePressed;
        }

        private void _escapeDetector_EscapePressed(object sender, EventArgs e)
        {
            if (_mainMenuManager.IsShown)
            {
                _mainMenuManager.DismissMenu();
            }
            else
            {
                _mainMenuManager.ShowMenu();
            }
        }
    }
}