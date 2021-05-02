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
        private readonly IHelpLabelManager _helpLabelManager;

        public EscapeHandler(IEscapeDetector escapeDetector, IMainMenuManager mainMenuManager, IHelpLabelManager helpLabelManager)
        {
            Helper.AssertIsNotNull(escapeDetector, mainMenuManager, helpLabelManager);

            _escapeDetector = escapeDetector;
            _mainMenuManager = mainMenuManager;
            _helpLabelManager = helpLabelManager;

            _escapeDetector.EscapePressed += _escapeDetector_EscapePressed;
        }

        private void _escapeDetector_EscapePressed(object sender, EventArgs e)
        {
            if (_helpLabelManager.IsShown.Value)
            {
                _helpLabelManager.HideHelpLabels();
            }
            else if (_mainMenuManager.IsShown)
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