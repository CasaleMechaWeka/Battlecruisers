using BattleCruisers.Utils;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.Buildables.Colours
{
    public class UserTargetTracker
    {
        private readonly IBroadcastingProperty<ITarget> _itemShownInInformator;
        private readonly IUserTargets _userTargets;

        public UserTargetTracker(
            IBroadcastingProperty<ITarget> itemShownInInformator, 
            IUserTargets userTargets)
        {
            Helper.AssertIsNotNull(itemShownInInformator, userTargets);

            _itemShownInInformator = itemShownInInformator;
            _userTargets = userTargets;

            _itemShownInInformator.ValueChanged += _itemShownInInformator_ValueChanged;
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
    }
}