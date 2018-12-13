using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI
{
    public class FilterToggler
    {
        private readonly ITogglable _togglable;
        private readonly IBroadcastingFilter _shouldBeEnabledFilter;

        public FilterToggler(ITogglable togglable, IBroadcastingFilter shouldBeEnabledFilter)
        {
            Helper.AssertIsNotNull(togglable, shouldBeEnabledFilter);

            _togglable = togglable;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;
            _togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
        }

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            _togglable.Enabled = _shouldBeEnabledFilter.IsMatch;
        }
    }
}