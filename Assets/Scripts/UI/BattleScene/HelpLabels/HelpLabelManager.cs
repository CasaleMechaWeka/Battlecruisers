using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Properties;

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

        private ISettableBroadcastingProperty<bool> _isShown;
        public IBroadcastingProperty<bool> IsShown { get; }

        public HelpLabelManager(IHelpStateFinder helpStateFinder, IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(helpStateFinder, pauseGameManager);

            _helpStateFinder = helpStateFinder;
            _pauseGameManager = pauseGameManager;

            _isShown = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsShown = new BroadcastingProperty<bool>(_isShown);
        }

        public void ShowHelpLables()
        {
            Logging.LogMethod(Tags.HELP_LABELS);

            if (_helpState != null)
            {
                return;
            }

            _helpState = _helpStateFinder.FindHelpState();
            _helpState.ShowHelpLabels();
            _pauseGameManager.PauseGame();
            _isShown.Value = true;
        }

        public void HideHelpLabels()
        {
            Logging.LogMethod(Tags.HELP_LABELS);

            if (_helpState == null)
            {
                return;
            }

            _helpState.HideHelpLables();
            _helpState = null;
            _pauseGameManager.ResumeGame();
            _isShown.Value = false;
        }
    }
}