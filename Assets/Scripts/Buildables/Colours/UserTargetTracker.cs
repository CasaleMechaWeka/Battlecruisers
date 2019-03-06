using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.Buildables.Colours
{
    public class UserTargetTracker
    {
        private readonly IBroadcastingProperty<ITarget> _itemShownInInformator;
        private readonly IRankedTargetTracker _userChosenTargetTracker;
        private readonly IUserTargets _userTargets;

        public UserTargetTracker(
            IBroadcastingProperty<ITarget> itemShownInInformator, 
            IRankedTargetTracker userChosenTargetTracker,
            IUserTargets userTargets)
        {
            Helper.AssertIsNotNull(itemShownInInformator, userChosenTargetTracker, userTargets);

            _itemShownInInformator = itemShownInInformator;
            _userChosenTargetTracker = userChosenTargetTracker;
            _userTargets = userTargets;

            _itemShownInInformator.ValueChanged += _itemShownInInformator_ValueChanged;
            _userChosenTargetTracker.HighestPriorityTargetChanged += _userChosenTargetTracker_HighestPriorityTargetChanged;
        }

        private void _itemShownInInformator_ValueChanged(object sender, EventArgs e)
        {
            _userTargets.SelectedTarget = null;

            if (_itemShownInInformator.Value != null
                && _itemShownInInformator.Value.IsInScene)
            {
                _userTargets.SelectedTarget = _itemShownInInformator.Value;
            }
        }

        private void _userChosenTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            _userTargets.TargetToAttack = null;

            if (_userChosenTargetTracker.HighestPriorityTarget != null)
            {
                _userTargets.TargetToAttack = _userChosenTargetTracker.HighestPriorityTarget.Target;
            }
        }
    }
}