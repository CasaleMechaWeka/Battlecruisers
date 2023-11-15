using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPFilterToggler
    {
        private readonly IPvPBroadcastingFilter _shouldBeEnabledFilter;
        private readonly IPvPTogglable[] _togglables;

        public PvPFilterToggler(IPvPBroadcastingFilter shouldBeEnabledFilter, params IPvPTogglable[] togglables)
        {
            PvPHelper.AssertIsNotNull(shouldBeEnabledFilter, togglables);

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _togglables = togglables;

            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;
            SetEnabledStatus();
        }

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            SetEnabledStatus();
        }

        private void SetEnabledStatus()
        {
            foreach (IPvPTogglable togglable in _togglables)
            {
                togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
            }
        }
    }
}