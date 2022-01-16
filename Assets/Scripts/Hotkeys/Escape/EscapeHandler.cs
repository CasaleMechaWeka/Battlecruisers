using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.Escape
{
    public class EscapeHandler
    {
        private readonly IEscapeDetector _escapeDetector;
        private readonly IMainMenuManager _mainMenuManager;

        public EscapeHandler(IEscapeDetector escapeDetector, IMainMenuManager mainMenuManager)
        {
            Helper.AssertIsNotNull(escapeDetector, mainMenuManager);

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