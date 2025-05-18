using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Target button help label string :D
    public class HelpLabelManager : ModalManager
    {
        private readonly HelpStateFinder _helpStateFinder;
        private HelpState _helpState;

        private ISettableBroadcastingProperty<bool> _isShown;
        public IBroadcastingProperty<bool> IsShown { get; }

        public HelpLabelManager(
            INavigationPermitterManager navigationPermitterManager,
            PauseGameManager pauseGameManager,
            HelpStateFinder helpStateFinder)
            : base(navigationPermitterManager, pauseGameManager)
        {
            Assert.IsNotNull(helpStateFinder);

            _helpStateFinder = helpStateFinder;

            _isShown = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsShown = new BroadcastingProperty<bool>(_isShown);
        }

        public void ShowHelpLabels()
        {
            Logging.LogMethod(Tags.HELP_LABELS);

            if (_helpState != null)
            {
                return;
            }

            base.ShowModal();

            _helpState = _helpStateFinder.FindHelpState();
            _helpState.ShowHelpLabels();
            _isShown.Value = true;
        }

        public void HideHelpLabels()
        {
            Logging.LogMethod(Tags.HELP_LABELS);

            if (_helpState == null)
            {
                return;
            }

            base.HideModal();

            _helpState.HideHelpLables();
            _helpState = null;
            _isShown.Value = false;
        }
    }
}