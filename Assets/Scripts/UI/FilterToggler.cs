using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI
{
    // FELIX  Update tests :)
    public class FilterToggler
    {
        private readonly IBroadcastingFilter _shouldBeEnabledFilter;
        private readonly ITogglable[] _togglables;

        public FilterToggler(IBroadcastingFilter shouldBeEnabledFilter, params ITogglable[] togglables)
        {
            Helper.AssertIsNotNull(shouldBeEnabledFilter, togglables);

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