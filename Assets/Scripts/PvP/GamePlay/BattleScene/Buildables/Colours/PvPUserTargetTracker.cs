using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours
{
    public class PvPUserTargetTracker
    {
        private readonly IBroadcastingProperty<ITarget> _itemShownInInformator;
        private readonly IUserTargets _userTargets;

        public PvPUserTargetTracker(
            IBroadcastingProperty<ITarget> itemShownInInformator,
            IUserTargets userTargets)
        {
            PvPHelper.AssertIsNotNull(itemShownInInformator, userTargets);

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