using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Target button help label string :D
    // FELIX  Test :D
    public class HelpLabelManager : ModalManager, IHelpLabelManager
    {
        private readonly IHelpStateFinder _helpStateFinder;
        private IHelpState _helpState;

        private ISettableBroadcastingProperty<bool> _isShown;
        public IBroadcastingProperty<bool> IsShown { get; }

        public HelpLabelManager(
            INavigationPermitterManager navigationPermitterManager, 
            IPauseGameManager pauseGameManager, 
            IHelpStateFinder helpStateFinder)
            : base(navigationPermitterManager, pauseGameManager)
        {
            Assert.IsNotNull(helpStateFinder);

            _helpStateFinder = helpStateFinder;

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