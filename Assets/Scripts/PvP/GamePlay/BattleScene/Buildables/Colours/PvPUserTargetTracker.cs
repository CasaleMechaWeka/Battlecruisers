using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours
{
    public class PvPUserTargetTracker
    {
        private readonly IPvPBroadcastingProperty<IPvPTarget> _itemShownInInformator;
        private readonly IPvPUserTargets _userTargets;

        public PvPUserTargetTracker(
            IPvPBroadcastingProperty<IPvPTarget> itemShownInInformator,
            IPvPUserTargets userTargets)
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