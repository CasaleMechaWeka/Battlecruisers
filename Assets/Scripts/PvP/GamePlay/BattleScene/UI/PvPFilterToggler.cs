using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI;
using BattleCruisers.UI.Filters;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPFilterToggler
    {
        private readonly IBroadcastingFilter _shouldBeEnabledFilter;
        private readonly ITogglable[] _togglables;

        public PvPFilterToggler(IBroadcastingFilter shouldBeEnabledFilter, params ITogglable[] togglables)
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
            foreach (ITogglable togglable in _togglables)
            {
                togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
            }
        }
    }
}