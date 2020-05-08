using System;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

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
            UserChosenTargetChanged?.Invoke(this, EventArgs.Empty);
        }

        // FELIX  Update tests
        public void ToggleChosenTarget(ITarget target)
        {
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