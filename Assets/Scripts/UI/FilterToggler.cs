using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI
{
    public class FilterToggler
    {
        private readonly IBroadcastingFilter _shouldBeEnabledFilter;
        private readonly ITogglable _togglable;

        public FilterToggler(IBroadcastingFilter shouldBeEnabledFilter, ITogglable togglable)
        {
            Helper.AssertIsNotNull(shouldBeEnabledFilter, togglable);

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _togglable = togglable;

            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;
            _togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
        }

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            _togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
        }
    }
}