using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Help button clicked mide panel slide? Will pause freeze slide? :P
    // FELIX  Esc should hide help menus
    // FELIX  Test :D
    public class HelpLabelManager : IHelpLabelManager
    {
        private readonly IHelpStateFinder _helpStateFinder;
        private readonly IPauseGameManager _pauseGameManager;
        private IHelpState _helpState;

        public HelpLabelManager(IHelpStateFinder helpStateFinder, IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(helpStateFinder, pauseGameManager);

            _helpStateFinder = helpStateFinder;
            _pauseGameManager = pauseGameManager;
        }

        public void ShowHelpLables()
        {
            if (_helpState != null)
            {
                return;
            }

            _helpState = _helpStateFinder.FindHelpState();
            _helpState.ShowHelpLabels();
            _pauseGameManager.PauseGame();
        }

        public void HideHelpLabels()
        {
            if (_helpState == null)
            {
                return;
            }

            _helpState.HideHelpLables();
            _helpState = null;
            _pauseGameManager.ResumeGame();
        }
    }
}