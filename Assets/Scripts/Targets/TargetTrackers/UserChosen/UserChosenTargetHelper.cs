using System;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    // FELIX  Update tests :)
    public class UserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetManager _userChosenTargetManager;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public ITarget UserChosenTarget
        {
            get
            {
                return
                    _userChosenTargetManager.HighestPriorityTarget != null ?
                    _userChosenTargetManager.HighestPriorityTarget.Target :
                    null;
            }
        }

        public event EventHandler UserChosenTargetChanged;

        public UserChosenTargetHelper(IUserChosenTargetManager userChosenTargetManager, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(userChosenTargetManager, soundPlayer);

            _userChosenTargetManager = userChosenTargetManager;
            _soundPlayer = soundPlayer;

            _userChosenTargetManager.HighestPriorityTargetChanged += _userChosenTargetManager_HighestPriorityTargetChanged;
        }

        private void _userChosenTargetManager_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            if (UserChosenTargetChanged != null)
            {
                UserChosenTargetChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void ToggleChosenTarget(ITarget target)
        {
            ITarget currentUserChosenTarget = _userChosenTargetManager.HighestPriorityTarget != null ? _userChosenTargetManager.HighestPriorityTarget.Target : null;

            if (ReferenceEquals(currentUserChosenTarget, target))
            {
                // Clear user chosen target
                _userChosenTargetManager.Target = null;
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Targetting.TargetCleared);
            }
            else
            {
                // Set user chosen target
                _userChosenTargetManager.Target = target;
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Targetting.NewTarget);
            }
        }
    }
}