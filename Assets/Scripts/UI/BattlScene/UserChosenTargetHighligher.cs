using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene
{
    /// <summary>
    /// Highlights the user chosen target.
    /// </summary>
    /// FELIX  Remove?
    public class UserChosenTargetHighligher : IManagedDisposable
    {
        private readonly IRankedTargetTracker _userChosenTargetTracker;
        private readonly IHighlightHelper _highlightHelper;

        private IHighlight _currentHighlight;

        public UserChosenTargetHighligher(IRankedTargetTracker userChosenTargetTracker, IHighlightHelper highlightHelper)
        {
            Helper.AssertIsNotNull(userChosenTargetTracker, highlightHelper);

            _userChosenTargetTracker = userChosenTargetTracker;
            _highlightHelper = highlightHelper;

            _userChosenTargetTracker.HighestPriorityTargetChanged += _userChosenTargetTracker_HighestPriorityTargetChanged;
        }

        private void _userChosenTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            if (_currentHighlight != null)
            {
                _currentHighlight.Destroy();
                _currentHighlight = null;
            }
                
            if (_userChosenTargetTracker.HighestPriorityTarget != null)
            {
                _currentHighlight = _highlightHelper.CreateHighlight(_userChosenTargetTracker.HighestPriorityTarget.Target, usePulsingAnimation: false);
            }
        }

        public void DisposeManagedState()
        {
            _userChosenTargetTracker.HighestPriorityTargetChanged -= _userChosenTargetTracker_HighestPriorityTargetChanged;
        }
    }
}