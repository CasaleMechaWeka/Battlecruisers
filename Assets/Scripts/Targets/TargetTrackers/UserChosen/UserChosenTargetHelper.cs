using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    public class UserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetManager _userChosenTargetManager;
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly ITargetIndicator _targetIndicator;

        public ITarget UserChosenTarget => _userChosenTargetManager.HighestPriorityTarget?.Target;

        public event EventHandler UserChosenTargetChanged;

        public UserChosenTargetHelper(
            IUserChosenTargetManager userChosenTargetManager, 
            IPrioritisedSoundPlayer soundPlayer,
            ITargetIndicator targetIndicator)
        {
            Helper.AssertIsNotNull(userChosenTargetManager, soundPlayer, targetIndicator);

            _userChosenTargetManager = userChosenTargetManager;
            _soundPlayer = soundPlayer;
            _targetIndicator = targetIndicator;

            _userChosenTargetManager.HighestPriorityTargetChanged += _userChosenTargetManager_HighestPriorityTargetChanged;
        }

        private void _userChosenTargetManager_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.USER_CHOSEN_TARGET, $"Highest priority target: {_userChosenTargetManager.HighestPriorityTarget}");

            UserChosenTargetChanged?.Invoke(this, EventArgs.Empty);

            if (_userChosenTargetManager.HighestPriorityTarget == null)
            {
                _targetIndicator.Hide();
            }
        }

        public void ToggleChosenTarget(ITarget target)
        {
            Logging.Log(Tags.USER_CHOSEN_TARGET, target);

            if (ReferenceEquals(UserChosenTarget, target))
            {
                // Clear user chosen target
                _userChosenTargetManager.Target = null;
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Targetting.TargetCleared);
                _targetIndicator.Hide();
            }
            else
            {
                // Set user chosen target
                _userChosenTargetManager.Target = target;
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Targetting.NewTarget);
                _targetIndicator.Show(target.Position);
            }
        }
    }
}